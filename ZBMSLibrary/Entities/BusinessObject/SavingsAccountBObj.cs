using System.Collections.Generic;
using Windows.System;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
using ZBMSLibrary.Entities.Model;

namespace ZBMSLibrary.Entities.BusinessObject
{
    public class SavingsAccountBObj: SavingsAccount
    {
        public string UserName { get; set; }
        public string BranchName { get; set; }

        public List<TransactionSummaryVObj> TransactionList { get; set; }

        public SavingsAccountBObj()
        {
            TransactionList = new List<TransactionSummaryVObj>();
        }
        //account based calculation will be coded in here


    }
}