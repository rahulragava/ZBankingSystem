using System.Collections.Generic;
using System;
using Microsoft.Extensions.DependencyInjection;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Data.Dependencies;
using ZBMSLibrary.Data;
using ZBMSLibrary.Entities.Model;

namespace ZBMSLibrary.UseCase
{
    public class GetUserLastSeenUseCase : UseCaseBase<GetUserLastSeenResponse>
    {
        private readonly IGetUserLastSeenManager _getUserLastSeenManager =
            DependencyContainer.DiContainer.GetRequiredService<IGetUserLastSeenManager>();
        public readonly GetUserLastSeenRequest GetUserLastSeenRequest;

        public GetUserLastSeenUseCase(GetUserLastSeenRequest getUserLastSeenRequest, IPresenterCallBack<GetUserLastSeenResponse> presenterCallBack) : base(presenterCallBack)
        {
            GetUserLastSeenRequest = getUserLastSeenRequest;
        }

        public override void Action()
        {
            _getUserLastSeenManager.GetUserLastSeen(GetUserLastSeenRequest, new GetUserLastSeenUseCaseCallBack(this));
        }
    }


    public class GetUserLastSeenUseCaseCallBack : IUseCaseCallBack<GetUserLastSeenResponse>
    {
        private readonly GetUserLastSeenUseCase _getUserLastSeenUseCase;

        public GetUserLastSeenUseCaseCallBack(GetUserLastSeenUseCase getUserLastSeenUseCase)
        {
            _getUserLastSeenUseCase = getUserLastSeenUseCase;
        }
  
        public void OnSuccess(GetUserLastSeenResponse responseObj)
        {
            _getUserLastSeenUseCase.PresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _getUserLastSeenUseCase?.PresenterCallBack?.OnError(ex);
        }
    }

    public class GetUserLastSeenRequest
    {
        public string UserId { get; set; }

        public GetUserLastSeenRequest(string userId)
        {
            UserId = userId;
        }
    }

    public class GetUserLastSeenResponse
    {
        public DateTime LastSeen { get; set; }
        public GetUserLastSeenResponse(DateTime lastSeen)
        {
            LastSeen = lastSeen;
        }
    }
}