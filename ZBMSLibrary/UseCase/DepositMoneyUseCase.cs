using System;
using Microsoft.Extensions.DependencyInjection;
using ZBMSLibrary.Data;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Data.Dependencies;
using ZBMSLibrary.Entities.Model;

namespace ZBMSLibrary.UseCase
{
    public class DepositMoneyUseCase : UseCaseBase<DepositMoneyResponse>
    {
        private readonly IDepositMoneyToAccountManager _depositMoneyToAccountManager= DependencyContainer.DiContainer.GetRequiredService<IDepositMoneyToAccountManager>();
        public readonly DepositMoneyRequest DepositMoneyRequest;
        public DepositMoneyUseCase(DepositMoneyRequest depositMoneyRequest, IPresenterCallBack<DepositMoneyResponse> presenterCallBack) : base(presenterCallBack)
        {
            DepositMoneyRequest = depositMoneyRequest;
        }

        public override void Action()
        {
            _depositMoneyToAccountManager.DepositMoneyToAccount(DepositMoneyRequest, new DepositMoneyUseCaseCallBack(this));
        }
    }

    public class DepositMoneyUseCaseCallBack : IUseCaseCallBack<DepositMoneyResponse>
    {
        private readonly DepositMoneyUseCase _depositMoneyUseCase;
        public DepositMoneyUseCaseCallBack(DepositMoneyUseCase depositMoneyUseCase)
        {
            _depositMoneyUseCase = depositMoneyUseCase;
        }

        public void OnSuccess(DepositMoneyResponse responseObj)
        {
            _depositMoneyUseCase?.PresenterCallBack?.OnSuccess(responseObj);

        }

        public void OnError(Exception ex)
        {
            _depositMoneyUseCase?.PresenterCallBack?.OnError(ex);

        }
    }

    public class DepositMoneyRequest
    {
        public double Amount;
        public Account Account;
        public DepositMoneyRequest(double amount, Account account)
        {
            Amount = amount;
            Account = account;
        }
    }

    public class DepositMoneyResponse
    {
        //public double Amount;
        //public DepositMoneyResponse(double amount)
        //{
        //    Amount = amount;
        //}
    }
}