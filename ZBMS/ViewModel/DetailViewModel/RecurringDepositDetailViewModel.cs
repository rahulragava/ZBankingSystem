using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Core;
using ZBMS.View.Pages.AccountsDetailsPage;
using ZBMSLibrary.Data;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;
using ZBMSLibrary.UseCase;

namespace ZBMS.ViewModel.DetailViewModel
{
    public class RecurringDepositDetailViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<Account> Accounts { get; set; }
        public RecurringAccountBObj RecurringAccountBObj { get; set; }
        public IRecurringDepositView RecurringDepositView;

        public RecurringDepositDetailViewModel(IRecurringDepositView recurringDepositView)
        {
            Accounts = new ObservableCollection<Account>();
            RecurringDepositView = recurringDepositView;
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

        public void ClosingAccountManually()
        {
            //closing rd account manually
            var request = new CloseRecurringDepositRequest(RecurringAccountBObj);
            var useCase = new ClosingRecurringDepositUseCase(request, new ClosingRecurringDepositPresenterCallBack(this));
            useCase.Execute();
        }

        public class ClosingRecurringDepositPresenterCallBack : IPresenterCallBack<CloseRecurringDepositResponse>
        {
            private readonly RecurringDepositDetailViewModel _viewModel;
            public ClosingRecurringDepositPresenterCallBack(RecurringDepositDetailViewModel viewModel)
            {
                _viewModel = viewModel;
            }

            public void OnSuccess(CloseRecurringDepositResponse response)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _viewModel.RecurringDepositView.CloseRecurringDepositSuccess();
                    }
                );
            }

            public void OnError(Exception ex)
            {
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