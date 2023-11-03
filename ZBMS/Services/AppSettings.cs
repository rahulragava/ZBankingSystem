using System;
using Windows.Storage;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml;
using Windows.UI.ViewManagement;

namespace ZBMS.Services
{
    public static class AppSettings
    {
        public static ApplicationDataContainer LocalSettings => ApplicationData.Current.LocalSettings;

        private static TResult GetSettingsValue<TResult>(string userId, TResult defaultValue)
        {   
            try
            {
                if (!LocalSettings.Values.ContainsKey(userId))
                {
                    LocalSettings.Values[userId] = defaultValue;
                }
                return (TResult)LocalSettings.Values[userId];
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
        public static ApplicationViewTitleBar TitleBar= TitleBar = ApplicationView.GetForCurrentView().TitleBar;
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
                    throw new System.Exception("Only set the theme to light or dark mode!");
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
    }
}