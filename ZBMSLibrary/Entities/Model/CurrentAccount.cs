using SQLite;

namespace ZBMSLibrary.Entities.Model
{
    [Table(nameof(CurrentAccount))]
    public class CurrentAccount : Account
    {
        public CurrentAccount()
        {
            MinimumBalance = 1000;
            ServiceCharges = 100;
        }
    }
}