//using System;
//using System.Threading.Tasks;
//using ZBMSLibrary.Data.DataHandler.Contract;
//using ZBMSLibrary.Data.DataManager.Contract;
//using ZBMSLibrary.UseCase;

//namespace ZBMSLibrary.Data.DataManager
//{
//    public class UpdateUserLoggedOutManager : IUpdateUserLoggedOutManager
//    {
//        private readonly IDbHandler _dbHandler;

//        public UpdateUserLoggedOutManager(IDbHandler dbHandler)
//        {
//            _dbHandler = dbHandler;
//        }

//        public async Task UpdateUserLoggedIn(UpdateUserLoggedOutRequest updateUserLoggedOutRequest,
//            UpdateUserLoggedOutUseCaseCallBack updateUserLoggedOutUseCaseCallBack)
//        {
//            try
//            {
//                var user = await _dbHandler.GetUserAsync(updateUserLoggedOutRequest.UserId);
//                user.LastLoggedOn = DateTime.Now;
//                await _dbHandler.UpdateUserLoggedInAsync(user);
//                updateUserLoggedInUseCaseCallBack?.OnSuccess(new UpdateUserLoggedInResponse());
//            }
//            catch (Exception e)
//            {
//                updateUserLoggedOutUseCaseCallBack?.OnError(e);
//            }
//        }
//    }
//}