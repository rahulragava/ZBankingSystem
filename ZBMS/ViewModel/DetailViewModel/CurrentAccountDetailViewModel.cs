using System.Collections.ObjectModel;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;

namespace ZBMS.ViewModel.DetailViewModel
{
    public class CurrentAccountDetailViewModel : ViewModelBase
    {
        public CurrentAccountBObj CurrentAccountBObj { get; set; }
        public ObservableCollection<TransactionSummary> TransactionList { get; set; }

        public CurrentAccountDetailViewModel()
        {
            TransactionList = new ObservableCollection<TransactionSummary>();
        }

        public void ClearAndAddTransaction()
        {
            TransactionList.Clear();
            foreach (var transaction in CurrentAccountBObj.TransactionList)
            {
                TransactionList.Add(transaction);
            }
        }
    }
}