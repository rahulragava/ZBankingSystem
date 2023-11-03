using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using ZBMSLibrary.Data;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Data.Dependencies;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;

namespace ZBMSLibrary.UseCase
{
    public class DepositSettlementUseCase : UseCaseBase<DepositSettlementResponse>
    {
        private readonly IDepositSettlementManager _depositSettlementManager =
            DependencyContainer.DiContainer.GetRequiredService<IDepositSettlementManager>();
        public DepositSettlementRequest DepositSettlementRequest;

        public DepositSettlementUseCase(DepositSettlementRequest depositSettlementRequest,IPresenterCallBack<DepositSettlementResponse> presenterCallBack) : base(presenterCallBack)
        {
            DepositSettlementRequest = depositSettlementRequest;
        }

        public override void Action()
        {
            _depositSettlementManager.DepositSettlementAsync(DepositSettlementRequest,
                new DepositSettlementUseCaseCallBack(this));
        }
    }
    public class DepositSettlementUseCaseCallBack : IUseCaseCallBack<DepositSettlementResponse>
    {
        private readonly DepositSettlementUseCase _depositSettlementUseCase;
        public DepositSettlementUseCaseCallBack(DepositSettlementUseCase  depositSettlementUseCase)
        {
            _depositSettlementUseCase = depositSettlementUseCase;
        }


        public void OnSuccess(DepositSettlementResponse responseObj)
        {
            _depositSettlementUseCase.PresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _depositSettlementUseCase.PresenterCallBack?.OnError(ex);
        }
    }

    public class DepositSettlementRequest
    {
        public IEnumerable<Deposit> Deposits;
        public DepositSettlementRequest(IEnumerable<Deposit> deposits)
        {
            Deposits = deposits;
        }
    }

    public class DepositSettlementResponse
    {
        public DepositSettlementResponse()
        {

        }
    }
}