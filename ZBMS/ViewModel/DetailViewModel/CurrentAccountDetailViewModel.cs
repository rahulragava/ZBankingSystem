using System.Collections.ObjectModel;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;

namespace ZBMS.ViewModel.DetailViewModel
{
    public class CurrentAccountDetailViewModel : ViewModelBase
    {
        public CurrentAccountBObj CurrentAccountBObj { get; set; }
        public ObservableCollection<TransactionSummaryVObj> TransactionList { get; set; }
        public ObservableCollection<Account> Accounts { get; set; }

        public CurrentAccountDetailViewModel()
        {
            TransactionList = new ObservableCollection<TransactionSummaryVObj>();
            Accounts = new ObservableCollection<Account>();

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