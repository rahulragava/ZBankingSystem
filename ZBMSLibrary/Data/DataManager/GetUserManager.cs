using System;
using System.Threading.Tasks;
using ZBMSLibrary.Data.DataHandler.Contract;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager
{
    public class GetUserManager : IGetUserManager
    {
        private readonly IDbHandler _dbHandler;

        public GetUserManager(IDbHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }

        public async Task GetUserAsync(GetUserRequest getUserRequest, GetUserUseCaseCallBack getUserUseCaseCallBack)
        {
            try
            {
                var user = await _dbHandler.GetUserAsync(getUserRequest.UserId);
                getUserUseCaseCallBack.OnSuccess(new GetUserResponse(user));
            }
            catch (Exception e)
            {
                getUserUseCaseCallBack.OnError(e);
            }
        }
    }
}