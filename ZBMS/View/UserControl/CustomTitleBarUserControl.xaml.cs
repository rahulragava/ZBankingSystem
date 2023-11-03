using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBMS.View.UserControl
{
    public sealed partial class CustomTitleBarUserControl : Windows.UI.Xaml.Controls.UserControl
    {
        public CustomTitleBarUserControl()
        {
            this.InitializeComponent();
            //Window.Current.SetTitleBar(AppTitleBar);
            //AppTitleTextBlock.Foreground =
            //    (SolidColorBrush)(Application.Current.Resources["ApplicationForegroundThemeBrush"]);
            //AppTitleBar.Background =
            //    (SolidColorBrush)(Application.Current.Resources["ApplicationBackground"]);

        }

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(CustomTitleBarUserControl), new PropertyMetadata(null));
        
    }
}
