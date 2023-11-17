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
                    InfoBar.IsOpen = true;
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
            });
        }

        public bool IsAccountPopupDragging { get; set; }
        public bool IsDepositPopupDragging{ get; set; }
        public bool IsLoanPopupDragging{ get; set; }

        private void CreateAccountButton_OnClick(object sender, RoutedEventArgs e)
        {
            AccountCreationPopup.IsOpen = true;
            AccountCreationPopup.HorizontalOffset = 0;
            AccountCreationPopup.VerticalOffset = 0;
        }

        private void AccountCreationUserControl_OnClosingPopup()
        {
            AccountCreationPopup.IsOpen = false;
        }


        private void CreateDepositButton_OnClick(object sender, RoutedEventArgs e)
        {
            DepositCreationPopup.IsOpen = true; 
            DepositCreationPopup.HorizontalOffset = 0;
            DepositCreationPopup.VerticalOffset = 0;
        }

        private void CreateLoanButton_OnClick(object sender, RoutedEventArgs e)
        {
            LoanCreationPopup.IsOpen = true;
            LoanCreationPopup.HorizontalOffset = 0;
            LoanCreationPopup.VerticalOffset = 0;
        }

        private void DepositCreationUserControl_OnOnClosingPopup()
        {
            DepositCreationPopup.IsOpen = false;
            DepositCreationPopup.HorizontalOffset = 0;
            DepositCreationPopup.VerticalOffset = 0;
        } 
        private void LoanCreationUserControl_OnClosingPopup()
        {
            LoanCreationPopup.IsOpen = false;
            LoanCreationPopup.HorizontalOffset = 0;
            LoanCreationPopup.VerticalOffset = 0;
        }

        private void DepositCreationPopup_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            IsDepositPopupDragging = true;
            e.Handled = true;
        }
        private void AccountCreationPopup_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            IsAccountPopupDragging = true;
            e.Handled = true;
        }

        private void DepositCreationPopup_OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
           
            
        }
        private void AccountCreationPopup_OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            IsAccountPopupDragging = false;

            e.Handled = true;
        }
        private void DepositCreationPopup_OnPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (IsDepositPopupDragging)
            {

                var pointerPosition = Windows.UI.Core.CoreWindow.GetForCurrentThread().PointerPosition;
                var x = pointerPosition.X - Window.Current.Bounds.X - 84;
                var y = pointerPosition.Y - Window.Current.Bounds.Y - 37;
                //change position
                DepositCreationPopup.HorizontalOffset = x;
                DepositCreationPopup.VerticalOffset = y;
            }
            //get pointer position


            e.Handled = true;
        }
        private void AccountCreationPopup_OnPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (IsAccountPopupDragging)
            {

                var pointerPosition = Windows.UI.Core.CoreWindow.GetForCurrentThread().PointerPosition;
                var x = pointerPosition.X - Window.Current.Bounds.X - 84;
                var y = pointerPosition.Y - Window.Current.Bounds.Y - 37;
                //change position
                AccountCreationPopup.HorizontalOffset = x;
                AccountCreationPopup.VerticalOffset = y;
            }
            //get pointer position


            e.Handled = true;
        }

        private void AccountListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var gridView = sender as AdaptiveGridView;

            if (gridView?.SelectedItem is SavingsAccountBObj savingsAccount)
            {
                this.Frame.Navigate(typeof(AccountsDetailsPage.SavingsAccountDetailPage), savingsAccount);
            }
            else if (gridView?.SelectedItem is CurrentAccountBObj currentAccount)
            {
                this.Frame.Navigate(typeof(AccountsDetailsPage.CurrentAccountDetailsPage), currentAccount);
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

        private void UIElement_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CardNameBoard_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //var dataTemplate = Application.Current.Resources["SavingsAccountCardTemplate"] as DataTemplate;
            //dataTemplate.GetElement()
            //MyStoryboard1.Stop();
            //MyStoryboard.Begin();
            //var stackPanel = sender as Button;
            //var WidthAnimation = new DoubleAnimation() { To = 130, Duration = TimeSpan.FromSeconds(0.3) };
            //var HeightAnimation = new DoubleAnimation() { To = 150, Duration = TimeSpan.FromSeconds(0.3) };

            //stackPanel.StartAnimation(StackPanel., WidthAnimation);
            //stackPanel.BeginAnimation(StackPanel.HeightProperty, HeightAnimation);
        }

        private void CardNameBoard_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            //var dataTemplate = Application.Current.Resources["SavingsAccountCardTemplate"] as DataTemplate;
            //MyStoryboard1.Begin();
        }
    }

    //public class PopupBehaviours
    //{
    //    #region IsMoveEnabled DP
    //    public static Boolean GetIsMoveEnabledProperty(DependencyObject obj)
    //    {
    //        return (Boolean)obj.GetValue(IsMoveEnabledPropertyProperty);
    //    }

    //    public static void SetIsMoveEnabledProperty(DependencyObject obj,
    //                                                Boolean value)
    //    {
    //        obj.SetValue(IsMoveEnabledPropertyProperty, value);
    //    }

    //    // Using a DependencyProperty as the backing store for 
    //    //IsMoveEnabledProperty. 
    //    public static readonly DependencyProperty IsMoveEnabledPropertyProperty =
    //        DependencyProperty.RegisterAttached("IsMoveEnabledProperty",
    //        typeof(Boolean), typeof(PopupBehaviours),
    //        new PropertyMetadata(false, OnIsMoveStatedChanged));


    //    private static void OnIsMoveStatedChanged(DependencyObject sender,
    //        DependencyPropertyChangedEventArgs e)
    //    {
    //        Thumb thumb = (Thumb)sender;

    //        if (thumb == null) return;

    //        thumb.DragStarted -= Thumb_DragStarted;
    //        thumb.DragDelta -= Thumb_DragDelta;
    //        thumb.DragCompleted -= Thumb_DragCompleted;

    //        if (e.NewValue is bool)
    //        {
    //            thumb.DragStarted += Thumb_DragStarted;
    //            thumb.DragDelta += Thumb_DragDelta;
    //            thumb.DragCompleted += Thumb_DragCompleted;
    //        }

    //    }
    //    #endregion

    //    #region Private Methods
    //    private static void Thumb_DragCompleted(object sender,
    //        DragCompletedEventArgs e)
    //    {
    //        Thumb thumb = (Thumb)sender;
    //        //thumb.C = null;
    //    }

    //    private static void Thumb_DragDelta(object sender,
    //    DragDeltaEventArgs e)
    //    {
    //        Thumb thumb = (Thumb)sender;
    //        Popup popup = thumb.Tag as Popup;

    //        if (popup != null)
    //        {
    //            popup.HorizontalOffset += e.HorizontalChange;
    //            popup.VerticalOffset += e.VerticalChange;
    //        }
    //    }

    //    private static void Thumb_DragStarted(object sender,
    //    DragStartedEventArgs e)
    //    {
    //        Thumb thumb = (Thumb)sender;
    //        //thumb.Cursor = Cursors.Hand;
    //    }
    //    #endregion

    //}

    public interface IAccountView
    {

    }
}
