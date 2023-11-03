using System;
using System.Threading.Tasks;
using ZBMSLibrary.Data.DataAdapter;
using ZBMSLibrary.Data.DataHandler.Contract;
using ZBMSLibrary.Data.DataHandler;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager
{
    public class GetAllBranchesManager : IGetAllBranchesManager
    {
        public GetAllBranchesManager(IDbHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }
        
        private readonly IDbHandler _dbHandler;

        public async Task GetAllBranchesAsync(GetAllBranchesRequest getAllBranchesRequest,
            GetAllBranchesUseCaseCallBack getAllBranchesUseCaseCallBack)
        {
            try
            {
                var branches = await _dbHandler.GetAllBranchesAsync();
                getAllBranchesUseCaseCallBack?.OnSuccess(new GetAllBranchesResponse(branches));
            }
            catch (Exception e)
            {
                getAllBranchesUseCaseCallBack?.OnError(e);
            }
        }
    }
}