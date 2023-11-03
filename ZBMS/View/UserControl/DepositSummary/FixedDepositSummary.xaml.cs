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
using ZBMS.View.UserControl.AccountSummary;
using ZBMSLibrary.Entities.BusinessObject;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBMS.View.UserControl.DepositSummary
{
    public sealed partial class FixedDepositSummary : Windows.UI.Xaml.Controls.UserControl
    {
        public FixedDepositSummary()
        {
            this.InitializeComponent();
        }
        public static readonly DependencyProperty FixedDepositBObjProperty =
            DependencyProperty.Register(nameof(FixedDepositBObj), typeof(FixedDepositBObj), typeof(FixedDepositSummary),
                new PropertyMetadata(null));

        public FixedDepositBObj FixedDepositBObj
        {
            get => (FixedDepositBObj)GetValue(FixedDepositBObjProperty);
            set => SetValue(FixedDepositBObjProperty, value);
        }
    }
}
