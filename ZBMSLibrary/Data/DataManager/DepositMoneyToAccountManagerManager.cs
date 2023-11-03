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
    public class DepositMoneyToAccountManagerManager : IDepositMoneyToAccountManager
    {
        private readonly IDbHandler _dbHandler;
        public DepositMoneyToAccountManagerManager(IDbHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }
        public async Task DepositMoneyToAccount(DepositMoneyRequest depositMoneyRequest,
            DepositMoneyUseCaseCallBack depositMoneyUseCaseCallBack)
        {
            try
            {
                TransactionSummary transactionSummary = new TransactionSummary()
                {
                    Amount = depositMoneyRequest.Amount,
                    Description = "Deposit",
                    SenderAccountNumber = "-",
                    TransactionOn = DateTime.Now,
                    TransactionType = TransactionType.Credit,
                };
                switch (depositMoneyRequest.Account)
                {
                    case SavingsAccount savingsAccount:
                        savingsAccount.Balance = savingsAccount.Balance + depositMoneyRequest.Amount;
                        transactionSummary.ReceiverAccountNumber = savingsAccount.AccountNumber;
                        await _dbHandler.InsertTransactionAsync(transactionSummary);
                        //transactionSummary.Id = transactionId;
                        NotificationEvents.UpdateSavingsAccountDepositTransaction?.Invoke(transactionSummary);
                        NotificationEvents.DepositSavingsAccountAmountUpdation?.Invoke(depositMoneyRequest.Amount);
                        await _dbHandler.UpdateSavingsAccountAsync(savingsAccount);
                        break;
                    case CurrentAccount currentAccount:
                        currentAccount.Balance += depositMoneyRequest.Amount;
                        transactionSummary.ReceiverAccountNumber = currentAccount.AccountNumber;
                        await _dbHandler.InsertTransactionAsync(transactionSummary);
                        //transactionSummary.Id = id;
                        NotificationEvents.UpdateCurrentAccountDepositTransaction(transactionSummary);
                        NotificationEvents.DepositCurrentAmountUpdation?.Invoke(depositMoneyRequest.Amount);
                        await _dbHandler.UpdateCurrentAccountAsync(currentAccount);
                        break;
                }
                depositMoneyUseCaseCallBack?.OnSuccess(new DepositMoneyResponse());
            }
            catch (Exception e)
            {
                depositMoneyUseCaseCallBack?.OnError(e);
            }
        }
    }
}