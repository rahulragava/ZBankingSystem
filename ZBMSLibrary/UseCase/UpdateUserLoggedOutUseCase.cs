//using System;
//using ZBMSLibrary.Data.DataManager.Contract;
//using ZBMSLibrary.Data.Dependencies;
//using ZBMSLibrary.Data;

//namespace ZBMSLibrary.UseCase
//{
//    public class UpdateUserLoggedOutUseCase : UseCaseBase<UpdateUserLoggedOutResponse>
//    {
//        //private readonly IUpdateUserLoggedInManager _updateUserLoggedInManager =
//        //    DependencyContainer.DiContainer.GetRequiredService<IUpdateUserLoggedInManager>();
//        public readonly UpdateUserLoggedOutRequest UpdateUserLoggedOutRequest;

//        public UpdateUserLoggedOutUseCase(UpdateUserLoggedOutRequest updateUserLoggedOutRequest, IPresenterCallBack<UpdateUserLoggedOutResponse> presenterCallBack) : base(presenterCallBack)
//        {
//            UpdateUserLoggedOutRequest = updateUserLoggedOutRequest;
//        }

//        public override void Action()
//        {
//            _updateUserLoggedInManager.UpdateUserLoggedIn(UpdateUserLoggedOutRequest, new UpdateUserLoggedOutUseCaseCallBack(this));
//        }
//    }


//    public class UpdateUserLoggedOutUseCaseCallBack : IUseCaseCallBack<UpdateUserLoggedOutResponse>
//    {
//        private readonly UpdateUserLoggedOutUseCase _updateUserLoggedOutUseCase;

//        public UpdateUserLoggedOutUseCaseCallBack(UpdateUserLoggedOutUseCase updateUserLoggedOutUseCase)
//        {
//            _updateUserLoggedOutUseCase = updateUserLoggedOutUseCase;
//        }

//        public void OnSuccess(UpdateUserLoggedOutResponse responseObj)
//        {
//            _updateUserLoggedOutUseCase.PresenterCallBack?.OnSuccess(responseObj);
//        }

//        public void OnError(Exception ex)
//        {
//            _updateUserLoggedOutUseCase.PresenterCallBack?.OnError(ex);
//        }
//    }

//    public class UpdateUserLoggedOutRequest
//    {
//        public string UserId { get; set; }

//        public UpdateUserLoggedOutRequest(string userId)
//        {
//            UserId = userId;
//        }
//    }

//    public class UpdateUserLoggedOutResponse
//    {
//        public UpdateUserLoggedOutResponse()
//        {
//        }
//    }
//}