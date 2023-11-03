using SQLite;
using System;
using ZBMSLibrary.Entities.Enums;

namespace ZBMSLibrary.Entities.Model
{
    [Table(nameof(RecurringAccount))]
    public class RecurringAccount : Deposit
    {
        public FrequencyType Frequency { get; set; }
        public double MonthlyInstallment { get; set; }
        public DateTime LastPaidDate { get; set; }
        public RecurringAccount()
        {
            InterestRate = 6.8;
        }
    }
}