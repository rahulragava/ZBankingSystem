using System.Collections.ObjectModel;
using Windows.UI.Xaml;
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
    }
}
