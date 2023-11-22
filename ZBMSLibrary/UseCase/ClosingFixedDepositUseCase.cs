using System;
using Microsoft.Extensions.DependencyInjection;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Data.Dependencies;
using ZBMSLibrary.Data;
using ZBMSLibrary.Entities.BusinessObject;

namespace ZBMSLibrary.UseCase
{
    public class ClosingFixedDepositUseCase : UseCaseBase<CloseFixedDepositResponse>
    {
        private readonly ICloseFixedDepositManager _closeFixedDepositManager = DependencyContainer.DiContainer.GetRequiredService<ICloseFixedDepositManager>();
        public CloseFixedDepositRequest CloseFixedDepositRequest;

        public ClosingFixedDepositUseCase(CloseFixedDepositRequest closeFixedDepositRequest, IPresenterCallBack<CloseFixedDepositResponse> presenterCallBack) : base(presenterCallBack)
        {
            CloseFixedDepositRequest = closeFixedDepositRequest;
        }

        public override void Action()
        {
            _closeFixedDepositManager.CloseFixedDepositAsync(CloseFixedDepositRequest,
                new CloseFixedDepositUseCaseCallBack(this));
        }
    }


    public class CloseFixedDepositUseCaseCallBack : IUseCaseCallBack<CloseFixedDepositResponse>
    {
        private readonly ClosingFixedDepositUseCase _closingFixedDepositUseCase;

        public CloseFixedDepositUseCaseCallBack(ClosingFixedDepositUseCase closingFixedDepositUseCase)
        {
            _closingFixedDepositUseCase = closingFixedDepositUseCase;
        }

        public void OnSuccess(CloseFixedDepositResponse responseObj)
        {
            _closingFixedDepositUseCase.PresenterCallBack?.OnSuccess(responseObj);

        }

        public void OnError(Exception ex)
        {
            _closingFixedDepositUseCase.PresenterCallBack?.OnError(ex);
        }
    }

    public class CloseFixedDepositRequest
    {
        public FixedDepositBObj FixedDepositBObj;
        public CloseFixedDepositRequest(FixedDepositBObj fixedDepositBObj)
        {
            FixedDepositBObj = fixedDepositBObj;
        }
    }

    public class CloseFixedDepositResponse
    {
        public CloseFixedDepositResponse()
        {
        }
    }
}