using System.Collections.ObjectModel;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;

namespace ZBMS.Util
{
    public class LoanPageArguments
    {
        public PersonalLoanBObj PersonalLoanBObj { get; set; }
        public ObservableCollection<Account> Accounts { get; set; }
    }
}