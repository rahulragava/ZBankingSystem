using System.Collections.ObjectModel;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;

namespace ZBMS.Util.PageArguments
{
    public class RecurringDepositPageArguments
    {
        public RecurringAccountBObj RecurringAccountBObj{ get; set; }
        public ObservableCollection<Account> Accounts { get; set; }
    }
}