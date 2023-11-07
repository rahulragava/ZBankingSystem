using System;
using Microsoft.Extensions.DependencyInjection;
using ZBMSLibrary.Data;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Data.Dependencies;
using ZBMSLibrary.Entities.Model;

namespace ZBMSLibrary.UseCase
{
    public class GetUserUseCase :UseCaseBase<GetUserResponse>
    {
        public IGetUserManager GetUserManager = DependencyContainer.DiContainer.GetRequiredService<IGetUserManager>();
        public GetUserRequest GetUserRequest;
        public GetUserUseCase(GetUserRequest getUserRequest,IPresenterCallBack<GetUserResponse> presenterCallBack) : base(presenterCallBack)
        {
            GetUserRequest = getUserRequest;
        }

        public override void Action()
        {
            GetUserManager.GetUserAsync(GetUserRequest, new GetUserUseCaseCallBack(this));
        }
    }

    public class GetUserUseCaseCallBack : IUseCaseCallBack<GetUserResponse>
    {
        private readonly GetUserUseCase _getUserUseCase;

        public GetUserUseCaseCallBack(GetUserUseCase getUserUseCase)
        {
            _getUserUseCase = getUserUseCase;
        }
        public void OnSuccess(GetUserResponse responseObj)
        {
            _getUserUseCase.PresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _getUserUseCase.PresenterCallBack?.OnError(ex);
        }
    }

    public class GetUserRequest
    {
        public string UserId;
        public GetUserRequest(string userId)
        {
            UserId = userId;
        }
    }

    public class GetUserResponse
    {
        public User User;
        public GetUserResponse(User user)
        {
            User = user;
        }
    }
}