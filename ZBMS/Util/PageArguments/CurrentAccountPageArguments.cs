using System.Collections.ObjectModel;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;

namespace ZBMS.Util.PageArguments
{
    public class CurrentAccountPageArguments
    {

        public CurrentAccountBObj CurrentAccountBObj { get; set; }
        public ObservableCollection<Account> Accounts { get; set; }
    }
}