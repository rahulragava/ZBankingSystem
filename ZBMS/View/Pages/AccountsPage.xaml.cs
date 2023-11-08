﻿using System;
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
            NotificationEvents.RecurringDepositCreated += RecurringDepositCreated;
            NotificationEvents.DepositSettled += DepositSettled;
            NotificationEvents.MonthlyInstallmentDeposited += MonthlyInstallmentDeposited;
        }


        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            NotificationEvents.SavingsAccountCreated -= SavingsAccountCreated;
            NotificationEvents.CurrentAccountCreated -= CurrentAccountCreated;
            NotificationEvents.FixedDepositCreated -= FixedDepositCreated;
            NotificationEvents.RecurringDepositCreated -= RecurringDepositCreated;
            NotificationEvents.DepositSettled -= DepositSettled;
            NotificationEvents.MonthlyInstallmentDeposited -= MonthlyInstallmentDeposited;
        }

        private void RecurringDepositCreated(RecurringAccountBObj recurringDeposit)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    DepositCreationPopup.IsOpen = false;
                    AccountViewModel.Deposits.Add(recurringDeposit);
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

        private void FixedDepositCreated(FixedDepositBObj fixedDeposit)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    DepositCreationPopup.IsOpen = false;
                    AccountViewModel.Deposits.Add(fixedDeposit);
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
                    AccountViewModel.Accounts.Add(currentAccount);
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
                AccountViewModel.Accounts.Add(savingsAccount);
                InfoBar.Message = "Savings Account is Successfully Created";
                CreateTimer();
                InfoBar.IsOpen = true;
            });
        }

        public bool IsAccountPopupDragging { get; set; }
        public bool IsDepositPopupDragging{ get; set; }

        private void CreateAccountButton_OnClick(object sender, RoutedEventArgs e)
        {
            AccountCreationPopup.IsOpen = !(AccountCreationPopup.IsOpen);
        }

        private void AccountCreationUserControl_OnClosingPopup()
        {
            AccountCreationPopup.IsOpen = false;
        }


        private void CreateDepositButton_OnClick(object sender, RoutedEventArgs e)
        {
            DepositCreationPopup.IsOpen = true;
        }

        private void CreateLoanButton_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void DepositCreationUserControl_OnOnClosingPopup()
        {
            DepositCreationPopup.IsOpen = false;

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
           
            IsDepositPopupDragging = false;

            e.Handled = true;
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
                this.Frame.Navigate(typeof(AccountsDetailsPage.FixedDepositDetailPage), fixedDeposit);
            }
            else if (gridView?.SelectedItem is RecurringAccountBObj recurringAccount)
            {
                this.Frame.Navigate(typeof(AccountsDetailsPage.RecurringDepositDetailPage), recurringAccount);
            }
        }
    }

    public interface IAccountView
    {

    }
}
