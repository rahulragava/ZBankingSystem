using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ZBMS.Services;
using ZBMS.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ZBMS.View.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPageViewModel MainPageViewModel {  get; set; }
        public MainPage()
        {
            MainPageViewModel = new MainPageViewModel();
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;

            coreTitleBar.ExtendViewIntoTitleBar = false;
            UserAlreadyLoggedIn();
        }

        public void UserAlreadyLoggedIn()
        {
            if (AppSettings.LocalSettings.Values["UserId"] is null) return;
            //MainPageViewModel.UpdateUserLoggedIn();
            LoginInPage_OnGoToHome();
        }

        private void LoginInPage_OnGoToHome()
        {
            //MainPageViewModel.UpdateUserLoggedIn();
            this.Frame.Navigate(typeof(HomePage));
        }
    }
}
