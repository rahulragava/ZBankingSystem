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
    public class DeduceMonthlyInstallmentUseCase : UseCaseBase<DeduceMonthlyInstallmentResponse>
    {
        private readonly IDeduceMonthlyInstallmentManager _deduceMonthlyInstallmentManager = DependencyContainer.DiContainer.GetRequiredService<IDeduceMonthlyInstallmentManager>();
        public DeduceMonthlyInstallmentRequest DeduceMonthlyInstallmentRequest;

        public DeduceMonthlyInstallmentUseCase(DeduceMonthlyInstallmentRequest deduceMonthlyInstallmentRequest, IPresenterCallBack<DeduceMonthlyInstallmentResponse> presenterCallBack) : base(presenterCallBack)
        {
            DeduceMonthlyInstallmentRequest = deduceMonthlyInstallmentRequest;
        }

        public override void Action()
        {
            _deduceMonthlyInstallmentManager.DeduceMonthlyInstallmentAsync(DeduceMonthlyInstallmentRequest,
                new DeduceMonthlyInstallmentUseCaseCallBack(this));
        }
    }
    public class DeduceMonthlyInstallmentUseCaseCallBack : IUseCaseCallBack<DeduceMonthlyInstallmentResponse>
    {
        private readonly DeduceMonthlyInstallmentUseCase _deduceMonthlyInstallmentUseCase;
        public DeduceMonthlyInstallmentUseCaseCallBack(DeduceMonthlyInstallmentUseCase deduceMonthlyInstallmentUseCase)
        {
            _deduceMonthlyInstallmentUseCase = deduceMonthlyInstallmentUseCase;
        }

        public void OnSuccess(DeduceMonthlyInstallmentResponse responseObj)
        {
          _deduceMonthlyInstallmentUseCase.PresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _deduceMonthlyInstallmentUseCase.PresenterCallBack?.OnError(ex);
        }
    }

    public class DeduceMonthlyInstallmentRequest
    {
        public Dictionary<RecurringAccount, int> MonthlyInstallments;

        public DeduceMonthlyInstallmentRequest(Dictionary<RecurringAccount, int> monthlyInstallments)
        {
            MonthlyInstallments = monthlyInstallments;
        }
    }

    public class DeduceMonthlyInstallmentResponse
    {
        public DeduceMonthlyInstallmentResponse()
        {

        }
    }
}