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
    public sealed partial class BalanceDetailUserControl : Windows.UI.Xaml.Controls.UserControl
    {
        public readonly ConsolidatedReportViewModel ConsolidatedReportViewModel;

        public BalanceDetailUserControl()
        {
            ConsolidatedReportViewModel = new ConsolidatedReportViewModel();
            this.InitializeComponent();
            Loaded += BalanceDetailUserControl_Loaded;
        }

        private void BalanceDetailUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //ConsolidatedReportViewModel.SetAccounts(AccountList,DepositList);
            //ConsolidatedReportViewModel.SetCumulativeAccountBalance();
            //ConsolidatedReportViewModel.SetTotalSavingsPercentage();
            //ConsolidatedReportViewModel.SetCumulativeDepositBalance();
        }

        public void SetBalanceDetails()
        {
            ConsolidatedReportViewModel.SetAccounts(AccountList, DepositList);
            ConsolidatedReportViewModel.SetCumulativeAccountBalance();
            ConsolidatedReportViewModel.SetTotalSavingsPercentage();
            ConsolidatedReportViewModel.SetCumulativeDepositBalance();
        }

        public static readonly DependencyProperty AccountListProperty = DependencyProperty.Register(
            nameof(AccountList), typeof(ObservableCollection<Account>), typeof(BalanceDetailUserControl), new PropertyMetadata(default(ObservableCollection<Account>)));

        public ObservableCollection<Account> AccountList
        {
            get => (ObservableCollection<Account>)GetValue(AccountListProperty);
            set => SetValue(AccountListProperty, value);
        }

        public static readonly DependencyProperty DepositListProperty = DependencyProperty.Register(
            nameof(DepositList), typeof(ObservableCollection<Deposit>), typeof(BalanceDetailUserControl), new PropertyMetadata(default(ObservableCollection<Deposit>)));

        public ObservableCollection<Deposit> DepositList
        {
            get => (ObservableCollection<Deposit>)GetValue(DepositListProperty);
            set => SetValue(DepositListProperty, value);
        }

    }
}
