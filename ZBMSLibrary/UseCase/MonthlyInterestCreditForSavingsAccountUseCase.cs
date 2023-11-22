using System.Collections.Generic;
using System;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Data.Dependencies;
using ZBMSLibrary.Data;
using ZBMSLibrary.Entities.Model;
using Microsoft.Extensions.DependencyInjection;
using ZBMSLibrary.Entities.BusinessObject;

namespace ZBMSLibrary.UseCase
{
    public class MonthlyInterestCreditForSavingsAccountUseCase : UseCaseBase<MonthlyInterestCreditForSavingsAccountResponse>
    {
        private readonly IMonthlyInterestCreditForSavingsAccountManager _monthlyInterestCreditForSavingsAccountManager = DependencyContainer.DiContainer.GetRequiredService<IMonthlyInterestCreditForSavingsAccountManager>();
        public readonly MonthlyInterestCreditForSavingsAccountRequest MonthlyInterestCreditForSavingsAccountRequest;

        public MonthlyInterestCreditForSavingsAccountUseCase(MonthlyInterestCreditForSavingsAccountRequest monthlyInterestCreditForSavingsAccountRequest, IPresenterCallBack<MonthlyInterestCreditForSavingsAccountResponse> presenterCallBack) : base(presenterCallBack)
        {
            MonthlyInterestCreditForSavingsAccountRequest = monthlyInterestCreditForSavingsAccountRequest;
        }

        public override void Action()
        {
            _monthlyInterestCreditForSavingsAccountManager.MonthlyInterestCreditForSavingsAccountAsync(MonthlyInterestCreditForSavingsAccountRequest,
                new MonthlyInterestCreditForSavingsAccountUseCaseCallBack(this));
        }
    }
    public class MonthlyInterestCreditForSavingsAccountUseCaseCallBack : IUseCaseCallBack<MonthlyInterestCreditForSavingsAccountResponse>
    {
        private readonly MonthlyInterestCreditForSavingsAccountUseCase _monthlyInterestCreditForSavingsAccountUseCase;
        public MonthlyInterestCreditForSavingsAccountUseCaseCallBack(MonthlyInterestCreditForSavingsAccountUseCase monthlyInterestCreditForSavingsAccountUseCase)
        {
            _monthlyInterestCreditForSavingsAccountUseCase = monthlyInterestCreditForSavingsAccountUseCase;
        }


        public void OnSuccess(MonthlyInterestCreditForSavingsAccountResponse responseObj)
        {
            _monthlyInterestCreditForSavingsAccountUseCase.PresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _monthlyInterestCreditForSavingsAccountUseCase.PresenterCallBack?.OnError(ex);
        }
    }

    public class MonthlyInterestCreditForSavingsAccountRequest
    {
        public readonly Dictionary<SavingsAccountBObj, int> MonthlyInterestCredits;
        //public List<TransactionSummary> TransactionSummaries;

        public MonthlyInterestCreditForSavingsAccountRequest(Dictionary<SavingsAccountBObj, int> monthlyInterestCredits)
        {
            MonthlyInterestCredits = monthlyInterestCredits;
            //TransactionSummaries = transactionSummaries;
        }
    }

    public class MonthlyInterestCreditForSavingsAccountResponse
    {
        public MonthlyInterestCreditForSavingsAccountResponse()
        {

        }
    }
}