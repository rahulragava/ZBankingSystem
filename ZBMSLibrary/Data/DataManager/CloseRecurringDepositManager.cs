using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZBMSLibrary.Data.DataHandler.Contract;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Enums;
using ZBMSLibrary.Entities.Model;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager
{
    public class CloseRecurringDepositManager : ICloseRecurringDepositManager
    {
        private readonly IDbHandler _dbHandler;

        public CloseRecurringDepositManager(IDbHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }

        public async Task CloseRecurringDepositAsync(CloseRecurringDepositRequest closeRecurringDepositRequest,
            CloseRecurringDepositUseCaseCallBack closeRecurringDepositUseCaseCallBack)
        {
            try
            {
                if (closeRecurringDepositRequest.RecurringAccountBObj.DepositedAmount > 0)
                {
                    var savingsAccount = await _dbHandler.GetSavingsAccountAsync(closeRecurringDepositRequest.RecurringAccountBObj.SavingsAccountId);
                    savingsAccount.Balance += closeRecurringDepositRequest.RecurringAccountBObj.DepositedAmount;
                    await _dbHandler.UpdateSavingsAccountAsync(savingsAccount);
                    //transaction
                    var transaction = new TransactionSummary
                    {
                        SenderAccountNumber = closeRecurringDepositRequest.RecurringAccountBObj.AccountNumber,
                        ReceiverAccountNumber = savingsAccount.AccountNumber,
                        TransactionOn = DateTime.Now,
                        Amount = closeRecurringDepositRequest.RecurringAccountBObj.DepositedAmount,
                        TransactionType = TransactionType.Credit,
                        Description = "Recurring Deposit closed"
                    };
                    await _dbHandler.InsertTransactionAsync(transaction);
                    closeRecurringDepositRequest.RecurringAccountBObj.DepositedAmount = 0;
                    closeRecurringDepositRequest.RecurringAccountBObj.AccountStatus = AccountStatus.Closed;

                }
                var recurringDeposit = new RecurringAccount
                {
                    AccountNumber = closeRecurringDepositRequest.RecurringAccountBObj.AccountNumber,
                    IfscCode = closeRecurringDepositRequest.RecurringAccountBObj.IfscCode,
                    UserId = closeRecurringDepositRequest.RecurringAccountBObj.UserId,
                    CreatedOn = closeRecurringDepositRequest.RecurringAccountBObj.CreatedOn,
                    AccountStatus = closeRecurringDepositRequest.RecurringAccountBObj.AccountStatus,
                    DepositedAmount = closeRecurringDepositRequest.RecurringAccountBObj.DepositedAmount,
                    InterestRate = closeRecurringDepositRequest.RecurringAccountBObj.InterestRate,
                    Tenure = closeRecurringDepositRequest.RecurringAccountBObj.Tenure,
                    SavingsAccountId = closeRecurringDepositRequest.RecurringAccountBObj.SavingsAccountId,
                    FromAccountId = closeRecurringDepositRequest.RecurringAccountBObj.FromAccountId,
                    Frequency = closeRecurringDepositRequest.RecurringAccountBObj.Frequency,
                    MonthlyInstallment = closeRecurringDepositRequest.RecurringAccountBObj.MonthlyInstallment,
                    NextDueDate = closeRecurringDepositRequest.RecurringAccountBObj.NextDueDate,
                };
                await _dbHandler.UpdateRecurringAccountAsync(recurringDeposit);
               closeRecurringDepositUseCaseCallBack?.OnSuccess(new CloseRecurringDepositResponse());
              
            }
            catch (Exception e)
            {
                closeRecurringDepositUseCaseCallBack?.OnError(e);
            }
        }
    }
}