using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;

namespace ZBMS.ViewModel.DetailViewModel
{
    public class PersonalLoanAccountDetailViewModel : INotifyPropertyChanged
    {
        public PersonalLoanBObj PersonalLoanBObj { get; set;}

        public ObservableCollection<Account> Accounts { get; set; }

        public PersonalLoanAccountDetailViewModel()
        {
            Accounts = new ObservableCollection<Account>();
        }

        private double _nextMonthDieAmount;

        public double NextMonthDueAmount
        {
            get => _nextMonthDieAmount;
            set => SetField(ref _nextMonthDieAmount, value);
        }

        public void GetNextMonthDueAmount()
        {
            NextMonthDueAmount = PersonalLoanBObj.EMICalculator();
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