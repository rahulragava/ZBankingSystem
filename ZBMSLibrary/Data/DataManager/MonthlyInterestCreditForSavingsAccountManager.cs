using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using ZBMSLibrary.Data.DataHandler.Contract;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Entities.Enums;
using ZBMSLibrary.Entities.Model;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager
{
    public class MonthlyInterestCreditForSavingsAccountManager : IMonthlyInterestCreditForSavingsAccountManager
    {
        private readonly IDbHandler _dbHandler;

        public MonthlyInterestCreditForSavingsAccountManager(IDbHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }

        public async Task MonthlyInterestCreditForSavingsAccountAsync(
            MonthlyInterestCreditForSavingsAccountRequest monthlyInterestCreditForSavingsAccountRequest,
            MonthlyInterestCreditForSavingsAccountUseCaseCallBack monthlyInterestCreditForSavingsAccountUseCaseCallBack)
        {
            try
            {
                foreach (var savingsAccountWithCreditMonths in monthlyInterestCreditForSavingsAccountRequest.MonthlyInterestCredits)
                {
                    var dueMonths = savingsAccountWithCreditMonths.Value;
                    var interestAmount = savingsAccountWithCreditMonths.Key.InterestRate * savingsAccountWithCreditMonths.Key.Balance * dueMonths / 100;

                    var savingsAccount = new SavingsAccount
                    {
                        AccountNumber = savingsAccountWithCreditMonths.Key.AccountNumber,
                        IfscCode = savingsAccountWithCreditMonths.Key.IfscCode,
                        UserId = savingsAccountWithCreditMonths.Key.UserId,
                        CreatedOn = savingsAccountWithCreditMonths.Key.CreatedOn,
                        AccountStatus = savingsAccountWithCreditMonths.Key.AccountStatus,
                        Balance = savingsAccountWithCreditMonths.Key.Balance,
                        MinimumBalance = savingsAccountWithCreditMonths.Key.MinimumBalance,
                        FineAmount = savingsAccountWithCreditMonths.Key.FineAmount,
                        ServiceCharges = savingsAccountWithCreditMonths.Key.ServiceCharges,
                        InterestRate = savingsAccountWithCreditMonths.Key.InterestRate,
                        ToBeCreditedAmount = savingsAccountWithCreditMonths.Key.ToBeCreditedAmount + interestAmount,
                        NextCreditDateTime = savingsAccountWithCreditMonths.Key.NextCreditDateTime,
                    };
                    await _dbHandler.UpdateSavingsAccountAsync(savingsAccount);
                    if (DateTime.Now.Subtract(savingsAccountWithCreditMonths.Key.CreatedOn).TotalDays >= 365.25)
                    {
                        TransactionSummary transactionSummary = new TransactionSummary()
                        {
                            Amount = interestAmount,
                            TransactionOn = DateTime.Now,
                            TransactionType = TransactionType.Credit,
                            ReceiverAccountNumber = savingsAccountWithCreditMonths.Key.AccountNumber,
                            SenderAccountNumber = "-",
                            Description = "Interest Credited",
                        };
                        savingsAccount.Balance += savingsAccount.ToBeCreditedAmount;
                        savingsAccount.ToBeCreditedAmount = 0;
                        await _dbHandler.UpdateSavingsAccountAsync(savingsAccount);

                        var transaction = savingsAccountWithCreditMonths.Key.TransactionList.FirstOrDefault(t => t.Description == "Interest Credited");
                        
                        if (transaction == null)
                        {
                            //first time credit 
                            await _dbHandler.InsertTransactionAsync(transactionSummary);

                        }
                        else if(DateTime.Now.Subtract(transaction.TransactionOn).TotalDays >= 365.25)
                        {
                            //using previous year calculating whether interest creditable time or not 
                            await _dbHandler.InsertTransactionAsync(transactionSummary);
                        }
                        NotificationEvents.MonthlyInterestCredited?.Invoke(savingsAccount,interestAmount);
                    }
                    monthlyInterestCreditForSavingsAccountUseCaseCallBack?.OnSuccess(new MonthlyInterestCreditForSavingsAccountResponse());
                }
            }
            catch (Exception ex)
            {
                monthlyInterestCreditForSavingsAccountUseCaseCallBack?.OnError(ex);
            }
        }
    }
}