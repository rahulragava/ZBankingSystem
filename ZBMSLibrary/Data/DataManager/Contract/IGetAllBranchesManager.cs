using System.Threading.Tasks;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager.Contract
{
    public interface IGetAllBranchesManager
    {
        Task GetAllBranchesAsync(GetAllBranchesRequest getAllBranchesRequest, GetAllBranchesUseCaseCallBack getAllBranchesUseCaseCallBack);
    }
}