using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using System.Transactions;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using ZBMS.Util.PageArguments;
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
    public sealed partial class SavingsAccountDetailPage : Page
    {
        public SavingsAccountDetailViewModel SavingsAccountDetailViewModel { get; set; }
        private DispatcherTimer _dispatchTimer;
        public SavingsAccountDetailPage()
        {
            SavingsAccountDetailViewModel = new SavingsAccountDetailViewModel();
            this.InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            NotificationEvents.UpdateSavingsAccountWithdrawTransaction -= UpdateWithdrawTransaction;
            NotificationEvents.UpdateSavingsAccountDepositTransaction -= UpdateDepositTransaction;
            NotificationEvents.MonthlyRdSavingsTransaction -= UpdateTransaction;
            NotificationEvents.SettlementDepositTransaction -= UpdateTransaction;
            NotificationEvents.RdCreationSavingsTransaction -= UpdateTransaction;
            NotificationEvents.SavingsLoanDuePaidNotification += UpdateTransaction;
            NotificationEvents.FdCreationSavingsTransaction -= UpdateTransaction;
            NotificationEvents.LoanCreationUsingSavingsAccountTransaction -= UpdateTransaction;
            //NotificationEvents.FixedDepositTransaction -= FixedDepositTransaction;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //SavingsAccountDetailViewModel.ClearAndAddTransaction();
            NotificationEvents.UpdateSavingsAccountWithdrawTransaction += UpdateWithdrawTransaction;
            NotificationEvents.UpdateSavingsAccountDepositTransaction += UpdateDepositTransaction;
            NotificationEvents.MonthlyRdSavingsTransaction += UpdateTransaction;
            NotificationEvents.SettlementDepositTransaction += UpdateTransaction;
            NotificationEvents.SavingsLoanDuePaidNotification += UpdateTransaction;
            NotificationEvents.RdCreationSavingsTransaction += UpdateTransaction;
            NotificationEvents.FdCreationSavingsTransaction += UpdateTransaction;
            NotificationEvents.LoanCreationUsingSavingsAccountTransaction += UpdateTransaction;

        }
        public void CreateTimer()
        {
            // get a timer to close the infobar after 2s
            _dispatchTimer = new DispatcherTimer();
            _dispatchTimer.Tick += DispatcherTimer_Tick; ;
            _dispatchTimer.Interval = new TimeSpan(0, 0, 5);
            _dispatchTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, object e)
        {
            // release the timer
            InfoBar.IsOpen = false;
            _dispatchTimer = null;
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
            var savingsAccountPageArguments = e.Parameter as SavingsAccountPageArguments;
            SavingsAccountDetailViewModel.SavingsAccountBObj = savingsAccountPageArguments?.SavingsAccountBObj;
            SavingsAccountDetailViewModel.Accounts = savingsAccountPageArguments?.Accounts;
            SavingsAccountDetailViewModel.ClearAndAddTransaction();
        }



        private void UpdateTransaction(TransactionSummaryVObj transactionSummary)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    //TransactionUserControl.TransactionList.Add(transactionSummary);
                    SavingsAccountDetailViewModel.TransactionList.Insert(0,transactionSummary);
                    SavingsAccountDetailViewModel.SavingsAccountBObj.TransactionList.Insert(0,transactionSummary);
                    //TransactionUserControl.OnTransactionUpdated(transactionSummary);
                }
            );

        }

        private void UpdateDepositTransaction(TransactionSummaryVObj transactionSummary)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    //TransactionUserControl.TransactionList.Add(transactionSummary);
                    SavingsAccountDetailViewModel.TransactionList.Insert(0, transactionSummary);
                    SavingsAccountDetailViewModel.SavingsAccountBObj.TransactionList.Insert(0, transactionSummary);
                    //TransactionUserControl.OnTransactionUpdated(transactionSummary);
                    InfoBar.Severity = InfoBarSeverity.Success;
                    InfoBar.Message = $"Successfully Deposited Rs.{transactionSummary.Amount}";
                    CreateTimer();
                    InfoBar.IsOpen = true;
                }
            );

        }

        private void UpdateWithdrawTransaction(TransactionSummaryVObj transactionSummary)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    //TransactionUserControl.TransactionList.Add(transactionSummary);
                    SavingsAccountDetailViewModel.TransactionList.Insert(0, transactionSummary);
                    SavingsAccountDetailViewModel.SavingsAccountBObj.TransactionList.Insert(0, transactionSummary);
                    //TransactionUserControl.OnTransactionUpdated(transactionSummary);
                    InfoBar.Severity = InfoBarSeverity.Success;
                    InfoBar.Message = $"Successfully Withdrawn Rs.{transactionSummary.Amount}";
                    CreateTimer();
                    InfoBar.IsOpen = true;
                }
            );

        }
        private void UIElement_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Escape)
            {
                e.Handled = true;
                BackButton_OnTapped(sender, new TappedRoutedEventArgs());
                //DepositButton_OnClick(sender, e);
            }
            var ctrl = Window.Current.CoreWindow.GetKeyState(VirtualKey.Control);
            if (ctrl.HasFlag(CoreVirtualKeyStates.Down) && e.Key == VirtualKey.W)
            {
                if (WithdrawButton.Flyout != null)
                {
                    WithdrawButton.Flyout.ShowAt(WithdrawButton);
                }
            }
            else if (ctrl.HasFlag(CoreVirtualKeyStates.Down) && e.Key == VirtualKey.D)
            {
                if (DepositButton.Flyout != null)
                {
                    DepositButton.Flyout.ShowAt(DepositButton);
                }
            }
        }

        private void SavingsAccountSummary_OnWithdrawSuccessNotification(double depositedAmount)
        {
            //NotificationControl.Show($"Successfully Withdrawn the amount {depositedAmount} to your account", 3000);
        }

        private void SavingsAccountSummary_OnDepositSuccessNotification(double depositedAmount)
        {
            //NotificationControl.Show($"Successfully deposited the amount {depositedAmount} to your account", 3000);

        }

        private void WithdrawalUserControl_OnWithDrawZeroWarning()
        {
            InfoBar.Severity = InfoBarSeverity.Warning;
            InfoBar.Message = "can't withdraw Rs.0";
            CreateTimer();
            InfoBar.IsOpen = true;


        }

        private void WithdrawalUserControl_OnWithdrawInsufficientBalanceWarning()
        {
            InfoBar.Severity = InfoBarSeverity.Warning;
            InfoBar.Message = "InSufficient Balance. Kindly check your balance and withdraw";
            CreateTimer();
            InfoBar.IsOpen = true;
        }

        private void DepositMoneyUserControl_OnZeroDepositWarning()
        {
            InfoBar.Severity = InfoBarSeverity.Warning;
            InfoBar.Message = "can't deposit Rs.0";
            CreateTimer();
            InfoBar.IsOpen = true;
        }

        private void TransferMoneyUserControl_OnTransferInsufficientBalanceWarning()
        {
            InfoBar.Severity = InfoBarSeverity.Warning;
            InfoBar.Message = "Insufficient Balance";
            CreateTimer();
            InfoBar.IsOpen = true;
        }

        private void TransferMoneyUserControl_OnTransferSuccess(TransactionSummaryVObj transactionSummaryVObj)
        {
            InfoBar.Severity = InfoBarSeverity.Success;
            InfoBar.Message = $"successfully transferred Rs.{transactionSummaryVObj.Amount}";
            CreateTimer();
            InfoBar.IsOpen = true;
            SavingsAccountDetailViewModel.TransactionList.Insert(0,transactionSummaryVObj);
            SavingsAccountDetailViewModel.SavingsAccountBObj.TransactionList.Insert(0,transactionSummaryVObj);
        }
        private void TransferMoneyUserControl_OnZeroDepositWarning()
        {
            InfoBar.Severity = InfoBarSeverity.Warning;
            InfoBar.Message = "can't transfer Rs.0";
            CreateTimer();
            InfoBar.IsOpen = true;
        }

        private void TransactionLimitReached()
        {
            InfoBar.Severity = InfoBarSeverity.Warning;
            InfoBar.Message = "transaction limit reached for today";
            CreateTimer();
            InfoBar.IsOpen = true;
        }

        private void TransferMoneyUserControl_OnTransferLimitExceededWarning()
        {
            TransactionLimitReached();
        }

        private void WithdrawalUserControl_OnTransactionSavingsLimitExceeded()
        {
            TransactionLimitReached();

        }

        private void DepositMoneyUserControl_OnTransactionLimitExceeded()
        {
            TransactionLimitReached();
        }

        private void Button_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Hand, 1);
        }

        private void Button_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
        }

        private void BackButton_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (sender is FontIcon icon)
            {
                icon.FontSize = 19;
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
            }
        }

        private void BackButton_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is FontIcon icon)
            {
                icon.FontSize = 21;
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Hand, 1);
            }
        }
    }
}
