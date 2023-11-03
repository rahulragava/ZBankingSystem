﻿using System;
using System.Collections.Generic;
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
using ZBMSLibrary.Data.DataManager;
using ZBMSLibrary.Entities.BusinessObject;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBMS.View.UserControl.AccountSummary
{
    public sealed partial class CurrentAccountSummary : Windows.UI.Xaml.Controls.UserControl
    {
        public CurrentAccountSummary()
        {
            this.InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {

            NotificationEvents.DepositCurrentAmountUpdation -= UpdateBalance;
            NotificationEvents.WithdrawCurrentAccountAmountUpdation -= WithdrawAmountUpdated;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            NotificationEvents.DepositCurrentAmountUpdation += UpdateBalance;
            NotificationEvents.WithdrawCurrentAccountAmountUpdation += WithdrawAmountUpdated;
        }

        private void WithdrawAmountUpdated(double depositedAmount)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    CurrentAccountBObj.Balance -= depositedAmount;
                }
            );
        }

        private void UpdateBalance(double depositedAmount)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    CurrentAccountBObj.Balance += depositedAmount;
                }
            );  
        }

        public static readonly DependencyProperty CurrentAccountBObjProperty =
            DependencyProperty.Register(nameof(CurrentAccountBObj), typeof(CurrentAccountBObj), typeof(CurrentAccountSummary),
                new PropertyMetadata(null));

        public CurrentAccountBObj CurrentAccountBObj
        {
            get => (CurrentAccountBObj)GetValue(CurrentAccountBObjProperty);
            set => SetValue(CurrentAccountBObjProperty, value);
        }
        //public static readonly DependencyProperty BalanceProperty =
        //    DependencyProperty.Register(nameof(Balance), typeof(double), typeof(CurrentAccountSummary),
        //        new PropertyMetadata(0.0));

        //public double Balance
        //{
        //    get => (double)GetValue(BalanceProperty);
        //    set => SetValue(BalanceProperty, value);
        //}
    }
}
