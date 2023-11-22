using System.Threading.Tasks;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager.Contract
{
    public interface IMonthlyInterestCreditForSavingsAccountManager
    { 
        Task MonthlyInterestCreditForSavingsAccountAsync(MonthlyInterestCreditForSavingsAccountRequest monthlyInterestCreditForSavingsAccountRequest, MonthlyInterestCreditForSavingsAccountUseCaseCallBack monthlyInterestCreditForSavingsAccountUseCaseCallBack);
    }
}