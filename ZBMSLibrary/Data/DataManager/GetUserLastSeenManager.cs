using System;
using System.Threading.Tasks;
using ZBMSLibrary.Data.DataHandler.Contract;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager
{
    public class GetUserLastSeenManager : IGetUserLastSeenManager
    {
        private readonly IDbHandler _dbHandler;

        public GetUserLastSeenManager(IDbHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }

        public async Task GetUserLastSeen(GetUserLastSeenRequest getUserLastSeenRequest,
            GetUserLastSeenUseCaseCallBack getUserLastSeenUseCaseCallBack)
        {
            try
            {
                var lastSeen = await _dbHandler.GetUserLastLoggedAsync(getUserLastSeenRequest.UserId);
                getUserLastSeenUseCaseCallBack?.OnSuccess(new GetUserLastSeenResponse(lastSeen));
            }
            catch (Exception e)
            {
               getUserLastSeenUseCaseCallBack?.OnError(e);
            }
        }
    }
}