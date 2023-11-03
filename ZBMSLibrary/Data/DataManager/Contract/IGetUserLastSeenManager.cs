using System.Threading.Tasks;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager.Contract
{
    public interface IGetUserLastSeenManager
    {
        Task GetUserLastSeen(GetUserLastSeenRequest getUserLastSeenRequest, GetUserLastSeenUseCaseCallBack getUserLastSeenUseCaseCallBack);
    }
}