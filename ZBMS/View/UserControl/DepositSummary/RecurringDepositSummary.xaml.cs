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
    public sealed partial class RecurringDepositSummary : Windows.UI.Xaml.Controls.UserControl
    {
        public RecurringDepositSummary()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty RecurringDepositBObjProperty =
            DependencyProperty.Register(nameof(RecurringDepositBObj), typeof(RecurringAccountBObj),typeof(RecurringDepositSummary),
                new PropertyMetadata(null));

        public RecurringAccountBObj RecurringDepositBObj
        {
            get => (RecurringAccountBObj)GetValue(RecurringDepositBObjProperty);
            set => SetValue(RecurringDepositBObjProperty, value);
        }
    }
}
