using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Windows.Security.Cryptography.Core;
using Windows.UI.Core;
using ZBMS.Services;
using ZBMS.View.Pages;
using ZBMSLibrary.Data;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Enums;
using ZBMSLibrary.Entities.Model;
using ZBMSLibrary.UseCase;

namespace ZBMS.ViewModel
{
    public class AccountPageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Account> Accounts { get; set; }
        public ObservableCollection<Deposit> Deposits { get; set; }
        public ObservableCollection<Loan> Loans { get; set; }
        public IAccountView AccountView { get; set; }
        public DateTime UserLastLogged { get; set; }

        public AccountPageViewModel(IAccountView accountView)
        {
            Accounts = new ObservableCollection<Account>();
            Deposits = new ObservableCollection<Deposit>();
            Loans = new ObservableCollection<Loan>();
            AccountView = accountView;
        }

        public void GetUserAccounts()
        {
            var userId = AppSettings.CustomerId;
            var getUserAccountsRequest = new GetUserAccountsRequest(userId);
            var useCase =
                new GetUserAccountsUseCase(getUserAccountsRequest, new GetUserAccountsPresenterCallback(this));
            useCase.Execute();
        }

        public void OnSuccessfullyRetrievedUserAccount(IEnumerable<Account> accounts,IEnumerable<Deposit> deposits, IEnumerable<Loan> loans)
        {
            Accounts.Clear();
            foreach (var account in accounts)
            {
                Accounts.Add(account);
            }
            Deposits.Clear();
            foreach (var deposit in deposits)
            {
                Deposits.Add(deposit);
            }
            Loans.Clear();
            foreach (var loan in loans)
            {
                Loans.Add(loan);
            }
        }

        private string _userPan;

        public string UserPan
        {
            get => _userPan;
            set => SetField(ref _userPan,value);
        }

        public void GetUserLastLogged()
        {
            var userId = AppSettings.CustomerId;
            var getUserLastLoggedRequest = new GetUserLastSeenRequest(userId);
            var useCase =
                new GetUserLastSeenUseCase(getUserLastLoggedRequest, new GetUserLastSeenPresenterCallBack(this));
            useCase.Execute();
        }


        public void GetUser()
        {
            var userId = AppSettings.CustomerId;
            var request = new GetUserRequest(userId);
            var useCase = new GetUserUseCase(request, new GetUserPresenterCallBack(this));
            useCase.Execute();
        }

        private void CheckDepositsForMonthlyInstallment(IEnumerable<Deposit> deposits)
        {
            List<RecurringAccountBObj> recurringDeposits = new List<RecurringAccountBObj>();

            foreach (Deposit deposit in deposits)
            {
                if (deposit is RecurringAccountBObj recurringDeposit)
                {
                    recurringDeposits.Add(recurringDeposit);
                }
            }
            Dictionary<RecurringAccount, int> monthlyInstallments = new Dictionary<RecurringAccount, int>();
            foreach (var recurringDeposit in recurringDeposits)
            {
                //if() user last logged + 1 day to this day is there any due
                var recurringAccount = new RecurringAccount
                {
                    AccountNumber = recurringDeposit.AccountNumber,
                    IfscCode = recurringDeposit.IfscCode,
                    UserId = recurringDeposit.UserId,
                    CreatedOn = recurringDeposit.CreatedOn,
                    AccountStatus = recurringDeposit.AccountStatus,
                    DepositedAmount = recurringDeposit.DepositedAmount,
                    InterestRate = recurringDeposit.InterestRate,
                    Tenure = recurringDeposit.Tenure,
                    SavingsAccountId = recurringDeposit.SavingsAccountId,
                    FromAccountId = recurringDeposit.FromAccountId,
                    Frequency = FrequencyType.Quarterly,
                    MonthlyInstallment = recurringDeposit.MonthlyInstallment,
                    LastPaidDate = recurringDeposit.LastPaidDate
                };

                if (DateTime.Now.Subtract(recurringAccount.LastPaidDate).TotalDays > 30)
                {
                    var totalDueMonths = (int)((DateTime.Now.Subtract(recurringAccount.LastPaidDate).TotalDays)/30.44);
                    monthlyInstallments.Add(recurringAccount,totalDueMonths);

                   
                }
            }
            

            //usecase
            if (monthlyInstallments.Count > 0)
            {
                var request = new DeduceMonthlyInstallmentRequest(monthlyInstallments);
                var useCase = new DeduceMonthlyInstallmentUseCase(request, new DeduceMonthlyInstallmentPresenterCallBack(this));
                useCase.Execute();
            }


        }

        private void CheckDepositsForSettlement(IEnumerable<Deposit> responseDeposits)
        {
            List<Deposit> settlementDepositBObjList = new List<Deposit>();
            foreach (var responseDeposit in responseDeposits)
            {
                if (responseDeposit.AccountStatus != AccountStatus.Closed)
                {
                    if (responseDeposit is FixedDepositBObj fixedDeposit)
                    {
                        if (DateTime.Now.Subtract(fixedDeposit.MaturityDate).TotalDays > 0)
                        {
                            settlementDepositBObjList.Add(fixedDeposit);
                        }
                    }
                    else if (responseDeposit is RecurringAccountBObj recurringAccount)
                    {
                        if (DateTime.Now.Subtract(recurringAccount.MaturityDate).TotalDays > 0)
                        {
                            settlementDepositBObjList.Add(recurringAccount);
                        }
                    }
                }
            }

            var request = new DepositSettlementRequest(settlementDepositBObjList);
            var useCase = new DepositSettlementUseCase(request, new DepositSettlementPresenterCallBack(this));
            useCase.Execute();


            //usecase
        }

        private void CheckAccountForFine(IEnumerable<Account> responseAccounts)
        {
            //var toBeFinedCurrentAccounts = new List<CurrentAccountBObj>();
            //var toBeFinedSavingsAccounts = new List<SavingsAccountBObj>();
            //foreach (var responseAccount in responseAccounts)
            //{
            //    if (responseAccount.Balance < responseAccount.MinimumBalance)
            //    {
            //        if (responseAccount is SavingsAccountBObj)
            //        {
            //            responseAccount.FineAmount += 100;
            //            toBeFinedSavingsAccounts.Add(responseAccount as SavingsAccountBObj);
            //        }
            //        else
            //        {
            //            responseAccount.FineAmount += 200;
            //            toBeFinedCurrentAccounts.Add(responseAccount as CurrentAccountBObj);
            //        }
            //    }
            //}
            //usecase
        }

        public class GetUserAccountsPresenterCallback : IPresenterCallBack<GetUserAccountsResponse>
        {
            private readonly AccountPageViewModel _viewModel;
            public GetUserAccountsPresenterCallback(AccountPageViewModel viewModel)
            {
                _viewModel = viewModel;
            }
            public void OnSuccess(GetUserAccountsResponse response)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _viewModel.OnSuccessfullyRetrievedUserAccount(response.Accounts,response.Deposits,response.Loans);
                        _viewModel.CheckAccountForFine(response.Accounts);
                        _viewModel.CheckDepositsForSettlement(response.Deposits);
                        _viewModel.CheckDepositsForMonthlyInstallment(response.Deposits);
                    }
                );

            }

            public void OnError(Exception ex)
            {
                //throw new NotImplementedException();
            }
        }

        public class GetUserLastSeenPresenterCallBack : IPresenterCallBack<GetUserLastSeenResponse>
        {
            private readonly AccountPageViewModel _viewModel;
            public GetUserLastSeenPresenterCallBack(AccountPageViewModel viewModel)
            {
                _viewModel = viewModel;
            }
            public void OnSuccess(GetUserLastSeenResponse response)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _viewModel.UserLastLogged = response.LastSeen;
                    }
                );
            }

            public void OnError(Exception ex)
            {
                //throw new NotImplementedException();
            }
        }



        public class DepositSettlementPresenterCallBack : IPresenterCallBack<DepositSettlementResponse>
        {
            private readonly AccountPageViewModel _viewModel;
            public DepositSettlementPresenterCallBack(AccountPageViewModel viewModel)
            {
                _viewModel = viewModel;
            }

            public void OnSuccess(DepositSettlementResponse response)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        //_viewModel.UserLastLogged = response.LastSeen;
                    }
                );
            }

            public void OnError(Exception ex)
            {
                //throw new NotImplementedException();
            }
        }

        public class DeduceMonthlyInstallmentPresenterCallBack : IPresenterCallBack<DeduceMonthlyInstallmentResponse>
        {
            private readonly AccountPageViewModel _viewModel;
            public DeduceMonthlyInstallmentPresenterCallBack(AccountPageViewModel viewModel)
            {
                _viewModel = viewModel;
            }

            public void OnSuccess(DeduceMonthlyInstallmentResponse response)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                    }
                );
            }

            public void OnError(Exception ex)
            {
                //throw new NotImplementedException();
            }
        }

        public class GetUserPresenterCallBack : IPresenterCallBack<GetUserResponse>
        {
            private readonly AccountPageViewModel _viewModel;

            public GetUserPresenterCallBack(AccountPageViewModel viewModel)
            {
                _viewModel = viewModel;
            }

            public void OnSuccess(GetUserResponse response)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        //var esponse = response.User;
                        _viewModel.UserPan = response.User.PAN;
                    }
                );
            }

            public void OnError(Exception ex)
            {
                throw new NotImplementedException();
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