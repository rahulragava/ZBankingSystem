using System.Threading.Tasks;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager.Contract
{
    public interface IGetUserAccountsManager
    {
        Task GetUserAccountsAsync(GetUserAccountsRequest getUserAccountsRequest, GetUserAccountsUseCaseCallBack getUserAccountsUseCaseCallBack);
    }
}