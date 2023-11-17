using System.Threading.Tasks;
using System;
using ZBMSLibrary.Data.DataHandler.Contract;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager
{
    public class ChangeSenderAccountDepositManager : IChangeSenderAccountDepositManager
    {
        private readonly IDbHandler _dbHandler;

        public ChangeSenderAccountDepositManager(IDbHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }

        public async Task ChangeSenderAccountDepositAsync(ChangeSenderAccountDepositRequest changeSenderAccountDepositRequest, ChangeSenderAccountDepositUseCaseCallBack changeSenderAccountDepositUseCaseCallBack)
        {
            try
            {
                if (changeSenderAccountDepositRequest.Deposit is FixedDepositBObj fixedDepositBObj)
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
                        SavingsAccountId = fixedDepositBObj.SavingsAccountId,
                        FromAccountId = changeSenderAccountDepositRequest.AccountNumber,
                    };
                    await _dbHandler.UpdateFixedDepositAsync(fixedDeposit);
                }
                else if (changeSenderAccountDepositRequest.Deposit is RecurringAccountBObj recurringAccountBObj)
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
                        SavingsAccountId = recurringAccountBObj.SavingsAccountId,
                        FromAccountId = changeSenderAccountDepositRequest.AccountNumber,
                    };
                    await _dbHandler.UpdateRecurringAccountAsync(recurringDeposit);
                }

                //NotificationEvents.UpdateSenderDepositDetail?.Invoke(changeSenderAccountDepositRequest.AccountNumber);
                changeSenderAccountDepositUseCaseCallBack?.OnSuccess(new ChangeSenderAccountDepositResponse(changeSenderAccountDepositRequest.AccountNumber));
            }
            catch (Exception e)
            {
                changeSenderAccountDepositUseCaseCallBack?.OnError(e);
            }
        }
    }
}