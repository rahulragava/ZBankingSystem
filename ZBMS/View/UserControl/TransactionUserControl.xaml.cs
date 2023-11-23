using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        public readonly TransactionViewModel TransactionViewModel;

        public TransactionUserControl()
        {
            TransactionViewModel = new TransactionViewModel();
            this.InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            TransactionList.CollectionChanged += TransactionListOnCollectionChanged;
            //TransactionListDataGrid.SelectedItem = 
            //PreviousPageButton.IsEnabled = false;
            //TransactionViewModel.AllTransactionSummaries = TransactionList;
            //TransactionViewModel.InitialValues();

        }

        private void TransactionListOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            TransactionViewModel.ListPropertyChanged(e.NewItems[0] as TransactionSummaryVObj);
        }

        public static readonly DependencyProperty TransactionListProperty = DependencyProperty.Register(
            nameof(TransactionList), typeof(ObservableCollection<TransactionSummaryVObj>), typeof(TransactionUserControl), new PropertyMetadata(default(ObservableCollection<TransactionSummaryVObj>),new PropertyChangedCallback(ListPropertyChangedCallback)));

        private static void ListPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            var transactionUserControl = d as TransactionUserControl;
            var transactions = e.NewValue as ObservableCollection<TransactionSummaryVObj>;
            transactionUserControl?.TransactionViewModel.GenerateTransactionByGroup(transactions);

            if (transactions?.Count > 0)
            {
                if (transactionUserControl != null && transactionUserControl.TransactionListDataGrid != null)
                {
                    transactionUserControl.TransactionListDataGrid.SelectedItem = transactionUserControl?.TransactionViewModel.TransactionSummaries[0];

                }
            }
        }

        public ObservableCollection<TransactionSummaryVObj> TransactionList
        {
            get => (ObservableCollection<TransactionSummaryVObj>)GetValue(TransactionListProperty);
            set => SetValue(TransactionListProperty, value);
        }



        private void TransactionListDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = sender as ListView;
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
            //switch (sender.Mode)
            //{
            //    case TwoPaneViewMode.SinglePane:
            //        TransactionListDataGrid.ItemTemplate = WideDataTemplate;
            //        TransactionListDataGrid.HeaderTemplate = WideHeaderDatatemplate;
            //        break;
            //    default:
            //        TransactionListDataGrid.ItemTemplate = NarrowDataTemplate;
            //        TransactionListDataGrid.HeaderTemplate = NarrowHeaderDataTemplate;
            //        break;
            //}

            //TransactionListDataGrid.ItemTemplate = Application.Current.Resources["NarrrowDataTemplate"];
        }
        private void PaneView_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {

        }


        private void DetailsContent_OnGoBack()
        {
            PaneView.PanePriority = TwoPaneViewPriority.Pane1;

        }

        private void TransactionListDataGrid_OnPreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case Windows.System.VirtualKey.Up:
                    {
                        e.Handled = true;
                        if (TransactionListDataGrid.SelectedIndex == 0)
                        {
                            return;
                        }
                        TransactionListDataGrid.SelectedIndex--;



                        // BackButton_OnTapped(sender, new TappedRoutedEventArgs());
                        //DepositButton_OnClick(sender, e);
                        break;
                    }
                case Windows.System.VirtualKey.Down:
                    {
                        e.Handled = true;
                        if (TransactionListDataGrid.SelectedIndex == TransactionList.Count - 1)
                        {
                            return;
                        }
                        TransactionListDataGrid.SelectedIndex++;
                        break;
                    }
                case Windows.System.VirtualKey.Escape:
                    {
                        if (PaneView.PanePriority == TwoPaneViewPriority.Pane1)
                        {
                            e.Handled = false;
                        }
                        else
                        {
                            e.Handled = true;
                            PaneView.PanePriority = TwoPaneViewPriority.Pane1;
                        }
                        break;
                    }
            }
        }
    }
}
