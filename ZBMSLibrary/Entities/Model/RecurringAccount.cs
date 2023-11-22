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
        public DateTime NextDueDate { get; set; }
        public RecurringAccount()
        {
            InterestRate = 6.8;
        }
    }
}