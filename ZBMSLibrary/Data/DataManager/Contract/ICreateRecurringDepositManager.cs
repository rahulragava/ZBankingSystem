using System.Threading.Tasks;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager.Contract
{
    public interface ICreateRecurringDepositManager
    {
        Task CreateRecurringDepositAsync(CreateRecurringDepositRequest createRecurringDepositRequest, CreateRecurringDepositUseCaseCallBack createRecurringDepositUseCaseCallBack);

    }
}