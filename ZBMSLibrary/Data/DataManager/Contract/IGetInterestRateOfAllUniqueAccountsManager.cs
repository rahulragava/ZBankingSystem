using System.Threading.Tasks;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager.Contract
{
    public interface IGetInterestRateOfAllUniqueAccountsManager
    {
        Task GetInterestRateOfAllUniqueAccounts(GetInterestRateOfAllUniqueAccountsRequest getInterestRateOfAllUniqueAccountsRequest, GetInterestRateOfAllUniqueAccountsUseCaseCallBack getInterestRateOfAllUniqueAccountsUseCaseCallBack);

    }
}