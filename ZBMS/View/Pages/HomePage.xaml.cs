using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
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
    public sealed partial class HomePage : Page, INotifyPropertyChanged, IHomePage
    {
        public HomePageViewModel HomePageViewModel;

        public HomePage()
        {
            HomePageViewModel = new HomePageViewModel(this);
            this.InitializeComponent();
            Loaded += OnLoaded;
            NavigationViewItem itemContent = NavigationMenu.MenuItems.ElementAt(0) as NavigationViewItem;
            NavigationMenu.SelectedItem = itemContent;
            FrameworkElement root = (FrameworkElement)Window.Current.Content;
            root.RequestedTheme = AppSettings.Theme;
            SetThemeToggle(AppSettings.Theme);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            HomePageViewModel.GetUser();
        }

        private void NavigationMenu_OnSelectionChanged(NavigationView sender,
            NavigationViewSelectionChangedEventArgs args)
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
                var titleBar = AppSettings.TitleBar;
                titleBar.ForegroundColor = Colors.Black;
                titleBar.BackgroundColor = Color.FromArgb(255, 240, 242, 245);
                //var a = new AcrylicBrush
                //{
                //    TintColor = Color.FromArgb(205,43,141,143),
                //    TintOpacity = 0.2,
                    
                //    BackgroundSource = AcrylicBackgroundSource.Backdrop
                //};
                titleBar.ForegroundColor = Colors.White;
                titleBar.BackgroundColor = Color.FromArgb(205, 43, 141, 143);
                titleBar.ButtonForegroundColor = Colors.White;
                titleBar.ButtonBackgroundColor = Color.FromArgb(205, 43, 141, 143);


                //var ab = Application.Current.Resources["AcrylicBrushBackground"] as Microsoft.UI.Xaml.Media.AcrylicBrush;
                //var myResourceDictionary = new ResourceDictionary();
                //myResourceDictionary.Source =
                //    new Uri("../../Styles/ThemeResource.xaml",
                //        UriKind.RelativeOrAbsolute);
                //var b = myResourceDictionary["AcrylicBackground"] as AcrylicBrush;
                // titleBar.ButtonBackgroundColor = Color.FromArgb(255, 240, 242, 245);
            }
            else
            {
                var titleBar = AppSettings.TitleBar;
                titleBar.ForegroundColor = Colors.White;
                //titleBar.BackgroundColor = Color.FromArgb(205, 43, 141, 143);
                //titleBar.ButtonBackgroundColor = Color.FromArgb(205, 43, 141, 143);
                titleBar.ButtonForegroundColor = Colors.White;
                titleBar.ButtonBackgroundColor = Colors.White;

                //titleBar.ButtonBackgroundColor = a.TintColor;
                //titleBar.BackgroundColor = Colors.Black;

                //var a =
                //    Application.Current.Resources.ThemeDictionaries.FirstOrDefault().Value;
                //var myResourceDictionary = new ResourceDictionary();
                //myResourceDictionary.Source =
                //    new Uri("Styles/ThemeResource.xaml",
                //        UriKind.RelativeOrAbsolute);
                //var b = myResourceDictionary["AcrylicBackground"] as AcrylicBrush;
                //titleBar.ButtonBackgroundColor = Colors.Black;
                //titleBar.BackgroundColor = Color.FromArgb(255, 24, 25, 26);
                //titleBar.ButtonBackgroundColor = Color.FromArgb(255, 24, 25, 26);

            }
        }

        //private string _themeIcon;

        //public string ThemeIcon
        //{
        //    get => _themeIcon;
        //    set => SetField(ref _themeIcon, value);
        //}

        //private void ThemeChanger_OnTapped(object sender, TappedRoutedEventArgs e)
        //{
        //    FrameworkElement window = (FrameworkElement)Window.Current.Content;

        //    if (AppSettings.Theme == AppSettings.LightTheme)
        //    {
        //        AppSettings.Theme = AppSettings.DarkTheme;
        //        var titleBar = AppSettings.TitleBar;
        //        //titleBar.BackgroundColor = Colors.Black;
        //        titleBar.ForegroundColor = Colors.White;
        //        //titleBar.ButtonBackgroundColor = Colors.Black;
        //        titleBar.ButtonForegroundColor = Colors.White;
        //        titleBar.BackgroundColor = Color.FromArgb(255, 24, 25, 26);
        //        titleBar.ButtonBackgroundColor = Color.FromArgb(255, 24, 25, 26);
        //        window.RequestedTheme = AppSettings.DarkTheme;
        //        //ThemeChanger.Glyph = "&#E793;";
        //    }
        //    else
        //    {
        //        AppSettings.Theme = AppSettings.LightTheme;
        //        window.RequestedTheme = AppSettings.LightTheme;
        //        //ThemeChanger.Glyph = "&#xE945;";
        //        var titleBar = AppSettings.TitleBar;
        //        //titleBar.BackgroundColor = Colors.White;
        //        titleBar.ForegroundColor = Colors.Black;
        //        //titleBar.ButtonBackgroundColor = Colors.White;
        //        titleBar.BackgroundColor = Color.FromArgb(255, 240, 242, 245);
        //        titleBar.ButtonBackgroundColor = Color.FromArgb(255, 240, 242, 245);
        //        titleBar.ButtonForegroundColor = Colors.Black;

        //    }
        //}


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

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var fe = PersonPicture;
            var menu = Flyout.GetAttachedFlyout(fe);
            menu.ShowAt(fe);
        }

        private void UserProfileControl_OnThemeChanged()
        {
            var menu = Flyout.GetAttachedFlyout(PersonPicture);
            menu.Hide();
        }

        private void UserProfileControl_OnLogOutClicked()
        {
            UserLogOut_OnTapped(new object(), new TappedRoutedEventArgs());
        }

        private void NavigationView_OnTapped(object sender, TappedRoutedEventArgs e)
        {

            NavigationMenu.IsPaneOpen = false;
            ContentFrame.Navigate(typeof(AccountsPage));


        }


        //private void AccentColorChangeItem_OnTapped(object sender, TappedRoutedEventArgs e)
        //{
        //    //var fe = PersonPicture;
        //    var menu = FlyoutBase.GetAttachedFlyout(AccentColorChangeItem);
        //    menu.ShowAt(AccentColorChangeItem);
        //}


        private void RedAccent_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            //AppSettings.AccentColor = Colors.Red;
            //AppSettings.UpdateSystemAccentColorAndBrushes(AppSettings.AccentColor);

        }

        private void OrangeAccent_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            //AppSettings.AccentColor = Colors.Orange;
            //AppSettings.UpdateSystemAccentColorAndBrushes(AppSettings.AccentColor);
        }

        private void GreenAccent_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            //AppSettings.AccentColor = Colors.Green;
            //AppSettings.UpdateSystemAccentColorAndBrushes(AppSettings.AccentColor);
        }

        private void BlueAccent_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            //AppSettings.AccentColor = Colors.Blue;
            //AppSettings.UpdateSystemAccentColorAndBrushes(AppSettings.AccentColor);
        }
    }
    public interface IHomePage
    {
        void Logout();
    }
}