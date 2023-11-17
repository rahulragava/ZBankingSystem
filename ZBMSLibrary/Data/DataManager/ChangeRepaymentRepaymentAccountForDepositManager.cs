using System;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using ZBMSLibrary.Data.DataHandler.Contract;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Enums;
using ZBMSLibrary.Entities.Model;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager
{
    public class ChangeRepaymentRepaymentAccountForDepositManager : IChangeRepaymentAccountForDepositManager
    {
        private readonly IDbHandler _dbHandler;

        public ChangeRepaymentRepaymentAccountForDepositManager(IDbHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }

        public async Task ChangeRepaymentAccountForDepositAsync(ChangeRepaymentAccountForDepositRequest changeRepaymentAccountForDepositRequest,
            ChangeRepaymentAccountForDepositUseCaseCallBack changeRepaymentAccountForDepositUseCaseCallBack)
        {
            try
            {
                if (changeRepaymentAccountForDepositRequest.Deposit is FixedDepositBObj fixedDepositBObj)
                {
                    var fixedDeposit = new FixedDeposit
                    {
                        AccountNumber = fixedDepositBObj.AccountNumber,
                        IfscCode = fixedDepositBObj.IfscCode,
                        UserId = fixedDepositBObj.UserId,
                        CreatedOn = fixedDepositBObj.CreatedOn,
                        AccountStatus = fixedDepositBObj.AccountStatus,
                        DepositedAmount = fixedDepositBObj.DepositedAmount,
                        InterestRate = fixedDepositBObj.InterestRate,
                        Tenure = fixedDepositBObj.Tenure,
                        SavingsAccountId = changeRepaymentAccountForDepositRequest.AccountNumber,
                        FromAccountId = fixedDepositBObj.FromAccountId,
                    };
                    await _dbHandler.UpdateFixedDepositAsync(fixedDeposit);
                    NotificationEvents.FixedDepositUpdated?.Invoke(fixedDepositBObj);
                }
                else if(changeRepaymentAccountForDepositRequest.Deposit is RecurringAccountBObj recurringAccountBObj)
                {
                    var recurringDeposit = new RecurringAccount()
                    {
                        AccountNumber = recurringAccountBObj.AccountNumber,
                        IfscCode = recurringAccountBObj.IfscCode,
                        UserId = recurringAccountBObj.UserId,
                        CreatedOn = recurringAccountBObj.CreatedOn,
                        AccountStatus = recurringAccountBObj.AccountStatus,
                        DepositedAmount = recurringAccountBObj.DepositedAmount,
                        InterestRate = recurringAccountBObj.InterestRate,
                        Tenure = recurringAccountBObj.Tenure,
                        SavingsAccountId = changeRepaymentAccountForDepositRequest.AccountNumber,
                        FromAccountId = recurringAccountBObj.FromAccountId,
                    };
                    await _dbHandler.UpdateRecurringAccountAsync(recurringDeposit);
                    NotificationEvents.RecurringDepositUpdated?.Invoke(recurringAccountBObj);

                }
                changeRepaymentAccountForDepositUseCaseCallBack?.OnSuccess(new ChangeRepaymentAccountForDepositResponse(changeRepaymentAccountForDepositRequest.AccountNumber));
            }
            catch (Exception e)
            {
                changeRepaymentAccountForDepositUseCaseCallBack?.OnError(e);
            }
        }
    }
}