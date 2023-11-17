using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;

namespace ZBMS.ViewModel.DetailViewModel
{
    public class RecurringDepositDetailViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<Account> Accounts { get; set; }
        public RecurringAccountBObj RecurringAccountBObj { get; set; }

        public RecurringDepositDetailViewModel()
        {
            Accounts = new ObservableCollection<Account>();
        }

        private string _fromAccountNumber;

        public string FromAccountNumber
        {
            get => _fromAccountNumber;
            set => SetField(ref _fromAccountNumber, value);
        }


        private string _repaymentAccountNumber;

        public string RepaymentAccountNumber
        {
            get => _repaymentAccountNumber;
            set => SetField(ref _repaymentAccountNumber, value);
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