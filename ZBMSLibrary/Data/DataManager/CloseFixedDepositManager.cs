using System;
using System.Threading.Tasks;
using ZBMSLibrary.Data.DataHandler.Contract;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Entities.Enums;
using ZBMSLibrary.Entities.Model;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager
{
    public class CloseFixedDepositManager : ICloseFixedDepositManager
    {
        private IDbHandler _dbHandler;

        public CloseFixedDepositManager(IDbHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }

        public async Task CloseFixedDepositAsync(CloseFixedDepositRequest closeFixedDepositRequest,
            CloseFixedDepositUseCaseCallBack closeFixedDepositUseCaseCallBack)
        {
            try
            {
                //for fixed deposit, have to calculate the closing amount, check whether the deposited amount is estimated return or current estimated return. Must show the current estimated return when try to close the deposit
                var returnAmount = closeFixedDepositRequest.FixedDepositBObj.ManualMaturityAmountCalculator(
                    closeFixedDepositRequest.FixedDepositBObj.DepositedAmount, DateTime.Now.Year - closeFixedDepositRequest.FixedDepositBObj.CreatedOn.Year);

                var savingsAccount = await _dbHandler.GetSavingsAccountAsync(closeFixedDepositRequest.FixedDepositBObj.SavingsAccountId);
                savingsAccount.Balance += returnAmount;
                await _dbHandler.UpdateSavingsAccountAsync(savingsAccount);
                //transaction
                var transaction = new TransactionSummary
                {
                    SenderAccountNumber = closeFixedDepositRequest.FixedDepositBObj.AccountNumber,
                    ReceiverAccountNumber = savingsAccount.AccountNumber,
                    TransactionOn = DateTime.Now,
                    Amount = closeFixedDepositRequest.FixedDepositBObj.DepositedAmount,
                    TransactionType = TransactionType.Credit,
                    Description = "Fixed Deposit closed"
                };
                await _dbHandler.InsertTransactionAsync(transaction);
                closeFixedDepositRequest.FixedDepositBObj.DepositedAmount = 0;
                closeFixedDepositRequest.FixedDepositBObj.AccountStatus = AccountStatus.Closed;

                var fixedDeposit = new FixedDeposit
                {
                    AccountNumber = closeFixedDepositRequest.FixedDepositBObj.AccountNumber,
                    IfscCode = closeFixedDepositRequest.FixedDepositBObj.IfscCode,
                    UserId = closeFixedDepositRequest.FixedDepositBObj.UserId,
                    CreatedOn = closeFixedDepositRequest.FixedDepositBObj.CreatedOn,
                    AccountStatus = AccountStatus.Closed,
                    DepositedAmount = closeFixedDepositRequest.FixedDepositBObj.DepositedAmount,
                    InterestRate = closeFixedDepositRequest.FixedDepositBObj.InterestRate,
                    Tenure = closeFixedDepositRequest.FixedDepositBObj.Tenure,
                    SavingsAccountId = closeFixedDepositRequest.FixedDepositBObj.SavingsAccountId,
                    FromAccountId = closeFixedDepositRequest.FixedDepositBObj.FromAccountId,
                };
                await _dbHandler.UpdateFixedDepositAsync(fixedDeposit);
                closeFixedDepositUseCaseCallBack?.OnSuccess(new CloseFixedDepositResponse());
            }
            catch (Exception e)
            {
               closeFixedDepositUseCaseCallBack?.OnError(e);
            }
        }
    }
}