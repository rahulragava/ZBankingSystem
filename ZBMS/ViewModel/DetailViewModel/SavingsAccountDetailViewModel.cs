using System.Collections.ObjectModel;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;

namespace ZBMS.ViewModel.DetailViewModel
{
    public class SavingsAccountDetailViewModel : ViewModelBase
    {
        public SavingsAccountBObj SavingsAccountBObj { get; set; }
        public ObservableCollection<TransactionSummaryVObj> TransactionList { get; set; }
        public ObservableCollection<Account> Accounts { get; set; }
        public SavingsAccountDetailViewModel()
        {
            TransactionList = new ObservableCollection<TransactionSummaryVObj>();
            Accounts = new ObservableCollection<Account>();
        }
        public void ClearAndAddTransaction()
        {
            TransactionList = new ObservableCollection<TransactionSummaryVObj>(SavingsAccountBObj.TransactionList);
            //TransactionList.Clear();
            //foreach (var transaction in SavingsAccountBObj.TransactionList)
            //{
            //    TransactionList.Add(transaction);
            //}
        }
    }
}