using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Windows.Gaming.Input;
using Windows.System;
using Microsoft.Extensions.DependencyInjection;
using ZBMSLibrary.Data.DataAdapter;
using ZBMSLibrary.Data.DataHandler.Contract;
using ZBMSLibrary.Data.Dependencies;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;
using User = ZBMSLibrary.Entities.Model.User;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
using SQLite;
using ZBMSLibrary.UseCase;
using System.Transactions;
using System.Collections.ObjectModel;
using ZBMS.Util;

namespace ZBMSLibrary.Data.DataHandler
{
    public class DbHandler : IDbHandler
    {
        private readonly IDatabaseAdapter _dbAdapter;

        public DbHandler(IDatabaseAdapter dbAdapter)
        {
            _dbAdapter = dbAdapter;
        }

        public async Task<IEnumerable<SavingsAccountBObj>> GetUserSavingsAccountsAsync(string userId)
        {
            var savingsAccounts = await _dbAdapter.Query<SavingsAccount>($"Select * From SavingsAccount where UserId = {userId}");
            var userName = (await _dbAdapter.GetObjectFromTableAsync<User>(userId).ConfigureAwait(false)).Name;
            var savingsAccountBObjList = new List<SavingsAccountBObj>();
            foreach (var savingsAccount in savingsAccounts)
            {
                var branchName = (await _dbAdapter.GetObjectFromTableAsync<Branch>(savingsAccount.IfscCode).ConfigureAwait(false)).Name;

                var transactions = new List<TransactionSummaryVObj>();


                var transactionAsSender = await _dbAdapter.Query<TransactionSummary>($"SELECT * FROM TransactionSummary WHERE SenderAccountNumber = '{savingsAccount.AccountNumber}';");
                foreach (var transactionSummary in transactionAsSender)
                {
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
                    transactions.Add(transactionSummaryVObj);
                }
                var transactionAsRecipient= await _dbAdapter.Query<TransactionSummary>($"SELECT * FROM TransactionSummary WHERE ReceiverAccountNumber = '{savingsAccount.AccountNumber}';");
                foreach (var transactionSummary in transactionAsRecipient)
                {
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
                    transactions.Add(transactionSummaryVObj);
                }
                //transactions.AddRange(transactionAsSender);
                //transactions.AddRange(transactionAsRecipient);
                OrderByUtil.OrderByDescending<TransactionSummaryVObj>(transactions);
                var savingsAccountBObj = new SavingsAccountBObj
                {
                    UserId = userId,
                    InterestRate = savingsAccount.InterestRate,
                    AccountStatus = savingsAccount.AccountStatus,
                    AccountNumber = savingsAccount.AccountNumber,
                    IfscCode = savingsAccount.IfscCode,
                    CreatedOn = savingsAccount.CreatedOn,
                    ToBeCreditedAmount = savingsAccount.ToBeCreditedAmount,
                    Balance = savingsAccount.Balance,
                    ServiceCharges = savingsAccount.ServiceCharges,
                    FineAmount = savingsAccount.FineAmount,
                    MinimumBalance = savingsAccount.MinimumBalance,
                    UserName = userName,
                    BranchName = branchName,
                    TransactionList = transactions,
                };
                savingsAccountBObjList.Add(savingsAccountBObj);
            }
            
            return savingsAccountBObjList;
        }
        
        public async Task<IEnumerable<CurrentAccountBObj>> GetUserCurrentAccountsAsync(string userId)
        {
            var currentAccountBObjList = new List<CurrentAccountBObj>();
            var userName = (await _dbAdapter.GetObjectFromTableAsync<User>(userId).ConfigureAwait(false)).Name;

            var currentAccounts =  await _dbAdapter.Query<CurrentAccount>($"Select * From CurrentAccount where UserId = {userId}");
            foreach (var currentAccount in currentAccounts)
            {
                var branchName = (await _dbAdapter.GetObjectFromTableAsync<Branch>(currentAccount.IfscCode).ConfigureAwait(false)).Name;

                var transactions = new List<TransactionSummaryVObj>();

                var transactionAsSender = await _dbAdapter.Query<TransactionSummary>($"SELECT * FROM TransactionSummary WHERE SenderAccountNumber = '{currentAccount.AccountNumber}';");
                foreach (var transactionSummary in transactionAsSender)
                {
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
                    transactions.Add(transactionSummaryVObj);
                }
                var transactionAsRecipient = await _dbAdapter.Query<TransactionSummary>($"SELECT * FROM TransactionSummary WHERE ReceiverAccountNumber = '{currentAccount.AccountNumber}';");
                foreach (var transactionSummary in transactionAsRecipient)
                {
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
                    transactions.Add(transactionSummaryVObj);
                }
                //transactions.AddRange(transactionAsSender);
                //transactions.AddRange(transactionAsRecipient);
                OrderByUtil.OrderByDescending<TransactionSummaryVObj>(transactions);

                var currentAccountBObj = new CurrentAccountBObj
                {
                    UserId = userId,
                    AccountStatus = currentAccount.AccountStatus,
                    AccountNumber = currentAccount.AccountNumber,
                    IfscCode = currentAccount.IfscCode,
                    CreatedOn = currentAccount.CreatedOn,
                    Balance = currentAccount.Balance,
                    ServiceCharges = currentAccount.ServiceCharges,
                    FineAmount = currentAccount.FineAmount,
                    MinimumBalance = currentAccount.MinimumBalance,
                    UserName = userName,
                    BranchName = branchName,
                    TransactionList = transactions,
                };
                currentAccountBObjList.Add(currentAccountBObj);
            }
            return currentAccountBObjList;
        }

        public async Task<IEnumerable<PersonalLoanBObj>> GetUserLoanAccountsAsync(string userId)
        {
            var loanAccounts =  await _dbAdapter.Query<PersonalLoan>($"Select * From PersonalLoan where UserId = {userId}");
            var userName = (await _dbAdapter.GetObjectFromTableAsync<User>(userId).ConfigureAwait(false)).Name;
            var loanAccountBObjList = new List<PersonalLoanBObj>();
            foreach (var loanAccount in loanAccounts)
            {
                var branchName = (await _dbAdapter.GetObjectFromTableAsync<Branch>(loanAccount.IfscCode).ConfigureAwait(false)).Name;

                var loanAccountBObj = new PersonalLoanBObj()
                {
                    UserId = userId,
                    AccountStatus = loanAccount.AccountStatus,
                    AccountNumber = loanAccount.AccountNumber,
                    IfscCode = loanAccount.IfscCode,
                    CreatedOn = loanAccount.CreatedOn,
                    Due = loanAccount.Due,
                    InterestRate = loanAccount.InterestRate,
                    FineAmount = loanAccount.FineAmount,
                    UserName = userName,
                    BranchName = branchName,
                    Tenure = loanAccount.Tenure,
                    DueWithInterestAmount = loanAccount.DueWithInterestAmount,
                    OriginalAmount = loanAccount.OriginalAmount,
                    NextDateToBePaid = loanAccount.NextDateToBePaid,
                };
                loanAccountBObjList.Add(loanAccountBObj);
            }
            return loanAccountBObjList;
        }

        public async Task<IEnumerable<FixedDepositBObj>> GetUserFixedDepositsAsync(string userId)
        {
            var userFixedDepositAccounts = await _dbAdapter.Query<FixedDeposit>($"Select * From FixedDeposit where UserId = {userId}");
            var userName = (await _dbAdapter.GetObjectFromTableAsync<User>(userId).ConfigureAwait(false)).Name;
            var userFixedDepositAccountsBObj = new List<FixedDepositBObj>();
            foreach (var fixedDeposit in userFixedDepositAccounts)
            {
                var branchName = (await _dbAdapter.GetObjectFromTableAsync<Branch>(fixedDeposit.IfscCode).ConfigureAwait(false)).Name;

                var fixedDepositBObj = new FixedDepositBObj()
                {
                    InterestRate = fixedDeposit.InterestRate,
                    Tenure = fixedDeposit.Tenure,
                    SavingsAccountId = fixedDeposit.SavingsAccountId,
                    AccountStatus = fixedDeposit.AccountStatus,
                    AccountNumber = fixedDeposit.AccountNumber,
                    IfscCode = fixedDeposit.IfscCode,
                    UserId = fixedDeposit.UserId,
                    CreatedOn = fixedDeposit.CreatedOn,
                    FromAccountId = fixedDeposit.FromAccountId,
                    DepositedAmount = fixedDeposit.DepositedAmount,
                    UserName = userName,
                    BranchName = branchName
                };
                fixedDepositBObj.SetDefault();

                userFixedDepositAccountsBObj.Add(fixedDepositBObj);
            }
            return userFixedDepositAccountsBObj;
        }

        public async Task<IEnumerable<RecurringAccountBObj>> GetUserRecurringDepositsAsync(string userId)
        {
            var userRecurringAccount = await _dbAdapter.Query<RecurringAccount>($"Select * From RecurringAccount where UserId = {userId}");
            var userName = (await _dbAdapter.GetObjectFromTableAsync<User>(userId).ConfigureAwait(false)).Name;
            var recurringAccounts = new List<RecurringAccountBObj>();
            foreach (var recurringAccount in userRecurringAccount)
            {
                var branchName = (await _dbAdapter.GetObjectFromTableAsync<Branch>(recurringAccount.IfscCode).ConfigureAwait(false)).Name;
                var recurringAccountBObj = new RecurringAccountBObj()
                {
                    InterestRate = recurringAccount.InterestRate,
                    Tenure = recurringAccount.Tenure,
                    SavingsAccountId = recurringAccount.SavingsAccountId,
                    AccountStatus = recurringAccount.AccountStatus,
                    AccountNumber = recurringAccount.AccountNumber,
                    IfscCode = recurringAccount.IfscCode,
                    UserId = recurringAccount.UserId,
                    CreatedOn = recurringAccount.CreatedOn,
                    FromAccountId = recurringAccount.FromAccountId,
                    Frequency = recurringAccount.Frequency,
                    DepositedAmount = recurringAccount.DepositedAmount,
                    UserName = userName,
                    MonthlyInstallment = recurringAccount.MonthlyInstallment,
                    LastPaidDate = recurringAccount.LastPaidDate,
                    BranchName = branchName
                };
                recurringAccountBObj.SetDefault();
                recurringAccounts.Add(recurringAccountBObj);
            }
            return recurringAccounts;
        }


        public async Task UpdateRecurringAccountAsync(RecurringAccount recurringAccount)
        {
            await _dbAdapter.UpdateObjectInTableAsync<RecurringAccount>(recurringAccount);
        }
        public async Task UpdateFixedDepositAsync(FixedDeposit fixedDeposit)
        {
            await _dbAdapter.UpdateObjectInTableAsync<FixedDeposit>(fixedDeposit);
        }

        public async Task UpdatePersonalLoanAsync(PersonalLoan personalLoan)
        {
            await _dbAdapter.UpdateObjectInTableAsync<PersonalLoan>(personalLoan);
        }

        public async Task<double> GetSavingsAccountInterestRate()
        {
            var savingsAccount =
                await _dbAdapter.Query<SavingsAccount>($"SELECT InterestRate FROM SavingsAccount LIMIT 1");
            var first =  savingsAccount.First(x => true);
            return first?.InterestRate ?? 0;
        }


        public async Task UpdateSavingsAccountAsync(SavingsAccount savingsAccount)
        {
            await _dbAdapter.UpdateObjectInTableAsync<SavingsAccount>(savingsAccount);
        }
        public async Task UpdateCurrentAccountAsync(CurrentAccount currentAccount)
        {
            await _dbAdapter.UpdateObjectInTableAsync<CurrentAccount>(currentAccount);
        }

        //must change 
        public async Task<double> GetFixedDepositInterestRate()
        {
            var fixedDeposit =
                await _dbAdapter.Query<FixedDeposit>($"SELECT InterestRate FROM FixedDeposit LIMIT 1");
            var first = fixedDeposit.First(x => true);
            return first?.InterestRate ?? 0;

        }
        public async Task<double> GetRecurringDepositInterestRate()
        {
            var recurringAccounts =
                await _dbAdapter.Query<RecurringAccount>($"SELECT InterestRate FROM RecurringAccount LIMIT 1");
            var first = recurringAccounts.First(x => true);
            return first?.InterestRate ?? 0;


        }
        public async Task<double> GetLoanAccountInterestRate()
        {
            var loanAccounts =
                await _dbAdapter.Query<PersonalLoan>($"SELECT InterestRate FROM PersonalLoan LIMIT 1");
            PersonalLoan first = loanAccounts.FirstOrDefault(x => true);

            return first?.InterestRate ?? 0;

        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _dbAdapter.GetObjectFromTableAsync<User>(userId);
            return user.Name;
        }

        public async Task<string> GetBranchNameAsync(string branchId)
        {
            var branch = await _dbAdapter.GetObjectFromTableAsync<Branch>(branchId);
            return branch.Name;
        }

        public async Task CreateCurrentAccountAsync(CurrentAccount currentAccount)
        {
            await _dbAdapter.InsertInTableAsync(currentAccount);
        }

        public async Task CreateSavingsAccountAsync(SavingsAccount savingsAccount)
        {
            await _dbAdapter.InsertInTableAsync(savingsAccount);
        }

        public async Task CreateFixedDepositAsync(FixedDeposit fixedDeposit)
        {
            //async Task CreateFixedDeposit()
            //{
            //    //try
            //    //{
            //    //    var account = await _dbAdapter.GetObjectFromTableAsync<SavingsAccount>(fixedDeposit.FromAccountId);
            //    //    //account = await _dbAdapter.GetObjectFromTableAsync<SavingsAccount>(fixedDeposit.AccountNumber);
            //    //    account.Balance -= fixedDeposit.DepositedAmount;
            //    //    await _dbAdapter.UpdateObjectInTableAsync(account);
            //    //}
            //    //catch (Exception ex)
            //    //{
            //    //    var currentAccount =
            //    //        await _dbAdapter.GetObjectFromTableAsync<CurrentAccount>(fixedDeposit.FromAccountId);
            //    //    currentAccount.Balance -= fixedDeposit.DepositedAmount;
            //    //    await _dbAdapter.UpdateObjectInTableAsync(currentAccount);
            //    //}
            //    //finally
            //    //{
            //    //    await _dbAdapter.InsertInTableAsync(fixedDeposit);
            //    //}
            //}
            await _dbAdapter.InsertInTableAsync((fixedDeposit));
            //await _dbAdapter.RunInTransactionAsync(CreateFixedDeposit);
        }

        public async Task<SavingsAccount> GetSavingsAccountAsync(string id)
        {
            //return await _dbAdapter.QueryFetchObject<SavingsAccount>($"SELECT * FROM SavingsAccount Where AccountNumber={id}");
            return await _dbAdapter.GetObjectFromTableAsync<SavingsAccount>(id);
        }

        public async Task<PersonalLoan> GetPersonalLoanAccountAsync(string id)
        {
            return await _dbAdapter.GetObjectFromTableAsync<PersonalLoan>(id);
        }

        public async Task<DateTime> GetUserLastLoggedAsync(string userId)
        {
            //return await _dbAdapter.QueryFetchObject<User>($"SELECT LastLoggedOn FROM User Where Id={userId}");
            var user = await _dbAdapter.GetObjectFromTableAsync<User>(userId);
            return user.LastLoggedOn;
        }

        public async Task UpdateUserLoggedInAsync(User user)
        {
            await _dbAdapter.UpdateObjectInTableAsync(user);
        }

        public async Task<User> GetUserAsync(string userId)
        {
            return await _dbAdapter.GetObjectFromTableAsync<User>(userId);
        }

        public async Task InsertTransactionAsync(TransactionSummary transactionSummary)
        { 
            await _dbAdapter.InsertInTableAsync(transactionSummary);
        }

        public async Task<CurrentAccount> GetCurrentAccountAsync(string id)
        {
            //return await _dbAdapter.QueryFetchObject<CurrentAccount>($"SELECT * FROM CurrentAccount Where AccountNumber={id}");

            return await _dbAdapter.GetObjectFromTableAsync<CurrentAccount>(id);
        }
        //public async Task UpdateSavingsAccountAsync(SavingsAccount savingsAccount)
        //{
        //    await _dbAdapter.UpdateObjectInTableAsync<SavingsAccount>(savingsAccount);
        //}
        //public async Task UpdateCurrentAccountAsync(CurrentAccount currentAccount)
        //{
        //    await _dbAdapter.UpdateObjectInTableAsync<SavingsAccount>(currentAccount);
        //}
        public async Task CreateRecurringDepositAsync(RecurringAccount recurringAccount)
        {
            //async Task CreateRecurringDeposit()
            //{
            //    try
            //    {
            //        var account = await _dbAdapter.GetObjectFromTableAsync<SavingsAccount>(recurringAccount.AccountNumber);
            //        //account = await _dbAdapter.GetObjectFromTableAsync<SavingsAccount>(recurringAccount.AccountNumber);
            //        account.Balance -= recurringAccount.MonthlyInstallment;
            //        await _dbAdapter.UpdateObjectInTableAsync(account);
            //    }
            //    catch (SQLiteException ex)
            //    {
            //        var currentAccount =
            //            await _dbAdapter.GetObjectFromTableAsync<CurrentAccount>(recurringAccount.AccountNumber);
            //        currentAccount.Balance -= recurringAccount.MonthlyInstallment;
            //        await _dbAdapter.UpdateObjectInTableAsync(currentAccount);
            //    }
            //    finally
            //    {
            //    }
            //}
            await _dbAdapter.InsertInTableAsync(recurringAccount);

            //await _dbAdapter.RunInTransactionAsync(CreateRecurringDeposit);
        }

        public async Task CreatePersonalLoanAsync(PersonalLoan personalLoan)
        {
            await _dbAdapter.InsertInTableAsync(personalLoan);
        }

        public async Task<IEnumerable<Branch>> GetAllBranchesAsync()
        {
            return await _dbAdapter.GetAllObjectsInTableAsync<Branch>();
        }

    }
}