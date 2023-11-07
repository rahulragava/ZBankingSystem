using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Core;
using ZBMS.Services;
using ZBMS.View.Pages;
using ZBMSLibrary.Data;
using ZBMSLibrary.Entities.Model;
using ZBMSLibrary.UseCase;

namespace ZBMS.ViewModel
{
    public class HomePageViewModel : INotifyPropertyChanged
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

        //public User User;

        private string _userName;

        public string UserName
        {
            get => _userName;
            set => SetField(ref _userName, value);
        }

        private string _phoneNumber;

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetField(ref _phoneNumber, value);
        }

        private string _email;

        public string Email
        {
            get => _email;
            set => SetField(ref _email, value);
        }

        private DateTime _lastLoggedOn;

        public DateTime LastLoggedOn
        {
            get => _lastLoggedOn;
            set => SetField(ref _lastLoggedOn, value);
        }
        public void GetUser()
        {
            var request = new GetUserRequest(AppSettings.CustomerId);
            var usecase = new GetUserUseCase(request, new GetUserPresenterCallBack(this));
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

        public class GetUserPresenterCallBack : IPresenterCallBack<GetUserResponse>
        {
            private readonly HomePageViewModel _homePageViewModel;

            public GetUserPresenterCallBack(HomePageViewModel homePageViewModel)
            {
                _homePageViewModel = homePageViewModel;
            }

            public void OnSuccess(GetUserResponse response)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        //_homePageViewModel._homePage.Logout();
                        _homePageViewModel.UserName = response.User.Name;
                        _homePageViewModel.PhoneNumber= response.User.PhoneNumber;
                        _homePageViewModel.Email = response.User.Email;
                        _homePageViewModel.LastLoggedOn= response.User.LastLoggedOn;
                        //_homePageViewModel.UserName = response.User.Name;
                    }
                );
            }

            public void OnError(Exception ex)
            {
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}