using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.UI.Xaml.Controls;
using ZBMS.View.Pages.AccountsDetailsPage;
using ZBMS.ViewModel;
using ZBMSLibrary.Data.DataManager;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;
using Windows.System;
using Microsoft.Toolkit.Uwp.UI;
using ZBMS.View.UserControl;
using Windows.UI.Xaml.Controls.Maps;
using ZBMS.Util;
using Windows.UI.Xaml.Media.Animation;
using ZBMS.Util.PageArguments;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBMS.View.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AccountsPage : Page, IAccountView
    {
        public AccountPageViewModel AccountViewModel { get; set; }
        private DispatcherTimer _dispatchTimer;
        
        public AccountsPage()
        {
            AccountViewModel = new AccountPageViewModel(this);
            IsAccountPopupDragging = false;
            IsDepositPopupDragging = false;
            IsLoanPopupDragging = false;
            this.InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

            AccountViewModel.GetUserAccounts();
            AccountViewModel.GetUserLastLogged();
            AccountViewModel.GetUser();
            NotificationEvents.SavingsAccountCreated += SavingsAccountCreated;
            NotificationEvents.CurrentAccountCreated += CurrentAccountCreated;
            NotificationEvents.FixedDepositCreated += FixedDepositCreated;
            NotificationEvents.FixedDepositUpdated += FixedDepositUpdated;
            NotificationEvents.RecurringDepositCreated += RecurringDepositCreated;
            NotificationEvents.RecurringDepositUpdated += RecurringDepositUpdated;
            NotificationEvents.PersonalLoanCreated += PersonalLoanCreated;
            NotificationEvents.PersonalLoanUpdated += PersonalLoanUpdated;
            NotificationEvents.DepositSettled += DepositSettled;
            NotificationEvents.MonthlyInstallmentDeposited += MonthlyInstallmentDeposited;
            NotificationEvents.MonthlyInterestCredited += MonthlyInterestCredited;

        }

        private void NoAccountVisibility()
        {
           
        }


        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            NotificationEvents.SavingsAccountCreated -= SavingsAccountCreated;
            NotificationEvents.CurrentAccountCreated -= CurrentAccountCreated;
            NotificationEvents.FixedDepositCreated -= FixedDepositCreated;
            NotificationEvents.FixedDepositUpdated += FixedDepositUpdated;
            NotificationEvents.RecurringDepositCreated -= RecurringDepositCreated;
            NotificationEvents.RecurringDepositUpdated-= RecurringDepositUpdated;
            NotificationEvents.PersonalLoanCreated -= PersonalLoanCreated;
            NotificationEvents.PersonalLoanUpdated -= PersonalLoanUpdated;
            NotificationEvents.DepositSettled -= DepositSettled;
            NotificationEvents.MonthlyInstallmentDeposited -= MonthlyInstallmentDeposited;
            NotificationEvents.MonthlyInterestCredited -= MonthlyInterestCredited;
        }

        private void MonthlyInterestCredited(SavingsAccount savingsAccount, double interestAmount)
        {

            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    if (AccountViewModel.Accounts.FirstOrDefault(x => x.AccountNumber == savingsAccount.AccountNumber) is SavingsAccountBObj account)
                    {
                        account.Balance = account.Balance + interestAmount;
                        account.ToBeCreditedAmount = 0;
                    }
                    InfoBar.Message = "Interest credited for savings account";
                    InfoBar.Severity = InfoBarSeverity.Informational;
                    CreateTimer();
                    InfoBar.IsOpen = true;

                }
            );
        }

        private void RecurringDepositCreated(RecurringAccountBObj recurringDeposit)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    DepositCreationPopup.IsOpen = false;
                    AccountViewModel.Deposits.Insert(0,recurringDeposit);
                    var account = AccountViewModel.Accounts.FirstOrDefault(x => x.AccountNumber == recurringDeposit.FromAccountId);
                    if (account != null)
                    {
                        account.Balance = account.Balance - recurringDeposit.DepositedAmount;
                    }
                    InfoBar.Message = "Recurring Deposit is Successfully Created";
                    CreateTimer();
                    InfoBar.IsOpen = true;
                    NoDepositIcon.Visibility = Visibility.Collapsed;
                    AccountStatusDetailsControl.NewDepositCreated();
                }
            );
        }
        private void RecurringDepositUpdated(RecurringAccountBObj recurringDeposit)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    var recurringAccount = AccountViewModel.Deposits.FirstOrDefault(x => x.AccountNumber == recurringDeposit.AccountNumber) as RecurringAccountBObj;
                    recurringAccount = recurringDeposit;
                }
            );
        }

        private void PersonalLoanCreated(PersonalLoanBObj personalLoanBObj)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    LoanCreationPopup.IsOpen = false;
                    AccountViewModel.Loans.Insert(0,personalLoanBObj);
                    InfoBar.Message = "Personal Loan Account is Successfully Created";
                    CreateTimer();
                    NoLoanIcon.Visibility = Visibility.Collapsed;
                    InfoBar.IsOpen = true;
                    AccountStatusDetailsControl.NewLoanCreated();
                }
            );
        }
        private void PersonalLoanUpdated(PersonalLoanBObj personalLoanBObj)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    if (AccountViewModel.Loans.FirstOrDefault(x => x.AccountNumber == personalLoanBObj.AccountNumber) is PersonalLoanBObj personalLoan)
                    {
                        //account.Balance = account.Balance - fixedDeposit.DepositedAmount;
                        personalLoan = personalLoanBObj;
                    }
                }
            );
        }

        private void FixedDepositCreated(FixedDepositBObj fixedDeposit)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    DepositCreationPopup.IsOpen = false;
                    AccountViewModel.Deposits.Insert(0,fixedDeposit);
                    var account = AccountViewModel.Accounts.FirstOrDefault(x => x.AccountNumber == fixedDeposit.FromAccountId);
                    if (account != null)
                    {
                        account.Balance = account.Balance - fixedDeposit.DepositedAmount;
                    }
                    InfoBar.Message = "Fixed Deposit is Successfully Created";
                    CreateTimer();
                    InfoBar.IsOpen = true;
                    NoDepositIcon.Visibility = Visibility.Collapsed;
                    AccountStatusDetailsControl.NewDepositCreated();

                }
            );
        }
        private void FixedDepositUpdated(FixedDepositBObj fixedDeposit)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    var fixedDepositAccount = AccountViewModel.Deposits.FirstOrDefault(x => x.AccountNumber == fixedDeposit.AccountNumber) as FixedDepositBObj;
                    fixedDepositAccount = fixedDeposit;
                }
            );
        }

        private void DepositSettled(Deposit deposit, double maturityAmount)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    var indexOf =
                        AccountViewModel.Deposits.IndexOf(deposit);
                    AccountViewModel.Deposits[indexOf] = deposit;


                    if (AccountViewModel.Accounts.FirstOrDefault(a => a.AccountNumber == deposit.SavingsAccountId) is SavingsAccountBObj savingsAccount)
                    {
                        savingsAccount.Balance += maturityAmount;
                    }
                    InfoBar.Message = "Deposit Amount is Deposited with interest in your savings Account";
                    InfoBar.Severity = InfoBarSeverity.Informational;
                    CreateTimer();
                    InfoBar.IsOpen = true;
                }
            );
        }


        private void MonthlyInstallmentDeposited(RecurringAccount recurringAccount, double dueAmount)
        {

            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    var indexOf = AccountViewModel.Deposits.IndexOf(recurringAccount);
                    if (indexOf != -1)
                    {
                        
                        AccountViewModel.Deposits[indexOf] = recurringAccount;
                        var account = AccountViewModel.Accounts.FirstOrDefault(x => x.AccountNumber == recurringAccount.FromAccountId);
                        if (account != null)
                        {
                            account.Balance = account.Balance - dueAmount;
                        }
                    }
                    InfoBar.Message = "Monthly installment for recurring deposit is withdrawn from your Account";
                    InfoBar.Severity = InfoBarSeverity.Informational;
                    CreateTimer();
                    InfoBar.IsOpen = true;

                }
            );

        }
        public void CreateTimer()
        {
            // get a timer to close the infobar after 2s
            _dispatchTimer = new DispatcherTimer();
            _dispatchTimer.Tick += DispatcherTimer_Tick; ;
            _dispatchTimer.Interval = new TimeSpan(0, 0, 2);
            _dispatchTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, object e)
        {
            // release the timer
            InfoBar.IsOpen = false;
            _dispatchTimer = null;
        }

        private void CurrentAccountCreated(CurrentAccountBObj currentAccount)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    AccountCreationPopup.IsOpen = false;
                    AccountViewModel.Accounts.Insert(0,currentAccount);
                    InfoBar.Message = "Current Account is Successfully Created";
                    CreateTimer();
                    InfoBar.IsOpen = true;
                    NoAccountIcon.Visibility = Visibility.Collapsed;
                    AccountStatusDetailsControl.NewAccountCreated();

                }
            );
        }

        private void SavingsAccountCreated(SavingsAccountBObj savingsAccount)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                AccountCreationPopup.IsOpen = false;
                AccountViewModel.Accounts.Insert(0, savingsAccount);
                InfoBar.Message = "Savings Account is Successfully Created";
                CreateTimer();
                InfoBar.IsOpen = true;
                NoAccountIcon.Visibility = Visibility.Collapsed;
                AccountStatusDetailsControl.NewAccountCreated();
            });
        }

        private bool IsAccountPopupDragging { get; set; }
        private bool IsDepositPopupDragging{ get; set; }
        private bool IsLoanPopupDragging{ get; set; }

        private void CreateAccountButton_OnClick(object sender, RoutedEventArgs e)
        {
            AccountCreationPopup.IsOpen = true;
            AccountCreationPopup.HorizontalOffset = -250;
            AccountCreationPopup.VerticalOffset = -280;
        }

        private void AccountCreationUserControl_OnClosingPopup()
        {
            AccountCreationPopup.IsOpen = false;
            AccountCreationPopup.HorizontalAlignment = HorizontalAlignment.Center;
            AccountCreationPopup.VerticalAlignment = VerticalAlignment.Center;
            AccountCreationPopup.HorizontalOffset = -250;
            AccountCreationPopup.VerticalOffset = -280;
        }


        private void CreateDepositButton_OnClick(object sender, RoutedEventArgs e)
        {
            DepositCreationPopup.IsOpen = true;
            DepositCreationPopup.HorizontalOffset = -250;
            DepositCreationPopup.VerticalOffset = -380;
        }

        private void CreateLoanButton_OnClick(object sender, RoutedEventArgs e)
        {
            LoanCreationPopup.IsOpen = true;
            LoanCreationPopup.HorizontalOffset = -250;
            LoanCreationPopup.VerticalOffset = -360;
        }

        private void DepositCreationUserControl_OnOnClosingPopup()
        {
            DepositCreationPopup.IsOpen = false;
            DepositCreationPopup.HorizontalAlignment = HorizontalAlignment.Center;
            DepositCreationPopup.VerticalAlignment = VerticalAlignment.Center;
            DepositCreationPopup.HorizontalOffset = -250;
            DepositCreationPopup.VerticalOffset = -360;
        } 
        private void LoanCreationUserControl_OnClosingPopup()
        {
            LoanCreationPopup.IsOpen = false;
            LoanCreationPopup.HorizontalAlignment = HorizontalAlignment.Center;
            LoanCreationPopup.VerticalAlignment = VerticalAlignment.Center;
            LoanCreationPopup.HorizontalOffset = -250;
            LoanCreationPopup.VerticalOffset = -360;
        }

        private void AccountListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var gridView = sender as AdaptiveGridView;

            if (gridView?.SelectedItem is SavingsAccountBObj savingsAccount)
            {
                var savingsAccountPageArgument = new SavingsAccountPageArguments()
                {
                    Accounts = AccountViewModel.Accounts,
                    SavingsAccountBObj = savingsAccount,
                };
                this.Frame.Navigate(typeof(AccountsDetailsPage.SavingsAccountDetailPage), savingsAccountPageArgument);
            }
            else if (gridView?.SelectedItem is CurrentAccountBObj currentAccount)
            {
                var currentAccountPageArgument = new CurrentAccountPageArguments()
                {
                    Accounts = AccountViewModel.Accounts,
                    CurrentAccountBObj = currentAccount,
                };
                this.Frame.Navigate(typeof(AccountsDetailsPage.CurrentAccountDetailsPage), currentAccountPageArgument);
            }
        }

        private void DepositListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var gridView = sender as AdaptiveGridView;
            if (gridView?.SelectedItem is FixedDepositBObj fixedDeposit)
            {
                var fixedDepositDetailPageArgument = new FixedDepositPageArguments()
                {
                    Accounts = AccountViewModel.Accounts,
                    FixedDepositBObj = fixedDeposit
                };
                this.Frame.Navigate(typeof(AccountsDetailsPage.FixedDepositDetailPage), fixedDepositDetailPageArgument);
            }
            else if (gridView?.SelectedItem is RecurringAccountBObj recurringAccount)
            {
                var recurringDepositDetailPageArgument = new RecurringDepositPageArguments()
                {
                    Accounts = AccountViewModel.Accounts,
                    RecurringAccountBObj = recurringAccount
                };
                this.Frame.Navigate(typeof(AccountsDetailsPage.RecurringDepositDetailPage), recurringDepositDetailPageArgument);
            }
        }

        private void LoanListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var gridView = sender as AdaptiveGridView;
            var loanPageArguments = new LoanPageArguments()
            {
                Accounts = AccountViewModel.Accounts,
                PersonalLoanBObj = gridView?.SelectedItem as PersonalLoanBObj,
            };
                this.Frame.Navigate(typeof(AccountsDetailsPage.PersonalLoanDetailPage), loanPageArguments);
            //if (gridView?.SelectedItem is PersonalLoanBObj personalLoanBObj)
            //{

            //}
            //else if (gridView?.SelectedItem is RecurringAccountBObj recurringAccount)
            //{
            //    this.Frame.Navigate(typeof(AccountsDetailsPage.RecurringDepositDetailPage), recurringAccount);
            //}
        }

        private void UIElement_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            var ctrl = Window.Current.CoreWindow.GetKeyState(VirtualKey.Control);
            if (ctrl.HasFlag(CoreVirtualKeyStates.Down) && e.Key == VirtualKey.A)
            {
                if (AccountCreationPopup.IsOpen)
                {
                    //Thumb.COntrolTe()
                    //var template = AccountThumb.Template;
                    //var myControl = (AccountCreationUserControl)template.FindDescendant("AccountCreationUserControl");
                    AccountCreationUserControl.ClearFields();
                    //AccountCreationUserControl.Clea
                }
                AccountCreationPopup.IsOpen = !AccountCreationPopup.IsOpen;
            }
            else if (ctrl.HasFlag(CoreVirtualKeyStates.Down) && e.Key == VirtualKey.D)
            {
                if (DepositCreationPopup.IsOpen)
                {
                    //var template = DepositThumb.Template;
                    //var myControl = (DepositCreationUserControl)template.FindDescendant("DepositCreationUserControl");
                    DepositCreationUserControl?.ClearFields();
                }
                DepositCreationPopup.IsOpen = !DepositCreationPopup.IsOpen;
            }
        }

        private void PreviewPopup_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            IsDepositPopupDragging = false;

            e.Handled = true;
        }
        private void LoanCreationPopup_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            IsLoanPopupDragging = false;

            e.Handled = true;
        }

        private void PreviewPopup_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            IsDepositPopupDragging = true;
            e.Handled = true;
        }
        private void LoanCreationPopup_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            IsLoanPopupDragging = true;
            e.Handled = true;
        }

        private void PreviewPopup_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (IsDepositPopupDragging)
            {
                DepositCreationPopup.HorizontalOffset += e.Delta.Translation.X;
                DepositCreationPopup.VerticalOffset += e.Delta.Translation.Y;
                e.Handled = true;
            }
        }
        private void LoanCreationPopup_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (IsLoanPopupDragging)
            {
                LoanCreationPopup.HorizontalOffset += e.Delta.Translation.X;
                LoanCreationPopup.VerticalOffset += e.Delta.Translation.Y;
                e.Handled = true;
            }
        }

        private void UIElement_OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            IsAccountPopupDragging = false;

            e.Handled = true;
        }

        private void UIElement_OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            IsAccountPopupDragging = true;
            e.Handled = true;
        }

        private void UIElement_OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (IsAccountPopupDragging)
            {
                AccountCreationPopup.HorizontalOffset += e.Delta.Translation.X;
                AccountCreationPopup.VerticalOffset += e.Delta.Translation.Y;
                e.Handled = true;
            }
        }

        

        public void RetrievedAccountSuccessfully()
        {
            NoAccountIcon.Visibility = AccountViewModel.Accounts.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            NoDepositIcon.Visibility = AccountViewModel.Deposits.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            NoLoanIcon.Visibility = AccountViewModel.Loans.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            ConsolidatedDataSheetControl.SetBalanceDetails();
            AccountStatusDetailsControl.SetStatusCounts();
            LoanAccountDetailsControl.SetLoans();
        }

        private void CreateAccount_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is FontIcon icon)
            {
                icon.FontSize = 18;
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Hand, 1);
            }
        }

        private void CreateAccount_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (sender is FontIcon icon)
            {
                icon.FontSize = 16;
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
            }
        }

        private void AccountCard_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);

        }

        private void AccountCard_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Hand, 1);

        }
    }

    public interface IAccountView
    {
        void RetrievedAccountSuccessfully();
    }
}
