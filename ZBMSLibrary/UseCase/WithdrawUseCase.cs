using System;
using Microsoft.Extensions.DependencyInjection;
using ZBMSLibrary.Data;
using ZBMSLibrary.Data.DataManager;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Data.Dependencies;
using ZBMSLibrary.Entities.Model;

namespace ZBMSLibrary.UseCase
{
    public class WithdrawUseCase : UseCaseBase<WithdrawMoneyResponse>
    {
        private readonly IWithdrawMoneyFromAccount _withdrawMoneyToAccountManager = DependencyContainer.DiContainer.GetRequiredService<IWithdrawMoneyFromAccount>();
        public readonly WithdrawMoneyRequest WithdrawMoneyRequest;
        public WithdrawUseCase(WithdrawMoneyRequest withdrawMoneyRequest ,IPresenterCallBack<WithdrawMoneyResponse> presenterCallBack) : base(presenterCallBack)
        {
            WithdrawMoneyRequest = withdrawMoneyRequest;
        }

        public override void Action()
        {
            _withdrawMoneyToAccountManager.WithdrawMoneyToAccount(WithdrawMoneyRequest, new WithdrawMoneyUseCaseCallBack(this));
        }
    }
    public class WithdrawMoneyUseCaseCallBack : IUseCaseCallBack<WithdrawMoneyResponse>
    {
        private readonly WithdrawUseCase _withdrawUseCase;
        public WithdrawMoneyUseCaseCallBack(WithdrawUseCase withdrawUseCase)
        {
            _withdrawUseCase = withdrawUseCase;
        }
        public void OnSuccess(WithdrawMoneyResponse responseObj)
        {
            _withdrawUseCase?.PresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _withdrawUseCase?.PresenterCallBack?.OnError(ex);
        }
    }

    public class WithdrawMoneyRequest
    {
        public double Amount;
        public Account Account;
        public WithdrawMoneyRequest(double amount, Account account)
        {
            Account = account;
            Amount = amount;
        }
    }

    public class WithdrawMoneyResponse
    {
        public WithdrawMoneyResponse()
        {

        }
    }
}