﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using ZBMS.View.UserControl.DepositSummary;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Enums;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBMS.View.UserControl.CardTemplates
{
    public sealed partial class SavingsAccountCardUserControl : Windows.UI.Xaml.Controls.UserControl
    {
        public SavingsAccountCardUserControl()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty AccountNumberProperty =
            DependencyProperty.Register(nameof(AccountNumber), typeof(string), typeof(SavingsAccountCardUserControl),
                new PropertyMetadata(default(string)));

        public string AccountNumber
        {
            get => (string)GetValue(AccountNumberProperty);
            set => SetValue(AccountNumberProperty, value);
        }

        public static readonly DependencyProperty AccountBalanceProperty =
            DependencyProperty.Register(nameof(AccountBalance), typeof(double), typeof(SavingsAccountCardUserControl),
                new PropertyMetadata(default(double)));

        public double AccountBalance
        {
            get => Math.Round((double)GetValue(AccountBalanceProperty),2);
            set => SetValue(AccountBalanceProperty, value);
        }

        public double GetAccountBalance()
        {
            return Math.Round(AccountBalance, 2);
        }
        public static readonly DependencyProperty AccountStatusProperty =
            DependencyProperty.Register(nameof(AccountStatus), typeof(AccountStatus), typeof(SavingsAccountCardUserControl),
                new PropertyMetadata(default(AccountStatus)));

        public AccountStatus AccountStatus
        {
            get => (AccountStatus)GetValue(AccountStatusProperty);
            set => SetValue(AccountStatusProperty, value);
        }

        private void CardNameBoard_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            MyStoryboard1.Stop();
            MyStoryboard.Begin();
            DetailView.Visibility = Visibility.Visible;
            CardName.Visibility = Visibility.Collapsed;
            e.Handled = true;
        }


        private void CardNameBoard_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            MyStoryboard1.Begin();
            CardName.Visibility = Visibility.Visible;
            DetailView.Visibility = Visibility.Collapsed;
        }

        private void Timeline_OnCompleted(object sender, object e)
        {
            DetailTextBlock.Visibility = Visibility.Visible;
        }
    }
}
