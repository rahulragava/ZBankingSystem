using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using ZBMSLibrary.Data;
using ZBMSLibrary.Data.DataManager;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Data.Dependencies;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;

namespace ZBMSLibrary.UseCase
{
    public class GetUserAccountsUseCase : UseCaseBase<GetUserAccountsResponse>
    {
        private readonly IGetUserAccountsManager _userAccountManager = DependencyContainer.DiContainer.GetRequiredService<IGetUserAccountsManager>();
        public readonly GetUserAccountsRequest GetUserAccountRequest;

        public GetUserAccountsUseCase(GetUserAccountsRequest getUserAccountRequest,
            IPresenterCallBack<GetUserAccountsResponse> accountPresenterCallBack) : base(accountPresenterCallBack)
        {
            GetUserAccountRequest = getUserAccountRequest;
        }

        public override void Action()
        {
            _userAccountManager.GetUserAccountsAsync(GetUserAccountRequest, new GetUserAccountsUseCaseCallBack(this));
        }
    }


    public class GetUserAccountsUseCaseCallBack : IUseCaseCallBack<GetUserAccountsResponse>
    {
        private readonly GetUserAccountsUseCase _getUserAccountsUseCase;

        public GetUserAccountsUseCaseCallBack(GetUserAccountsUseCase getUserAccountsUseCase)
        {
            _getUserAccountsUseCase = getUserAccountsUseCase;
        }
        public void OnSuccess(GetUserAccountsResponse response)
        {
            _getUserAccountsUseCase?.PresenterCallBack?.OnSuccess(response);

        }

        public void OnError(Exception ex)
        {
            _getUserAccountsUseCase?.PresenterCallBack?.OnError(ex);
        }
    }

    public class GetUserAccountsRequest
    {
        public string UserId { get; set; }

        public GetUserAccountsRequest(string userId)
        {
            UserId = userId;
        }
    }

    public class GetUserAccountsResponse
    {
        public IEnumerable<Account> Accounts { get; }
        public IEnumerable<Deposit> Deposits { get; }
        public IEnumerable<Loan> Loans { get; }
        public GetUserAccountsResponse(IEnumerable<Account> accounts, IEnumerable<Deposit> deposits, IEnumerable<Loan> loans)
        {
            Accounts = accounts;
            Deposits = deposits;
            Loans = loans;
        }
    }
}