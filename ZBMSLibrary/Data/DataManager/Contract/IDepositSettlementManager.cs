using System.Threading.Tasks;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager.Contract
{
    public interface IDepositSettlementManager
    {
        Task DepositSettlementAsync(DepositSettlementRequest depositSettlementRequest, DepositSettlementUseCaseCallBack depositSettlementUseCaseCallBack);

    }
}