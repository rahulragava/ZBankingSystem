using System;
using Microsoft.Extensions.DependencyInjection;
using ZBMSLibrary.Data;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Data.Dependencies;
using ZBMSLibrary.Entities.Model;

namespace ZBMSLibrary.UseCase
{
    public class CreateSavingsAccountUseCase : UseCaseBase<CreateSavingsAccountResponse>
    {
        private readonly ICreateSavingsAccountManager _createSavingsAccountManager =
            DependencyContainer.DiContainer.GetRequiredService<ICreateSavingsAccountManager>();
        public CreateSavingsAccountRequest CreateSavingsAccountRequest;
        public CreateSavingsAccountUseCase(CreateSavingsAccountRequest createSavingsAccountRequest, IPresenterCallBack<CreateSavingsAccountResponse> presenterCallBack) : base(presenterCallBack)
        {
            CreateSavingsAccountRequest = createSavingsAccountRequest;
        }

        public override void Action()
        {
            _createSavingsAccountManager.CreateSavingsAccountAsync(CreateSavingsAccountRequest,
                new CreateSavingsAccountUseCaseCallBack(this));
        }
    }
    public class CreateSavingsAccountUseCaseCallBack : IUseCaseCallBack<CreateSavingsAccountResponse>
    {
        private readonly CreateSavingsAccountUseCase _createCurrentAccountUseCase;

        public CreateSavingsAccountUseCaseCallBack(CreateSavingsAccountUseCase createSavingsAccountUseCase)
        {
            _createCurrentAccountUseCase = createSavingsAccountUseCase;
        }

        public void OnSuccess(CreateSavingsAccountResponse responseObj)
        {
            _createCurrentAccountUseCase?.PresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _createCurrentAccountUseCase?.PresenterCallBack?.OnError(ex);
        }
    }

    public class CreateSavingsAccountRequest
    {
        public SavingsAccount SavingsAccount;
        public CreateSavingsAccountRequest(SavingsAccount savingsAccount)
        {
            SavingsAccount = savingsAccount;
        }
    }

    public class CreateSavingsAccountResponse
    {
        public CreateSavingsAccountResponse()
        {

        }
    }
}