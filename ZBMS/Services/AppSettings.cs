using System;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media;
using Microsoft.Toolkit.Uwp.Helpers;

namespace ZBMS.Services
{
    public static class AppSettings
    {
        public static ApplicationDataContainer LocalSettings => ApplicationData.Current.LocalSettings;

        private static TResult GetSettingsValue<TResult>(string containerKey, TResult defaultValue)
        {
            //var appResources = Application.Current.Resources;
            try
            {
                if (!LocalSettings.Values.ContainsKey(containerKey))
                {
                    LocalSettings.Values[containerKey] = defaultValue;
                }
                return (TResult)LocalSettings.Values[containerKey];
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return defaultValue;
            }
        }

        private static void SetSettingsValue(string name, object value)
        {
            LocalSettings.Values[name] = value;
        }

        public static string CustomerId
        {
            get => GetSettingsValue("UserId", default(string));
            set => LocalSettings.Values["UserId"] = value;
        }

        public const ElementTheme LightTheme = ElementTheme.Light;
        public const ElementTheme DarkTheme = ElementTheme.Dark;
        public static readonly ApplicationViewTitleBar TitleBar = ApplicationView.GetForCurrentView().TitleBar;

        //Theme change
        public static ElementTheme Theme
        {
            get
            {
                // Never set: default theme
                if (LocalSettings.Values["AppTheme"] == null)
                {
                    LocalSettings.Values["AppTheme"] = (int)LightTheme;
                    return LightTheme;
                }
                // Previously set to default theme
                else if ((int)LocalSettings.Values["AppTheme"] == (int)LightTheme)
                {
                    return LightTheme;
                }
                // Previously set to non-default theme
                else
                {
                    return DarkTheme;
                }
            }
            set
            {
                // Error check
                if (value == ElementTheme.Default)
                {
                    //throw new System.Exception("Only set the theme to light or dark mode!");
                }
                // Never set
                else if (LocalSettings.Values["AppTheme"] == null)
                {
                    LocalSettings.Values["AppTheme"] = (int)value;

                }
                // No change
                else if ((int)value == (int)LocalSettings.Values["AppTheme"])
                {
                    return;
                }
                // Change
                else
                {
                    LocalSettings.Values["AppTheme"] = (int)value;
                }
            }
        }



        ////Custom accent color

        public static string CustomColor
        {
            get => GetSettingsValue("CustomAccentColor", "0 139 139");
            set => LocalSettings.Values["CustomAccentColor"] = value;
        }

        public static void InitializeCustomAccent()
        {
            var colorComponents = CustomColor.Split(" ");
            SetColor(colorComponents);
        }

        public static void SetCustomAccent(string color)
        {
            CustomColor = color;
            var colorComponents = color.Split(" ");
            SetColor(colorComponents);
            
        }

        private static void SetColor(string[] colorComponents)
        {
            if (colorComponents.Length == 3 &&
                byte.TryParse(colorComponents[0], out byte r) &&
                byte.TryParse(colorComponents[1], out byte g) &&
                byte.TryParse(colorComponents[2], out byte b))
            {
                TitleBar.BackgroundColor = Color.FromArgb(255, r, g, b);
                TitleBar.ButtonBackgroundColor = Color.FromArgb(255, r, g, b);
                TitleBar.ButtonForegroundColor = Colors.White;
                TitleBar.InactiveBackgroundColor = Color.FromArgb(100, r, g, b);
                TitleBar.ButtonInactiveBackgroundColor = Color.FromArgb(100, r, g, b);
                TitleBar.ForegroundColor = Colors.White;
                ((SolidColorBrush)Application.Current.Resources["SystemControlBackgroundAccentBrush"]).Color = Color.FromArgb(255, r, g, b); ;
                ((SolidColorBrush)Application.Current.Resources["SystemControlHighlightAccentBrush"]).Color = Color.FromArgb(255, r, g, b); ;
                ((SolidColorBrush)Application.Current.Resources["SystemControlHighlightAltAccentBrush"]).Color = Color.FromArgb(255, r, g, b); ;
                ((SolidColorBrush)Application.Current.Resources["SystemControlHighlightAltListAccentHighBrush"]).Color = Color.FromArgb(255, r, g, b); ;
                ((SolidColorBrush)Application.Current.Resources["SystemControlHighlightAltListAccentLowBrush"]).Color = Color.FromArgb(255, r, g, b);
                ((SolidColorBrush)Application.Current.Resources["SystemControlHighlightAltListAccentMediumBrush"]).Color = Color.FromArgb(255, r, g, b); ;
                ((SolidColorBrush)Application.Current.Resources["SystemControlHighlightListAccentLowBrush"]).Color = Color.FromArgb(255, r, g, b); ;
                ((SolidColorBrush)Application.Current.Resources["SystemControlHighlightListAccentHighBrush"]).Color = Color.FromArgb(255, r, g, b); ;
                ((SolidColorBrush)Application.Current.Resources["SystemControlHighlightListAccentMediumBrush"]).Color = Color.FromArgb(255, r, g, b); ;
                ((SolidColorBrush)Application.Current.Resources["SystemControlHyperlinkTextBrush"]).Color = Color.FromArgb(255, r, g, b); ;
                ((SolidColorBrush)Application.Current.Resources["ContentDialogBorderThemeBrush"]).Color = Color.FromArgb(255, r, g, b); ;
                ((SolidColorBrush)Application.Current.Resources["JumpListDefaultEnabledBackground"]).Color = Color.FromArgb(255, r, g, b);
                ((SolidColorBrush)Application.Current.Resources["ListViewItemBackgroundSelected"]).Color = Color.FromArgb(255, r, g, b);
                ((SolidColorBrush)Application.Current.Resources["ListViewItemBackgroundSelectedPointerOver"]).Color = Color.FromArgb(255, r, g, b);
                ((SolidColorBrush)Application.Current.Resources["NavigationViewSelectionIndicatorForeground"]).Color = Color.FromArgb(255, r, g, b);
                ((SolidColorBrush)Application.Current.Resources["SelectionHighlightColor"]).Color = Color.FromArgb(255, r, g, b);
                ((SolidColorBrush)Application.Current.Resources["PageBackground"]).Color = Color.FromArgb(85, r, g, b);
                ((SolidColorBrush)Application.Current.Resources["NavigationViewExpandedPaneBackground"]).Color = Color.FromArgb(55, r, g, b);
                ((SolidColorBrush)Application.Current.Resources["NavigationViewDefaultPaneBackground"]).Color = Color.FromArgb(55, r, g, b);
                ((SolidColorBrush)Application.Current.Resources["NewWindowBackground"]).Color = Color.FromArgb(85, r, g, b);

            }
        }

        public static void SetTitleBar(ApplicationViewTitleBar appTitleBar)
        {
            var colorComponents = CustomColor.Split(" ");
            if (colorComponents.Length == 3 &&
                byte.TryParse(colorComponents[0], out byte r) &&
                byte.TryParse(colorComponents[1], out byte g) &&
                byte.TryParse(colorComponents[2], out byte b))
            {
                appTitleBar.BackgroundColor = Color.FromArgb(255, r, g, b);
                appTitleBar.ButtonBackgroundColor = Color.FromArgb(255, r, g, b);
                appTitleBar.InactiveBackgroundColor = Color.FromArgb(100, r, g, b);
                appTitleBar.ButtonInactiveBackgroundColor = Color.FromArgb(100, r, g, b);
                appTitleBar.ButtonForegroundColor = Colors.White;
                appTitleBar.ForegroundColor = Colors.White;
            }
        }
    }
}