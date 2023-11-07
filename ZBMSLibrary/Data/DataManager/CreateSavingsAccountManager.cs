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
    public class CreateSavingsAccountManager : ICreateSavingsAccountManager
    {
        private readonly IDbHandler _dbHandler;

        public CreateSavingsAccountManager(IDbHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }

        public async Task CreateSavingsAccountAsync(CreateSavingsAccountRequest createSavingsAccountRequest,
            CreateSavingsAccountUseCaseCallBack createSavingsAccountUseCaseCallBack)
        {
            try
            {
                TransactionSummary transactionSummary = new TransactionSummary()
                {
                    Amount = createSavingsAccountRequest.SavingsAccount.Balance,
                    Description = "First Deposit in savings Account",
                    ReceiverAccountNumber = createSavingsAccountRequest.SavingsAccount.AccountNumber,
                    TransactionOn = DateTime.Now,
                    TransactionType = TransactionType.Credit,
                    SenderAccountNumber = "-"
                };
                await _dbHandler.InsertTransactionAsync(transactionSummary);
                await _dbHandler.CreateSavingsAccountAsync(createSavingsAccountRequest.SavingsAccount);
                var savingsAccountBObj = new SavingsAccountBObj()
                {
                    UserId = createSavingsAccountRequest.SavingsAccount.UserId,
                    AccountStatus = createSavingsAccountRequest.SavingsAccount.AccountStatus,
                    Balance = createSavingsAccountRequest.SavingsAccount.Balance,
                    IfscCode = createSavingsAccountRequest.SavingsAccount.IfscCode,
                    InterestRate = createSavingsAccountRequest.SavingsAccount.InterestRate,
                    CreatedOn = createSavingsAccountRequest.SavingsAccount.CreatedOn,
                    AccountNumber = createSavingsAccountRequest.SavingsAccount.AccountNumber,
                    ServiceCharges = createSavingsAccountRequest.SavingsAccount.ServiceCharges,
                    MinimumBalance = createSavingsAccountRequest.SavingsAccount.MinimumBalance,
                    FineAmount = createSavingsAccountRequest.SavingsAccount.FineAmount,
                    ToBeCreditedAmount = createSavingsAccountRequest.SavingsAccount.ToBeCreditedAmount,
                };
                var userName = await _dbHandler.GetUserNameAsync(createSavingsAccountRequest.SavingsAccount.UserId);
                var branchName = await _dbHandler.GetBranchNameAsync(createSavingsAccountRequest.SavingsAccount.IfscCode);
                TransactionSummaryVObj transactionSummaryVObj = new TransactionSummaryVObj()
                {
                    Amount = transactionSummary.Amount,
                    Description = transactionSummary.Description,
                    ReceiverAccountNumber = transactionSummary.ReceiverAccountNumber,
                    TransactionOn = transactionSummary.TransactionOn,
                    TransactionType = transactionSummary.TransactionType,
                    SenderAccountNumber = transactionSummary.SenderAccountNumber,
                    UserName = userName,
                    Id = transactionSummary.Id,
                };
                savingsAccountBObj.TransactionList.Add(transactionSummaryVObj);
                savingsAccountBObj.BranchName = branchName;
                savingsAccountBObj.UserName = userName;
                NotificationEvents.SavingsAccountCreated?.Invoke(savingsAccountBObj);
                createSavingsAccountUseCaseCallBack?.OnSuccess(new CreateSavingsAccountResponse());
            }
            catch (Exception e)
            {
                createSavingsAccountUseCaseCallBack?.OnError(e);
            }
        }
    }
}