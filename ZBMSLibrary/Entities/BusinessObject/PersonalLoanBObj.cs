using ZBMSLibrary.Entities.Model;

namespace ZBMSLibrary.Entities.BusinessObject
{
    public class PersonalLoanBObj : Loan
    {
        public string UserName { get; set; }
        public string BranchName { get; set; }
    }
}