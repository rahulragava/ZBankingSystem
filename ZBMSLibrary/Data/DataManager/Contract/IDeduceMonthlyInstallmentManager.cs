using System.Threading.Tasks;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager.Contract
{
    public interface IDeduceMonthlyInstallmentManager
    {
        Task DeduceMonthlyInstallmentAsync(DeduceMonthlyInstallmentRequest deduceMonthlyInstallmentRequest, DeduceMonthlyInstallmentUseCaseCallBack deduceMonthlyInstallmentUseCaseCallBack);
    }
}