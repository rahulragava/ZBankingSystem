using System;
using Microsoft.Extensions.DependencyInjection;
using ZBMSLibrary.Data;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Data.Dependencies;
using ZBMSLibrary.Entities.Model;

namespace ZBMSLibrary.UseCase
{
    public class CreateCurrentAccountUseCase: UseCaseBase<CreateCurrentAccountResponse>
    {
        private readonly ICreateCurrentAccountManager _createCurrentAccountManager = DependencyContainer.DiContainer.GetRequiredService<ICreateCurrentAccountManager>();
        public CreateCurrentAccountRequest CreateCurrentAccountRequest;

        public CreateCurrentAccountUseCase(CreateCurrentAccountRequest createCurrentAccountRequest,IPresenterCallBack<CreateCurrentAccountResponse> presenterCallBack) : base(presenterCallBack)
        {
            CreateCurrentAccountRequest = createCurrentAccountRequest;
        }

        public override void Action()
        {
            _createCurrentAccountManager.CreateCurrentAccountAsync(CreateCurrentAccountRequest,
                new CreateCurrentAccountUseCaseCallBack(this));
        }
    }


    public class CreateCurrentAccountUseCaseCallBack : IUseCaseCallBack<CreateCurrentAccountResponse>
    {
        private readonly CreateCurrentAccountUseCase _createCurrentAccountUseCase;

        public CreateCurrentAccountUseCaseCallBack(CreateCurrentAccountUseCase createCurrentAccountUseCase)
        {
            _createCurrentAccountUseCase = createCurrentAccountUseCase;
        }

        public void OnSuccess(CreateCurrentAccountResponse responseObj)
        {
            _createCurrentAccountUseCase.PresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _createCurrentAccountUseCase.PresenterCallBack?.OnError(ex);
        }
    }

    public class CreateCurrentAccountRequest
    {
        public CurrentAccount CurrentAccount;
        public CreateCurrentAccountRequest(CurrentAccount currentAccount)
        {
            CurrentAccount = currentAccount;
        }
    }

    public class CreateCurrentAccountResponse
    {
        public CreateCurrentAccountResponse()
        {

        }
    }
}