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
using ZBMSLibrary.Entities.Model;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBMS.View.UserControl
{
    public sealed partial class TransactionUserControl : Windows.UI.Xaml.Controls.UserControl
    {

        public TransactionUserControl()
        {
            this.InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (TransactionList != null || TransactionList.Count == 0)
            {

            }
            else
            {

            }
        }

        public static readonly DependencyProperty TransactionListProperty = DependencyProperty.Register(
            nameof(TransactionList), typeof(ObservableCollection<TransactionSummary>), typeof(TransactionUserControl), new PropertyMetadata(default(ObservableCollection<TransactionSummary>)));

        public ObservableCollection<TransactionSummary> TransactionList
        {
            get => (ObservableCollection<TransactionSummary>)GetValue(TransactionListProperty);
            set => SetValue(TransactionListProperty, value);
        }


    }
}
