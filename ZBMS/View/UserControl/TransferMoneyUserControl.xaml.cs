using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZBMS.ViewModel;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBMS.View.UserControl
{
    public sealed partial class TransferMoneyUserControl : Windows.UI.Xaml.Controls.UserControl, ITransferMoneyView
    {
        public TransferMoneyViewModel TransferMoneyViewModel;
        public TransferMoneyUserControl()
        {
            TransferMoneyViewModel = new TransferMoneyViewModel(this);
            this.InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //TransferMoneyViewModel.TransferToAccount = TransferToAccount;
            TransferMoneyViewModel.TransferFromAccount = TransferFromAccount;
            TransferMoneyViewModel.SetAccountNumbers(AccountList);
            AccountNumbers.SelectedIndex = 0;
        }

        public static readonly DependencyProperty TransferFromAccountProperty =
            DependencyProperty.Register(nameof(TransferFromAccount), typeof(Account), typeof(TransferMoneyUserControl),
                new PropertyMetadata(null));

        public Account TransferFromAccount
        {
            get => (Account)GetValue(TransferFromAccountProperty);
            set => SetValue(TransferFromAccountProperty, value);
        }

        public static readonly DependencyProperty AccountListProperty = DependencyProperty.Register(
            nameof(AccountList), typeof(ObservableCollection<Account>), typeof(TransferMoneyUserControl), new PropertyMetadata(default(ObservableCollection<Account>)));

        public ObservableCollection<Account> AccountList
        {
            get => (ObservableCollection<Account>)GetValue(AccountListProperty);
            set => SetValue(AccountListProperty, value);
        }
        public event Action TransferZeroWarning;
        public event Action TransferInsufficientBalanceWarning;
        public event Action<TransactionSummaryVObj> TransferSuccess;
        private void TransferButton_OnClick(object sender, RoutedEventArgs e)
        {
            //  throw new NotImplementedException();
            var amount = double.Parse(AmountTextBox.Text);
            if (amount > 0 && (TransferFromAccount.Balance - amount >= 0))
            {
                TransferMoneyViewModel.TransferMoney(amount, AccountNumbers.SelectedItem as string);
                AmountTextBox.Text = string.Empty;
                ErrorTextBlock.Visibility = Visibility.Collapsed;
            }
            else if (amount == 0)
            {
                //Cant transfer 0 error
                TransferZeroWarning?.Invoke();
                ErrorTextBlock.Visibility = Visibility.Collapsed;

                AmountTextBox.Text = string.Empty;
            }
            else if (TransferFromAccount.Balance - amount < 0)
            {
                //dont have sufficient balance error
                TransferInsufficientBalanceWarning?.Invoke();
                //ErrorTextBlock.Visibility = Visibility.Visible;
                AmountTextBox.Text = string.Empty;
            }
        }

        private void AmountTextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            sender.Text = new String(sender.Text.Where(c => char.IsDigit(c) | c == '.').ToArray());

            sender.SelectionStart = sender.Text.Length;
            TransferButton.IsEnabled = AmountTextBox.Text.Length > 0;
        }

        private void AmountTextBox_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                e.Handled = true;
                if (string.IsNullOrEmpty(AmountTextBox.Text) || string.IsNullOrWhiteSpace(AmountTextBox.Text))
                {
                    return;
                }
                TransferButton_OnClick(sender, e);
            }
        }

        private void AmountTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TransferButton.IsEnabled = AmountTextBox.Text.Length > 0;
        }

        public void TransferSuccessful(TransactionSummaryVObj transactionSummaryVObj)
        {
            ErrorTextBlock.Visibility = Visibility.Collapsed;
            TransferSuccess?.Invoke(transactionSummaryVObj);
        }

        public void TransferFailed(string errorMessage)
        {
            //ErrorTextBlock.Visibility = Visibility.Visible;
            TransferInsufficientBalanceWarning?.Invoke();
            //ErrorTextBlock.Text = errorMessage;
        }
    }

    public interface ITransferMoneyView
    {
        void TransferSuccessful(TransactionSummaryVObj transactionSummaryVObj);
        void TransferFailed(string errorMessage);
    }
}
