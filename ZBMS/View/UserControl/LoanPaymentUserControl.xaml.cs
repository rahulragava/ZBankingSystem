using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class LoanPaymentUserControl : Windows.UI.Xaml.Controls.UserControl, ILoanPaymentView
    {
        public LoanPaymentViewModel LoanPaymentViewModel;
        public LoanPaymentUserControl()
        {
            LoanPaymentViewModel = new LoanPaymentViewModel(this);
            this.InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            LoanPaymentViewModel.SetAccountNames(AccountList);
            LoanPaymentViewModel.DueCalculator(PersonalLoanBObj);
            if (LoanPaymentViewModel.DueAmount == 0)
            {
                LoanPayButton.IsEnabled = false;
            }
            else
            {
                LoanPayButton.IsEnabled = true;
            }
            LoanedAmountGoesToAccountNumber.SelectedIndex = 0;
        }

        public void LoanPaymentFailedDueToAccountInsufficientAmount()
        {
            ErrorTextBlock.Visibility = Visibility.Visible;
            ErrorTextBlock.Foreground = new SolidColorBrush(Colors.Red);

        }

        public void LoanPaymentSuccessful()
        {
            ErrorTextBlock.Visibility = Visibility.Collapsed;
        }


        public static readonly DependencyProperty AccountListProperty = DependencyProperty.Register(
            nameof(AccountList), typeof(ObservableCollection<Account>), typeof(LoanPaymentUserControl), new PropertyMetadata(default(ObservableCollection<Account>)));

        public ObservableCollection<Account> AccountList
        {
            get => (ObservableCollection<Account>)GetValue(AccountListProperty);
            set => SetValue(AccountListProperty, value);
        }

        public static readonly DependencyProperty PersonalLoanBObjProperty = DependencyProperty.Register(
            nameof(PersonalLoanBObj), typeof(PersonalLoanBObj), typeof(LoanPaymentUserControl), new PropertyMetadata(default(PersonalLoanBObj)));

        public PersonalLoanBObj PersonalLoanBObj
        {
            get => (PersonalLoanBObj)GetValue(PersonalLoanBObjProperty);
            set => SetValue(PersonalLoanBObjProperty, value);
        }

        private void LoanPayButton_OnClick(object sender, RoutedEventArgs e)
        {
            LoanPaymentViewModel.LoanDuePayment(PersonalLoanBObj,LoanedAmountGoesToAccountNumber.SelectedItem as string);
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

    public interface ILoanPaymentView
    {
        void LoanPaymentFailedDueToAccountInsufficientAmount();
        void LoanPaymentSuccessful();
    }
}
