using System;
using System.Threading.Tasks;
using System.Transactions;
using ZBMSLibrary.Data.DataHandler;
using ZBMSLibrary.Data.DataHandler.Contract;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Enums;
using ZBMSLibrary.Entities.Model;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager
{
    public class WithdrawMoneyFromAccountManager : IWithdrawMoneyFromAccount
    {
        private readonly IDbHandler _dbHandler;
        public WithdrawMoneyFromAccountManager(IDbHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }
        public async Task WithdrawMoneyToAccount(WithdrawMoneyRequest withdrawMoneyRequest,
            WithdrawMoneyUseCaseCallBack withdrawMoneyUseCaseCallBack)
        {
            try
            {
                TransactionSummary transactionSummary = new TransactionSummary()
                {
                    Amount = withdrawMoneyRequest.Amount,
                    Description = "Withdraw",
                    ReceiverAccountNumber = "-",
                    TransactionOn = DateTime.Now,
                    TransactionType = TransactionType.Debit,
                };

                switch (withdrawMoneyRequest.Account)
                {
                    case SavingsAccount savingsAccount:
                        savingsAccount.Balance -= withdrawMoneyRequest.Amount;
                        transactionSummary.SenderAccountNumber = savingsAccount.AccountNumber;
                        await _dbHandler.UpdateSavingsAccountAsync(savingsAccount);
                        await _dbHandler.InsertTransactionAsync(transactionSummary);
                        NotificationEvents.UpdateSavingsAccountWithdrawTransaction?.Invoke(transactionSummary);
                        NotificationEvents.WithdrawSavingsAccountAmountUpdation?.Invoke(withdrawMoneyRequest.Amount);
                        //await _dbHandler.InsertTransactionAsync(transactionSummary);
                        break;
                    case CurrentAccount currentAccount: 
                        transactionSummary.SenderAccountNumber = currentAccount.AccountNumber;
                        currentAccount.Balance -= withdrawMoneyRequest.Amount;
                        await _dbHandler.InsertTransactionAsync(transactionSummary);
                        NotificationEvents.UpdateCurrentAccountWithdrawTransaction?.Invoke(transactionSummary);
                        NotificationEvents.WithdrawCurrentAccountAmountUpdation?.Invoke(withdrawMoneyRequest.Amount);
                        await _dbHandler.UpdateCurrentAccountAsync(currentAccount);
                        break;
                }
                withdrawMoneyUseCaseCallBack?.OnSuccess(new WithdrawMoneyResponse());

            }
            catch (Exception e)
            {
                withdrawMoneyUseCaseCallBack?.OnError(e);
            }
        }
    }
}