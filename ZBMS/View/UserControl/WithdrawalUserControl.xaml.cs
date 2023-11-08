using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class WithdrawalUserControl : Windows.UI.Xaml.Controls.UserControl
    {
        public WithdrawMoneyViewModel WithdrawMoneyViewModel;
        public WithdrawalUserControl()
        {
            WithdrawMoneyViewModel = new WithdrawMoneyViewModel();
            this.InitializeComponent();
            Loaded += OnLoaded;

        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Account is SavingsAccountBObj)
            {
                WithdrawMoneyViewModel.SavingsAccountBObj = (SavingsAccountBObj)Account;
            }
            else if (Account is CurrentAccountBObj)
            {
                WithdrawMoneyViewModel.CurrentAccountBObj = (CurrentAccountBObj)Account;
            }
        }

        private void AmountTextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            sender.Text = new String(sender.Text.Where(c => char.IsDigit(c) | c == '.').ToArray());
            
            sender.SelectionStart = sender.Text.Length;
            WithdrawButton.IsEnabled = AmountTextBox.Text.Length > 0;
        }



        public static readonly DependencyProperty AccountProperty =
            DependencyProperty.Register(nameof(Account), typeof(Account), typeof(WithdrawalUserControl),
                new PropertyMetadata(null));

        public Account Account
        {
            get => (Account)GetValue(AccountProperty);
            set => SetValue(AccountProperty, value);
        }

        public event Action WithDrawZeroWarning;
        public event Action WithdrawInsufficientBalanceWarning;
        private void WithdrawButton_OnClick(object sender, RoutedEventArgs e)
        {
            var amount = double.Parse(AmountTextBox.Text);
            if (amount > 0 && (Account.Balance - amount >= 0))
            {
                WithdrawMoneyViewModel.WithdrawMoney(double.Parse(AmountTextBox.Text));
                AmountTextBox.Text = string.Empty;
            }
            else if(amount == 0)
            {
                //Cant withdraw 0 error
                WithDrawZeroWarning?.Invoke();
                AmountTextBox.Text = string.Empty;
            }
            else if (Account.Balance - amount < 0)
            {
                //dont have sufficient balance error
                WithdrawInsufficientBalanceWarning?.Invoke();
                AmountTextBox.Text = string.Empty;
            }
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
                WithdrawButton_OnClick(sender,e);
            }
        }

        private void AmountTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            WithdrawButton.IsEnabled = AmountTextBox.Text.Length > 0;
        }
    }
}
