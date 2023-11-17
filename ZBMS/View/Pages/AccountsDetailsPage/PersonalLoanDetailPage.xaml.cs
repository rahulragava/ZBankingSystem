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
using ZBMS.Util;
using ZBMS.View.UserControl;
using ZBMS.ViewModel.DetailViewModel;
using ZBMSLibrary.Data.DataManager;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Enums;
using ZBMSLibrary.Entities.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBMS.View.Pages.AccountsDetailsPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PersonalLoanDetailPage : Page
    {
        public PersonalLoanAccountDetailViewModel PersonalLoanAccountDetailViewModel { get; set; }
        public PersonalLoanDetailPage()
        {
            PersonalLoanAccountDetailViewModel = new PersonalLoanAccountDetailViewModel();
            this.InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += PersonalLoanDetailPage_Unloaded;
        }

        private void PersonalLoanDetailPage_Unloaded(object sender, RoutedEventArgs e)
        {
            NotificationEvents.PersonalLoanUpdated -= PersonalLoanUpdated;

        }

        private void PersonalLoanUpdated(PersonalLoanBObj personalLoan)
        {
            AmountPaidTillNow += PersonalLoanAccountDetailViewModel.NextMonthDueAmount;
            if (personalLoan.AccountStatus == AccountStatus.Closed)
            {
                TextBlock1.Visibility = Visibility.Collapsed;
                NextDueDateTextBlock.Visibility = Visibility.Collapsed;
                PersonalLoanAccountDetailViewModel.NextMonthDueAmount = 0;
            }
            PersonalLoanAccountDetailViewModel.PersonalLoanBObj.NextDateToBePaid = personalLoan.NextDateToBePaid;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            NotificationEvents.PersonalLoanUpdated += PersonalLoanUpdated;
            PersonalLoanAccountDetailViewModel.GetNextMonthDueAmount();
            AmountPaidTillNow = PersonalLoanAccountDetailViewModel.PersonalLoanBObj.GetTotalAmount() - PersonalLoanAccountDetailViewModel.PersonalLoanBObj.DueWithInterestAmount;
            MaturityDate = PersonalLoanAccountDetailViewModel.PersonalLoanBObj.MaturityDateCalculator(PersonalLoanAccountDetailViewModel.PersonalLoanBObj.CreatedOn,PersonalLoanAccountDetailViewModel.PersonalLoanBObj.Tenure*12);
        }

        public static readonly DependencyProperty AmountPaidTillNowProperty= DependencyProperty.Register(
            nameof(AmountPaidTillNow), typeof(double), typeof(PersonalLoanDetailPage), new PropertyMetadata(default(double)));

        public double AmountPaidTillNow
        {
            get => (double)GetValue(AmountPaidTillNowProperty);
            set => SetValue(AmountPaidTillNowProperty, value);
        }

        public static readonly DependencyProperty MaturityDateProperty = DependencyProperty.Register(
            nameof(MaturityDate), typeof(DateTime), typeof(PersonalLoanDetailPage), new PropertyMetadata(default(DateTime)));

        public DateTime MaturityDate
        {
            get => (DateTime)GetValue(MaturityDateProperty);
            set => SetValue(MaturityDateProperty, value);
        }

        public static readonly DependencyProperty AccountListProperty = DependencyProperty.Register(
            nameof(AccountList), typeof(ObservableCollection<Account>), typeof(PersonalLoanDetailPage), new PropertyMetadata(default(ObservableCollection<Account>)));

        public ObservableCollection<Account> AccountList
        {
            get => (ObservableCollection<Account>)GetValue(AccountListProperty);
            set => SetValue(AccountListProperty, value);
        }

        private void BackButton_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var loanArguments = e.Parameter as LoanPageArguments;

            var personalLoanBObj = loanArguments?.PersonalLoanBObj;
            var accounts = loanArguments?.Accounts;
            PersonalLoanAccountDetailViewModel.PersonalLoanBObj= personalLoanBObj;
            PersonalLoanAccountDetailViewModel.Accounts = accounts;
        }

        private void UIElement_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Escape)
            {
                e.Handled = true;
                BackButton_OnTapped(sender, new TappedRoutedEventArgs());
                //DepositButton_OnClick(sender, e);
            }
        }
    }
}
