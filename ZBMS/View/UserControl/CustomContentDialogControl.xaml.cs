using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class CustomContentDialogControl : Windows.UI.Xaml.Controls.UserControl
    {
        public CustomContentDialogControl()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(DialogTitle), typeof(string), typeof(CustomContentDialogControl), null);

        public static readonly DependencyProperty DialogContentTextProperty =
            DependencyProperty.Register(nameof(DialogContent), typeof(string), typeof(CustomContentDialogControl), null);

        public static readonly DependencyProperty PrimaryButtonTextProperty =
            DependencyProperty.Register(nameof(DialogPrimaryButtonText), typeof(string), typeof(CustomContentDialogControl), null);

        public static readonly DependencyProperty SecondaryButtonTextProperty =
            DependencyProperty.Register(nameof(DialogSecondaryButtonText), typeof(string), typeof(CustomContentDialogControl), null);

        public static readonly DependencyProperty CloseButtonTextProperty =
            DependencyProperty.Register(nameof(DialogCloseButtonText), typeof(string), typeof(CustomContentDialogControl), null);


        // PropertyWrappers
        public string DialogTitle
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public string DialogContent
        {
            get => (string)GetValue(DialogContentTextProperty);
            set => SetValue(DialogContentTextProperty, value);
        }

        public string DialogPrimaryButtonText
        {
            get => (string)GetValue(PrimaryButtonTextProperty);
            set => SetValue(PrimaryButtonTextProperty, value);
        }

        public string DialogSecondaryButtonText
        {
            get => (string)GetValue(SecondaryButtonTextProperty);
            set => SetValue(SecondaryButtonTextProperty, value);
        }

        public string DialogCloseButtonText
        {
            get => (string)GetValue(CloseButtonTextProperty);
            set => SetValue(CloseButtonTextProperty, value);
        }

        private void UserDefinedDialog_OnCloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        public event Action PrimaryButtonClicked;
        private void UserDefinedDialog_OnPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            PrimaryButtonClicked?.Invoke();
        }

        public void ShowDialog()
        {
            UserDefinedDialog.ShowAsync();
        }
    }
}
