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
    public sealed class GetUserAccountsManager : IGetUserAccountsManager
    {
        public GetUserAccountsManager(IDbHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }

        private readonly IDbHandler _dbHandler;
        public async Task GetUserAccountsAsync(GetUserAccountsRequest getUserAccountsRequest,
            GetUserAccountsUseCaseCallBack getUserAccountsUseCaseCallBack)
        {
            try
            {
                var accounts = new List<Account>();
                var deposits = new List<Deposit>();
                var loans = new List<Loan>();
                 var savingsAccounts = await _dbHandler.GetUserSavingsAccountsAsync(getUserAccountsRequest.UserId)
                    .ConfigureAwait(false);
                var currentAccounts = await _dbHandler.GetUserCurrentAccountsAsync(getUserAccountsRequest.UserId)
                    .ConfigureAwait(false);
                var recurringAccounts = await _dbHandler
                    .GetUserRecurringDepositsAsync(getUserAccountsRequest.UserId).ConfigureAwait(false);
                var fixedDepositAccounts = await _dbHandler
                    .GetUserFixedDepositsAsync(getUserAccountsRequest.UserId).ConfigureAwait(false);
                var loanAccounts = await _dbHandler.GetUserLoanAccountsAsync(getUserAccountsRequest.UserId)
                    .ConfigureAwait(false);

                accounts.AddRange(savingsAccounts);
                accounts.AddRange(currentAccounts);
                deposits.AddRange(fixedDepositAccounts);
                deposits.AddRange(recurringAccounts);
                loans.AddRange(loanAccounts);

                getUserAccountsUseCaseCallBack?.OnSuccess(new GetUserAccountsResponse(accounts,deposits,loans));
            }
            catch (Exception e)
            {
                getUserAccountsUseCaseCallBack?.OnError(e);

            }
        }
    }
}