using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class DepositCreationUserControl : Windows.UI.Xaml.Controls.UserControl, IAccountCreationView
    {
        public AccountCreationViewModel AccountCreationViewModel { get; set; }
        public DepositCreationUserControl()
        {
            AccountCreationViewModel = new AccountCreationViewModel(this);
            this.InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AccountCreationViewModel.SetAccounts(AccountList);
            AccountCreationViewModel.SetDepositMetaData();
            AccountCreationViewModel.GetAllBranches();
        }

        public static readonly DependencyProperty AccountListProperty = DependencyProperty.Register(
            nameof(AccountList), typeof(ObservableCollection<Account>), typeof(DepositCreationUserControl), new PropertyMetadata(default(ObservableCollection<Account>)));

        public ObservableCollection<Account> AccountList
        {
            get => (ObservableCollection<Account>)GetValue(AccountListProperty);
            set => SetValue(AccountListProperty, value);
        }


        private void FixedDepositRadioButton_OnChecked(object sender, RoutedEventArgs e)
        {

            if (FixedDepositRadioButton.IsChecked != null && (bool)FixedDepositRadioButton.IsChecked)
            {
                FixedDepositInterestRate.Visibility = Visibility.Visible;
                DepositAmountTextBlock.Visibility = Visibility.Visible;
                RecurringDepositInterestRateText.Visibility = Visibility.Collapsed;
                MonthlyInstallmentTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                FixedDepositInterestRate.Visibility = Visibility.Collapsed;
                RecurringDepositInterestRateText.Visibility = Visibility.Visible;
                MonthlyInstallmentTextBlock.Visibility = Visibility.Visible;
                DepositAmountTextBlock.Visibility = Visibility.Collapsed;

            }
        }

        private void RecurringDepositRadioButton_OnChecked(object sender, RoutedEventArgs e)
        {
            if (RecurringDepositRadioButton.IsChecked != null && (bool)RecurringDepositRadioButton.IsChecked)
            {
                FixedDepositInterestRate.Visibility = Visibility.Collapsed;
                RecurringDepositInterestRateText.Visibility = Visibility.Visible;
                MonthlyInstallmentTextBlock.Visibility = Visibility.Visible;
                DepositAmountTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                FixedDepositInterestRate.Visibility = Visibility.Visible;
                RecurringDepositInterestRateText.Visibility = Visibility.Collapsed;
                DepositAmountTextBlock.Visibility = Visibility.Visible;
                MonthlyInstallmentTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void CreateDeposit_OnClick(object sender, RoutedEventArgs e)
        {
            if (AccountCreationViewModel.IsValidPan())
            {
                if (FixedDepositRadioButton.IsChecked != null && (bool)FixedDepositRadioButton.IsChecked)
                {
                    var branchName = BranchNameAutoSuggestBox.Text;
                    var ifsc = AccountCreationViewModel.Branches.Where(b => b.Name == branchName).FirstOrDefault(f => true)?.Ifsc;
                    AccountCreationViewModel.CreateFixedDepositAccount(ifsc,FromAccountTextBox.Text,RepaymentAccountTextBox.Text,(int)TenureSlider.Value);
                }
                else if (RecurringDepositRadioButton.IsChecked != null && (bool)RecurringDepositRadioButton.IsChecked)
                {
                    var branchName = BranchNameAutoSuggestBox.Text;
                    var ifsc = AccountCreationViewModel.Branches.Where(b => b.Name == branchName).FirstOrDefault(f => true)?.Ifsc;
                    AccountCreationViewModel.CreateRecurringDepositAccount(ifsc, FromAccountTextBox.Text, RepaymentAccountTextBox.Text,(int)TenureSlider.Value);

                }
            }
            else
            {
                PanTextBox.Background = new SolidColorBrush(Color.FromArgb(1, 255, 0, 0));
                InvalidPanTextBlock.Visibility = Visibility.Visible;
            }
        }
        private void TextBox_OnTextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            sender.Text = new String(sender.Text.Where(c => char.IsDigit(c) | c == '.').ToArray());
            sender.SelectionStart = sender.Text.Length;
        }
        public event Action OnClosingPopup;

        private void ClosePopup_OnClick(object sender, RoutedEventArgs e)
        {
            //ClosingPopup?.Invoke();
            OnClosingPopup?.Invoke();
        }

        public void SuccessfullyAccountCreated()
        {
            FixedDepositInterestRate.Visibility = Visibility.Collapsed;
            RecurringDepositRadioButton.IsChecked = false;
            FixedDepositRadioButton.IsChecked = false;
            BranchNameAutoSuggestBox.Text = string.Empty;
        }

        private void AutoSuggestBox_OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            BalanceSlider.IsEnabled = true;
            TenureSlider.IsEnabled = true;

            var savingsAccount = AccountCreationViewModel.Accounts.First(a => a.AccountNumber == (string)args.SelectedItem);
            if (savingsAccount.Balance - savingsAccount.MinimumBalance <= BalanceSlider.Minimum)
            {
                BalanceSlider.Minimum = 0;
            }
            BalanceSlider.Maximum = savingsAccount.Balance - savingsAccount.MinimumBalance;


        }

        private void BalanceSlider_OnValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (FixedDepositRadioButton.IsChecked != null && (bool)FixedDepositRadioButton.IsChecked)
            {
                var dep = BalanceSlider.Value;
                AccountCreationViewModel.EstimatedReturnCalculationForFixedDeposit(double.Parse(FixedDepositInterestRate.Text),BalanceSlider.Value,(int)TenureSlider.Value);
            }
            else if(RecurringDepositRadioButton.IsChecked != null && (bool)RecurringDepositRadioButton.IsChecked)
            {
                var interestRate = double.Parse(RecurringDepositInterestRateText.Text);
                AccountCreationViewModel.EstimatedReturnCalculationForRecurringDeposit(interestRate, BalanceSlider.Value, (int)TenureSlider.Value);
            }
        }
    }
}
