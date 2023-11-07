using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Transactions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZBMSLibrary.Entities.Enums;
using ZBMSLibrary.Entities.Model;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBMS.View.UserControl
{
    public sealed partial class TransactionDataControl : Windows.UI.Xaml.Controls.UserControl
    {
        public TransactionDataControl()
        {
            this.InitializeComponent();
        }

        //public static readonly DependencyProperty TransactionListProperty = DependencyProperty.Register(
        //    nameof(TransactionList), typeof(ObservableCollection<TransactionSummary>), typeof(TransactionDataControl), new PropertyMetadata(default(ObservableCollection<TransactionSummary>)));

        //public ObservableCollection<TransactionSummary> TransactionList
        //{
        //    get => (ObservableCollection<TransactionSummary>)GetValue(TransactionListProperty);
        //    set => SetValue(TransactionListProperty, value);
        //}

        public static readonly DependencyProperty TransactionAmountProperty = DependencyProperty.Register(
            nameof(TransactionAmount), typeof(double), typeof(TransactionDataControl), new PropertyMetadata(default(double)));

        public double TransactionAmount
        {
            get => (double)GetValue(TransactionAmountProperty);
            set => SetValue(TransactionAmountProperty, value);
        }


        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(
            nameof(Status), typeof(TransactionType), typeof(TransactionDataControl), new PropertyMetadata(default(TransactionType)));

        public TransactionType Status
        {
            get => (TransactionType)GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }

        public static readonly DependencyProperty TransactionOnProperty = DependencyProperty.Register(
            nameof(TransactionOn), typeof(DateTime), typeof(TransactionDataControl), new PropertyMetadata(default(DateTime)));

        public DateTime TransactionOn
        {
            get => (DateTime)GetValue(TransactionOnProperty);
            set => SetValue(TransactionOnProperty, value);
        }

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            nameof(Description), typeof(string), typeof(TransactionDataControl), new PropertyMetadata(default(string)));

        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public static readonly DependencyProperty ReceiverAccountNumberProperty = DependencyProperty.Register(
            nameof(ReceiverAccountNumber), typeof(string), typeof(TransactionDataControl), new PropertyMetadata(default(string)));

        public string ReceiverAccountNumber
        {
            get => (string)GetValue(ReceiverAccountNumberProperty);
            set => SetValue(ReceiverAccountNumberProperty, value);
        }

        public static readonly DependencyProperty SenderAccountNumberProperty = DependencyProperty.Register(
            nameof(SenderAccountNumber), typeof(string), typeof(TransactionDataControl), new PropertyMetadata(default(string)));

        public string SenderAccountNumber
        {
            get => (string)GetValue(SenderAccountNumberProperty);
            set => SetValue(SenderAccountNumberProperty, value);
        }

        public static readonly DependencyProperty IdProperty = DependencyProperty.Register(
            nameof(Id), typeof(string), typeof(TransactionDataControl), new PropertyMetadata(default(string)));

        public string Id
        {
            get => (string)GetValue(IdProperty);
            set => SetValue(IdProperty, value);
        }

        public static readonly DependencyProperty UserNameProperty = DependencyProperty.Register(
            nameof(UserName), typeof(string), typeof(TransactionDataControl), new PropertyMetadata(default(string)));

        public string UserName
        {
            get => (string)GetValue(UserNameProperty);
            set => SetValue(UserNameProperty, value);
        }
        public event Action GoBack;

        private void GoBackButton_OnClick(object sender, RoutedEventArgs e)
        {
            GoBack?.Invoke();
        }


        public void PaneViewModeChange(TwoPaneView sender, object obj)
        {
            GoBackButton.Visibility = sender.Mode == (TwoPaneViewMode)Microsoft.UI.Xaml.Controls.TwoPaneViewMode.SinglePane ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
