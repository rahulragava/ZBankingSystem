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
    public class CreateLoanAccountManager : ICreateLoanAccountManager
    {
        private readonly IDbHandler _dbHandler;

        public CreateLoanAccountManager(IDbHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }

        public async Task CreatePersonalLoanAsync(CreatePersonalLoanRequest createPersonalLoanRequest,
            CreatePersonalLoanUseCaseCallBack createPersonalLoanUseCaseCallBack)
        {
            try
            {
                TransactionSummary transactionSummary = new TransactionSummary()
                {
                    Amount = createPersonalLoanRequest.PersonalLoan.OriginalAmount,
                    Description = "Loan Sanctioned",
                    SenderAccountNumber = createPersonalLoanRequest.PersonalLoan.AccountNumber,
                    TransactionOn = DateTime.Now,
                    TransactionType = TransactionType.Credit,
                };
                var userName = await _dbHandler.GetUserNameAsync(createPersonalLoanRequest.PersonalLoan.UserId);
                var branchName = await _dbHandler.GetBranchNameAsync(createPersonalLoanRequest.PersonalLoan.IfscCode);
                try
                {
                    var account = await _dbHandler.GetSavingsAccountAsync(createPersonalLoanRequest.LoanedAmountGoesToAccountNumber);
                    account.Balance += createPersonalLoanRequest.PersonalLoan.OriginalAmount;
                    transactionSummary.ReceiverAccountNumber = account.AccountNumber;
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
                    NotificationEvents.LoanCreationUsingSavingsAccountTransaction?.Invoke(transactionSummaryVObj);

                }
                catch (Exception ex)
                {
                    var account = await _dbHandler.GetCurrentAccountAsync(createPersonalLoanRequest.LoanedAmountGoesToAccountNumber);
                    account.Balance += createPersonalLoanRequest.PersonalLoan.OriginalAmount;
                    transactionSummary.ReceiverAccountNumber = account.AccountNumber;
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
                    NotificationEvents.LoanCreationUsingCurrentAccountTransaction?.Invoke(transactionSummaryVObj);
                }


                await _dbHandler.CreatePersonalLoanAsync(createPersonalLoanRequest.PersonalLoan);
                var personalLoanBObj = new PersonalLoanBObj
                {
                    AccountNumber = createPersonalLoanRequest.PersonalLoan.AccountNumber,
                    IfscCode = createPersonalLoanRequest.PersonalLoan.IfscCode,
                    UserId = createPersonalLoanRequest.PersonalLoan.UserId,
                    CreatedOn = createPersonalLoanRequest.PersonalLoan.CreatedOn,
                    InterestRate = createPersonalLoanRequest.PersonalLoan.InterestRate,
                    Tenure = createPersonalLoanRequest.PersonalLoan.Tenure,
                    FineAmount = createPersonalLoanRequest.PersonalLoan.FineAmount,
                    AccountStatus = createPersonalLoanRequest.PersonalLoan.AccountStatus,
                    Due = createPersonalLoanRequest.PersonalLoan.Due,
                    DueWithInterestAmount = createPersonalLoanRequest.PersonalLoan.DueWithInterestAmount,
                    OriginalAmount = createPersonalLoanRequest.PersonalLoan.OriginalAmount,
                    NextDateToBePaid = createPersonalLoanRequest.PersonalLoan.NextDateToBePaid,
                    BranchName = branchName,
                    UserName = userName
                };

                NotificationEvents.PersonalLoanCreated?.Invoke(personalLoanBObj);
                createPersonalLoanUseCaseCallBack?.OnSuccess(new CreatePersonalLoanResponse());
            }
            catch (Exception e)
            {
                createPersonalLoanUseCaseCallBack?.OnError(e);
            }
        }
    }
}