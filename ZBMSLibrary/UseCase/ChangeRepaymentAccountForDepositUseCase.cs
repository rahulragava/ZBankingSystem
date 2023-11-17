using System;
using Microsoft.Extensions.DependencyInjection;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Data.Dependencies;
using ZBMSLibrary.Data;
using ZBMSLibrary.Entities.Model;

namespace ZBMSLibrary.UseCase
{
    public class ChangeRepaymentAccountForDepositUseCase : UseCaseBase<ChangeRepaymentAccountForDepositResponse>
    {
        private readonly IChangeRepaymentAccountForDepositManager _changeRepaymentAccountForDepositManager = DependencyContainer.DiContainer.GetRequiredService<IChangeRepaymentAccountForDepositManager>();
        public ChangeRepaymentAccountForDepositRequest ChangeRepaymentAccountForDepositRequest;

        public ChangeRepaymentAccountForDepositUseCase(ChangeRepaymentAccountForDepositRequest changeRepaymentAccountForDepositRequest, IPresenterCallBack<ChangeRepaymentAccountForDepositResponse> presenterCallBack) : base(presenterCallBack)
        {
            ChangeRepaymentAccountForDepositRequest = changeRepaymentAccountForDepositRequest;
        }

        public override void Action()
        {
            _changeRepaymentAccountForDepositManager.ChangeRepaymentAccountForDepositAsync(
                ChangeRepaymentAccountForDepositRequest, new ChangeRepaymentAccountForDepositUseCaseCallBack(this));
        }
    }


    public class ChangeRepaymentAccountForDepositUseCaseCallBack : IUseCaseCallBack<ChangeRepaymentAccountForDepositResponse>
    {
        private readonly ChangeRepaymentAccountForDepositUseCase _changeRepaymentAccountForDepositUseCase;

        public ChangeRepaymentAccountForDepositUseCaseCallBack(ChangeRepaymentAccountForDepositUseCase changeRepaymentAccountForDepositUseCase)
        {
            _changeRepaymentAccountForDepositUseCase = changeRepaymentAccountForDepositUseCase;
        }

        

        public void OnSuccess(ChangeRepaymentAccountForDepositResponse responseObj)
        {
            _changeRepaymentAccountForDepositUseCase.PresenterCallBack?.OnSuccess(responseObj);

        }

        public void OnError(Exception ex)
        {
            _changeRepaymentAccountForDepositUseCase.PresenterCallBack?.OnError(ex);
        }
    }

    public class ChangeRepaymentAccountForDepositRequest
    {
        public string AccountNumber;
        public Deposit Deposit;
        public ChangeRepaymentAccountForDepositRequest(string accountNumber,Deposit deposit)
        {
            AccountNumber = accountNumber;
            Deposit = deposit;
        }
    }

    public class ChangeRepaymentAccountForDepositResponse
    {
        public string AccountNumber;
        public ChangeRepaymentAccountForDepositResponse(string accountNumber)
        {
            AccountNumber = accountNumber;
        }
    }
}