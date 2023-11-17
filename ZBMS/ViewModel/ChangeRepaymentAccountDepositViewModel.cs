using System;
using System.Collections.ObjectModel;
using Windows.UI.Core;
using ZBMS.View.UserControl;
using ZBMSLibrary.Data;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;
using ZBMSLibrary.UseCase;

namespace ZBMS.ViewModel
{
    public class ChangeRepaymentAccountDepositViewModel
    {
        public readonly IChangeRepaymentView ChangeRepaymentView;
        public ObservableCollection<string> AccountNumbers;
        public ObservableCollection<string> SavingsAccountNumbers;
        public Deposit Deposit;

        public ChangeRepaymentAccountDepositViewModel(IChangeRepaymentView changeRepaymentView)
        {
            ChangeRepaymentView = changeRepaymentView;
            AccountNumbers = new ObservableCollection<string>();
            SavingsAccountNumbers = new ObservableCollection<string>();
        }


        public void SetAccountNumbers(ObservableCollection<Account> accounts)
        {
            AccountNumbers.Clear();
            foreach (var account in accounts)
            {
                if (account is SavingsAccountBObj savingsAccount)
                {
                    ////skips currently selected accountNumber
                    //if (savingsAccount.AccountNumber == Deposit.SavingsAccountId)
                    //{
                    //    continue;
                    //}
                    SavingsAccountNumbers.Add(account.AccountNumber);
                }
                AccountNumbers.Add(account.AccountNumber);
            }
        }

        public void ChangeAccountForDeposit(string accountNumber, Deposit deposit)
        {
            if (accountNumber == Deposit.SavingsAccountId)
            {
                return;
            }
            var request = new ChangeRepaymentAccountForDepositRequest(accountNumber, deposit);
            var useCase =
                new ChangeRepaymentAccountForDepositUseCase(request, new ChangeAccountForDepositPresenterCallBack(this));
            useCase.Execute();
        }

        public class ChangeAccountForDepositPresenterCallBack : IPresenterCallBack<ChangeRepaymentAccountForDepositResponse>
        {
            private readonly ChangeRepaymentAccountDepositViewModel _changeRepaymentAccountDepositViewModel;

            public ChangeAccountForDepositPresenterCallBack(ChangeRepaymentAccountDepositViewModel changeRepaymentAccountDepositViewModel)
            {
                _changeRepaymentAccountDepositViewModel = changeRepaymentAccountDepositViewModel;
            }

            public void OnSuccess(ChangeRepaymentAccountForDepositResponse response)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _changeRepaymentAccountDepositViewModel.ChangeRepaymentView.UpdateRepaymentAccount(response.AccountNumber);
                    }
                );
            }

        public void OnError(Exception ex)
            {
                //throw new NotImplementedException();
            }
        }
    }
}