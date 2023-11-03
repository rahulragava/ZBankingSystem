using System.Collections.ObjectModel;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;

namespace ZBMS.ViewModel.DetailViewModel
{
    public class SavingsAccountDetailViewModel : ViewModelBase
    {
        public SavingsAccountBObj SavingsAccountBObj { get; set; }
        public ObservableCollection<TransactionSummary> TransactionList { get; set; }

        public SavingsAccountDetailViewModel()
        {
            TransactionList = new ObservableCollection<TransactionSummary>();
        }

        public void ClearAndAddTransaction()
        {
            TransactionList.Clear();
            foreach (var transaction in SavingsAccountBObj.TransactionList)
            {
                TransactionList.Add(transaction);
            }
        }
    }
}