using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZBMSLibrary.Data.DataHandler;
using ZBMSLibrary.Data.DataHandler.Contract;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Entities.Model;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager
{
    public class GetInterestRateOfAllUniqueAccountsManager : IGetInterestRateOfAllUniqueAccountsManager
    {
        public GetInterestRateOfAllUniqueAccountsManager(IDbHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }

        private readonly IDbHandler _dbHandler;
        
        public async Task GetInterestRateOfAllUniqueAccounts(GetInterestRateOfAllUniqueAccountsRequest getInterestRateOfAllUniqueAccountsRequest,
            GetInterestRateOfAllUniqueAccountsUseCaseCallBack getInterestRateOfAllUniqueAccountsUseCaseCallBack)
        {
            try
            {
                var accountsToInterestMap = new Dictionary<string, double>();
                var savingsAccountInterestRate = await _dbHandler.GetSavingsAccountInterestRate();
                accountsToInterestMap.Add("Savings Account", savingsAccountInterestRate);
                accountsToInterestMap.Add("Current Account", 0);
                var loanAccountInterestRate = await _dbHandler.GetLoanAccountInterestRate();
                accountsToInterestMap.Add("Loan Account", loanAccountInterestRate);
                var recurringAccountInterestRate = await _dbHandler.GetRecurringDepositInterestRate();
                accountsToInterestMap.Add("Recurring Deposit", recurringAccountInterestRate);
                var fixedAccountInterestRate = await _dbHandler.GetFixedDepositInterestRate();
                accountsToInterestMap.Add("Fixed Deposit", fixedAccountInterestRate);

                getInterestRateOfAllUniqueAccountsUseCaseCallBack?.OnSuccess(new GetInterestRateOfAllUniqueAccountsResponse(accountsToInterestMap));

            }
            catch (Exception e)
            {
                getInterestRateOfAllUniqueAccountsUseCaseCallBack?.OnError(e);
            }
        }
    }
}