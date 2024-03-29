﻿using System.Collections.ObjectModel;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;

namespace ZBMS.Util.PageArguments
{
    public class FixedDepositPageArguments
    {
        public FixedDepositBObj FixedDepositBObj { get; set; }
        public ObservableCollection<Account> Accounts { get; set; }
    }
}