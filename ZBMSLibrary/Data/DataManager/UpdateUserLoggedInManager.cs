using System;
using System.Threading.Tasks;
using ZBMSLibrary.Data.DataHandler.Contract;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Entities.Model;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager
{
    public class UpdateUserLoggedInManager : IUpdateUserLoggedInManager
    {
        private readonly IDbHandler _dbHandler;

        public UpdateUserLoggedInManager(IDbHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }

        public async Task UpdateUserLoggedIn(UpdateUserLoggedInRequest updateUserLoggedInRequest,
            UpdateUserLoggedInUseCaseCallBack updateUserLoggedInUseCaseCallBack)
        {
            try
            {
                var user = await _dbHandler.GetUserAsync(updateUserLoggedInRequest.UserId); 
                user.LastLoggedOn = DateTime.Now;
                await _dbHandler.UpdateUserLoggedInAsync(user);
                updateUserLoggedInUseCaseCallBack?.OnSuccess(new UpdateUserLoggedInResponse());
            }
            catch (Exception e)
            {
                updateUserLoggedInUseCaseCallBack?.OnError(e);
            }
        }
    }
}