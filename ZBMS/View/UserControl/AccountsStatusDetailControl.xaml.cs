using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZBMS.ViewModel;
using ZBMSLibrary.Entities.Model;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBMS.View.UserControl
{
    public sealed partial class AccountsStatusDetailControl : Windows.UI.Xaml.Controls.UserControl
    {
        public readonly ConsolidatedReportViewModel ConsolidatedReportViewModel;
        public AccountsStatusDetailControl()
        {
            ConsolidatedReportViewModel = new ConsolidatedReportViewModel();
            this.InitializeComponent();
        }

        public void SetStatusCounts()
        {
                ConsolidatedReportViewModel.SetAccounts(AccountList, DepositList);
                ConsolidatedReportViewModel.SetLoans(LoanList);
                ConsolidatedReportViewModel.SetStatusCounts();
            
        }

        public static readonly DependencyProperty AccountListProperty = DependencyProperty.Register(
            nameof(AccountList), typeof(ObservableCollection<Account>), typeof(AccountsStatusDetailControl), new PropertyMetadata(default(ObservableCollection<Account>)));

        public ObservableCollection<Account> AccountList
        {
            get => (ObservableCollection<Account>)GetValue(AccountListProperty);
            set => SetValue(AccountListProperty, value);
        }

        public static readonly DependencyProperty DepositListProperty = DependencyProperty.Register(
            nameof(DepositList), typeof(ObservableCollection<Deposit>), typeof(AccountsStatusDetailControl), new PropertyMetadata(default(ObservableCollection<Deposit>)));

        public ObservableCollection<Deposit> DepositList
        {
            get => (ObservableCollection<Deposit>)GetValue(DepositListProperty);
            set => SetValue(DepositListProperty, value);
        }

        public static readonly DependencyProperty LoanListProperty = DependencyProperty.Register(
            nameof(LoanList), typeof(ObservableCollection<Loan>), typeof(AccountsStatusDetailControl), new PropertyMetadata(default(ObservableCollection<Loan>)));

        public ObservableCollection<Loan> LoanList
        {
            get => (ObservableCollection<Loan>)GetValue(LoanListProperty);
            set => SetValue(LoanListProperty, value);
        }

        public void NewAccountCreated()
        {
            ConsolidatedReportViewModel.TotalActiveDeposits += 1;
        }

        public void NewLoanCreated()
        {
            ConsolidatedReportViewModel.TotalActiveLoans += 1;
        }

        public void NewDepositCreated()
        {
            ConsolidatedReportViewModel.TotalActiveDeposits += 1;

        }

        public void LoanClosed()
        {
            ConsolidatedReportViewModel.TotalClosedLoans += 1;
            ConsolidatedReportViewModel.TotalActiveLoans -= 1;
        }

        public void DepositClosed()
        {
            ConsolidatedReportViewModel.TotalClosedDeposits += 1;
            ConsolidatedReportViewModel.TotalActiveDeposits -= 1;
        }

    }
}
