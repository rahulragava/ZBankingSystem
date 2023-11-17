using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Core;
using ZBMS.View.UserControl;
using ZBMSLibrary.Data;
using ZBMSLibrary.Data.DataManager.CustomException;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;
using ZBMSLibrary.UseCase;

namespace ZBMS.ViewModel
{
    public class TransferMoneyViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> AccountNumbers;
        
        public ITransferMoneyView TransferMoneyView;

        private string _amountToBeDeposited;
        public string AmountToBeDeposited
        {
            get => _amountToBeDeposited;
            set => SetField(ref _amountToBeDeposited, value);
        }

        public Account TransferFromAccount;

        public TransferMoneyViewModel(ITransferMoneyView transferMoneyView)
        {
            AccountNumbers = new ObservableCollection<string>();
            TransferMoneyView = transferMoneyView;
        }

        public void SetAccountNumbers(ObservableCollection<Account> accounts)
        {
            AccountNumbers.Clear();
            foreach (var account in accounts)
            {
                if (account.AccountNumber == TransferFromAccount.AccountNumber)
                {
                    continue;
                } 
                AccountNumbers.Add(account.AccountNumber);
            }
        }


        public void TransferMoney(double amount, string accountNumber)
        {
            var request = new TransferRequest(TransferFromAccount, accountNumber, amount);
            var useCase = new TransferUseCase(request, new TransferMoneyPresenterCallBack(this));
            useCase.Execute();
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
        
        //presenterCallBack class 
        public class TransferMoneyPresenterCallBack : IPresenterCallBack<TransferResponse>
        {
            private readonly TransferMoneyViewModel _transferMoneyViewModel;

            public TransferMoneyPresenterCallBack(TransferMoneyViewModel transferMoneyViewModel)
            {
                _transferMoneyViewModel = transferMoneyViewModel;
            }

            public void OnSuccess(TransferResponse response)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _transferMoneyViewModel.TransferMoneyView.TransferSuccessful(response.TransactionSummaryVObj);
                    }
                );
            }

            public void OnError(Exception ex)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    () =>
                    {
                        if (ex is InsufficientBalanceException)
                        {
                            _transferMoneyViewModel.TransferMoneyView.TransferFailed(ex.Message);
                        }
                        else
                        {

                        }
                    }
                );
            }
        }
    }
}