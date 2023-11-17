using System.Threading.Tasks;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager.Contract
{
    public interface IChangeRepaymentAccountForDepositManager
    {
        Task ChangeRepaymentAccountForDepositAsync(ChangeRepaymentAccountForDepositRequest changeRepaymentAccountForDepositRequest, ChangeRepaymentAccountForDepositUseCaseCallBack changeRepaymentAccountForDepositUseCaseCallBack);

    }
}