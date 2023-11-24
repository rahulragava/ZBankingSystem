using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZBMS.Services;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBMS.View.UserControl
{
    public sealed partial class UserProfileControl : Windows.UI.Xaml.Controls.UserControl
    {
        public UserProfileControl()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty MailProperty =
            DependencyProperty.Register(nameof(Mail), typeof(string), typeof(UserProfileControl),
                new PropertyMetadata(null));

        public string Mail
        {
            get => (string)GetValue(MailProperty);
            set => SetValue(MailProperty, value);
        }

        public static readonly DependencyProperty PhoneNumberProperty =
            DependencyProperty.Register(nameof(PhoneNumber), typeof(string), typeof(UserProfileControl),
                new PropertyMetadata(null));

        public string PhoneNumber
        {
            get => (string)GetValue(PhoneNumberProperty);
            set => SetValue(PhoneNumberProperty, value);
        }

        public static readonly DependencyProperty UserNameProperty =
            DependencyProperty.Register(nameof(UserName), typeof(string), typeof(UserProfileControl),
                new PropertyMetadata(null));

        public string UserName
        {
            get => (string)GetValue(UserNameProperty);
            set => SetValue(UserNameProperty, value);
        }

        public static readonly DependencyProperty LastLoggedOnProperty =
            DependencyProperty.Register(nameof(LastLoggedOn), typeof(DateTime), typeof(UserProfileControl),
                new PropertyMetadata(default(DateTime)));

        public DateTime LastLoggedOn
        {
            get => (DateTime)GetValue(LastLoggedOnProperty);
            set => SetValue(LastLoggedOnProperty, value);
        }

        //private void ThemeChanger_OnTapped(object sender, TappedRoutedEventArgs e)
        //{
           
        //}
        public event Action ThemeChanged;
        private void ThemeChanger_OnClick(object sender, RoutedEventArgs e)
        {
            FrameworkElement window = (FrameworkElement)Window.Current.Content;

            if (AppSettings.Theme == AppSettings.DarkTheme)
            {
                AppSettings.Theme = AppSettings.LightTheme;
                var titleBar = AppSettings.TitleBar;
                titleBar.ForegroundColor = Colors.White;
                titleBar.BackgroundColor = Color.FromArgb(205, 43, 141, 143);
                titleBar.ButtonForegroundColor = Colors.White;
                titleBar.ButtonBackgroundColor = Color.FromArgb(205, 43, 141, 143);
                //titleBar.BackgroundColor = Colors.Black;
                //titleBar.ForegroundColor = Colors.White;
                ////titleBar.ButtonBackgroundColor = Colors.Black;
                //titleBar.ButtonForegroundColor = Colors.White;
                //titleBar.BackgroundColor = Color.FromArgb(255, 24, 25, 26);
                //titleBar.ButtonBackgroundColor = Color.FromArgb(255, 24, 25, 26);
                window.RequestedTheme = AppSettings.LightTheme;
                //ThemeChangerNavigationItem.Content = "Light";
                //ThemeChanger.Glyph = "&#E793;";
            }
            else
            {
                AppSettings.Theme = AppSettings.DarkTheme;
                window.RequestedTheme = AppSettings.DarkTheme;
                //ThemeChanger.Glyph = "&#xE945;";
                var titleBar = AppSettings.TitleBar;
                titleBar.ForegroundColor = Colors.White;
                //titleBar.BackgroundColor = Color.FromArgb(205, 43, 141, 143);
                //titleBar.ButtonBackgroundColor = Color.FromArgb(205, 43, 141, 143);
                titleBar.ButtonForegroundColor = Colors.White;
                titleBar.BackgroundColor = Color.FromArgb(205, 43, 141, 143);
                titleBar.ButtonBackgroundColor = Color.FromArgb(205, 43, 141, 143);

                //titleBar.BackgroundColor = Colors.White;
                //titleBar.ForegroundColor = Colors.Black;
                //titleBar.ButtonBackgroundColor = Colors.White;
                //titleBar.BackgroundColor = Color.FromArgb(255, 240, 242, 245);
                //titleBar.ButtonBackgroundColor = Colors.White;
                //titleBar.ButtonForegroundColor = Colors.Black;

                //ThemeChangerNavigationItem.Content = "Dark";
            }
            ThemeChanged?.Invoke();
            //ProfileControl.RequestedTheme = AppSettings.Theme;
        }
        private void PersonPicture_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
        }

        private void PersonPicture_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Hand, 1);

        }
        public event Action LogOutClicked;
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ClosingDepositContentDialog.Visibility = Visibility.Visible;
            ClosingDepositContentDialog.ShowDialog();

        }

        private void LogOutContentDialog_OnPrimaryButtonClicked()
        {
            LogOutClicked?.Invoke();
        }
    }
}
