using System;
using SQLite;
using ZBMSLibrary.Entities.Enums;

namespace ZBMSLibrary.Entities.Model
{
    [Table(nameof(FixedDeposit))]
    public class FixedDeposit : Deposit
    {

        public FixedDeposit()
        {
            InterestRate = 7.8;
        }
    }

}