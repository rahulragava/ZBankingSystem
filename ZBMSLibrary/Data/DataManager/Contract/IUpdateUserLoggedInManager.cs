using System.Threading.Tasks;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager.Contract
{
    public interface IUpdateUserLoggedInManager
    {
        Task UpdateUserLoggedIn(UpdateUserLoggedInRequest updateUserLoggedInRequest, UpdateUserLoggedInUseCaseCallBack updateUserLoggedInUseCaseCallBack);
    }
}