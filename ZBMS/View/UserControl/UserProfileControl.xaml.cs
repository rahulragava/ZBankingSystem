using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using ZBMS.Services;
using ZBMS.Util;
using ZBMS.Util.PageArguments;
using ZBMS.View.Pages;
using NewsStyleUriParser = System.NewsStyleUriParser;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBMS.View.UserControl
{
    public sealed partial class UserProfileControl : Windows.UI.Xaml.Controls.UserControl
    {
        public UserProfileControl()
        {
            this.InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //ViewNotifier.Instance.ThemeChanged += OnThemeChanged;
            SetFontVisibility();
        }

       

        private void OnThemeChanged(ElementTheme obj)
        {
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
        //public event Action<ElementTheme> ThemeChanged;
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
            ViewNotifier.Instance.OnThemeChanged(window.RequestedTheme);
            //ThemeChanged?.Invoke(window.RequestedTheme);
        }
        private void PersonPicture_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (sender is FontIcon icon)
            {
                icon.FontSize = 19;
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
            }
            //else if(sender is Ellipse ellipse)
            //{
            //    ellipse.Height = 20;
            //    ellipse.Width = 20;
            //    Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);

            //}
            else
            {
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);

            }
        }

        private void PersonPicture_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is FontIcon icon)
            {
                icon.FontSize = 21;
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Hand, 1);
            }
            //else if (sender is Ellipse ellipse)
            //{
            //    ellipse.Height = 23;
            //    ellipse.Width = 23;
            //    Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);

            //}
            else
            {
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Hand, 1);

            }
        }
        public event Action LogOutClicked;
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ClosingDepositContentDialog.Visibility = Visibility.Visible;
            ClosingDepositContentDialog.ShowDialog();

        }

        private void LogOutContentDialog_OnPrimaryButtonClicked()
        {

            //WindowService.CloseWindow();
            var a = Thread.CurrentThread.ManagedThreadId;
            ViewNotifier.Instance.OnUserLoggedOut();
            WindowService.CloseWindow();
            //LogOutClicked?.Invoke();
        }

        //public event Action AccentChanged;
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
                //((Ellipse)sender).Stroke = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                //((Ellipse)sender).StrokeThickness = 2;


                //if (((Ellipse)sender) == Accent1)
                //{
                //    Accent2.StrokeThickness = 0;
                //    Accent3.StrokeThickness = 0;
                //    Accent4.StrokeThickness = 0;
                //}
                //else if (((Ellipse)sender) == Accent2)
                //{
                //    Accent1.StrokeThickness = 0;
                //    Accent3.StrokeThickness = 0;
                //    Accent4.StrokeThickness = 0;
                //}
                //else if (((Ellipse)sender) == Accent3)
                //{
                //    Accent1.StrokeThickness = 0;
                //    Accent2.StrokeThickness = 0;
                //    Accent4.StrokeThickness = 0;
                //}
                //else if (((Ellipse)sender) == Accent4)
                //{
                //    Accent1.StrokeThickness = 0;
                //    Accent3.StrokeThickness = 0;
                //    Accent1.StrokeThickness = 0;
                //}
                byte red = changeColor.Color.R;
                byte green = changeColor.Color.G;
                byte blue = changeColor.Color.B;

                string colorString = $"{red} {green} {blue}";
                AppSettings.SetCustomAccent(colorString);
                ViewNotifier.Instance.OnAccentColorChanged(colorString);

                //AccentChanged?.Invoke();
            }
        }

        public void AccentChanged()
        {
            SetFontVisibility();
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

        private async void NewWindowIcon_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            //var profileControl = new UserProfileControl() { DataContext = this.DataContext };
            var profileArgs = new ProfilePageArguments
            {
                UserName = this.UserName,
                Mail = this.Mail,
                LastLoggedOn = this.LastLoggedOn,
                PhoneNumber = this.PhoneNumber,
            };
            await WindowService.ShowOrSwitchAsync<ProfilePage>( profileArgs,false);

        }
    }
}
