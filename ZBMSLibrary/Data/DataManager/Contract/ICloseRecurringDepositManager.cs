using System.Threading.Tasks;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager.Contract
{
    public interface ICloseRecurringDepositManager
    {
        Task CloseRecurringDepositAsync(CloseRecurringDepositRequest closeRecurringDepositRequest, CloseRecurringDepositUseCaseCallBack closeRecurringDepositUseCaseCallBack);
    }
}