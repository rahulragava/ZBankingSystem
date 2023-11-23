using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Enums;
using ZBMSLibrary.Entities.Model;

namespace ZBMS.ViewModel
{
    public class ConsolidatedReportViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Account> Accounts;
        public ObservableCollection<Deposit> Deposits;
        public ObservableCollection<Loan> Loans;

        public ConsolidatedReportViewModel()
        {
            Accounts = new ObservableCollection<Account>();
            Deposits = new ObservableCollection<Deposit>();
            Loans = new ObservableCollection<Loan>();
        }


        private double _netSavings;

        public double NetSavings
        {
            get => _netSavings;
            set => SetField(ref _netSavings, value);
        }

        private double _netCurrentAccount;

        public double NetCurrentAccount
        {
            get => _netCurrentAccount;
            set => SetField(ref _netCurrentAccount, value);
        }

        private double _netBalance;

        public double NetBalance
        {
            get => _netBalance;
            set => SetField(ref _netBalance, value);
        }
        private double _totalDue;

        public double TotalDue
        {
            get => _totalDue;
            set => SetField(ref _totalDue, value);
        }
        private double _netDeposit;

        public double NetDeposit
        {
            get => _netDeposit;
            set => SetField(ref _netDeposit, value);
        }

        private int _totalActiveAccounts;

        public int TotalActiveAccounts
        {
            get => _totalActiveAccounts;
            set => SetField(ref _totalActiveAccounts, value);
        }

        private int _totalActiveDeposits;

        public int TotalActiveDeposits
        {
            get => _totalActiveDeposits;
            set => SetField(ref _totalActiveDeposits, value);
        }

        private int _totalClosedAccounts;

        public int TotalClosedAccounts
        {
            get => _totalClosedAccounts; 
            set => SetField(ref _totalClosedAccounts, value);
        }

        private int _totalClosedDeposits;

        public int TotalClosedDeposits
        {
            get => _totalClosedDeposits;
            set => SetField(ref _totalClosedDeposits, value);
        }

        private int _totalActiveLoans;

        public int TotalActiveLoans
        {
            get => _totalActiveLoans;
            set => SetField(ref _totalActiveLoans, value);
        }

        private int _totalClosedLoans;

        public int TotalClosedLoans
        {
            get => _totalClosedLoans;
            set => SetField(ref _totalClosedLoans, value);
        }

        private int _loanRangeTo = 1_000_000;

        public int LoanRangeTo
        {
            get => _loanRangeTo;
            set => SetField(ref _loanRangeTo, value);
        }

        private int _loanRangeFrom = 100_000;

        public int LoanRangeFrom
        {
            get => _loanRangeFrom;
            set => SetField(ref _loanRangeFrom, value);
        }

        private double _totalSavingsPercentage;

        public double TotalSavingsPercentage
        {
            get => _totalSavingsPercentage;
            set => SetField(ref _totalSavingsPercentage, value);
        }

        public void SetAccounts(ObservableCollection<Account> accounts, ObservableCollection<Deposit> deposits)
        {
            Accounts.Clear();
            Deposits.Clear();
            foreach (var account in accounts)
            {
                Accounts.Add(account);
            }
            foreach (var deposit in deposits)
            {
                Deposits.Add(deposit);
            }
        }

        public void SetLoans(ObservableCollection<Loan> loans)
        {
            Loans.Clear();
            foreach (var loan in loans)
            {
                Loans.Add(loan);
            }
        }

        public void SetStatusCounts()
        {
            foreach (var account in Accounts)
            {
                switch (account.AccountStatus)
                {
                    case AccountStatus.Active:
                        TotalActiveAccounts += 1;
                        break;
                    case AccountStatus.Closed:
                        TotalClosedAccounts += 1;
                        break;
                }
            }

            foreach (var deposit in Deposits)
            {
                switch (deposit.AccountStatus)
                {
                    case AccountStatus.Active:
                        TotalActiveDeposits += 1;
                        break;
                    case AccountStatus.Closed:
                        TotalClosedDeposits += 1;
                        break;
                }
            }

            foreach (var loan in Loans)
            {
                switch (loan.AccountStatus)
                {
                    case AccountStatus.Active:
                        TotalActiveLoans += 1;
                        break;
                    case AccountStatus.Closed:
                        TotalClosedLoans += 1;
                        break;
                }
            }
        }

        //when creating new account/deposit/loan or when closing a deposit/loan
        public void UpdateStatusCount()
        {

        }

        public void SetCumulativeAccountBalance()
        {
            foreach (var account in Accounts)
            {
                if (account is SavingsAccountBObj)
                {
                    NetSavings += account.Balance;
                }
                else if (account is CurrentAccountBObj)
                {
                    NetCurrentAccount += account.Balance;
                }
                NetBalance += account.Balance;
            }
        }

        public void SetCumulativeLoanDues()
        {
            foreach (var loan in Loans)
            {
                TotalDue += loan.DueWithInterestAmount;
            }
        }

        public void SetCumulativeDepositBalance()
        {
            foreach (var deposit in Deposits)
            {
                NetDeposit += deposit.DepositedAmount;
            }
        }
        public void SetTotalSavingsPercentage()
        {
            TotalSavingsPercentage = NetSavings * 100 / NetBalance;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }


    
    }
}