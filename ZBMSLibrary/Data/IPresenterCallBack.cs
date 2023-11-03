using System;

namespace ZBMSLibrary.Data
{

    public interface IPresenterCallBack<T>
    {

        void OnSuccess(T response);
        void OnError(Exception ex);

    }
}