using System.Threading.Tasks;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager.Contract
{
    public interface ICreateFixedDepositManager
    {
        Task CreateFixedDepositAsync(CreateFixedDepositRequest createFixedDepositRequest, CreateFixedDepositUseCaseCallBack createFixedDepositUseCaseCallBack);

    }
}