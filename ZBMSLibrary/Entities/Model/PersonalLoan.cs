using SQLite;

namespace ZBMSLibrary.Entities.Model
{
    [Table(nameof(PersonalLoan))]
    public class PersonalLoan : Loan
    {
        public PersonalLoan()
        {
            InterestRate = 9.24;
        }
    }
}