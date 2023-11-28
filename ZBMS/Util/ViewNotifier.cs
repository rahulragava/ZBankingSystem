using System;
using Windows.UI;
using Windows.UI.Xaml;

namespace ZBMS.Util
{
    public class ViewNotifier
    {
        private ViewNotifier() { }

        private static ViewNotifier _instance = null;

        public static ViewNotifier Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ViewNotifier();
                }
                return _instance;
            }
        }


        public event Action<string> AccentColorChanged;
        public void OnAccentColorChanged(string colorString)
        {
            AccentColorChanged?.Invoke(colorString);
        }

        public event Action CloseCollectionWindows;
        public void OnCloseCollectionWindows()
        {
            CloseCollectionWindows?.Invoke();
        }

        public event Action<ElementTheme> ThemeChanged;
        public void OnThemeChanged(ElementTheme theme)
        {
            ThemeChanged?.Invoke(theme);
        }

        public event Action UserLogOut;
        public void OnUserLoggedOut()
        {
            UserLogOut?.Invoke();
        }

    }
}