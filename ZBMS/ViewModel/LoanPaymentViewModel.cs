using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Windows.UI.Core;
using ZBMS.View.UserControl;
using ZBMSLibrary.Data;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;
using ZBMSLibrary.UseCase;

namespace ZBMS.ViewModel
{
    public class LoanPaymentViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> AccountNumbers;
        public ILoanPaymentView LoanPaymentView;
        public LoanPaymentViewModel(ILoanPaymentView loanPaymentView)
        {
            LoanPaymentView = loanPaymentView;
            AccountNumbers = new ObservableCollection<string>();
        }

        public void SetAccountNames(ObservableCollection<Account> accounts)
        {
            AccountNumbers.Clear();
            foreach (var account in accounts)
            {
                AccountNumbers.Add(account.AccountNumber);
            }
        }

        private double _dueAmount;

        public double DueAmount
        {
            get => _dueAmount;
            set => SetField(ref _dueAmount, value);
        }

        public void DueCalculator(PersonalLoanBObj personalLoanBObj)
        {
            var current = DateTime.Now;
            var nextDueDate = personalLoanBObj.NextDateToBePaid;
            if (current.Subtract(nextDueDate).TotalDays >= 0)
            {
                DueAmount = personalLoanBObj.EMICalculator() * ((current.Subtract(nextDueDate).TotalDays / 30) + 1);
            }
            else
            {
                DueAmount = 0;
            }
        }

        public void LoanDuePayment(string accountNumber)
        {
            //var request = 
        }


        public class LoanMonthlyDuePaymentPresenterCallBack : IPresenterCallBack<LoanMonthlyDuePaymentResponse>
        {
            private readonly LoanPaymentViewModel _loanPaymentViewModel;

            public LoanMonthlyDuePaymentPresenterCallBack(LoanPaymentViewModel loanPaymentViewModel)
            {
                _loanPaymentViewModel = loanPaymentViewModel;
            }


            public void OnSuccess(LoanMonthlyDuePaymentResponse response)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _loanPaymentViewModel.DueAmount = 0;
                        _loanPaymentViewModel.LoanPaymentView.LoanPaymentSuccessful();
                    }
                );
            }

            public void OnError(Exception ex)
            {
                if (ex.Message == "No sufficient balance")
                {
                    _loanPaymentViewModel.LoanPaymentView.LoanPaymentFailedDueToAccountInsufficientAmount();

                }
                //throw new NotImplementedException();
            }
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