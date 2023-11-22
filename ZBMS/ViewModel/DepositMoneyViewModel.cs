using System;
using System.Linq;
using ZBMS.View.UserControl;
using ZBMSLibrary.Data;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;
using ZBMSLibrary.UseCase;

namespace ZBMS.ViewModel
{
    public class DepositMoneyViewModel : ViewModelBase
    {
        private readonly IDepositMoneyUserControl _depositMoneyUserControl;

        public DepositMoneyViewModel(IDepositMoneyUserControl depositMoneyUserControl)
        {
            _depositMoneyUserControl = depositMoneyUserControl;
        }
        private string _amountToBeDeposited;

        public string AmountToBeDeposited
        {
            get => _amountToBeDeposited;
            set => Set(ref _amountToBeDeposited, value);
        }

        public CurrentAccountBObj CurrentAccountBObj { get; set; }
        public SavingsAccountBObj SavingsAccountBObj { get; set; }

        public bool IsTransactionLimitExceeded()
        {
            var today = DateTime.Today;
            DateTime startOfDay = today.Date;
            DateTime endOfDay = today.Date.AddDays(1);
            var transactionsOnToday = SavingsAccountBObj.TransactionList
                .Where(t =>
                    (t.SenderAccountNumber == SavingsAccountBObj.AccountNumber || 
                    t.ReceiverAccountNumber == SavingsAccountBObj.AccountNumber) &&
                    t.TransactionOn >= startOfDay &&
                    t.TransactionOn < endOfDay)
                .ToList();
            return transactionsOnToday.Count() < 10;
        }

        public void DepositMoney(double depositAmount)
        {
            
                if (SavingsAccountBObj != null)
                {
                    if (IsTransactionLimitExceeded())
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
                            NextCreditDateTime = SavingsAccountBObj.NextCreditDateTime,

                        };
                        var request = new DepositMoneyRequest(depositAmount, savingsAccount);
                        var depositMoneyUseCase =
                            new DepositMoneyUseCase(request, new DepositMoneyPresenterCallBack(this));
                        depositMoneyUseCase.Execute();
                    }
                    else
                    {
                        _depositMoneyUserControl.TransactionLimitExceed();
                    }
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
                    var request = new DepositMoneyRequest(depositAmount, currentAccount);
                    var depositMoneyUseCase = new DepositMoneyUseCase(request, new DepositMoneyPresenterCallBack(this));
                    depositMoneyUseCase.Execute();
                }
        }

        public class DepositMoneyPresenterCallBack : IPresenterCallBack<DepositMoneyResponse>
        {
            private readonly DepositMoneyViewModel _depositMoneyViewModel;

            public DepositMoneyPresenterCallBack(DepositMoneyViewModel depositMoneyViewModel)
            {
                _depositMoneyViewModel = depositMoneyViewModel;
            }

            public void OnSuccess(DepositMoneyResponse response)
            {
                //_depositMoneyViewModel.OnSuccessfullyDeposited();
                //_depositMoneyViewModel._depositMoneyUserControl.OnMoneyDeposited(response.Amount);
            }

            public void OnError(Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        private void OnSuccessfullyDeposited()
        {
            //throw new NotImplementedException();
        }
    }
}