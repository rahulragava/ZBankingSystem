using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;

namespace ZBMSLibrary.Data.DataHandler.Contract
{
    public interface IDbHandler
    {
        Task<IEnumerable<Branch>> GetAllBranchesAsync();
        Task<IEnumerable<SavingsAccountBObj>> GetUserSavingsAccountsAsync(string userId);
        Task<IEnumerable<CurrentAccountBObj>> GetUserCurrentAccountsAsync(string userId);
        Task<IEnumerable<PersonalLoanBObj>> GetUserLoanAccountsAsync(string userId);
        Task<IEnumerable<FixedDepositBObj>> GetUserFixedDepositsAsync(string userId);
        Task<IEnumerable<RecurringAccountBObj>> GetUserRecurringDepositsAsync(string userId);
        Task UpdateSavingsAccountAsync(SavingsAccount savingsAccount);
        Task UpdateCurrentAccountAsync(CurrentAccount currentAccount);
        Task UpdateRecurringAccountAsync(RecurringAccount recurringAccount);
        Task UpdateFixedDepositAsync(FixedDeposit fixedDeposit);
        Task UpdatePersonalLoanAsync(PersonalLoan personalLoan);
        Task<double> GetSavingsAccountInterestRate();
        //Task<double> GetCurrentAccountInterestRate();
        Task<double> GetFixedDepositInterestRate();
        Task<double> GetRecurringDepositInterestRate();
        Task<double> GetLoanAccountInterestRate();
        Task<string> GetUserNameAsync(string userId);
        Task<string> GetBranchNameAsync(string branchId);
        Task CreateCurrentAccountAsync(CurrentAccount currentAccount);
        Task CreateSavingsAccountAsync(SavingsAccount savingsAccount);
        Task CreateFixedDepositAsync(FixedDeposit fixedDeposit);
        Task CreateRecurringDepositAsync(RecurringAccount recurringAccount);
        Task CreatePersonalLoanAsync(PersonalLoan personalLoan);
        Task<CurrentAccount> GetCurrentAccountAsync(string id);
        Task<SavingsAccount> GetSavingsAccountAsync(string id);
        Task<PersonalLoan> GetPersonalLoanAccountAsync(string id);
        Task<DateTime> GetUserLastLoggedAsync(string userId);
        Task UpdateUserLoggedInAsync(User user);
        Task<User> GetUserAsync(string userId);
        Task InsertTransactionAsync(TransactionSummary transactionSummary);
    }
}