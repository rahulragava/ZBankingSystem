using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
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
            AccountCreationViewModel.GetAllBranches();
            this.InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AccountCreationViewModel.SetAccounts(AccountList);
            AccountCreationViewModel.SetDepositMetaData();
            BranchNameComboBox.SelectedIndex = 0;
            FromAccountComboBox.SelectedIndex = 0;
            RepaymentComboBox.SelectedIndex = 0;
            FixedDepositRadioButton.IsChecked = true;
        }

        public static readonly DependencyProperty AccountListProperty = DependencyProperty.Register(
            nameof(AccountList), typeof(ObservableCollection<Account>), typeof(DepositCreationUserControl), new PropertyMetadata(default(ObservableCollection<Account>)));

        public ObservableCollection<Account> AccountList
        {
            get => (ObservableCollection<Account>)GetValue(AccountListProperty);
            set => SetValue(AccountListProperty, value);
        }


        public static readonly DependencyProperty PanProperty = DependencyProperty.Register(
            nameof(Pan), typeof(string), typeof(AccountCreationUserControl), new PropertyMetadata(default(string)));

        public string Pan
        {
            get => (string)GetValue(PanProperty);
            set => SetValue(PanProperty, value);
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

            BalanceSlider.Value = BalanceSlider.Minimum;
            TenureSlider.Value = TenureSlider.Minimum;
            AccountCreationViewModel.EstimatedReturnCalculationForFixedDeposit(double.Parse(FixedDepositInterestRate.Text), BalanceSlider.Value, (int)TenureSlider.Value);

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

            BalanceSlider.Value = BalanceSlider.Minimum;
            TenureSlider.Value = TenureSlider.Minimum;
            AccountCreationViewModel.EstimatedReturnCalculationForRecurringDeposit(BalanceSlider.Value, (int)TenureSlider.Value);

        }

        private void CreateDeposit_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PanTextBox.Text) || string.IsNullOrWhiteSpace(PanTextBox.Text))
            {
                PanTextBox.Background = new SolidColorBrush(Color.FromArgb(1, 255, 0, 0));
                InvalidPanTextBlock.Text = "PAN field cannot be empty";
                InvalidPanTextBlock.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                InvalidPanTextBlock.Visibility = Visibility.Collapsed;
            }

            if (string.IsNullOrEmpty(BranchNameComboBox.SelectedItem as string) || string.IsNullOrWhiteSpace(BranchNameComboBox.SelectedItem as string))
            {
                //PanTextBox.Background = new SolidColorBrush(Color.FromArgb(1, 255, 0, 0));
                BranchNameComboBox.Header = "Branch field cannot be empty";
                return;
            }

            if (string.IsNullOrEmpty(FromAccountComboBox.SelectedItem as string) || string.IsNullOrWhiteSpace(FromAccountComboBox.SelectedItem as string))
            {
                FromAccountComboBox.Header = "Account field cannot be empty";
                return;
            }

            if (string.IsNullOrEmpty(RepaymentComboBox.SelectedItem as string) || string.IsNullOrWhiteSpace(RepaymentComboBox.SelectedItem as string))
            {
                RepaymentComboBox.Header = "Account field cannot be empty";
                return;
            }
            if (AccountCreationViewModel.IsValidPan())
            {
                if (AccountCreationViewModel.IsUserPan(Pan))
                {
                    if (FixedDepositRadioButton.IsChecked != null && (bool)FixedDepositRadioButton.IsChecked)
                    {
                        var branchName = BranchNameComboBox.SelectionBoxItem as string;
                        var ifsc = AccountCreationViewModel.Branches.Where(b => b.Name == branchName).FirstOrDefault(f => true)?.Ifsc;
                        AccountCreationViewModel.CreateFixedDepositAccount(ifsc, FromAccountComboBox.SelectedItem as string, RepaymentComboBox.SelectedItem as string, (int)TenureSlider.Value);
                    }
                    else if (RecurringDepositRadioButton.IsChecked != null && (bool)RecurringDepositRadioButton.IsChecked)
                    {
                        var branchName = BranchNameComboBox.SelectionBoxItem as string;
                        var ifsc = AccountCreationViewModel.Branches.Where(b => b.Name == branchName).FirstOrDefault(f => true)?.Ifsc;
                        AccountCreationViewModel.CreateRecurringDepositAccount(ifsc, FromAccountComboBox.SelectedItem as string, RepaymentComboBox.SelectedItem as string, (int)TenureSlider.Value);

                    }
                    ClearFields();
                }
                else
                {
                    InvalidPanTextBlock.Text = "wrong PAN";
                    InvalidPanTextBlock.Visibility = Visibility.Visible;
                }
            }
            else
            {
                PanTextBox.Background = new SolidColorBrush(Color.FromArgb(1, 255, 0, 0));
                InvalidPanTextBlock.Text = "Invalid PAN";
                InvalidPanTextBlock.Visibility = Visibility.Visible;
            }
        }

        public void ClearFields()
        {
            PanTextBox.Text = String.Empty;
            InvalidPanTextBlock.Visibility = Visibility.Collapsed;
            InvalidPanTextBlock.Text = string.Empty;
            BalanceSlider.Value = BalanceSlider.Minimum;
            TenureSlider.Value = TenureSlider.Minimum;
            BranchNameComboBox.SelectedIndex = 0;
        }
        private void TextBox_OnTextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            sender.Text = new String(sender.Text.Where(c => char.IsDigit(c) | c == '.').ToArray());
            sender.SelectionStart = sender.Text.Length;
        }
        public event Action OnClosingPopup;

        private void ClosePopup_OnClick(object sender, RoutedEventArgs e)
        {
            InvalidPanTextBlock.Visibility = Visibility.Collapsed;
            PanTextBox.Text = string.Empty;
            BalanceSlider.Value = BalanceSlider.Minimum;
            TenureSlider.Value = TenureSlider.Minimum;
            //ClosingPopup?.Invoke();
            OnClosingPopup?.Invoke();
        }

        public void SuccessfullyAccountCreated()
        {
            FixedDepositInterestRate.Visibility = Visibility.Collapsed;
            RecurringDepositRadioButton.IsChecked = false;
            FixedDepositRadioButton.IsChecked = false;
        }

        //private void AutoSuggestBox_OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        //{
        //    BalanceSlider.IsEnabled = true;
        //    TenureSlider.IsEnabled = true;

        //    var savingsAccount = AccountCreationViewModel.Accounts.First(a => a.AccountNumber == (string)args.SelectedItem);
        //    if (savingsAccount.Balance - savingsAccount.MinimumBalance <= BalanceSlider.Minimum)
        //    {
        //        BalanceSlider.Minimum = 0;
        //    }
        //    BalanceSlider.Maximum = savingsAccount.Balance - savingsAccount.MinimumBalance;


        //}

        private void BalanceSlider_OnValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (FixedDepositRadioButton.IsChecked != null && (bool)FixedDepositRadioButton.IsChecked)
            {
                switch (AccountCreationViewModel.Tenure)
                {
                    case 0:
                        AccountCreationViewModel.FixedDepositInterestRate = 0.0;
                        break;
                    case 1:
                        AccountCreationViewModel.FixedDepositInterestRate = 6;
                        break;
                    case 2:
                    case 3:
                        AccountCreationViewModel.FixedDepositInterestRate = 6.8;
                        break;
                    case 4:
                        AccountCreationViewModel.FixedDepositInterestRate = 7.2;
                        break;
                    default:
                        AccountCreationViewModel.FixedDepositInterestRate = 7.2;
                        break;
                }

                var dep = BalanceSlider.Value;
                AccountCreationViewModel.EstimatedReturnCalculationForFixedDeposit(double.Parse(FixedDepositInterestRate.Text), BalanceSlider.Value, (int)TenureSlider.Value);
            }
            else if (RecurringDepositRadioButton.IsChecked != null && (bool)RecurringDepositRadioButton.IsChecked)
            {
                AccountCreationViewModel.EstimatedReturnCalculationForRecurringDeposit(BalanceSlider.Value, (int)TenureSlider.Value);
            }

        }

        private void FromAccountTextBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BalanceSlider.IsEnabled = true;
            TenureSlider.IsEnabled = true;
            //combobox selection changed but value is " " check out whats the problem
            var accountNumber = (sender as ComboBox)?.SelectedItem;
            if (accountNumber != null)
            {
                var account = AccountCreationViewModel.Accounts.First(a => a.AccountNumber == accountNumber);
                if (account.Balance - account.MinimumBalance <= BalanceSlider.Minimum)
                {
                    BalanceSlider.Minimum = 0;
                }
                BalanceSlider.Maximum = account.Balance - account.MinimumBalance;
            }

            //var savings
        }

        private void UIElement_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            var ctrl = Window.Current.CoreWindow.GetKeyState(VirtualKey.Control);
            if (ctrl.HasFlag(CoreVirtualKeyStates.Down) && e.Key == VirtualKey.Enter)
            {
                CreateDeposit_OnClick(sender, new RoutedEventArgs());
            }
            e.Handled = true;

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
}
