using System.Threading.Tasks;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager.Contract
{
    public interface ICreateCurrentAccountManager
    {
        Task CreateCurrentAccountAsync(CreateCurrentAccountRequest createCurrentAccountRequest, CreateCurrentAccountUseCaseCallBack createCurrentAccountUseCaseCallBack);

    }
}