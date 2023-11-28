using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using Microsoft.UI.Xaml.Controls;
using ZBMS.ViewModel;
using ZBMSLibrary.Entities.Model;
using RadioButton = Windows.UI.Xaml.Controls.RadioButton;
using Windows.UI.Core;
using Windows.UI.Xaml.Shapes;
using ZBMS.Util;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBMS.View.UserControl
{
    public sealed partial class AccountCreationUserControl : Windows.UI.Xaml.Controls.UserControl, IAccountCreationView
    {
        public AccountCreationViewModel AccountCreationViewModel;
        
        public AccountCreationUserControl()
        {
            AccountCreationViewModel = new AccountCreationViewModel(this);
            AccountCreationViewModel.GetAllBranches();
            this.InitializeComponent();
            Loaded += AccountCreationUserControl_Loaded;
        }



        private void AccountCreationUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            AccountCreationViewModel.SetAccounts(AccountList);
            BranchNameComboBox.SelectedIndex = 0;
            SavingsAccountRadioButton.IsChecked = true;
            //AccountCreationViewModel.SetAccountMetaData();

        }

        public event Action OnClosingPopup;

        private void ClosePopup_OnClick(object sender, RoutedEventArgs e)
        {
            //ClosingPopup?.Invoke();
            InvalidPanTextBlock.Visibility = Visibility.Collapsed;
            InvalidBalanceTextBlock.Visibility = Visibility.Collapsed;
            PanTextBox.Text = string.Empty;
            BalanceTextBox.Text = string.Empty;
            OnClosingPopup?.Invoke();
        }

        public static readonly DependencyProperty AccountListProperty = DependencyProperty.Register(
            nameof(AccountList), typeof(ObservableCollection<Account>), typeof(AccountCreationUserControl), new PropertyMetadata(default(ObservableCollection<Account>)));

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


        private void SavingsAccountRadioButton_OnChecked(object sender, RoutedEventArgs e)
        {

            if (SavingsAccountRadioButton.IsChecked != null && (bool)SavingsAccountRadioButton.IsChecked)
            {
                InterestRateText.Visibility = Visibility.Visible;
                CurrentInterestRateText.Visibility = Visibility.Collapsed;
            }
            else
            {
                InterestRateText.Visibility = Visibility.Collapsed;
                CurrentInterestRateText.Visibility = Visibility.Visible;
            }
        }

        private void CurrentAccountRadioButton_OnChecked(object sender, RoutedEventArgs e)
        {
            if (CurrentAccountRadioButton.IsChecked != null && (bool)CurrentAccountRadioButton.IsChecked)
            {
                InterestRateText.Visibility = Visibility.Collapsed;
                CurrentInterestRateText.Visibility = Visibility.Visible;
            }
            else
            {
                InterestRateText.Visibility = Visibility.Visible;
                CurrentInterestRateText.Visibility = Visibility.Collapsed;
            }
        }

        private void CreateAccount_OnClick(object sender, RoutedEventArgs e)
        {
            if ((string.IsNullOrEmpty(BalanceTextBox.Text)) ||
                  (string.IsNullOrWhiteSpace(BalanceTextBox.Text)))
            {
                InvalidBalanceTextBlock.Visibility = Visibility.Visible;
                InvalidBalanceTextBlock.Text = "Field cannot be empty";
                return;
            }
            else
            {
                InvalidBalanceTextBlock.Visibility = Visibility.Collapsed;
            }

            if (String.IsNullOrWhiteSpace(PanTextBox.Text) || String.IsNullOrEmpty(PanTextBox.Text))
            {
                InvalidPanTextBlock.Visibility = Visibility.Visible;
                InvalidPanTextBlock.Text = "Field cannot be empty";
                return;
            }
            else
            {
                InvalidPanTextBlock.Visibility = Visibility.Collapsed;
            }
            if (CurrentAccountRadioButton.IsChecked != null && (bool)CurrentAccountRadioButton.IsChecked)
            {
                if (double.Parse(BalanceTextBox.Text) < AccountCreationViewModel.CurrentAccountMinimumBalance)
                {
                    //minimum balance
                    InvalidBalanceTextBlock.Visibility = Visibility.Visible;
                    InvalidBalanceTextBlock.Text = $"balance amount should be greater than {AccountCreationViewModel.CurrentAccountMinimumBalance}";
                    return;
                }
               
            }
            else
            {
                if (double.Parse(BalanceTextBox.Text) < AccountCreationViewModel.SavingsAccountMinimumBalance)
                {
                    //minimum balance
                    InvalidBalanceTextBlock.Visibility = Visibility.Visible;
                    InvalidBalanceTextBlock.Text = $"balance amount should be greater than {AccountCreationViewModel.SavingsAccountMinimumBalance}";
                    return;
                }
            }
            
            if(AccountCreationViewModel.IsValidPan())
            {
                if (AccountCreationViewModel.IsUserPan(Pan))
                {
                    if (SavingsAccountRadioButton.IsChecked != null && (bool)SavingsAccountRadioButton.IsChecked)
                    {
                        var branchName = BranchNameComboBox.SelectionBoxItem as string;
                        var ifsc = AccountCreationViewModel.Branches.Where(b => b.Name == branchName).FirstOrDefault(f => true)?.Ifsc;

                        AccountCreationViewModel.CreateSavingsAccount(ifsc, double.Parse(BalanceTextBox.Text));
                    }
                    else if (CurrentAccountRadioButton.IsChecked != null && (bool)CurrentAccountRadioButton.IsChecked)
                    {
                        var branchName = BranchNameComboBox.SelectionBoxItem as string;
                        var ifsc = AccountCreationViewModel.Branches.Where(b => b.Name == branchName).FirstOrDefault(f => true)?.Ifsc;

                        AccountCreationViewModel.CreateCurrentAccount(ifsc, double.Parse(BalanceTextBox.Text));
                    }

                    ClearFields();
                }
                PanTextBox.Background = new SolidColorBrush(Color.FromArgb(1, 255, 0, 0));
                InvalidPanTextBlock.Visibility = Visibility.Visible;
                InvalidPanTextBlock.Text = "wrong PAN";
                InvalidBalanceTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                PanTextBox.Background = new SolidColorBrush(Color.FromArgb(1, 255, 0, 0));
                InvalidPanTextBlock.Visibility = Visibility.Visible;
                InvalidPanTextBlock.Text = "Invalid PAN";
                InvalidBalanceTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        public void ClearFields()
        {
            PanTextBox.Text = String.Empty;
            InvalidPanTextBlock.Visibility = Visibility.Collapsed;
            InvalidPanTextBlock.Text = string.Empty;    
            InvalidBalanceTextBlock.Text = string.Empty;
            BalanceTextBox.Text = string.Empty;
            InvalidBalanceTextBlock.Visibility = Visibility.Collapsed;
            SavingsAccountRadioButton.IsChecked = true;
            BranchNameComboBox.SelectedIndex = 0;
        }
        private void NumberBox_OnValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            //sender.Value = double.Parse(new String(sender.Text.Where(c => char.IsDigit(c) | c == '.').ToArray()));
        }

        private void TextBox_OnTextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            sender.Text = new String(sender.Text.Where(c => char.IsDigit(c) | c == '.').ToArray());
            sender.SelectionStart = sender.Text.Length;
        }

        public void SuccessfullyAccountCreated()
        {
            BalanceTextBox.Text = string.Empty;
            InterestRateText.Visibility = Visibility.Collapsed;
            CurrentAccountRadioButton.IsChecked = false;
            SavingsAccountRadioButton.IsChecked = false; 
        }

        private void BalanceTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            var dotCount = 0;
            if (textBox != null)
            {
                foreach (var ch in textBox.Text)
                {
                    if (ch == '.')
                    {
                        dotCount++;
                    }
                }

                if (dotCount > 1)
                {
                    InvalidBalanceTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                    InvalidBalanceTextBlock.Visibility = Visibility.Visible;
                }
            }
        }

        private void Button_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Hand, 1);
        }

        private void Button_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
        }

        //private void SavingsAccountRadioButton_OnLoaded(object sender, RoutedEventArgs e)
        //{
        //    foreach (Ellipse ellipse in FindVisualChild.FindVisualChildren<Ellipse>(this))
        //    {
        //        // do something with tb here
        //        ellipse.Fill = Application.Current.Resources["SystemControlBackgroundAccentBrush"] as SolidColorBrush;
        //    }
        //}
    }
    
    public interface IAccountCreationView
    {
        void SuccessfullyAccountCreated();

    }
}
