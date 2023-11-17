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
        //public static UISettings UiSettings;
        //public static ResourceDictionary AppResource = Application.Current.Resources;

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
        public static ApplicationViewTitleBar TitleBar = ApplicationView.GetForCurrentView().TitleBar;

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
        //private static void LoadAccentColor()
        //{
        //    //var appResources = Application.Current.Resources;

        //    // Load the saved accent color
        //    if (ApplicationData.Current.LocalSettings.Values.TryGetValue("UserAccentColor", out var savedColor))
        //    {
        //        var colorComponents = savedColor.ToString().Split();

        //        if (colorComponents.Length == 3 &&
        //            byte.TryParse(colorComponents[0], out byte r) &&
        //            byte.TryParse(colorComponents[1], out byte g) &&
        //            byte.TryParse(colorComponents[2], out byte b))
        //        {
        //            Color savedAccentColor = Color.FromArgb(255, r, g, b);
        //            AppResources["CustomAccentColor"] = savedAccentColor;
        //        }
        //    }
        //}
        //private static void SaveAccentColor(Color newAccentColor)
        //{

        //    // Save the accent color
        //    ApplicationData.Current.LocalSettings.Values["UserAccentColor"] = $"{newAccentColor.R},{newAccentColor.G},{newAccentColor.B}";
        //}
       


    }
}