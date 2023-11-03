using System;
using SQLite;
using ZBMSLibrary.Entities.Enums;

namespace ZBMSLibrary.Entities.Model
{
    [Table(nameof(TransactionSummary))]
    public class TransactionSummary
    {
        [PrimaryKey]
        public string Id { get; set; }
        public TransactionType TransactionType { get; set; }
        public double Amount { get; set; }
        public string SenderAccountNumber { get;set; }
        public string ReceiverAccountNumber { get; set; }
        public DateTime TransactionOn { get; set; }
        public string Description { get; set; }

        public TransactionSummary()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}