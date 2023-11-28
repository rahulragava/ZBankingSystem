using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml;

namespace ZBMS.Services
{
    public  class SystemAccentColorSetting : INotifyPropertyChanged
    {
        private SolidColorBrush _systemAccentBrush = new SolidColorBrush();

        public SolidColorBrush SystemAccentBrush
        {
            get => _systemAccentBrush;
            set => SetField(ref _systemAccentBrush, value);
        }

        private Color _systemAccentColor = Color.FromArgb(255, 255,0 ,0);

        public Color SystemAccentColor
        {
            get => _systemAccentColor;
            set => SetField(ref _systemAccentColor, value);
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