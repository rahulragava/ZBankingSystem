using System;
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
            SetLoanMetaData();
            AccountCreationView = accountCreationView;
        }

        public void SetAccounts(ObservableCollection<Account> accountList)
        {
            Accounts.Clear();
            AccountNames.Clear();
            SavingsAccountNames.Clear();
            foreach (var account in accountList)
            {
                Accounts.Add(account);
                AccountNames.Add(account.AccountNumber);

                if (account is SavingsAccountBObj savingsAccount)
                {
                    SavingsAccountNames.Add(savingsAccount.AccountNumber);
                }
            }
        }

        public double GetLoanedValue(double loanedValue)
        {
            LoanedValue = loanedValue;
            return Math.Round(LoanedValue,2);
        }

        public void SetLoanedValue(double loanedValue)
        {
            LoanedValue = loanedValue;
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

        public double GetDepositedValue(double depositedValue)
        {
            return Math.Round(DepositedValue, 2);
        }
        public void SetDepositedValue(double depositedValue)
        {
            DepositedValue = depositedValue;
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

        private double _depositedValue = 100.0;

        public double DepositedValue
        {
            get => _depositedValue;
            set => Set(ref _depositedValue, value);
        }

        private double _loanedValue;

        public double LoanedValue
        {
            get => _loanedValue;
            set => Set(ref _loanedValue, value);
        }

        private double _estimatedReturns;

        public double EstimatedReturns
        {
            get => _estimatedReturns;
            set => Set(ref _estimatedReturns, value);
        }

        private double _originalValuePlusInterestRate = 0.0;

        public double OriginalValuePlusInterestRate 
        {
            get => _originalValuePlusInterestRate;
            set => Set(ref _originalValuePlusInterestRate, value);
        }

        private double _emiValue = 0.0;

        public double EmiValue
        {
            get => _emiValue;
            set => Set(ref _emiValue, value);
        }

        private int _tenure = 1;

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

        public bool IsUserPan(string pan)
        {
            return PanNumber == pan;
        }
        public void CreateSavingsAccount(string ifsc,double balance)
        {
            var savingsAccount = new SavingsAccount()
            {
                AccountStatus = AccountStatus.Active,
                CreatedOn = DateTime.Now,
                FineAmount = 0,
                ToBeCreditedAmount = 0,
                NextCreditDateTime = DateTime.Now.AddDays(30),
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
                DepositedAmount = Math.Round(DepositedValue,2),
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
                DepositedAmount = Math.Round(DepositedValue,2),
                FromAccountId = fromAccountNumber,
                SavingsAccountId = repaymentAccountNumber,
                InterestRate = RecurringDepositInterestRate,
                MonthlyInstallment = Math.Round(DepositedValue, 2),
                NextDueDate = DateTime.Now.AddDays(30),
                Tenure = tenureValue,
            };
            var request = new CreateRecurringDepositRequest(recurringAccount);
            var usecase = new CreateRecurringDepositUseCase(request, new CreateRecurringDepositPresenterCallBack(this));
            usecase.Execute();
        }

        public void CreatePersonalLoanAccount(string ifsc, string loanedAmountGoesToAccountNumber, int tenureValue)
        {
            var personalLoan = new PersonalLoan()
            {
                AccountStatus = AccountStatus.Active,
                CreatedOn = DateTime.Now,
                IfscCode = ifsc,
                AccountNumber = Generator.GenerateAccountNumber(),
                UserId = AppSettings.CustomerId,
                InterestRate = PersonalLoanInterestRate,
                Tenure = tenureValue,
                FineAmount = 0,
                OriginalAmount = Math.Round(LoanedValue, 2),
                NextDateToBePaid = DateTime.Now.AddDays(30),
                DueWithInterestAmount = OriginalValuePlusInterestRate,
                Due = 0,
            };

            var request = new CreatePersonalLoanRequest(personalLoan,loanedAmountGoesToAccountNumber);
            var usecase = new CreatePersonalLoanUseCase(request, new CreatePersonalLoanAccountPresenterCallBack(this));
            usecase.Execute();
        }

        public void EstimatedReturnCalculationForFixedDeposit(double interestRate,double balance, int years)
        {
            var val = (4 * years);
            var interest = interestRate / 400;
            var estimatedValue = balance * (Math.Pow(1 + (interest), val));
            EstimatedReturns = Math.Round(estimatedValue, 2);
        }
        public void EstimatedReturnCalculationForRecurringDeposit(double balance, int years)
        {
            //A = P * (1 + R / N) ^ (Nt)

            var interest = RecurringDepositInterestRate / 400;
            var tenureMonths = years * 12;
            int quarters = tenureMonths / 3;
            var amount = 0.0;
            for (double i = 1.0; (int)i <= tenureMonths ; i++)
            {
                double monthlyInterestPlusAmount = Math.Pow(1 + interest, (4 * (i / tenureMonths)));
                amount += (balance*monthlyInterestPlusAmount);
                amount += (balance*monthlyInterestPlusAmount);
            }
            EstimatedReturns = Math.Round(amount, 2);
        }
        public void EstimatedReturnCalculationForPersonalLoan(double interestRate, double originalAmount, int years)
        {
            var rateOfInterest = interestRate / 12 / 100;
            var value = Math.Pow(1 + rateOfInterest, years * 12);
            var numerator = originalAmount * (rateOfInterest) * (value);
            var denominator = value - 1;
            EmiValue =  Math.Round((numerator / denominator),2);
            OriginalValuePlusInterestRate = Math.Round(EmiValue * (years * 12),2);
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

        private class CreateRecurringDepositPresenterCallBack : IPresenterCallBack<CreateRecurringDepositResponse>
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

        private class CreatePersonalLoanAccountPresenterCallBack : IPresenterCallBack<CreatePersonalLoanResponse>
        {
            private readonly AccountCreationViewModel _accountCreationViewModel;

            public CreatePersonalLoanAccountPresenterCallBack(AccountCreationViewModel accountCreationViewModel)
            {
                _accountCreationViewModel = accountCreationViewModel;
            }

            public void OnSuccess(CreatePersonalLoanResponse response)
            {
               // throw new NotImplementedException();
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

        private bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}