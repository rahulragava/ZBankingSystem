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
using ZBMSLibrary.Data.DataManager;
using ZBMSLibrary.Entities.BusinessObject;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBMS.View.UserControl.AccountSummary
{
    public sealed partial class SavingsAccountSummary : Windows.UI.Xaml.Controls.UserControl
    {
        public SavingsAccountSummary()
        {
            this.InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            NotificationEvents.DepositSavingsAccountAmountUpdation -= DepositAmountUpdated;
            NotificationEvents.WithdrawSavingsAccountAmountUpdation -= WithdrawAmountUpdated;
            NotificationEvents.TransferSavingsAccountBalanceUpdation -= UpdateBalance;

        }


        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            NotificationEvents.DepositSavingsAccountAmountUpdation += DepositAmountUpdated;
            NotificationEvents.WithdrawSavingsAccountAmountUpdation += WithdrawAmountUpdated;
            NotificationEvents.TransferSavingsAccountBalanceUpdation += UpdateBalance;
        }

        public event Action<double> WithdrawSuccessNotification;
        public event Action<double> DepositSuccessNotification;
        private void WithdrawAmountUpdated(double depositedAmount)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    SavingsAccountBObj.Balance -= depositedAmount;
                    Debug.WriteLine(10);
                    //Balance = Balance - depositedAmount;
                    WithdrawSuccessNotification?.Invoke(depositedAmount);

                }
            );
        }

        private void DepositAmountUpdated(double depositedAmount)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
              
                        SavingsAccountBObj.Balance += depositedAmount;
                        DepositSuccessNotification?.Invoke(depositedAmount);
                    
                
                }
            );
        }

        private void UpdateBalance(double transferredAmount)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                        SavingsAccountBObj.Balance -= transferredAmount;
                    
                }
            );
        }

        public static readonly DependencyProperty SavingsAccountBObjProperty =
            DependencyProperty.Register(nameof(SavingsAccountBObj), typeof(SavingsAccountBObj), typeof(SavingsAccountSummary),
                new PropertyMetadata(null));

        public SavingsAccountBObj SavingsAccountBObj
        {
            get => (SavingsAccountBObj)GetValue(SavingsAccountBObjProperty);
            set => SetValue(SavingsAccountBObjProperty, value);
        }

    }
}

