﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZBMS.Services;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBMS.View.UserControl
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginUserControl : Windows.UI.Xaml.Controls.UserControl, INotifyPropertyChanged
    {
        public LoginUserControl()
        {
            this.InitializeComponent();
        }

        private string _userId;
        public string UserId
        {
            get => _userId;
            set => SetField(ref _userId, value);
        }
        public event Action GoToLogInControl;
        private void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrWhiteSpace(UserId))
            {
                ErrorBlock.Visibility = Visibility.Visible;
                return;
            }
            switch (_userId)
            {
                case "1":
                    AppSettings.CustomerId = "1";
                    break;
                case "2":
                    AppSettings.CustomerId = "2";
                    break;
                default:
                    ErrorBlock.Visibility = Visibility.Visible;
                    break;
            }

            GoToLogInControl?.Invoke();
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
    }
}