using System;
using System.Threading.Tasks;
using ZBMSLibrary.Data;

namespace ZBMSLibrary.UseCase
{
    public abstract class UseCaseBase<T>
    {
        public IPresenterCallBack<T> PresenterCallBack { get; set; }

        protected UseCaseBase(IPresenterCallBack<T> presenterCallBack)
        {
            PresenterCallBack = presenterCallBack;
        }

        public abstract void Action();

        public void Execute()
        {
            if (GetIfAvailableCache())
            {
                return;
            }

            Task.Run(() =>
            {
                try
                {
                    Action();
                }
                catch (Exception ex)
                {
                    //PresenterCallBack?.OnError(ex);
                }
            });
        }

        public virtual bool GetIfAvailableCache()
        {
            return false;
        }
    }

    public interface IUseCaseCallBack<T>
    {
        void OnSuccess(T responseObj);
        void OnError(Exception ex);
    }
   
}