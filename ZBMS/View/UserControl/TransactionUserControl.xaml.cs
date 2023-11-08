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
using Microsoft.Toolkit.Uwp.UI.Controls;
using ZBMS.ViewModel;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBMS.View.UserControl
{
    public sealed partial class TransactionUserControl : Windows.UI.Xaml.Controls.UserControl
    {
        public TransactionViewModel TransactionViewModel;

        public TransactionUserControl()
        {
            TransactionViewModel = new TransactionViewModel();
            this.InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //TransactionListDataGrid.SelectedItem = 
            //PreviousPageButton.IsEnabled = false;
            //TransactionViewModel.AllTransactionSummaries = TransactionList;
            //TransactionViewModel.InitialValues();
            if (TransactionList.Count > 0)
            {
                TransactionListDataGrid.SelectedItem = TransactionList[0];
            }
            if (TransactionViewModel.CurrentPage == TransactionViewModel.TotalPages)
            {
                //NextPageButton.IsEnabled = false;
                //LastPageButton.IsEnabled = false;
                //FirstPageButton.IsEnabled = false;
                //PreviousPageButton.IsEnabled = false;
            }
        }

        public static readonly DependencyProperty TransactionListProperty = DependencyProperty.Register(
            nameof(TransactionList), typeof(ObservableCollection<TransactionSummaryVObj>), typeof(TransactionUserControl), new PropertyMetadata(default(ObservableCollection<TransactionSummaryVObj>)));

        public ObservableCollection<TransactionSummaryVObj> TransactionList
        {
            get => (ObservableCollection<TransactionSummaryVObj>)GetValue(TransactionListProperty);
            set => SetValue(TransactionListProperty, value);
        }

    

        private void TransactionListDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            PaneView.PanePriority = TwoPaneViewPriority.Pane2;
            if (dataGrid != null)
            {
                var transaction = dataGrid.SelectedItem as TransactionSummaryVObj;
                TransactionViewModel.TransactionSummary = transaction;
                if (transaction != null)
                {
                    TransactionViewModel.Id = transaction.Id;
                    TransactionViewModel.Amount = transaction.Amount;
                    TransactionViewModel.Description = transaction.Description;
                    TransactionViewModel.TransactionType = transaction.TransactionType;
                    TransactionViewModel.TransactionOn = transaction.TransactionOn;
                    TransactionViewModel.SenderAccountNumber = transaction.SenderAccountNumber;
                    TransactionViewModel.ReceiverAccountNumber = transaction.ReceiverAccountNumber;
                    TransactionViewModel.UserName = transaction.UserName;
                }
            }
        }

        //public event Action<TwoPaneView,object> OnPaveViewModeChange;
        private void PaneView_OnModeChanged(TwoPaneView sender, object args)
        {
            DetailsContent.PaneViewModeChange(sender, args);
            
            //if (sender.Mode == Windows.UI.Xaml.Controls.TwoPaneViewMode.SinglePane)
            //{
            //}
            //else
            //{
            //    sender.PanePriority = TwoPaneViewPriority.Pane2;
            //}
            //OnPaveViewModeChange?.Invoke(sender,args);
        }

        public void OnTransactionUpdated(TransactionSummaryVObj transaction)
        {
            //if (TransactionViewModel.CurrentPage == 1)
            //{
            //    TransactionViewModel.PageTransactionSummaries.Insert(0,transaction);
            //    if (TransactionViewModel.PageTransactionSummaries.Count >= TransactionViewModel.NumberOfRowsPerPage)
            //    {
            //        TransactionViewModel.PageTransactionSummaries.RemoveAt(TransactionViewModel.PageTransactionSummaries.Count - 1);
            //    }
            //    TransactionViewModel.TotalPages = TransactionViewModel.AllTransactionSummaries.Count / TransactionViewModel.NumberOfRowsPerPage;
            //    if (TransactionViewModel.TotalPages == 0)
            //    {
            //        TransactionViewModel.TotalPages = 1;
            //        //NextPageButton.IsEnabled = false;
            //        //LastPageButton.IsEnabled = false;
            //        //PreviousPageButton.IsEnabled = false;
            //        //FirstPageButton.IsEnabled = false;
            //    }
            //    if (TransactionViewModel.TotalPages > 1)
            //    {
            //        //NextPageButton.IsEnabled = true;
            //        //LastPageButton.IsEnabled = true;
                    
            //    }
            //}
        }

        private void DetailsContent_OnGoBack()
        {
            PaneView.PanePriority = TwoPaneViewPriority.Pane1;
            
        }


        //private void NextPageButton_OnClick(object sender, RoutedEventArgs e)
        //{
        //    PreviousPageButton.IsEnabled = true;
        //    NextPageButton.IsEnabled = TransactionViewModel.CurrentPage + 2 <= TransactionViewModel.TotalPages;
        //    TransactionViewModel.NextPage();
        //}

        //private void PreviousPageButton_OnClick(object sender, RoutedEventArgs e)
        //{
        //    NextPageButton.IsEnabled = true;
        //    PreviousPageButton.IsEnabled = TransactionViewModel.CurrentPage - 2 > 0;
        //    TransactionViewModel.PreviousPage();
        //}

        //private void FirstPageButton_OnClick(object sender, RoutedEventArgs e)
        //{
        //    PreviousPageButton.IsEnabled = false;
        //    NextPageButton.IsEnabled = true;
        //    FirstPageButton.IsEnabled = false;
        //    LastPageButton.IsEnabled = true;
        //    TransactionViewModel.FirstPage();
        //}

        //private void LastPageButton_OnClick(object sender, RoutedEventArgs e)
        //{
        //    NextPageButton.IsEnabled = false;
        //    PreviousPageButton.IsEnabled = true;
        //    LastPageButton.IsEnabled = false;
        //    FirstPageButton.IsEnabled = true;
        //    TransactionViewModel.LastPage();
        //}
    }
}
