using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using ZBMS.View.UserControl;
using ZBMS.ViewModel.DetailViewModel;
using ZBMSLibrary.Data.DataManager;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBMS.View.Pages.AccountsDetailsPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CurrentAccountDetailsPage : Page
    {
        public CurrentAccountDetailViewModel CurrentAccountDetailsViewModel { get; set; }
        public CurrentAccountDetailsPage()
        {
            CurrentAccountDetailsViewModel = new CurrentAccountDetailViewModel();
            this.InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            NotificationEvents.UpdateCurrentAccountWithdrawTransaction -= UpdateTransaction;

            NotificationEvents.UpdateCurrentAccountDepositTransaction -= UpdateTransaction;
            NotificationEvents.MonthlyRdCurrentTransaction -= UpdateTransaction;
            NotificationEvents.RdCreationCurrentTransaction -= UpdateTransaction;
            NotificationEvents.FdCreationCurrentTransaction -= UpdateTransaction;

        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            NotificationEvents.UpdateCurrentAccountWithdrawTransaction += UpdateTransaction;

            NotificationEvents.UpdateCurrentAccountDepositTransaction += UpdateTransaction;
            NotificationEvents.MonthlyRdCurrentTransaction += UpdateTransaction;
            NotificationEvents.RdCreationCurrentTransaction += UpdateTransaction;
            NotificationEvents.FdCreationCurrentTransaction += UpdateTransaction;
        }

        private void UpdateTransaction(TransactionSummary transactionSummary)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    //TransactionUserControl.TransactionList.Add(transactionSummary);
                    CurrentAccountDetailsViewModel.TransactionList.Insert(0,transactionSummary);
                    TransactionUserControl.OnTransactionUpdated(transactionSummary);
                }
            );

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            base.OnNavigatedTo(e);
            var currentAccountBObj = e.Parameter as CurrentAccountBObj;
            CurrentAccountDetailsViewModel.CurrentAccountBObj = currentAccountBObj;
            CurrentAccountDetailsViewModel.ClearAndAddTransaction();

        }

        private void BackButton_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        //private void DepositMoneyUserControl_OnUpdateBalanceInParentUserControls(double amount)
        //{
        //    CurrentAccountDetailsViewModel.CurrentAccountBObj.Balance += amount;
        //}
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
