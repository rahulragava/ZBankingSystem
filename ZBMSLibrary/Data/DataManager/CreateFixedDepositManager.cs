using System;
using System.Threading.Tasks;
using ZBMSLibrary.Data.DataHandler.Contract;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Enums;
using ZBMSLibrary.Entities.Model;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager
{
    public class CreateFixedDepositManager : ICreateFixedDepositManager
    {
        private readonly IDbHandler _dbHandler;

        public CreateFixedDepositManager(IDbHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }

        public async Task CreateFixedDepositAsync(CreateFixedDepositRequest createFixedDepositRequest,
            CreateFixedDepositUseCaseCallBack createFixedDepositUseCaseCallBack)
        {
            try
            {
                TransactionSummary transactionSummary = new TransactionSummary()
                {
                    Amount = createFixedDepositRequest.FixedDeposit.DepositedAmount,
                    Description = "Deposit",
                    ReceiverAccountNumber = createFixedDepositRequest.FixedDeposit.AccountNumber,
                    TransactionOn = DateTime.Now,
                    TransactionType = TransactionType.Debit,
                };
                var userName = await _dbHandler.GetUserNameAsync(createFixedDepositRequest.FixedDeposit.UserId);

                try
                {
                    var account = await _dbHandler.GetSavingsAccountAsync(createFixedDepositRequest.FixedDeposit.FromAccountId);
                    account.Balance -= createFixedDepositRequest.FixedDeposit.DepositedAmount;
                    transactionSummary.SenderAccountNumber = account.AccountNumber;
                    await _dbHandler.InsertTransactionAsync(transactionSummary);
                    await _dbHandler.UpdateSavingsAccountAsync(account);
                    TransactionSummaryVObj transactionSummaryVObj = new TransactionSummaryVObj()
                    {
                        Amount = transactionSummary.Amount,
                        Description = transactionSummary.Description,
                        ReceiverAccountNumber = transactionSummary.ReceiverAccountNumber,
                        TransactionOn = transactionSummary.TransactionOn,
                        TransactionType = transactionSummary.TransactionType,
                        SenderAccountNumber = transactionSummary.SenderAccountNumber,
                        Id = transactionSummary.Id,
                        UserName = userName,
                    };
                    NotificationEvents.FdCreationSavingsTransaction?.Invoke(transactionSummaryVObj);

                }
                catch (Exception ex)
                {
                    var account = await _dbHandler.GetCurrentAccountAsync(createFixedDepositRequest.FixedDeposit.FromAccountId);
                    account.Balance -= createFixedDepositRequest.FixedDeposit.DepositedAmount;
                    transactionSummary.SenderAccountNumber = account.AccountNumber;
                    await _dbHandler.InsertTransactionAsync(transactionSummary);
                    await _dbHandler.UpdateCurrentAccountAsync(account);
                    TransactionSummaryVObj transactionSummaryVObj = new TransactionSummaryVObj()
                    {
                        Amount = transactionSummary.Amount,
                        Description = transactionSummary.Description,
                        ReceiverAccountNumber = transactionSummary.ReceiverAccountNumber,
                        TransactionOn = transactionSummary.TransactionOn,
                        TransactionType = transactionSummary.TransactionType,
                        SenderAccountNumber = transactionSummary.SenderAccountNumber,
                        Id = transactionSummary.Id,
                        UserName = userName,
                        
                    };
                    NotificationEvents.FdCreationCurrentTransaction?.Invoke(transactionSummaryVObj);
                } 
                await _dbHandler.CreateFixedDepositAsync(createFixedDepositRequest.FixedDeposit);

                var fixedDepositBObj = new FixedDepositBObj()
                {
                    UserId = createFixedDepositRequest.FixedDeposit.UserId,
                    AccountStatus = createFixedDepositRequest.FixedDeposit.AccountStatus,
                    DepositedAmount = createFixedDepositRequest.FixedDeposit.DepositedAmount,
                    IfscCode = createFixedDepositRequest.FixedDeposit.IfscCode,
                    CreatedOn = createFixedDepositRequest.FixedDeposit.CreatedOn,
                    AccountNumber = createFixedDepositRequest.FixedDeposit.AccountNumber,
                   InterestRate = createFixedDepositRequest.FixedDeposit.InterestRate,
                   Tenure = createFixedDepositRequest.FixedDeposit.Tenure,
                   FromAccountId = createFixedDepositRequest.FixedDeposit.FromAccountId,
                   SavingsAccountId= createFixedDepositRequest.FixedDeposit.SavingsAccountId,
                };
                var branchName = await _dbHandler.GetBranchNameAsync(createFixedDepositRequest.FixedDeposit.IfscCode);
                fixedDepositBObj.SetDefault();
                fixedDepositBObj.BranchName = branchName;
                fixedDepositBObj.UserName = userName;
                NotificationEvents.FixedDepositCreated?.Invoke(fixedDepositBObj);
                //NotificationEvents.FixedDepositTransaction?.Invoke(transactionSummary);
                createFixedDepositUseCaseCallBack?.OnSuccess(new CreateFixedDepositResponse());
            }
            catch (Exception e)
            {
                createFixedDepositUseCaseCallBack?.OnError(e);
            }
        }
    }
}