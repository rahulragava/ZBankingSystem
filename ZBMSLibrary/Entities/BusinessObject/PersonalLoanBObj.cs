using System;
using ZBMSLibrary.Entities.Model;

namespace ZBMSLibrary.Entities.BusinessObject
{
    public class PersonalLoanBObj : PersonalLoan
    {
        public string UserName { get; set; }
        public string BranchName { get; set; }

        public DateTime MaturityDateCalculator(DateTime date, int months)
        {
            return date.AddMonths(months);
        }

        //public double MonthlyPrincipalAmount()
        //{
        //    return Balance * (InterestRate / 100) / (Tenure*12);
        //}



        public double EMICalculator()
        {
            var rateOfInterest = InterestRate / 12 / 100;
            var value = Math.Pow(1 + rateOfInterest, Tenure * 12);
            var numerator = OriginalAmount * (rateOfInterest) * (value);
            var denominator = value - 1;
            return Math.Round((numerator / denominator),2);         
        }

        public double GetTotalAmount()
        {
            return EMICalculator() * (Tenure * 12);
        }
    } 
}