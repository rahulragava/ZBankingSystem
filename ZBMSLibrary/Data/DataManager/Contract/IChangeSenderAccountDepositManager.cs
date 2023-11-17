using System.Threading.Tasks;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager.Contract
{
    public interface IChangeSenderAccountDepositManager
    {
        Task ChangeSenderAccountDepositAsync(ChangeSenderAccountDepositRequest changeSenderAccountDepositRequest, ChangeSenderAccountDepositUseCaseCallBack changeSenderAccountDepositUseCaseCallBack);
    }
}