using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite;
using ZBMSLibrary.Entities.Enums;

namespace ZBMSLibrary.Entities.Model
{
    public abstract class Account : INotifyPropertyChanged
    {
        private string _accountNumber;

        [PrimaryKey]
        public string AccountNumber
        {
            get => _accountNumber; 
            set => SetField(ref _accountNumber,value);
        }

        private string _ifscCode;

        public string IfscCode
        {
            get => _ifscCode; 
            set => SetField(ref _ifscCode,value);
        }

        private string _userId;

        public string UserId
        {
            get => _userId;
            set => SetField(ref _userId, value);
        }

        private DateTime _createdOn;

        public DateTime CreatedOn
        {
            get => _createdOn; 
            set => SetField(ref _createdOn,value);
        }

        private AccountStatus _accountStatus;

        public AccountStatus AccountStatus
        {
            get => _accountStatus; 
            set => SetField(ref _accountStatus,value);
        }

        private double _balance;

        public double Balance
        {
            get => _balance; 
            set => SetField(ref _balance,value);
        }

        private double _minimumBalance;

        public double MinimumBalance
        {
            get => _minimumBalance; 
            set => SetField(ref _minimumBalance,value);
        }

        private double _fineAmount;

        public double FineAmount
        {
            get => _fineAmount; 
            set => SetField(ref _fineAmount,value);
        }

        public double ServiceCharges {get; set; }
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