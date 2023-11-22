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
        private int _lastItemIndex;

        public int LastItemIndex
        {
            get => _lastItemIndex; 
            set => SetField(ref _lastItemIndex, value);
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

        private string _userName;

        public string UserName
        {
            get => _userName;
            set => SetField(ref _userName, value);
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