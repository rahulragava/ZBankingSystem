using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using ZBMSLibrary.Entities.Model;

namespace ZBMSLibrary.Entities.BusinessObject
{
    public class CurrentAccountBObj : CurrentAccount
    {
        public string UserName { get; set; }
        public string BranchName { get; set; }
        public List<TransactionSummary> TransactionList { get; set; }

        public CurrentAccountBObj()
        {
            TransactionList = new List<TransactionSummary>();
        }
        //account based calculation will be coded in here

    }
}