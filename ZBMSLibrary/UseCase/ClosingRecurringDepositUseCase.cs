using System;
using Microsoft.Extensions.DependencyInjection;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Data.Dependencies;
using ZBMSLibrary.Data;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;

namespace ZBMSLibrary.UseCase
{
    public class ClosingRecurringDepositUseCase : UseCaseBase<CloseRecurringDepositResponse>
    {
        private readonly ICloseRecurringDepositManager _closeRecurringDepositManager = DependencyContainer.DiContainer.GetRequiredService<ICloseRecurringDepositManager>();
        public CloseRecurringDepositRequest CloseRecurringDepositRequest;

        public ClosingRecurringDepositUseCase(CloseRecurringDepositRequest closeRecurringDepositRequest, IPresenterCallBack<CloseRecurringDepositResponse> presenterCallBack) : base(presenterCallBack)
        {
            CloseRecurringDepositRequest = closeRecurringDepositRequest;
        }

        public override void Action()
        {
            _closeRecurringDepositManager.CloseRecurringDepositAsync(CloseRecurringDepositRequest,
                new CloseRecurringDepositUseCaseCallBack(this));
        }
    }


    public class CloseRecurringDepositUseCaseCallBack : IUseCaseCallBack<CloseRecurringDepositResponse>
    {
        private readonly ClosingRecurringDepositUseCase _closingRecurringDepositUseCase;

        public CloseRecurringDepositUseCaseCallBack(ClosingRecurringDepositUseCase closingRecurringDepositUseCase)
        {
            _closingRecurringDepositUseCase = closingRecurringDepositUseCase;
        }

        public void OnSuccess(CloseRecurringDepositResponse responseObj)
        {
            _closingRecurringDepositUseCase.PresenterCallBack?.OnSuccess(responseObj);

        }

        public void OnError(Exception ex)
        {
            _closingRecurringDepositUseCase.PresenterCallBack?.OnError(ex);
        }
    }

    public class CloseRecurringDepositRequest
    {
        public RecurringAccountBObj RecurringAccountBObj;
        public CloseRecurringDepositRequest(RecurringAccountBObj recurringAccountBObj)
        {
            RecurringAccountBObj = recurringAccountBObj;
        }
    }

    public class CloseRecurringDepositResponse
    {
        public CloseRecurringDepositResponse()
        {
        }
    }
}