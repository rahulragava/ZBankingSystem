using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.NetworkOperators;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZBMS.Services;
using ZBMS.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBMS.View.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page, INotifyPropertyChanged,IHomePage
    {
        public HomePageViewModel HomePageViewModel;
        public HomePage()
        {
            HomePageViewModel = new HomePageViewModel(this);
            this.InitializeComponent();
            NavigationViewItem itemContent = NavigationMenu.MenuItems.ElementAt(1) as NavigationViewItem;
            NavigationMenu.SelectedItem = itemContent;
            FrameworkElement root = (FrameworkElement)Window.Current.Content;
            root.RequestedTheme = AppSettings.Theme;
            SetThemeToggle(AppSettings.Theme);
        }

        private void NavigationMenu_OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {

            var selectedTag = args.SelectedItemContainer.Tag.ToString();

            switch (selectedTag)
            {
                case "DashBoard":
                    NavigationMenu.IsPaneOpen = false;
                    ContentFrame.Navigate(typeof(DashboardPage));
                    break;
                case "AccountsPage":
                    NavigationMenu.IsPaneOpen = false;
                    ContentFrame.Navigate(typeof(AccountsPage));
                    break;
                default:
                    NavigationMenu.IsPaneOpen = false;
                    ContentFrame.Navigate(typeof(AccountsPage));
                    break;
            }
        }

        private void SetThemeToggle(ElementTheme theme)
        {
            if (theme == AppSettings.LightTheme)
            {
                ThemeChangerNavigationItem.Content = "Dark";
                var titleBar = AppSettings.TitleBar;
                //titleBar.BackgroundColor = Colors.White;
                titleBar.ForegroundColor = Colors.Black;
                //titleBar.ButtonBackgroundColor= Colors.White;
                titleBar.BackgroundColor = Color.FromArgb(255, 240, 242, 245);
                titleBar.ButtonBackgroundColor = Color.FromArgb(255, 240, 242, 245);
                titleBar.ButtonForegroundColor = Colors.Black;
            }
            else
            {
                var titleBar = AppSettings.TitleBar;
                //titleBar.BackgroundColor = Colors.Black;

                titleBar.ForegroundColor = Colors.White;

                //titleBar.ButtonBackgroundColor = Colors.Black;
                titleBar.BackgroundColor = Color.FromArgb(255, 24, 25, 26);
                titleBar.ButtonBackgroundColor = Color.FromArgb(255, 24, 25, 26);
                titleBar.ButtonForegroundColor = Colors.White;
                ThemeChangerNavigationItem.Content = "Light";

            }
        }

        //private string _themeIcon;

        //public string ThemeIcon
        //{
        //    get => _themeIcon;
        //    set => SetField(ref _themeIcon, value);
        //}

        private void ThemeChanger_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            FrameworkElement window = (FrameworkElement)Window.Current.Content;

            if (AppSettings.Theme == AppSettings.LightTheme)
            {
                AppSettings.Theme = AppSettings.DarkTheme;
                var titleBar = AppSettings.TitleBar;
                //titleBar.BackgroundColor = Colors.Black;
                titleBar.ForegroundColor = Colors.White;
                //titleBar.ButtonBackgroundColor = Colors.Black;
                titleBar.ButtonForegroundColor = Colors.White;
                titleBar.BackgroundColor = Color.FromArgb(255, 24, 25, 26);
                titleBar.ButtonBackgroundColor = Color.FromArgb(255, 24, 25, 26);
                window.RequestedTheme = AppSettings.DarkTheme;
                ThemeChangerNavigationItem.Content = "Light";
                //ThemeChanger.Glyph = "&#E793;";
            }
            else
            {
                AppSettings.Theme = AppSettings.LightTheme;
                window.RequestedTheme = AppSettings.LightTheme;
                //ThemeChanger.Glyph = "&#xE945;";
                var titleBar = AppSettings.TitleBar;
                //titleBar.BackgroundColor = Colors.White;
                titleBar.ForegroundColor = Colors.Black;
                //titleBar.ButtonBackgroundColor = Colors.White;
                titleBar.BackgroundColor = Color.FromArgb(255, 240, 242, 245);
                titleBar.ButtonBackgroundColor = Color.FromArgb(255, 240, 242, 245);
                titleBar.ButtonForegroundColor = Colors.Black;
                ThemeChangerNavigationItem.Content = "Dark";

            }
        }


        private void UserLogOut_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            HomePageViewModel.UpdateUserLoggedOut();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public void Logout()
        {
            AppSettings.LocalSettings.Values.Remove("UserId");
            AppSettings.CustomerId = null;
            this.Frame.Navigate(typeof(MainPage));
        }
    }

    public interface IHomePage
    {
        void Logout();
    }
}
