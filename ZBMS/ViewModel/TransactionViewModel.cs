using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Enums;
using ZBMSLibrary.Entities.Model;

namespace ZBMS.ViewModel
{
    public class TransactionViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TransactionSummary> AllTransactionSummaries;
        public ObservableCollection<TransactionSummary> PageTransactionSummaries;



        public TransactionViewModel()
        {
            AllTransactionSummaries = new ObservableCollection<TransactionSummary>();
            PageTransactionSummaries = new ObservableCollection<TransactionSummary>();
        }

        private int _totalPages;

        public int TotalPages
        {
            get => _totalPages;
            set => SetField(ref _totalPages, value);
        }

        private int _numberOfRowsPerPage;

        public int NumberOfRowsPerPage
        {
            get => _numberOfRowsPerPage;
            set => SetField(ref _numberOfRowsPerPage, value);
        }

        private int _currentPage;

        public int CurrentPage
        {
            get => _currentPage;
            set => SetField(ref _currentPage, value);
        }

        private int _firstItemIndex;

        public int FirstItemIndex
        {
            get => _firstItemIndex;
            set => SetField(ref _firstItemIndex, value);
        }

        private int _lastItemIndex;

        public int LastItemIndex
        {
            get => _lastItemIndex; 
            set => SetField(ref _lastItemIndex, value);
        }


        public void InitialValues()
        {
            CurrentPage = 1;
            NumberOfRowsPerPage = 12;
            FirstItemIndex = 0;
            if (AllTransactionSummaries.Count < NumberOfRowsPerPage)
            {
                LastItemIndex = AllTransactionSummaries.Count - 1;
            }
            else
            {
                LastItemIndex = FirstItemIndex + NumberOfRowsPerPage - 1;
            }
            TotalPages = AllTransactionSummaries.Count / NumberOfRowsPerPage;
            if (TotalPages == 0)
            {
                TotalPages = 1;
            }
            if (AllTransactionSummaries.Count > 0)
            {
                for (int i = FirstItemIndex; i < LastItemIndex; i++)
                {
                    PageTransactionSummaries.Add(AllTransactionSummaries[i]);
                }
            }
            
        }

        public void NextPage()
        {
            CurrentPage = CurrentPage + 1;
            FirstItemIndex = FirstItemIndex + NumberOfRowsPerPage;
            LastItemIndex = FirstItemIndex + NumberOfRowsPerPage - 1;
            PageTransactionSummaries.Clear();
            for (int i = FirstItemIndex; i < LastItemIndex; i++)
            {
                PageTransactionSummaries.Add(AllTransactionSummaries[i]);
            }
        }

        public void PreviousPage()
        {
            CurrentPage = CurrentPage - 1;
            LastItemIndex = FirstItemIndex - 1;
            FirstItemIndex = LastItemIndex - NumberOfRowsPerPage + 1;
            PageTransactionSummaries.Clear();
            for (int i = FirstItemIndex; i < LastItemIndex; i++)
            {
                PageTransactionSummaries.Add(AllTransactionSummaries[i]);
            }
        }

        public void LastPage()
        {
            CurrentPage = TotalPages;
            LastItemIndex = AllTransactionSummaries.Count - 1;
            FirstItemIndex = AllTransactionSummaries.Count - (AllTransactionSummaries.Count % NumberOfRowsPerPage);
            PageTransactionSummaries.Clear();
            for (int i = FirstItemIndex; i < LastItemIndex; i++)
            {
                PageTransactionSummaries.Add(AllTransactionSummaries[i]);
            }
        }

        public void FirstPage()
        {
            CurrentPage =  1;
            FirstItemIndex = 0;
            if (AllTransactionSummaries.Count > NumberOfRowsPerPage)
            {
                LastItemIndex = FirstItemIndex + NumberOfRowsPerPage - 1;
            }
            else
            {
                LastItemIndex = PageTransactionSummaries.Count-1;
            }
            PageTransactionSummaries.Clear();
            for (int i = FirstItemIndex; i < LastItemIndex; i++)
            {
                PageTransactionSummaries.Add(AllTransactionSummaries[i]);
            }
        }

        public TransactionSummary TransactionSummary;

        private string _id;

        public string Id
        {
            get => _id;
            set => SetField(ref _id, value);
        }

        private string _senderAccountNumber;

        public string SenderAccountNumber
        {
            get => _senderAccountNumber;
            set => SetField(ref _senderAccountNumber, value);
        }

        private string _receiverAccountNumber;

        public string ReceiverAccountNumber
        {
            get => _receiverAccountNumber;
            set => SetField(ref _receiverAccountNumber, value);
        }

        private DateTime _transactionOn;

        public DateTime TransactionOn
        {
            get => _transactionOn;
            set => SetField(ref _transactionOn, value);
        }

        private double _amount;

        public double Amount
        {
            get => _amount;
            set => SetField(ref _amount, value);
        }

        private TransactionType _transactionType;

        public TransactionType TransactionType
        {
            get => _transactionType;
            set => SetField(ref _transactionType, value);
        }

        private string _description;

        public string Description
        {
            get => _description;
            set => SetField(ref _description, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

      
    }
}