﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Windows.Storage.Pickers.Provider;
using Windows.UI.Core;
using Microsoft.Toolkit.Collections;
using ZBMS.Services;
using ZBMS.View.UserControl;
using ZBMSLibrary.Data;
using ZBMSLibrary.Entities.Model;
using ZBMSLibrary.UseCase;
using ZBMS.Util;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Enums;
using ZBMSLibrary.Util;

namespace ZBMS.ViewModel
{
    public class AccountCreationViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Account> Accounts;
        public ObservableCollection<Deposit> Deposits;
        public ObservableCollection<string> BranchNames;
        public ObservableCollection<string> SavingsAccountNames;
        public ObservableCollection<string> AccountNames;
        public ObservableCollection<Branch> Branches;
        //public ObservableDictionary<string, double> AccountToInterestRateMap; 
        public IAccountCreationView AccountCreationView;

        public AccountCreationViewModel(IAccountCreationView accountCreationView)
        {
            Accounts = new ObservableCollection<Account>();
            SavingsAccountNames = new ObservableCollection<string>();
            AccountNames = new ObservableCollection<string>();
            Deposits = new ObservableCollection<Deposit>();    
            BranchNames = new ObservableCollection<string>();
            Branches = new ObservableCollection<Branch>();
            //AccountToInterestRateMap = new ObservableDictionary<string,double>();
            SetAccountMetaData();
            SetDepositMetaData();
            AccountCreationView = accountCreationView;
        }

        public void SetAccounts(ObservableCollection<Account> accountList)
        {
            Accounts.Clear();
            foreach (var account in accountList)
            {
                Accounts.Add(account);
                AccountNames.Add(account.AccountNumber);

                if (account is SavingsAccountBObj)
                {
                    SavingsAccountNames.Add((account as SavingsAccountBObj).AccountNumber);
                }
            }
        }

        public void SetAccountMetaData()
        {
            var savingsAccount = new SavingsAccount();
            var currentAccount = new CurrentAccount();
            SavingsAccountInterestRate = savingsAccount.InterestRate;
            CurrentAccountInterestRate = 0;
            SavingsAccountMinimumBalance = savingsAccount.MinimumBalance;
            CurrentAccountMinimumBalance = currentAccount.MinimumBalance;
            SavingsAccountServiceCharge = savingsAccount.ServiceCharges;
            CurrentAccountServiceCharge = currentAccount.ServiceCharges;
        }

        public void SetDepositMetaData()
        {
            var fixedDeposit = new FixedDeposit();
            var recurringDeposit = new RecurringAccount();
            FixedDepositInterestRate = fixedDeposit.InterestRate;
            RecurringDepositInterestRate = recurringDeposit.InterestRate;
        }

        public void SetLoanMetaData()
        {
            var personalLoan = new PersonalLoan();
            PersonalLoanInterestRate = personalLoan.InterestRate;
        }
        private string _panNumber;

        public string PanNumber
        {
            get => _panNumber;
            set => Set(ref _panNumber, value);
        }

        private double _savingsAccountInterestRate;

        public double SavingsAccountInterestRate
        {
            get => _savingsAccountInterestRate;
            set => Set(ref _savingsAccountInterestRate, value);
        }

        private double _savingsAccountMinimumBalance;

        public double SavingsAccountMinimumBalance
        {
            get => _savingsAccountMinimumBalance;
            set => Set(ref _savingsAccountMinimumBalance, value);
        }

        private double _savingsAccountServiceCharge;
        public double SavingsAccountServiceCharge
        {
            get => _savingsAccountServiceCharge;
            set => Set(ref _savingsAccountServiceCharge, value);
        }

        private double _currentAccountInterestRate;

        public double CurrentAccountInterestRate
        {
            get => _currentAccountInterestRate;
            set => Set(ref _currentAccountInterestRate, value);
        }

        private double _currentAccountMinimumBalance;

        public double CurrentAccountMinimumBalance
        {
            get => _currentAccountMinimumBalance;
            set => Set(ref _currentAccountMinimumBalance, value);
        }
        private double _currentAccountServiceCharge;

        public double CurrentAccountServiceCharge
        {
            get => _currentAccountServiceCharge;
            set => Set(ref _currentAccountServiceCharge, value);
        }

        private double _fixedDepositInterestRate;

        public double FixedDepositInterestRate
        {
            get => _fixedDepositInterestRate;
            set => Set(ref _fixedDepositInterestRate, value);
        }

        private double _recurringDepositInterestRate;

        public double RecurringDepositInterestRate
        {
            get => _recurringDepositInterestRate;
            set => Set(ref _recurringDepositInterestRate, value);
        }

        private double _personalLoanInterestRate;

        public double PersonalLoanInterestRate
        {
            get => _personalLoanInterestRate;
            set => Set(ref _personalLoanInterestRate, value);
        }

        private double _depositedValue;

        public double DepositedValue
        {
            get => _depositedValue;
            set => Set(ref _depositedValue, value);
        }

        private double _estimatedReturns;

        public double EstimatedReturns
        {
            get => _estimatedReturns;
            set => Set(ref _estimatedReturns, value);
        }

        private int _tenure;

        public int Tenure
        {
            get => _tenure;
            set => Set(ref _tenure, value);
        }

        public bool IsValidPan()
        {
            string panValidation = "^[A-Z]{5}[0-9]{4}[A-Z]{1}$";
            Regex r = new Regex(panValidation, RegexOptions.Compiled);
            return r.IsMatch(PanNumber);
            
        }

        public void CreateSavingsAccount(string ifsc,double balance)
        {
            var savingsAccount = new SavingsAccount()
            {
                AccountStatus = AccountStatus.Active,
                CreatedOn = DateTime.Now,
                FineAmount = 0,
                ToBeCreditedAmount = 0,
                IfscCode = ifsc,
                AccountNumber = Generator.GenerateAccountNumber(),
                UserId = AppSettings.CustomerId,
                Balance = balance,
            };
            var request = new CreateSavingsAccountRequest(savingsAccount);
            var useCase = new CreateSavingsAccountUseCase(request, new CreateSavingsAccountPresenterCallBack(this));
            useCase.Execute();
        }

        public void CreateCurrentAccount(string ifsc,double balance)
        {
            var currentAccount = new CurrentAccount()
            {
                AccountStatus = AccountStatus.Active,
                CreatedOn = DateTime.Now,
                FineAmount = 0,
                IfscCode = ifsc,
                AccountNumber = Generator.GenerateAccountNumber(),
                UserId = AppSettings.CustomerId,
                Balance = balance
            };
            var request = new CreateCurrentAccountRequest(currentAccount);
            var useCase = new CreateCurrentAccountUseCase(request,new CreateCurrentAccountPresenterCallBack(this));
            useCase.Execute();
        }

        public void CreateFixedDepositAccount(string ifsc, string fromAccountNumber, string repaymentAccountNumber,int tenureValue)
        {
            var fixedDeposit = new FixedDeposit()
            {
                AccountStatus = AccountStatus.Active,
                CreatedOn = DateTime.Now,
                IfscCode = ifsc,
                AccountNumber = Generator.GenerateAccountNumber(),
                UserId = AppSettings.CustomerId,
                DepositedAmount = DepositedValue,
                FromAccountId = fromAccountNumber,
                SavingsAccountId = repaymentAccountNumber,
                InterestRate = FixedDepositInterestRate,
                Tenure = tenureValue
            };

            var request = new CreateFixedDepositRequest(fixedDeposit);
            var usecase = new CreateFixedDepositUseCase(request, new CreateFixedDepositPresenterCallBack(this));
            usecase.Execute();
        }
        public void CreateRecurringDepositAccount(string ifsc, string fromAccountNumber, string repaymentAccountNumber,int tenureValue)
        {
            var recurringAccount = new RecurringAccount()
            {
                AccountStatus = AccountStatus.Active,
                CreatedOn = DateTime.Now,
                IfscCode = ifsc,
                AccountNumber = Generator.GenerateAccountNumber(),
                UserId = AppSettings.CustomerId,
                DepositedAmount = DepositedValue,
                FromAccountId = fromAccountNumber,
                SavingsAccountId = repaymentAccountNumber,
                InterestRate = RecurringDepositInterestRate,
                MonthlyInstallment = DepositedValue,
                LastPaidDate = DateTime.Now,
                Tenure = tenureValue,
            };
            var request = new CreateRecurringDepositRequest(recurringAccount);
            var usecase = new CreateRecurringDepositUseCase(request, new CreateRecurringDepositPresenterCallBack(this));
            usecase.Execute();
        }

        public void EstimatedReturnCalculationForFixedDeposit(double interestRate,double balance, int years)
        {
            var val = (4 * years);
            var interest = interestRate / 400;
            var estimatedValue = balance * (Math.Pow(1 + (interest), val));
            EstimatedReturns = Math.Round(estimatedValue, 2);
        }
        public void EstimatedReturnCalculationForRecurringDeposit(double interestRate, double balance, int years)
        {
            //A = P * (1 + R / N) ^ (Nt)

            var interest = interestRate / 400;
            var tenureMonths = years * 12;
            int quarters = tenureMonths / 3;
            var amount = 0.0;
            for (double i = 1.0; (int)i <= tenureMonths ; i++)
            {
                double monthlyInterestPlusAmount = Math.Pow(1 + interest, (4 * (i / tenureMonths)));
                amount += (balance*monthlyInterestPlusAmount);
                amount += (balance*monthlyInterestPlusAmount);
            }
            //M = P * [(1 + r) ^ n - 1] / (1 - (1 + r) ^ (-1 / 3))

            //var numerator = (Math.Pow(1 + interest, quarters - 1));
            //var denominator = 1 - 1 / (Math.Pow((1 + interest), (1.0 / 3)));
            ////var numerator = (1 + interest) * (quarters - 1);
            ////var denominator = 1 - ((1 + interest)*(-1.0/3));
            //var estimatedValue = balance * numerator / denominator;
            ////var estimatedValue = (balance * (Math.Pow(1 + (interest), quarters-1)))/denominator;
            EstimatedReturns = Math.Round(amount, 2);
        }

        public void GetAllBranches()
        {
            var getAllBranchesUseCase =
                new GetAllBranchesUseCase(new GetAllBranchesRequest(), new GetAllBranchesPresenterCallBack(this));
            getAllBranchesUseCase.Execute();
        }


        private void OnSuccessfullyRetrievedBranches(IEnumerable<Branch> responseBranches)
        {
            foreach (var branch in responseBranches)
            {
                //Branches.Add(branch);
                Branches.Add(branch);
                BranchNames.Add(branch.Name);
            }
        }

        public class CreateSavingsAccountPresenterCallBack : IPresenterCallBack<CreateSavingsAccountResponse>
        {
            private readonly AccountCreationViewModel _accountCreationViewModel;

            public CreateSavingsAccountPresenterCallBack(AccountCreationViewModel accountCreationViewModel)
            {
                _accountCreationViewModel = accountCreationViewModel;
            }

            public void OnSuccess(CreateSavingsAccountResponse response)
            {
                _accountCreationViewModel.PanNumber = string.Empty;
            }

            public void OnError(Exception ex)
            {
            }
        }

        public class CreateCurrentAccountPresenterCallBack : IPresenterCallBack<CreateCurrentAccountResponse>
        {
            private readonly AccountCreationViewModel _accountCreationViewModel;

            public CreateCurrentAccountPresenterCallBack(AccountCreationViewModel accountCreationViewModel)
            {
                _accountCreationViewModel = accountCreationViewModel;
            }

            public void OnSuccess(CreateCurrentAccountResponse response)
            {
                
            }

            public void OnError(Exception ex)
            {
            }
        }

        public class CreateFixedDepositPresenterCallBack : IPresenterCallBack<CreateFixedDepositResponse>
        {
            private readonly AccountCreationViewModel _accountCreationViewModel;

            public CreateFixedDepositPresenterCallBack(AccountCreationViewModel accountCreationViewModel)
            {
                _accountCreationViewModel = accountCreationViewModel;
            }


            public void OnSuccess(CreateFixedDepositResponse response)
            {

            }

            public void OnError(Exception ex)
            {
            }
        }
        public class CreateRecurringDepositPresenterCallBack : IPresenterCallBack<CreateRecurringDepositResponse>
        {
            private readonly AccountCreationViewModel _accountCreationViewModel;

            public CreateRecurringDepositPresenterCallBack(AccountCreationViewModel accountCreationViewModel)
            {
                _accountCreationViewModel = accountCreationViewModel;
            }


            public void OnSuccess(CreateRecurringDepositResponse response)
            {
            }

            public void OnError(Exception ex)
            {
            }
        }

        public class GetAllBranchesPresenterCallBack : IPresenterCallBack<GetAllBranchesResponse>
        {
            private readonly AccountCreationViewModel _accountCreationViewModel;
            public GetAllBranchesPresenterCallBack(AccountCreationViewModel accountCreationViewModel)
            {
                _accountCreationViewModel = accountCreationViewModel;
            }
            public void OnSuccess(GetAllBranchesResponse response)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        //_viewModel.OnSuccessfullyRetrievedUserAccount(response.Accounts);
                        _accountCreationViewModel.OnSuccessfullyRetrievedBranches(response.Branches);
                    }
                );
            }

            public void OnError(Exception ex)
            {
                //throw new NotImplementedException();
            }
        }

        public class GetInterestRateOfAllUniqueAccountsPresenterCallBack : IPresenterCallBack<GetInterestRateOfAllUniqueAccountsResponse>
        {
            //private readonly AccountCreationViewModel _accountCreationViewModel;
            //public GetInterestRateOfAllUniqueAccountsPresenterCallBack(AccountCreationViewModel accountCreationViewModel)
            //{
            //    _accountCreationViewModel = accountCreationViewModel;
            //}

            //public void OnSuccess(GetInterestRateOfAllUniqueAccountsResponse response)
            //{
            //    Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            //        () =>
            //        {
            //            _accountCreationViewModel.OnSuccessfullyRetrievedInterestAccountMap(response.AccountToInterestRateMap);
            //        }
            //    );
            //}

            //public void OnError(Exception ex)
            //{
            //    //throw new NotImplementedException();
            //}
            public void OnSuccess(GetInterestRateOfAllUniqueAccountsResponse response)
            {
                //throw new NotImplementedException();
            }

            public void OnError(Exception ex)
            {
                //throw new NotImplementedException();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}