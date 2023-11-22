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
    public class FixedDepositDetailViewModel : INotifyPropertyChanged
    {
        public FixedDepositBObj FixedDepositBObj { get; set; }

        public ObservableCollection<Account> Accounts { get; set; }

        public IFixedDepositView FixedDepositView;

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


        public FixedDepositDetailViewModel(IFixedDepositView fixedDepositView)
        {
            Accounts = new ObservableCollection<Account>();
            FixedDepositView = fixedDepositView;
        }


        public void ClosingAccountManually()
        {
            //closing fd account manually
            var request = new CloseFixedDepositRequest(FixedDepositBObj);
            var useCase = new ClosingFixedDepositUseCase(request, new ClosingFixedDepositPresenterCallBack(this));
            useCase.Execute();
        }



        public class ClosingFixedDepositPresenterCallBack : IPresenterCallBack<CloseFixedDepositResponse>
        {
            private readonly FixedDepositDetailViewModel _viewModel;
            public ClosingFixedDepositPresenterCallBack(FixedDepositDetailViewModel viewModel)
            {
                _viewModel = viewModel;
            }

            public void OnSuccess(CloseFixedDepositResponse response)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _viewModel.FixedDepositView.OnFixedDepositSuccessfullyClosed();
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