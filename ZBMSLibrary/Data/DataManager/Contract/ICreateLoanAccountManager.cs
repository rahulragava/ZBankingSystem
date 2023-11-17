using System.Threading.Tasks;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager.Contract
{
    public interface ICreateLoanAccountManager
    {
        Task CreatePersonalLoanAsync(CreatePersonalLoanRequest createPersonalLoanRequest, CreatePersonalLoanUseCaseCallBack createPersonalLoanUseCaseCallBack);

    }
}