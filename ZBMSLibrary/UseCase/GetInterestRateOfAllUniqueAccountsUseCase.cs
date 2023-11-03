using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using ZBMSLibrary.Data;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Data.DataManager;
using ZBMSLibrary.Data.Dependencies;
using ZBMSLibrary.Entities.Model;

namespace ZBMSLibrary.UseCase
{
    public class GetInterestRateOfAllUniqueAccountsUseCase : UseCaseBase<GetInterestRateOfAllUniqueAccountsResponse>
    {
        private readonly IGetInterestRateOfAllUniqueAccountsManager _getInterestRateOfAllUniqueAccountsManager= DependencyContainer.DiContainer.GetRequiredService<IGetInterestRateOfAllUniqueAccountsManager>();
        public readonly GetInterestRateOfAllUniqueAccountsRequest GetInterestRateOfAllUniqueAccountsRequest;

        public GetInterestRateOfAllUniqueAccountsUseCase(GetInterestRateOfAllUniqueAccountsRequest getInterestRateOfAllUniqueAccountsRequest,IPresenterCallBack<GetInterestRateOfAllUniqueAccountsResponse> presenterCallBack) : base(presenterCallBack)
        {
            GetInterestRateOfAllUniqueAccountsRequest = getInterestRateOfAllUniqueAccountsRequest;
        }

        public override void Action()
        {
            _getInterestRateOfAllUniqueAccountsManager.GetInterestRateOfAllUniqueAccounts(
                GetInterestRateOfAllUniqueAccountsRequest, new GetInterestRateOfAllUniqueAccountsUseCaseCallBack(this));
        }
    }

    public class GetInterestRateOfAllUniqueAccountsUseCaseCallBack : IUseCaseCallBack<GetInterestRateOfAllUniqueAccountsResponse>
    {
        private readonly GetInterestRateOfAllUniqueAccountsUseCase _getInterestRateOfAllUniqueAccountsUseCase;
        public GetInterestRateOfAllUniqueAccountsUseCaseCallBack(GetInterestRateOfAllUniqueAccountsUseCase getInterestRateOfAllUniqueAccountsUseCase)
        {
            _getInterestRateOfAllUniqueAccountsUseCase = getInterestRateOfAllUniqueAccountsUseCase;
        }
        public void OnSuccess(GetInterestRateOfAllUniqueAccountsResponse responseObj)
        {
            _getInterestRateOfAllUniqueAccountsUseCase.PresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _getInterestRateOfAllUniqueAccountsUseCase.PresenterCallBack?.OnError(ex);
        }
    }

    public class GetInterestRateOfAllUniqueAccountsRequest
    {

    }

    public class GetInterestRateOfAllUniqueAccountsResponse
    {
        public IDictionary<string,double> AccountToInterestRateMap{ get; }
        public GetInterestRateOfAllUniqueAccountsResponse(IDictionary<string,double> accountToInterestRateMap)
        {
            AccountToInterestRateMap = accountToInterestRateMap;
        }
    }
}