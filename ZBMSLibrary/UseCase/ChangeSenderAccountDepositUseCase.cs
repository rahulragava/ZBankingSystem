using System;
using Microsoft.Extensions.DependencyInjection;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Data.Dependencies;
using ZBMSLibrary.Data;
using ZBMSLibrary.Entities.Model;

namespace ZBMSLibrary.UseCase
{
    public class ChangeSenderAccountDepositUseCase : UseCaseBase<ChangeSenderAccountDepositResponse>
    {
        private readonly IChangeSenderAccountDepositManager _changeSenderAccountDepositManager = DependencyContainer.DiContainer.GetRequiredService<IChangeSenderAccountDepositManager>();
        public readonly ChangeSenderAccountDepositRequest ChangeSenderAccountDepositRequest;

        public ChangeSenderAccountDepositUseCase(ChangeSenderAccountDepositRequest changeSenderAccountDepositRequest, IPresenterCallBack<ChangeSenderAccountDepositResponse> presenterCallBack) : base(presenterCallBack)
        {
            ChangeSenderAccountDepositRequest = changeSenderAccountDepositRequest;
        }

        public override void Action()
        {
            _changeSenderAccountDepositManager.ChangeSenderAccountDepositAsync(ChangeSenderAccountDepositRequest,
                new ChangeSenderAccountDepositUseCaseCallBack(this));
        }
    }


    public class ChangeSenderAccountDepositUseCaseCallBack : IUseCaseCallBack<ChangeSenderAccountDepositResponse>
    {
        private readonly ChangeSenderAccountDepositUseCase _changeSenderAccountDepositUseCase;

        public ChangeSenderAccountDepositUseCaseCallBack(ChangeSenderAccountDepositUseCase changeSenderAccountDepositUseCase)
        {
            _changeSenderAccountDepositUseCase = changeSenderAccountDepositUseCase;
        }


        public void OnSuccess(ChangeSenderAccountDepositResponse responseObj)
        {
            _changeSenderAccountDepositUseCase.PresenterCallBack?.OnSuccess(responseObj);

        }

        public void OnError(Exception ex)
        {
            _changeSenderAccountDepositUseCase.PresenterCallBack?.OnError(ex);
        }
    }

    public class ChangeSenderAccountDepositRequest
    {
        public string AccountNumber;
        public Deposit Deposit;
        public ChangeSenderAccountDepositRequest(string accountNumber, Deposit deposit)
        {
            AccountNumber = accountNumber;
            Deposit = deposit;
        }
    }

    public class ChangeSenderAccountDepositResponse
    {
        public string AccountNumber;
        public ChangeSenderAccountDepositResponse(string accountNumber)
        {
            AccountNumber = accountNumber;
        }
    }
}