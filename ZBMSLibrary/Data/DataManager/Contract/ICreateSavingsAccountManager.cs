using System.Threading.Tasks;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager.Contract
{
    public interface ICreateSavingsAccountManager
    {
        Task CreateSavingsAccountAsync(CreateSavingsAccountRequest createSavingsAccountRequest, CreateSavingsAccountUseCaseCallBack createSavingsAccountUseCaseCallBack);

    }
}