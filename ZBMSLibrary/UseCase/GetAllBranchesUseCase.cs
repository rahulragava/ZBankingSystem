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
    public class GetAllBranchesUseCase : UseCaseBase<GetAllBranchesResponse>
    {
        private readonly IGetAllBranchesManager _getAllBranchesManager = DependencyContainer.DiContainer.GetRequiredService<IGetAllBranchesManager>();
        public readonly GetAllBranchesRequest GetAllBranchesRequest;

        public GetAllBranchesUseCase(GetAllBranchesRequest getAllBranchesRequest, IPresenterCallBack<GetAllBranchesResponse> presenterCallBack) : base(presenterCallBack)
        {
            GetAllBranchesRequest = getAllBranchesRequest;
        }

        public override void Action()
        {
            _getAllBranchesManager.GetAllBranchesAsync(GetAllBranchesRequest, new GetAllBranchesUseCaseCallBack(this));
        }
    }

    public class GetAllBranchesUseCaseCallBack : IUseCaseCallBack<GetAllBranchesResponse>
    {
        private readonly GetAllBranchesUseCase _getAllBranchesUseCase;
        public GetAllBranchesUseCaseCallBack(GetAllBranchesUseCase getAllBranchesUseCase)
        {
            _getAllBranchesUseCase = getAllBranchesUseCase;
        }

        public void OnSuccess(GetAllBranchesResponse responseObj)
        {
            _getAllBranchesUseCase.PresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _getAllBranchesUseCase.PresenterCallBack?.OnError(ex);
        }
    }

    public class GetAllBranchesRequest
    {

    }

    public class GetAllBranchesResponse
    {
        public IEnumerable<Branch> Branches{ get; }
        public GetAllBranchesResponse(IEnumerable<Branch> branches)
        {
            Branches = branches;
        }
    }

}