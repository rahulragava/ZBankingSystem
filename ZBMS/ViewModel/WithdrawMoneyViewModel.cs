using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ZBMSLibrary.Data;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;
using ZBMSLibrary.UseCase;

namespace ZBMS.ViewModel
{
    public class WithdrawMoneyViewModel : INotifyPropertyChanged
    {
        private string _amountToBeDeposited;

        public string AmountToBeDeposited
        {
            get => _amountToBeDeposited;
            set => SetField(ref _amountToBeDeposited, value);
        }

        public CurrentAccountBObj CurrentAccountBObj { get; set; }
        public SavingsAccountBObj SavingsAccountBObj { get; set; }

        public void WithdrawMoney(double withdrawAmount)
        {
            if (SavingsAccountBObj != null)
            {
                var savingsAccount = new SavingsAccount
                {
                    AccountNumber = SavingsAccountBObj.AccountNumber,
                    IfscCode = SavingsAccountBObj.IfscCode,
                    UserId = SavingsAccountBObj.UserId,
                    CreatedOn = SavingsAccountBObj.CreatedOn,
                    AccountStatus = SavingsAccountBObj.AccountStatus,
                    Balance = SavingsAccountBObj.Balance,
                    MinimumBalance = SavingsAccountBObj.MinimumBalance,
                    FineAmount = SavingsAccountBObj.FineAmount,
                    ServiceCharges = SavingsAccountBObj.ServiceCharges,
                    InterestRate = SavingsAccountBObj.InterestRate,
                    ToBeCreditedAmount = SavingsAccountBObj.ToBeCreditedAmount,

                };
                var request = new WithdrawMoneyRequest(withdrawAmount , savingsAccount);
                var withdrawMoneyUseCase = new WithdrawUseCase(request, new WithdrawMoneyPresenterCallBack(this));
                withdrawMoneyUseCase.Execute();
            }
            else
            {
                var currentAccount = new CurrentAccount()
                {
                    AccountNumber = CurrentAccountBObj.AccountNumber,
                    IfscCode = CurrentAccountBObj.IfscCode,
                    UserId = CurrentAccountBObj.UserId,
                    CreatedOn = CurrentAccountBObj.CreatedOn,
                    AccountStatus = CurrentAccountBObj.AccountStatus,
                    Balance = CurrentAccountBObj.Balance,
                    MinimumBalance = CurrentAccountBObj.MinimumBalance,
                    FineAmount = CurrentAccountBObj.FineAmount,
                    ServiceCharges = CurrentAccountBObj.ServiceCharges,
                };
                var request = new WithdrawMoneyRequest(withdrawAmount, currentAccount);
                var withdrawMoneyUseCase = new WithdrawUseCase(request, new WithdrawMoneyPresenterCallBack(this));
                withdrawMoneyUseCase.Execute();
            }
        }


        private void OnSuccessfullyDeposited()
        {
            //notification fire
        }

        public class WithdrawMoneyPresenterCallBack : IPresenterCallBack<WithdrawMoneyResponse>
        {
            private readonly WithdrawMoneyViewModel _withdrawMoneyViewModel;

            public WithdrawMoneyPresenterCallBack(WithdrawMoneyViewModel withdrawMoneyViewModel)
            {
                _withdrawMoneyViewModel = withdrawMoneyViewModel;
            }

            public void OnSuccess(WithdrawMoneyResponse response)
            {
                //_withdrawMoneyViewModel.OnSuccessfullyDeposited();
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