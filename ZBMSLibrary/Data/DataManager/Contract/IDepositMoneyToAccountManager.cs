using System.Threading.Tasks;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager.Contract
{
    public interface IDepositMoneyToAccountManager
    {
        Task DepositMoneyToAccount(DepositMoneyRequest depositMoneyRequest, DepositMoneyUseCaseCallBack depositMoneyUseCaseCallBack);

    }
}