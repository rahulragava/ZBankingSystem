using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZBMSLibrary.Data.DataHandler.Contract;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Enums;
using ZBMSLibrary.Entities.Model;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager
{
    public class CreateCurrentAccountManager : ICreateCurrentAccountManager
    {
        private readonly IDbHandler _dbHandler;

        public CreateCurrentAccountManager(IDbHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }

        public async Task CreateCurrentAccountAsync(CreateCurrentAccountRequest createCurrentAccountRequest,
            CreateCurrentAccountUseCaseCallBack createCurrentAccountUseCaseCallBack)
        {
            try
            {
                TransactionSummary transactionSummary = new TransactionSummary()
                {
                    Amount = createCurrentAccountRequest.CurrentAccount.Balance,
                    Description = "First Deposit",
                    ReceiverAccountNumber = createCurrentAccountRequest.CurrentAccount.AccountNumber,
                    TransactionOn = DateTime.Now,
                    TransactionType = TransactionType.Credit,
                    SenderAccountNumber = "-"
                };
                await _dbHandler.InsertTransactionAsync(transactionSummary);
                await _dbHandler.CreateCurrentAccountAsync(createCurrentAccountRequest.CurrentAccount);
                var currentAccountBObj = new CurrentAccountBObj()
                {
                    UserId = createCurrentAccountRequest.CurrentAccount.UserId,
                    AccountStatus = createCurrentAccountRequest.CurrentAccount.AccountStatus,
                    Balance = createCurrentAccountRequest.CurrentAccount.Balance,
                    IfscCode = createCurrentAccountRequest.CurrentAccount.IfscCode,
                    CreatedOn = createCurrentAccountRequest.CurrentAccount.CreatedOn,
                    AccountNumber = createCurrentAccountRequest.CurrentAccount.AccountNumber,
                    ServiceCharges = createCurrentAccountRequest.CurrentAccount.ServiceCharges,
                    MinimumBalance = createCurrentAccountRequest.CurrentAccount.MinimumBalance,
                    FineAmount = createCurrentAccountRequest.CurrentAccount.FineAmount,
                };

                var userName = await _dbHandler.GetUserNameAsync(createCurrentAccountRequest.CurrentAccount.UserId);
                var branchName = await _dbHandler.GetBranchNameAsync(createCurrentAccountRequest.CurrentAccount.IfscCode);
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
                currentAccountBObj.TransactionList.Add(transactionSummaryVObj);
                currentAccountBObj.BranchName = branchName;
                currentAccountBObj.UserName = userName;
                NotificationEvents.CurrentAccountCreated?.Invoke(currentAccountBObj);
                createCurrentAccountUseCaseCallBack?.OnSuccess(new CreateCurrentAccountResponse());
            }
            catch (Exception e)
            {
                createCurrentAccountUseCaseCallBack?.OnError(e);
            }
            //throw new System.NotImplementedException();
        }
    }
}