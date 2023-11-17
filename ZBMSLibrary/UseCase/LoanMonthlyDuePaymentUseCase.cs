using System;
using Microsoft.Extensions.DependencyInjection;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Data.Dependencies;
using ZBMSLibrary.Data;

namespace ZBMSLibrary.UseCase
{
    public class LoanMonthlyDuePaymentUseCase : UseCaseBase<LoanMonthlyDuePaymentResponse>
    {
        private readonly ILoanMonthlyDuePaymentManager _loanMonthlyDuePaymentManager =
            DependencyContainer.DiContainer.GetRequiredService<ILoanMonthlyDuePaymentManager>();
        public readonly LoanMonthlyDuePaymentRequest LoanMonthlyDuePaymentRequest;

        public LoanMonthlyDuePaymentUseCase(LoanMonthlyDuePaymentRequest loanMonthlyDuePaymentRequest, IPresenterCallBack<LoanMonthlyDuePaymentResponse> presenterCallBack) : base(presenterCallBack)
        {
            LoanMonthlyDuePaymentRequest = loanMonthlyDuePaymentRequest;
        }

        public override void Action()
        {
            _loanMonthlyDuePaymentManager.LoanMonthlyDuePaymentAsync(LoanMonthlyDuePaymentRequest, new LoanMonthlyDuePaymentUseCaseCallBack(this));
        }
    }


    public class LoanMonthlyDuePaymentUseCaseCallBack : IUseCaseCallBack<LoanMonthlyDuePaymentResponse>
    {
        private readonly LoanMonthlyDuePaymentUseCase _loanMonthlyDuePaymentUseCase;

        public LoanMonthlyDuePaymentUseCaseCallBack(LoanMonthlyDuePaymentUseCase loanMonthlyDuePaymentUseCase)
        {
            _loanMonthlyDuePaymentUseCase = loanMonthlyDuePaymentUseCase;
        }

        public void OnSuccess(LoanMonthlyDuePaymentResponse responseObj)
        {
           _loanMonthlyDuePaymentUseCase.PresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _loanMonthlyDuePaymentUseCase?.PresenterCallBack?.OnError(ex);
        }
    }

    public class LoanMonthlyDuePaymentRequest
    {
        public string LoanAccountNumber { get; set; }
        public string AccountNumber { get; set; }
        public double DueAmount { get; set; }
        public string UserId { get; set; }
        public LoanMonthlyDuePaymentRequest(string loanAccountNumber,string accountNumber,double dueAmount, string userId)
        {
            AccountNumber = accountNumber;
            DueAmount = dueAmount;
            UserId = userId;
            LoanAccountNumber = loanAccountNumber;
        }
    }

    public class LoanMonthlyDuePaymentResponse
    {
        public LoanMonthlyDuePaymentResponse()
        {

        }
    }
}