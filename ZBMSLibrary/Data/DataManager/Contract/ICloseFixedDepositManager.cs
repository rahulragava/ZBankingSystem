using System.Threading.Tasks;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager.Contract
{
    public interface ICloseFixedDepositManager
    {
        Task CloseFixedDepositAsync(CloseFixedDepositRequest closeFixedDepositRequest, CloseFixedDepositUseCaseCallBack closeFixedDepositUseCaseCallBack);

    }
}