using System;
using Microsoft.Extensions.DependencyInjection;
using ZBMSLibrary.Data;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Data.Dependencies;
using ZBMSLibrary.Entities.Model;

namespace ZBMSLibrary.UseCase
{
    public class CreateFixedDepositUseCase : UseCaseBase<CreateFixedDepositResponse>
    {
        private readonly ICreateFixedDepositManager _createFixedDepositManager  = DependencyContainer.DiContainer.GetRequiredService<ICreateFixedDepositManager>();

        public CreateFixedDepositRequest CreateFixedDepositRequest;

        public CreateFixedDepositUseCase(CreateFixedDepositRequest createFixedDepositRequest, IPresenterCallBack<CreateFixedDepositResponse> presenterCallBack) : base(presenterCallBack)
        {
            CreateFixedDepositRequest = createFixedDepositRequest;
        }

        public override void Action()
        {
            _createFixedDepositManager.CreateFixedDepositAsync(CreateFixedDepositRequest,
                new CreateFixedDepositUseCaseCallBack(this));
        }
    }
    public class CreateFixedDepositUseCaseCallBack : IUseCaseCallBack<CreateFixedDepositResponse>
    {
        private readonly CreateFixedDepositUseCase _createFixedDepositUseCase;
        public CreateFixedDepositUseCaseCallBack(CreateFixedDepositUseCase createFixedDepositUseCase)
        {
            _createFixedDepositUseCase = createFixedDepositUseCase;
        }

        public void OnSuccess(CreateFixedDepositResponse responseObj)
        {
            _createFixedDepositUseCase.PresenterCallBack?.OnSuccess(responseObj);

        }

        public void OnError(Exception ex)
        {
            _createFixedDepositUseCase.PresenterCallBack?.OnError(ex);
        }
    }

    public class CreateFixedDepositRequest
    {
        public FixedDeposit FixedDeposit;
        public CreateFixedDepositRequest(FixedDeposit fixedDeposit)
        {
            FixedDeposit = fixedDeposit;
        }
    }

    public class CreateFixedDepositResponse
    {
        public CreateFixedDepositResponse()
        {

        }
    }
}