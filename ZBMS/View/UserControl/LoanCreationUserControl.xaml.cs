using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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
using ZBMSLibrary.Entities.Model;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBMS.View.UserControl
{
    public sealed partial class LoanCreationUserControl : Windows.UI.Xaml.Controls.UserControl,IAccountCreationView
    {
        public AccountCreationViewModel AccountCreationViewModel { get; set; }
        public LoanCreationUserControl()
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
            LoanedAmountGoesToAccountNumber.SelectedIndex = 0;
            PersonalLoanRadioButton.IsChecked = true;
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

        private void PersonalLoanRadioButton_OnChecked(object sender, RoutedEventArgs e)
        {
        }

        private void CreateLoan_OnClick(object sender, RoutedEventArgs e)
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

            if (string.IsNullOrEmpty(LoanedAmountGoesToAccountNumber.SelectedItem as string) || string.IsNullOrWhiteSpace(LoanedAmountGoesToAccountNumber.SelectedItem as string))
            {
                LoanedAmountGoesToAccountNumber.Header = "Account field cannot be empty";
                return;
            }

            if (AccountCreationViewModel.IsValidPan())
            {
                if (AccountCreationViewModel.IsUserPan(Pan))
                {
                    if (PersonalLoanRadioButton.IsChecked != null && (bool)PersonalLoanRadioButton.IsChecked)
                    {
                        var branchName = BranchNameComboBox.SelectionBoxItem as string;
                        var ifsc = AccountCreationViewModel.Branches.Where(b => b.Name == branchName).FirstOrDefault(f => true)?.Ifsc;
                        var loanedAmountGoesToAccountNumber = LoanedAmountGoesToAccountNumber.SelectedItem as string;
                        AccountCreationViewModel.CreatePersonalLoanAccount(ifsc, loanedAmountGoesToAccountNumber, (int)TenureSlider.Value);
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
            LoanAmountSlider.Value = LoanAmountSlider.Minimum;
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
            ClearFields();
            OnClosingPopup?.Invoke();
        }

        public void SuccessfullyAccountCreated()
        {
            //FixedDepositInterestRate.Visibility = Visibility.Collapsed;
        }


        private void BalanceSlider_OnValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            //LoanAmountTextBox.Text = Math.Round(((Slider)sender).Value, 2).ToString();
            if (PersonalLoanRadioButton.IsChecked != null && (bool)PersonalLoanRadioButton.IsChecked)
            {
                var dep = LoanAmountSlider.Value;
                AccountCreationViewModel.EstimatedReturnCalculationForPersonalLoan(double.Parse(PersonalLoanInterestRate.Text), LoanAmountSlider.Value, (int)TenureSlider.Value);
            }
        }

        private void UIElement_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            var ctrl = Window.Current.CoreWindow.GetKeyState(VirtualKey.Control);
            if (ctrl.HasFlag(CoreVirtualKeyStates.Down) && e.Key == VirtualKey.Enter)
            {
                CreateLoan_OnClick(sender, new RoutedEventArgs());
            }
            e.Handled = true;

        }

        private void NumberBox_OnValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            //sender.Text = new String(sender.Text.Where(c => char.IsDigit(c) | c == '.').ToArray());
            //sender.Value = double.Parse(new String(sender.Text.Where(c => char.IsDigit(c) | c == '.').ToArray()));
            //sender.Value = Math.Round(sender.Value, 2);
            //sender.Sele = sender.Text.Length;
                //NumberBox txt = (NumberBox)sender;
                //txt.Text = txt.Value.ToString();
                //var regex = new Regex(@"^[0-9]*(?:\.[0-9]{0,1})?$");
                //string str = txt.Text.ToString();
                //int cntPrc = 0;
                //if (str.Contains('.'))
                //{
                //    string[] tokens = str.Split('.');
                //    if (tokens.Count() > 0)
                //    {
                //        string result = tokens[1];
                //        char[] prc = result.ToCharArray();
                //        cntPrc = prc.Count();
                //    }
                //}
                //if (regex.IsMatch(txt.Text) && !(txt.Text == "." && ((NumberBox)sender).Text.Contains(txt.Text)) && (cntPrc < 3))
                //{
                //    //e.Handled = false;
                //}
                //else
                //{
                //    //e.Handled = true;
                //}

            if (PersonalLoanRadioButton.IsChecked != null && (bool)PersonalLoanRadioButton.IsChecked)
            {
                var dep = LoanAmountSlider.Value;
                AccountCreationViewModel.EstimatedReturnCalculationForPersonalLoan(double.Parse(PersonalLoanInterestRate.Text), LoanAmountSlider.Value, (int)TenureSlider.Value);
            }
        }

        private void UIElement_OnCharacterReceived(UIElement sender, CharacterReceivedRoutedEventArgs args)
        {
            if (sender is NumberBox numberBox)
                numberBox.Text = new String(numberBox.Text.Where(c => char.IsDigit(c) | c == '.').ToArray());
            //sender.Sele = sender.Text.Length;

            if (PersonalLoanRadioButton.IsChecked != null && (bool)PersonalLoanRadioButton.IsChecked)
            {
                var dep = LoanAmountSlider.Value;
                AccountCreationViewModel.EstimatedReturnCalculationForPersonalLoan(double.Parse(PersonalLoanInterestRate.Text), LoanAmountSlider.Value, (int)TenureSlider.Value);
            }
        }

        private void LoanAmountTextBox_OnLoaded(object sender, RoutedEventArgs e)
        {
            //if (sender is NumberBox loanedAmount)
            //{
            //    loanedAmount.Value = Math.Round(loanedAmount.Value, 2);
            //}
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
