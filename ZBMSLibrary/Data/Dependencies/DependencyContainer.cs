using Microsoft.Extensions.DependencyInjection;
using ZBMSLibrary.Data.DataAdapter;
using ZBMSLibrary.Data.DataHandler;
using ZBMSLibrary.Data.DataHandler.Contract;
using ZBMSLibrary.Data.DataManager;
using ZBMSLibrary.Data.DataManager.Contract;

namespace ZBMSLibrary.Data.Dependencies
{
    static class DependencyContainer
    {
        static DependencyContainer()
        {
            DiContainer = new ServiceCollection()
                .AddSingleton<IDatabaseAdapter, DbAdapter>()
                .AddSingleton<IDbHandler, DbHandler>()
                .AddSingleton<IGetAllBranchesManager,GetAllBranchesManager>()
                .AddSingleton<IGetUserAccountsManager,GetUserAccountsManager>()
                .AddSingleton<IWithdrawMoneyFromAccount,WithdrawMoneyFromAccountManager>()
                .AddSingleton<IDepositMoneyToAccountManager,DepositMoneyToAccountManagerManager>()
                .AddSingleton<ICreateCurrentAccountManager,CreateCurrentAccountManager>()
                .AddSingleton<ICreateSavingsAccountManager,CreateSavingsAccountManager>()
                .AddSingleton<ICreateFixedDepositManager, CreateFixedDepositManager>()
                .AddSingleton<IGetUserLastSeenManager, GetUserLastSeenManager>()
                .AddSingleton<IDeduceMonthlyInstallmentManager,DeduceMonthlyInstallmentManager>()
                .AddSingleton<IDepositSettlementManager,DepositSettlementManager>()
                .AddSingleton<ICreateRecurringDepositManager, CreateRecurringDepositManager>()
                .AddSingleton<IUpdateUserLoggedInManager, UpdateUserLoggedInManager>()
                .AddSingleton<IGetInterestRateOfAllUniqueAccountsManager,GetInterestRateOfAllUniqueAccountsManager>()
                .BuildServiceProvider();
        }

        public static ServiceProvider DiContainer;
    }
}