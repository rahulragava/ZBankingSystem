using System;
using Microsoft.Extensions.DependencyInjection;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Data.Dependencies;
using ZBMSLibrary.Data;
using ZBMSLibrary.Entities.Model;

namespace ZBMSLibrary.UseCase
{
    public class CreatePersonalLoanUseCase : UseCaseBase<CreatePersonalLoanResponse>
    {
        private readonly ICreateLoanAccountManager _createPersonalLoanAccountManager = DependencyContainer.DiContainer.GetRequiredService<ICreateLoanAccountManager>();
        public CreatePersonalLoanRequest CreatePersonalLoanRequest;

        public CreatePersonalLoanUseCase(CreatePersonalLoanRequest createPersonalLoanRequest, IPresenterCallBack<CreatePersonalLoanResponse> presenterCallBack) : base(presenterCallBack)
        {
            CreatePersonalLoanRequest = createPersonalLoanRequest;
        }

        public override void Action()
        {
            _createPersonalLoanAccountManager.CreatePersonalLoanAsync(CreatePersonalLoanRequest,
                new CreatePersonalLoanUseCaseCallBack(this));
        }
    }


    public class CreatePersonalLoanUseCaseCallBack : IUseCaseCallBack<CreatePersonalLoanResponse>
    {
        private readonly CreatePersonalLoanUseCase _createPersonalLoanUseCase;

        public CreatePersonalLoanUseCaseCallBack(CreatePersonalLoanUseCase createPersonalLoanUseCase)
        {
            _createPersonalLoanUseCase = createPersonalLoanUseCase;
        }

        public void OnSuccess(CreatePersonalLoanResponse responseObj)
        {
            _createPersonalLoanUseCase.PresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _createPersonalLoanUseCase.PresenterCallBack?.OnError(ex);
        }
    }

    public class CreatePersonalLoanRequest
    {
        public PersonalLoan PersonalLoan;
        public string LoanedAmountGoesToAccountNumber;
        public CreatePersonalLoanRequest(PersonalLoan personalLoan, string loanedAmountGoesToAccountNumber)
        {
            PersonalLoan = personalLoan;
            LoanedAmountGoesToAccountNumber = loanedAmountGoesToAccountNumber;
        }
    }

    public class CreatePersonalLoanResponse
    {
        public CreatePersonalLoanResponse()
        {

        }
    }
}