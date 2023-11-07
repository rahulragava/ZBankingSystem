using System.Threading.Tasks;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager.Contract
{
    public interface IGetUserManager
    {
        Task GetUserAsync(GetUserRequest getUserRequest, GetUserUseCaseCallBack getUserUseCaseCallBack);

    }
}