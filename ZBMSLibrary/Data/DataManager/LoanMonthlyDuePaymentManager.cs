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
    public class LoanMonthlyDuePaymentManager : ILoanMonthlyDuePaymentManager
    {
        private readonly IDbHandler _dbHandler;

        public LoanMonthlyDuePaymentManager(IDbHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }

        public async Task LoanMonthlyDuePaymentAsync(LoanMonthlyDuePaymentRequest loanMonthlyDuePaymentRequest,
            LoanMonthlyDuePaymentUseCaseCallBack loanMonthlyDuePaymentUseCaseCallBack)
        {
            try
            {
                TransactionSummary transactionSummary = new TransactionSummary()
                {
                    Amount = loanMonthlyDuePaymentRequest.DueAmount,
                    Description = "Due Payment",
                    SenderAccountNumber = loanMonthlyDuePaymentRequest.AccountNumber,
                    ReceiverAccountNumber = loanMonthlyDuePaymentRequest.LoanAccountNumber,
                    TransactionOn = DateTime.Now,
                    TransactionType = TransactionType.Debit,
                };
                var userName = await _dbHandler.GetUserNameAsync(loanMonthlyDuePaymentRequest.UserId);

                try
                {
                    var account = await _dbHandler.GetSavingsAccountAsync(loanMonthlyDuePaymentRequest.AccountNumber);
                    account.Balance -= loanMonthlyDuePaymentRequest.DueAmount;
                    if (account.Balance < 0)
                    {
                        throw new Exception("No sufficient balance");
                        //account.Balance += loanMonthlyDuePaymentRequest.DueAmount;
                    }

                    //transactionSummary.SenderAccountNumber = account.AccountNumber;
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
                    NotificationEvents.SavingsLoanDuePaidNotification?.Invoke(transactionSummaryVObj);
                }
                catch (InvalidOperationException invalidOperationException)
                {
                    var account =
                        await _dbHandler.GetCurrentAccountAsync(loanMonthlyDuePaymentRequest.AccountNumber);
                    account.Balance -= loanMonthlyDuePaymentRequest.DueAmount;
                    if (account.Balance < 0)
                    {
                        throw new Exception("No sufficient balance");
                    }
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
                    NotificationEvents.CurrentAccountLoanDuePaidNotification?.Invoke(transactionSummaryVObj);
                }
                //get loan account manager
                var personalLoan =
                    await _dbHandler.GetPersonalLoanAccountAsync(loanMonthlyDuePaymentRequest.AccountNumber);
                if (personalLoan.DueWithInterestAmount == 0)
                {
                    personalLoan.AccountStatus = AccountStatus.Closed;
                    personalLoan.NextDateToBePaid = DateTime.Now;

                }
                else
                {
                    personalLoan.NextDateToBePaid = DateTime.Now.AddDays(30);
                    personalLoan.DueWithInterestAmount -= loanMonthlyDuePaymentRequest.DueAmount;
                }
                
                await _dbHandler.UpdatePersonalLoanAsync(personalLoan);
                var branchName = await _dbHandler.GetBranchNameAsync(personalLoan.IfscCode);

                var personalLoanBObj = new PersonalLoanBObj
                {
                    AccountNumber = personalLoan.AccountNumber,
                    IfscCode = personalLoan.IfscCode,
                    UserId = personalLoan.UserId,
                    CreatedOn = personalLoan.CreatedOn,
                    AccountStatus = personalLoan.AccountStatus,
                    Due = personalLoan.Due,
                    DueWithInterestAmount = personalLoan.DueWithInterestAmount,
                    OriginalAmount = personalLoan.OriginalAmount,
                    NextDateToBePaid = personalLoan.NextDateToBePaid,
                    Tenure = personalLoan.Tenure,
                    FineAmount = personalLoan.FineAmount,
                    InterestRate = personalLoan.InterestRate,
                    UserName = userName,
                    BranchName = branchName,
                };
                NotificationEvents.PersonalLoanUpdated?.Invoke(personalLoanBObj);


                loanMonthlyDuePaymentUseCaseCallBack?.OnSuccess(new LoanMonthlyDuePaymentResponse());

            }
            catch (Exception e)
            {
                //when no sufficient balance in account
                loanMonthlyDuePaymentUseCaseCallBack?.OnError(e);
            }
        }
    }
}