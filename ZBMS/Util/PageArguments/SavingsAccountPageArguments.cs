using System.Collections.ObjectModel;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;

namespace ZBMS.Util.PageArguments
{
    public class SavingsAccountPageArguments
    {
        public SavingsAccountBObj SavingsAccountBObj { get; set; }
        public ObservableCollection<Account> Accounts { get; set; }
    }
}