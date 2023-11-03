using SQLite;
using System;
using ZBMSLibrary.Entities.Enums;

namespace ZBMSLibrary.Entities.Model
{
    public abstract class Deposit
    {
        [PrimaryKey]
        public string AccountNumber { get; set; }
        public string IfscCode { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public double DepositedAmount { get; set; }
        public double InterestRate { get; set; }
        public int Tenure { get; set; }
        public string SavingsAccountId { get; set; }
        public string FromAccountId { get; set; }
    }
}