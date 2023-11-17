using System.Threading.Tasks;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager.Contract
{
    public interface ILoanMonthlyDuePaymentManager
    {
        Task LoanMonthlyDuePaymentAsync(LoanMonthlyDuePaymentRequest loanMonthlyDuePaymentRequest, LoanMonthlyDuePaymentUseCaseCallBack loanMonthlyDuePaymentUseCaseCallBack);

    }
}