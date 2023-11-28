using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Core.Preview;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using ZBMS.Services;
using ZBMS.Util;
using ZBMS.Util.PageArguments;
using ZBMS.View.UserControl;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBMS.View.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            this.InitializeComponent();
            Loaded += ProfilePage_Loaded;
            Unloaded += ProfilePage_Unloaded;
        }

        //private UserProfileControl _userProfileControl;

        private void ProfilePage_Unloaded(object sender, RoutedEventArgs e)
        {
            //_userProfileControl.ThemeChanged -= ProfilePage_ThemeChanged;
            ViewNotifier.Instance.ThemeChanged -= OnThemeChanged;
            ViewNotifier.Instance.AccentColorChanged -= InstanceOnAccentColorChanged;
            //ViewNotifier.Instance.ThemeChanged -= OnThemeChanged;

        }

       

        private void ProfilePage_Loaded(object sender, RoutedEventArgs e)
        {
            //var a = Thread.CurrentThread.ManagedThreadId;
            //RootGrid.Children.Add(_userProfileControl);
            //_userProfileControl.ThemeChanged += ProfilePage_ThemeChanged;
            SetFontVisibility();
            ViewNotifier.Instance.ThemeChanged -= OnThemeChanged;
            ViewNotifier.Instance.ThemeChanged += OnThemeChanged;
            ViewNotifier.Instance.AccentColorChanged -= InstanceOnAccentColorChanged;
            ViewNotifier.Instance.AccentColorChanged += InstanceOnAccentColorChanged;
            ViewNotifier.Instance.UserLogOut -= InstanceOnUserLogOut;
            ViewNotifier.Instance.UserLogOut += InstanceOnUserLogOut;
            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested -= OnCloseRequested;
            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += OnCloseRequested;

            //var colorComponents = AppSettings.CustomColor.Split(" ");
            //var r = Byte.Parse(colorComponents[0]);
            //var g = Byte.Parse(colorComponents[1]);
            //var b = Byte.Parse(colorComponents[2]);

            //var color = Color.FromArgb(255, r, g, b);
            //var ab = Accent4.Fill;
            
            //if (Accent1.Fill == new SolidColorBrush(color))
            //{
            //    Accent1.Stroke = new SolidColorBrush(Colors.White);
            //    Accent1.StrokeThickness = 2;
            //}
            //else if (Accent2.Fill == new SolidColorBrush(color))
            //{
            //    Accent2.Stroke = new SolidColorBrush(Colors.White);
            //    Accent2.StrokeThickness = 2;
            //}
            //else if (Accent3.Fill == new SolidColorBrush(color))
            //{
            //    Accent3.Stroke = new SolidColorBrush(Colors.White);
            //    Accent3.StrokeThickness = 2;
            //}
            //else if (Accent4.Fill == new SolidColorBrush(color))
            //{
            //    Accent4.Stroke = new SolidColorBrush(Colors.White);
            //    Accent4.StrokeThickness = 2;
            //}
            //ViewNotifier.Instance.UserLogOut += InstanceOnUserLogOut;
            //ViewNotifier.Instance.AccentColorChanged += ThemeSelector_OnAccentColorChanged;
        }

       

        private void OnCloseRequested(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            ViewNotifier.Instance.ThemeChanged -= OnThemeChanged;
            ViewNotifier.Instance.AccentColorChanged -= InstanceOnAccentColorChanged;
            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested -= OnCloseRequested;

            var viewId = WindowService.ViewCollection.First(view => view.Value.Name == nameof(ProfilePage)).Key;
            WindowService.ViewCollection.Remove(viewId);
            CoreApplication.GetCurrentView().CoreWindow.Close();
        }

        public string PhoneNumber;
        public string Mail;
        public string UserName;
        public DateTime LastLoggedOn;

        private async void ThemeSelector_OnAccentColorChanged(Color color)
        {
            await Dispatcher.CallOnUIThreadAsync(() =>
            {
                var r = color.R; var g = color.G;
                var b = color.B;
                var colorString = $"{r} {g} {b}";
                AppSettings.SetCustomAccent(colorString);
            });
        }
        //public ApplicationViewTitleBar TitleBar = ApplicationView.GetForCurrentView().TitleBar;

        private async void OnThemeChanged(ElementTheme theme)
        {
            await Dispatcher.CallOnUIThreadAsync(() =>
            {
                ((FrameworkElement)Window.Current.Content).RequestedTheme = this.RequestedTheme = theme;
                //ApplicationView.GetForCurrentView().TitleBar.BackgroundColor = Color.FromArgb(255,255,0,0);
            });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            base.OnNavigatedTo(e);
            //TitleBar.ButtonBackgroundColor = Color.FromArgb();
            if (e.Parameter is ProfilePageArguments pageArgs)
            {
                PhoneNumber = pageArgs.PhoneNumber;
                Mail = pageArgs.Mail;
                UserName = pageArgs.UserName;
                LastLoggedOn = pageArgs.LastLoggedOn;
            }
            AppSettings.SetCustomAccent(AppSettings.CustomColor);
            AppSettings.SetTitleBar(ApplicationView.GetForCurrentView().TitleBar);
            //_userProfileControl = ((UserProfileControl)e.Parameter);
        }

        private async void InstanceOnAccentColorChanged(string colorString)
        {
            await Dispatcher.CallOnUIThreadAsync(() =>
            {
                AppSettings.SetCustomAccent(colorString);
                SetFontVisibility();
                AppSettings.SetTitleBar(ApplicationView.GetForCurrentView().TitleBar);
            });
  
        }
        //private void OnAccentChanged()
        //{
        //    ViewNotifier.Instance.OnAccentColorChanged();
        //}

        //private void ProfilePage_ThemeChanged(ElementTheme elementTheme)
        //{
        //    ViewNotifier.Instance.OnThemeChanged(elementTheme);
        //}
        private void PersonPicture_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
        }

        private void PersonPicture_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Hand, 1);
        }
        //public readonly ApplicationViewTitleBar TitleBar = ApplicationView.GetForCurrentView().TitleBar;

        private void ThemeChanger_OnClick(object sender, RoutedEventArgs e)
        {

            FrameworkElement window = (FrameworkElement)Window.Current.Content;

            if (AppSettings.Theme == AppSettings.DarkTheme)
            {
                AppSettings.Theme = AppSettings.LightTheme;
                window.RequestedTheme = AppSettings.LightTheme;

            }
            else
            {
                AppSettings.Theme = AppSettings.DarkTheme;
                window.RequestedTheme = AppSettings.DarkTheme;
            }
            ViewNotifier.Instance.OnThemeChanged(AppSettings.Theme);
            //ApplicationView.GetForCurrentView().TitleBar = TitleBar;
            //ThemeChanged?.Invoke(window.RequestedTheme);
        }

        private void AccentChange_OnClick(object sender, RoutedEventArgs e)
        {
            if ((sender as StackPanel)?.Background is SolidColorBrush changeColor)
            {
                if ((bool)((StackPanel)sender)?.Name.Equals(nameof(Accent1)))
                {
                    AccentFont1.Visibility = Visibility.Visible;
                    AccentFont2.Visibility = Visibility.Collapsed;
                    AccentFont3.Visibility = Visibility.Collapsed;
                    AccentFont4.Visibility = Visibility.Collapsed;
                }
                else if ((bool)((StackPanel)sender)?.Name.Equals(nameof(Accent2)))
                {
                    AccentFont2.Visibility = Visibility.Visible;
                    AccentFont1.Visibility = Visibility.Collapsed;
                    AccentFont3.Visibility = Visibility.Collapsed;
                    AccentFont4.Visibility = Visibility.Collapsed;
                }
                else if ((bool)((StackPanel)sender)?.Name.Equals(nameof(Accent3)))
                {
                    AccentFont3.Visibility = Visibility.Visible;
                    AccentFont1.Visibility = Visibility.Collapsed;
                    AccentFont2.Visibility = Visibility.Collapsed;
                    AccentFont4.Visibility = Visibility.Collapsed;
                }
                else if ((bool)((StackPanel)sender)?.Name.Equals(nameof(Accent4)))
                {
                    AccentFont4.Visibility = Visibility.Visible;
                    AccentFont1.Visibility = Visibility.Collapsed;
                    AccentFont3.Visibility = Visibility.Collapsed;
                    AccentFont2.Visibility = Visibility.Collapsed;
                }

                byte red = changeColor.Color.R;
                byte green = changeColor.Color.G;
                byte blue = changeColor.Color.B;

                string colorString = $"{red} {green} {blue}";
                AppSettings.SetCustomAccent(colorString);
                AppSettings.SetTitleBar(ApplicationView.GetForCurrentView().TitleBar);
                ViewNotifier.Instance.OnAccentColorChanged(colorString);
            }
        }


        private void SetFontVisibility()
        {
            var colorComponents = AppSettings.CustomColor.Split(" ");
            var r = Byte.Parse(colorComponents[0]);
            var g = Byte.Parse(colorComponents[1]);
            var b = Byte.Parse(colorComponents[2]);

            var color = Color.FromArgb(255, r, g, b);
            //var brush = Application.Current.Resources["AppBarItemForegroundThemeBrush"];

            //Accent3.Fill.()
            if (((SolidColorBrush)Accent1.Background).Color.Equals(color))
            {
                AccentFont1.Visibility = Visibility.Visible;
                AccentFont2.Visibility = Visibility.Collapsed;
                AccentFont3.Visibility = Visibility.Collapsed;
                AccentFont4.Visibility = Visibility.Collapsed;
            }
            else if (((SolidColorBrush)Accent2.Background).Color.Equals(color))
            {
                AccentFont2.Visibility = Visibility.Visible;
                AccentFont1.Visibility = Visibility.Collapsed;
                AccentFont3.Visibility = Visibility.Collapsed;
                AccentFont4.Visibility = Visibility.Collapsed;
            }
            else if (((SolidColorBrush)Accent3.Background).Color.Equals(color))
            {
                AccentFont3.Visibility = Visibility.Visible;
                AccentFont1.Visibility = Visibility.Collapsed;
                AccentFont2.Visibility = Visibility.Collapsed;
                AccentFont4.Visibility = Visibility.Collapsed;
            }
            else if (((SolidColorBrush)Accent4.Background).Color.Equals(color))
            {
                AccentFont4.Visibility = Visibility.Visible;
                AccentFont1.Visibility = Visibility.Collapsed;
                AccentFont3.Visibility = Visibility.Collapsed;
                AccentFont2.Visibility = Visibility.Collapsed;
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ClosingDepositContentDialog.Visibility = Visibility.Visible;
            ClosingDepositContentDialog.ShowDialog();

        }

        private void LogOutContentDialog_OnPrimaryButtonClicked()
        {
            ViewNotifier.Instance.ThemeChanged -= OnThemeChanged;
            ViewNotifier.Instance.AccentColorChanged -= InstanceOnAccentColorChanged;
            ViewNotifier.Instance.UserLogOut -= InstanceOnUserLogOut;

            ViewNotifier.Instance.OnUserLoggedOut();
            var viewId = WindowService.ViewCollection.First(view => view.Value.Name == typeof(ProfilePage).Name).Key;
            WindowService.ViewCollection.Remove(viewId);
            CoreApplication.GetCurrentView().CoreWindow.Close();
        }

        private void InstanceOnUserLogOut()
        {
            var a = Thread.CurrentThread.ManagedThreadId;

     
                var b = Thread.CurrentThread.ManagedThreadId;
                ViewNotifier.Instance.ThemeChanged -= OnThemeChanged;
                ViewNotifier.Instance.AccentColorChanged -= InstanceOnAccentColorChanged;
                ViewNotifier.Instance.UserLogOut -= InstanceOnUserLogOut;
                //var viewId = WindowService.ViewCollection.First(view => view.Value.Name == typeof(ProfilePage).Name).Key;
                //WindowService.ViewCollection.Remove(viewId);


                //CoreApplication.GetCurrentView().CoreWindow.Close();
                //LogOutContentDialog_OnPrimaryButtonClicked();

            
            //LogOutContentDialog_OnPrimaryButtonClicked();
        }
    }
}
