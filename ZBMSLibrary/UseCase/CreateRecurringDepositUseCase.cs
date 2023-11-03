using System;
using Microsoft.Extensions.DependencyInjection;
using ZBMSLibrary.Data;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Data.Dependencies;
using ZBMSLibrary.Entities.Model;

namespace ZBMSLibrary.UseCase
{
    public class CreateRecurringDepositUseCase : UseCaseBase<CreateRecurringDepositResponse>
    {
        private readonly ICreateRecurringDepositManager _createRecurringDepositManager = DependencyContainer.DiContainer.GetRequiredService<ICreateRecurringDepositManager>();

        public CreateRecurringDepositRequest CreateRecurringDepositRequest;

        public CreateRecurringDepositUseCase(CreateRecurringDepositRequest createRecurringDepositRequest,IPresenterCallBack<CreateRecurringDepositResponse> presenterCallBack) : base(presenterCallBack)
        {
            CreateRecurringDepositRequest = createRecurringDepositRequest;
        }

        public override void Action()
        {
                _createRecurringDepositManager.CreateRecurringDepositAsync(CreateRecurringDepositRequest,
                    new CreateRecurringDepositUseCaseCallBack(this));
        }
    }

    public class CreateRecurringDepositUseCaseCallBack : IUseCaseCallBack<CreateRecurringDepositResponse>
    {
        private readonly CreateRecurringDepositUseCase _createRecurringDepositUseCase;
        public CreateRecurringDepositUseCaseCallBack(CreateRecurringDepositUseCase createRecurringDepositUseCase)
        {
            _createRecurringDepositUseCase = createRecurringDepositUseCase;
        }

        public void OnSuccess(CreateRecurringDepositResponse responseObj)
        {
            _createRecurringDepositUseCase.PresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _createRecurringDepositUseCase.PresenterCallBack?.OnError(ex);
        }
    }

    public class CreateRecurringDepositRequest
    {
        public RecurringAccount RecurringAccount;
        public CreateRecurringDepositRequest(RecurringAccount recurringAccount)
        {
            RecurringAccount = recurringAccount;
        }
    }

    public class CreateRecurringDepositResponse
    {
        public CreateRecurringDepositResponse()
        {

        }
    }
}