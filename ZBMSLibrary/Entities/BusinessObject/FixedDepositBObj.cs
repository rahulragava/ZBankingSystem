using System;
using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
using ZBMSLibrary.Entities.Enums;
using ZBMSLibrary.Entities.Model;

namespace ZBMSLibrary.Entities.BusinessObject
{
    public class FixedDepositBObj : FixedDeposit
    {
        public DateTime MaturityDate { get; set; }
        public double MaturityAmount { get; set; }
        public string UserName { get; set; }
        public string BranchName { get; set; }
        public List<TransactionSummary> TransactionList { get; set; }
        //public int TenureInMonths { get; set; }
        public FixedDepositBObj()
        {
            TransactionList = new List<TransactionSummary>();
        }
        public void SetDefault()
        {
            //InterestRate = GetFixedInterestRate(Tenure);
            MaturityAmount = MaturityAmountCalculator(DepositedAmount, Tenure*12);
            MaturityDate = MaturityDateCalculator(CreatedOn, Tenure*12);
        }

        public double MaturityAmountCalculator(double amount, double interestRate)
        {
            //P x(1 + r / 100)^nt
            var val = (4 * Tenure);
            var interest = interestRate / 400;
            var estimatedValue = DepositedAmount * (Math.Pow(1 + (interest), val));
            return Math.Round(estimatedValue, 2);
            //return amount + (amount * (interestRate / 100)*Tenure);
        }

        public DateTime MaturityDateCalculator(DateTime date, int months)
        {
            return date.AddMonths(months);
        }

        public static double GetFixedInterestRate(int tenureInMonths)
        {

            //switch (tenureInMonths)
            //{
            //    case int n when (n > 0 && n <= 3): return 1;
            //    case int n when (n > 3 && n <= 6): return 1.3;
            //    case int n when (n == 9): return 1.69;
            //    case int n when (n == 10): return 1.86;
            //    case int n when (n > 10 && n <= 24): return 3;
            //    case int n when (n > 24 && n <= 36): return 5;
            //    default: return 0;
            //}
            return 0.0;
        }

        internal double CalculateClosingAmount(DateTime now)
        {
            if (AccountStatus == AccountStatus.Closed)
            {
                //TenureInMonths = ((now.Year - CreatedOn.Year) * 12) + now.Month - CreatedOn.Month;
                var months = ((now.Year - CreatedOn.Year) * 12) + now.Month - CreatedOn.Month;
                if (months <= 0) return 0;
                var interestRate = GetFixedInterestRate(months);
                return MaturityAmountCalculator(DepositedAmount, interestRate);
            }
            return 0;
        }
    }
}