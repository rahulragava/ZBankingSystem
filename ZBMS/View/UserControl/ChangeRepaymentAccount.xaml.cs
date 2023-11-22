using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZBMS.ViewModel;
using ZBMSLibrary.Entities.Enums;
using ZBMSLibrary.Entities.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBMS.View.UserControl
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChangeRepaymentAccount : Windows.UI.Xaml.Controls.UserControl, IChangeRepaymentView
    {
        public ChangeRepaymentAccountDepositViewModel ChangeRepaymentAccountDepositViewModel;
        public ChangeRepaymentAccount()
        {
            ChangeRepaymentAccountDepositViewModel = new ChangeRepaymentAccountDepositViewModel(this);
            this.InitializeComponent();
            Loaded += ChangeRepaymentAccount_Loaded;
        }

        private void ChangeRepaymentAccount_Loaded(object sender, RoutedEventArgs e)
        {
            ChangeRepaymentAccountDepositViewModel.Deposit = DepositAccount;
            ChangeRepaymentAccountDepositViewModel.SetAccountNumbers(AccountList);
            AccountNumbers.SelectedIndex = 0;
            EditButton.IsEnabled = ChangeRepaymentAccountDepositViewModel.Deposit.AccountStatus != AccountStatus.Closed;
        }
        public static readonly DependencyProperty AccountListProperty = DependencyProperty.Register(
            nameof(AccountList), typeof(ObservableCollection<Account>), typeof(LoanPaymentUserControl), new PropertyMetadata(default(ObservableCollection<Account>)));

        public ObservableCollection<Account> AccountList
        {
            get => (ObservableCollection<Account>)GetValue(AccountListProperty);
            set => SetValue(AccountListProperty, value);
        }

        public static readonly DependencyProperty DepositAccountProperty = DependencyProperty.Register(
            nameof(DepositAccount), typeof(Deposit), typeof(ChangeRepaymentAccount), new PropertyMetadata(default(Deposit)));

        public Deposit DepositAccount
        {
            get => (Deposit)GetValue(DepositAccountProperty);
            set => SetValue(DepositAccountProperty, value);
        }

        private void AccountNumbers_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }


        public event Action<string> UpdateRepaymentAccountDeposit;

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ChangeRepaymentAccountDepositViewModel.ChangeAccountForDeposit(AccountNumbers.SelectedItem as string,
                DepositAccount);
        }

        public void UpdateRepaymentAccount(string accountNumber)
        {
            UpdateRepaymentAccountDeposit?.Invoke(accountNumber);
        }

        public void OnCloseDeposit()
        {
            EditButton.IsEnabled = false;
        }
        private void Button_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Hand, 1);
        }

        private void Button_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
        }
    }

    public interface IChangeRepaymentView
    {
        void UpdateRepaymentAccount(string accountNumber);
    }
}
