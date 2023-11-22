using System;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls.Primitives;
using ZBMSLibrary.Data.DataHandler.Contract;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Data.DataManager.CustomException;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Enums;
using ZBMSLibrary.Entities.Model;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager
{
    public class TransferManager : ITransferManager
    {
        private readonly IDbHandler _dbHandler;

        public TransferManager(IDbHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }

        public async Task TransferAsync(TransferRequest transferRequest, TransferUseCaseCallBack transferUseCaseCallBack)
        {
            try
            {
                var fromTransactionSummary = new TransactionSummary
                {
                    TransactionOn = DateTime.Now,
                    Amount = transferRequest.Amount,
                    TransactionType = TransactionType.Debit,
                    Description = "Transaction"
                };
                var userName = await _dbHandler.GetUserNameAsync(transferRequest.Account.UserId);
                if (transferRequest.Account is SavingsAccountBObj savingsAccountBObj)
                {
                    if (IsTransactionLimitExceeded(savingsAccountBObj))
                    {
                        var savingsAccount = new SavingsAccount
                        {
                            AccountNumber = savingsAccountBObj.AccountNumber,
                            IfscCode = savingsAccountBObj.IfscCode,
                            UserId = savingsAccountBObj.UserId,
                            CreatedOn = savingsAccountBObj.CreatedOn,
                            AccountStatus = savingsAccountBObj.AccountStatus,
                            Balance = savingsAccountBObj.Balance,
                            MinimumBalance = savingsAccountBObj.MinimumBalance,
                            FineAmount = savingsAccountBObj.FineAmount,
                            ServiceCharges = savingsAccountBObj.ServiceCharges,
                            InterestRate = savingsAccountBObj.InterestRate,
                            ToBeCreditedAmount = savingsAccountBObj.ToBeCreditedAmount,
                            NextCreditDateTime = savingsAccountBObj.NextCreditDateTime,
                        };
                        if (savingsAccount.Balance - transferRequest.Amount < 0)
                        {
                            //no sufficient balance in account
                            throw new InsufficientBalanceException("Insufficient balance amount for transaction");
                        }
                        else
                        {
                            savingsAccount.Balance -= transferRequest.Amount;
                        }
                        await _dbHandler.UpdateSavingsAccountAsync(savingsAccount);
                        NotificationEvents.TransferSavingsAccountBalanceUpdation(transferRequest.Amount);
                        fromTransactionSummary.SenderAccountNumber = savingsAccount.AccountNumber;
                        //TransactionSummary toTransactionSummary = new TransactionSummary
                        //{
                        //    SenderAccountNumber = savingsAccount.AccountNumber,
                        //    TransactionOn = DateTime.Now,
                        //    Amount = transferRequest.Amount,
                        //    TransactionType = TransactionType.Credit,
                        //    Description = "Transaction"
                        //};
                        try
                        {
                            var account = await _dbHandler.GetSavingsAccountAsync(transferRequest.AccountNumber);
                            account.Balance += transferRequest.Amount;
                            await _dbHandler.UpdateSavingsAccountAsync(account);
                            fromTransactionSummary.ReceiverAccountNumber = account.AccountNumber;
                            //toTransactionSummary.ReceiverAccountNumber = account.AccountNumber;
                            await _dbHandler.InsertTransactionAsync(fromTransactionSummary);
                            //await _dbHandler.InsertTransactionAsync(toTransactionSummary);

                        }
                        catch (InvalidOperationException e)
                        {
                            var account = await _dbHandler.GetCurrentAccountAsync(transferRequest.AccountNumber);
                            account.Balance += transferRequest.Amount;
                            await _dbHandler.UpdateCurrentAccountAsync(account);
                            fromTransactionSummary.ReceiverAccountNumber = account.AccountNumber;
                            //toTransactionSummary.ReceiverAccountNumber = account.AccountNumber;
                            await _dbHandler.InsertTransactionAsync(fromTransactionSummary);
                            //await _dbHandler.InsertTransactionAsync(toTransactionSummary);
                        }

                    }
                    else
                    {
                        throw new TransactionLimitExceededException("Savings account limit exceeded");
                    }


                }
                else if (transferRequest.Account is CurrentAccountBObj currentAccountBObj)
                {
                    var currentAccount = new CurrentAccount()
                    {
                        AccountNumber = currentAccountBObj.AccountNumber,
                        IfscCode = currentAccountBObj.IfscCode,
                        UserId = currentAccountBObj.UserId,
                        CreatedOn = currentAccountBObj.CreatedOn,
                        AccountStatus = currentAccountBObj.AccountStatus,
                        Balance = currentAccountBObj.Balance,
                        MinimumBalance = currentAccountBObj.MinimumBalance,
                        FineAmount = currentAccountBObj.FineAmount,
                        ServiceCharges = currentAccountBObj.ServiceCharges,
                    };
                    if (currentAccount.Balance - transferRequest.Amount < 0)
                    {
                        //no sufficient balance in account
                        throw new InsufficientBalanceException("Insufficient balance amount for transaction");
                    }
                    else
                    {
                        currentAccount.Balance -= transferRequest.Amount;
                    }
                    await _dbHandler.UpdateCurrentAccountAsync(currentAccount);
                    
                    NotificationEvents.TransferCurrentAccountBalanceUpdation(transferRequest.Amount);

                    fromTransactionSummary.SenderAccountNumber = currentAccount.AccountNumber;
                    //TransactionSummary toTransactionSummary = new TransactionSummary
                    //{
                    //    SenderAccountNumber = currentAccount.AccountNumber,
                    //    TransactionOn = DateTime.Now,
                    //    Amount = transferRequest.Amount,
                    //    TransactionType = TransactionType.Credit,
                    //    Description = "Transaction"
                    //};
                    try
                    {
                        var account = await _dbHandler.GetSavingsAccountAsync(transferRequest.AccountNumber);
                        account.Balance += transferRequest.Amount;
                        await _dbHandler.UpdateSavingsAccountAsync(account);
                        fromTransactionSummary.ReceiverAccountNumber = account.AccountNumber;
                        //toTransactionSummary.ReceiverAccountNumber = account.AccountNumber;
                        await _dbHandler.InsertTransactionAsync(fromTransactionSummary);
                        //await _dbHandler.InsertTransactionAsync(toTransactionSummary);
                        // NotificationEvents.UpdateSavingsAccountDepositTransaction(toTransactionSummary);
                    }
                    catch (InvalidOperationException e)
                    {
                        var account = await _dbHandler.GetCurrentAccountAsync(transferRequest.AccountNumber);
                        account.Balance += transferRequest.Amount;
                        await _dbHandler.UpdateCurrentAccountAsync(account);
                        fromTransactionSummary.ReceiverAccountNumber = account.AccountNumber;
                        //toTransactionSummary.ReceiverAccountNumber = account.AccountNumber;
                        await _dbHandler.InsertTransactionAsync(fromTransactionSummary);
                        //await _dbHandler.InsertTransactionAsync(toTransactionSummary);
                        //  NotificationEvents.UpdateCurrentAccountDepositTransaction(toTransactionSummary);
                    }
                }

                var fromTransactionSummaryVObj = new TransactionSummaryVObj
                {
                    Id = fromTransactionSummary.Id,
                    SenderAccountNumber = fromTransactionSummary.SenderAccountNumber,
                    ReceiverAccountNumber = fromTransactionSummary.ReceiverAccountNumber,
                    TransactionOn = fromTransactionSummary.TransactionOn,
                    Amount = fromTransactionSummary.Amount,
                    TransactionType = fromTransactionSummary.TransactionType,
                    Description = fromTransactionSummary.Description,
                    UserName = userName,
                };
                transferUseCaseCallBack?.OnSuccess(new TransferResponse(fromTransactionSummaryVObj));
            }
            catch (InsufficientBalanceException insufficientBalanceException)
            {
                transferUseCaseCallBack?.OnError(insufficientBalanceException);
            }
            catch (TransactionLimitExceededException transactionLimitExceededException)
            {
                transferUseCaseCallBack?.OnError(transactionLimitExceededException);
            }
            catch (Exception ex)
            {
                transferUseCaseCallBack?.OnError(ex);
            }
        }

        public bool IsTransactionLimitExceeded(SavingsAccountBObj savingsAccountBObj)
        {
            var today = DateTime.Today;
            DateTime startOfDay = today.Date;
            DateTime endOfDay = today.Date.AddDays(1);
            var transactionsOnToday = savingsAccountBObj.TransactionList
                .Where(t =>
                    t.SenderAccountNumber == savingsAccountBObj.AccountNumber &&
                    t.TransactionOn >= startOfDay &&
                    t.TransactionOn < endOfDay)
                .ToList();
            return transactionsOnToday.Count() < 10;
            //return transactionsOnToday.Count() < 10;
        }
    }
}