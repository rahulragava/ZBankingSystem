using System;
using System.Collections.Generic;
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
using ZBMS.View.UserControl.AccountSummary;
using ZBMS.ViewModel;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBMS.View.UserControl
{
    public sealed partial class DepositMoneyUserControl : Windows.UI.Xaml.Controls.UserControl,IDepositMoneyUserControl
    {
        public DepositMoneyViewModel DepositMoneyViewModel;
        public DepositMoneyUserControl()
        {
            DepositMoneyViewModel = new DepositMoneyViewModel(this);
            this.InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Account is SavingsAccountBObj)
            {
                DepositMoneyViewModel.SavingsAccountBObj = (SavingsAccountBObj)Account;
            }
            else if (Account is CurrentAccountBObj)
            {
                DepositMoneyViewModel.CurrentAccountBObj = (CurrentAccountBObj)Account;
            }

        }

        //private void AmountTextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        //{
            //string newText = sender.Text;
            //sender.Text = new string(newText.Where(c => char.IsDigit(c) || c == '.').ToArray());
            //sender.SelectionStart = newText.Length;
            //ViewModel.CurrentTransaction.Amount = decimal.TryParse(newText, out decimal amount) ? amount : 0m;
            //ViewModel.FieldErrors["Amount"] = string.Empty;
        //}
        //private void TextBox_OnBeforeTextChanging(TextBox sender,
        //    TextBoxBeforeTextChangingEventArgs args)
        //{
        //    args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        //}
        private void AmountTextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            sender.Text = new String(sender.Text.Where(char.IsDigit).ToArray());
            sender.SelectionStart = sender.Text.Length;
        }

        public event Action ZeroDepositWarning;
        private void DepositButton_OnClick(object sender, RoutedEventArgs e)
        {
            var amount = double.Parse(AmountTextBox.Text);
            if (amount > 0)
            {
                DepositMoneyViewModel.DepositMoney(double.Parse(AmountTextBox.Text));
                AmountTextBox.Text = string.Empty;
            }
            else if(amount == 0)
            {
                AmountTextBox.Text = string.Empty;
                //error
                ZeroDepositWarning?.Invoke();
            }
            
        }

        public static readonly DependencyProperty AccountProperty =
            DependencyProperty.Register(nameof(Account), typeof(Account), typeof(DepositMoneyUserControl),
                new PropertyMetadata(null));

        public Account Account
        {
            get => (Account)GetValue(AccountProperty);
            set => SetValue(AccountProperty, value);
        }

        private void DepositButton_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                e.Handled = true;
                DepositButton_OnClick(sender, e);
            }
        }

        private void AmountTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            DepositButton.IsEnabled = AmountTextBox.Text.Length > 0;
        }
    }

    public interface IDepositMoneyUserControl
    {
        //void OnMoneyDeposited(double amount);
    }
}
