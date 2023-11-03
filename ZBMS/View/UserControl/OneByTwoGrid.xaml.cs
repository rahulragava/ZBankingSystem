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
    public sealed partial class OneByTwoGrid : Windows.UI.Xaml.Controls.UserControl
    {
        public OneByTwoGrid()
        {
            this.InitializeComponent();
        }

        public int NarrowScreenBreakPoint
        {
            get => (int)GetValue(NarrowScreenBreakPointProperty);
            set => SetValue(NarrowScreenBreakPointProperty, value);
        }

        public static readonly DependencyProperty NarrowScreenBreakPointProperty =
            DependencyProperty.Register(nameof(NarrowScreenBreakPoint), typeof(int), typeof(OneByTwoGrid), new PropertyMetadata(800));



        public FrameworkElement Column1Content
        {
            get => (FrameworkElement)GetValue(Column1ContentProperty);
            set => SetValue(Column1ContentProperty, value);
        }

        public static readonly DependencyProperty Column1ContentProperty =
            DependencyProperty.Register(nameof(Column1Content), typeof(FrameworkElement), typeof(OneByTwoGrid), new PropertyMetadata(null));

        public FrameworkElement Column2Content
        {
            get => (FrameworkElement)GetValue(Column2ContentProperty);
            set => SetValue(Column2ContentProperty, value);
        }

        public static readonly DependencyProperty Column2ContentProperty =
            DependencyProperty.Register(nameof(Column2Content), typeof(FrameworkElement), typeof(OneByTwoGrid), new PropertyMetadata(null));


        public string Width2Ratio
        {
            get => (string)GetValue(Width2RatioProperty);
            set => SetValue(Width2RatioProperty, value);
        }

        public static readonly DependencyProperty Width2RatioProperty =
            DependencyProperty.Register(nameof(Width2Ratio), typeof(string), typeof(OneByTwoGrid), new PropertyMetadata("*"));


        public string Width1Ratio
        {
            get => (string)GetValue(Width1RatioProperty);
            set
            {
                if (value == "1*")
                {
                    NarrowScreenBreakPoint = 800;
                }
                else if (value == "2*")
                {
                    NarrowScreenBreakPoint = 1000;
                }

                SetValue(Width1RatioProperty, value);
            }
        }

        public static readonly DependencyProperty Width1RatioProperty =
            DependencyProperty.Register(nameof(Width1Ratio), typeof(string), typeof(OneByTwoGrid), new PropertyMetadata("*"));
    }
}
