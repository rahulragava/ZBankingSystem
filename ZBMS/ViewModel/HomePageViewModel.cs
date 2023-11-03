using System;
using Windows.UI.Core;
using ZBMS.Services;
using ZBMS.View.Pages;
using ZBMSLibrary.Data;
using ZBMSLibrary.UseCase;

namespace ZBMS.ViewModel
{
    public class HomePageViewModel
    {
        private IHomePage _homePage;

        public HomePageViewModel(IHomePage homePage)
        {
            _homePage = homePage;
        }
        public void UpdateUserLoggedOut()
        {
            var request = new UpdateUserLoggedInRequest(AppSettings.CustomerId);
            var usecase = new UpdateUserLoggedInUseCase(request, new UpdateUserLoggedInPresenterCallBack(this));
            usecase.Execute();
        }

        public class UpdateUserLoggedInPresenterCallBack : IPresenterCallBack<UpdateUserLoggedInResponse>
        {
            private readonly HomePageViewModel _homePageViewModel;

            public UpdateUserLoggedInPresenterCallBack(HomePageViewModel homePageViewModel)
            {
                _homePageViewModel = homePageViewModel;
            }

            public void OnSuccess(UpdateUserLoggedInResponse response)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _homePageViewModel._homePage.Logout();
                    }
                );
            }

            public void OnError(Exception ex)
            {
            }
        }
       
    }
}