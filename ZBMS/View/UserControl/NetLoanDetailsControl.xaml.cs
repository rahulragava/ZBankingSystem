using System;
using System.Collections.ObjectModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using ZBMS.ViewModel;
using ZBMSLibrary.Entities.Model;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBMS.View.UserControl
{
    public sealed partial class NetLoanDetailsControl : Windows.UI.Xaml.Controls.UserControl
    {
        public readonly ConsolidatedReportViewModel ConsolidatedReportViewModel;
        public NetLoanDetailsControl()
        {
            ConsolidatedReportViewModel = new ConsolidatedReportViewModel();
            this.InitializeComponent();
        }

        public void SetLoans()
        {
            ConsolidatedReportViewModel.SetLoans(LoanList);
            ConsolidatedReportViewModel.SetCumulativeLoanDues();
        }

        public static readonly DependencyProperty LoanListProperty = DependencyProperty.Register(
            nameof(LoanList), typeof(ObservableCollection<Loan>), typeof(NetLoanDetailsControl), new PropertyMetadata(default(ObservableCollection<Loan>)));

        public ObservableCollection<Loan> LoanList
        {
            get => (ObservableCollection<Loan>)GetValue(LoanListProperty);
            set => SetValue(LoanListProperty, value);
        }

        public event Action MoneyBagClicked;
        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            MoneyBagClicked?.Invoke();
        }

        private void CreateAccount_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Hand, 1);
        }

        private void CreateAccount_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {

            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
        }
    }
}
