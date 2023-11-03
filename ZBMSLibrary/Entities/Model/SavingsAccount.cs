using SQLite;

namespace ZBMSLibrary.Entities.Model
{
    [Table(nameof(SavingsAccount))]
    public class SavingsAccount : Account
    {
        public double InterestRate { get; set; } 
        public double ToBeCreditedAmount { get; set; }

        public SavingsAccount()
        {
            InterestRate = 4.4;
            MinimumBalance = 1000;
            ServiceCharges = 100;
        }
    }
}