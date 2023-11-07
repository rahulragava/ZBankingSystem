using System;
using Microsoft.Extensions.DependencyInjection;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Data.Dependencies;
using ZBMSLibrary.Data;
using ZBMSLibrary.Data.DataManager;

namespace ZBMSLibrary.UseCase
{
    public class UpdateUserLoggedInUseCase : UseCaseBase<UpdateUserLoggedInResponse>
    {
        private readonly IUpdateUserLoggedInManager _updateUserLoggedInManager =
            DependencyContainer.DiContainer.GetRequiredService<IUpdateUserLoggedInManager>();
        public readonly UpdateUserLoggedInRequest UpdateUserLoggedInRequest;

        public UpdateUserLoggedInUseCase(UpdateUserLoggedInRequest updateUserLoggedInRequest, IPresenterCallBack<UpdateUserLoggedInResponse> presenterCallBack) : base(presenterCallBack)
        {
            UpdateUserLoggedInRequest = updateUserLoggedInRequest;
        }

        public override void Action()
        {
            _updateUserLoggedInManager.UpdateUserLoggedIn(UpdateUserLoggedInRequest, new UpdateUserLoggedInUseCaseCallBack(this));
        }
    }


    public class UpdateUserLoggedInUseCaseCallBack : IUseCaseCallBack<UpdateUserLoggedInResponse>
    {
        private readonly UpdateUserLoggedInUseCase _updateUserLoggedInUseCase;

        public UpdateUserLoggedInUseCaseCallBack(UpdateUserLoggedInUseCase updateUserLoggedInUseCase)
        {
            _updateUserLoggedInUseCase = updateUserLoggedInUseCase;
        }


        public void OnSuccess(UpdateUserLoggedInResponse responseObj)
        {
            //throw new NotImplementedException();
            _updateUserLoggedInUseCase.PresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _updateUserLoggedInUseCase?.PresenterCallBack?.OnError(ex);
        }
    }

    public class UpdateUserLoggedInRequest
    {
        public string UserId { get; set; }

        public UpdateUserLoggedInRequest(string userId)
        {
            UserId = userId;
        }
    }

    public class UpdateUserLoggedInResponse
    {
        public UpdateUserLoggedInResponse()
        {

        }
    }
}