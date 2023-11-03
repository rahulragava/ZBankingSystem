using System;
using System.Collections.Generic;
using ZBMSLibrary.Entities.Enums;
using ZBMSLibrary.Entities.Model;

namespace ZBMSLibrary.Entities.BusinessObject
{
    public class RecurringAccountBObj : RecurringAccount
    {
        public DateTime MaturityDate { get; set; }
        public double MaturityAmount { get; set; }
        public string UserName { get; set; }
        public string BranchName { get; set; }
        public void SetDefault()
        {
            //InterestRate = GetFixedInterestRate(Tenure);
            MaturityAmount = MaturityAmountCalculator(DepositedAmount, InterestRate);
            MaturityDate = MaturityDateCalculator(CreatedOn, Tenure*12);
        }
        public List<TransactionSummary> TransactionList { get; set; }
        public RecurringAccountBObj()
        {
            TransactionList = new List<TransactionSummary>();
        }

        public double MaturityAmountCalculator(double amount, double interestRate)
        {
            //M = P * [(1 + r) ^ n - 1] / (1 - (1 + r) ^ (-1 / 3))

            var interest = interestRate / 400;
            var tenureMonths = Tenure * 12;
            int quarters = tenureMonths / 3;
            var cumulativeAmount = 0.0;
            for (double i = 1.0; (int)i <= tenureMonths; i++)
            {
                double monthlyInterestPlusAmount = Math.Pow(1 + interest, (4 * (i / tenureMonths)));
                cumulativeAmount += (amount * monthlyInterestPlusAmount);
            }
            return Math.Round(cumulativeAmount, 2);
            

            //var interest = interestRate / 100;
            //var tenureMonths = Tenure * 12;
            //int quarters = tenureMonths / 3;

            //var denominator = 1 - (Math.Pow((1 + interest), (-1.0 / 3)));
            //var estimatedValue = (MonthlyInstallment * (Math.Pow(1 + (interest), quarters - 1))) / denominator;
            //return Math.Round(estimatedValue, 2);
            //return amount + (amount * (interestRate / 100));
        }

        public DateTime MaturityDateCalculator(DateTime date, int months)
        {
            return date.AddMonths(months);
        }

        //public static double GetFixedInterestRate(int tenureInMonths)
        //{

        //    switch (tenureInMonths)
        //    {
        //        case int n when (n > 0 && n <= 3): return 1;
        //        case int n when (n > 3 && n <= 6): return 1.3;
        //        case int n when (n == 9): return 1.69;
        //        case int n when (n == 10): return 1.86;
        //        case int n when (n > 10 && n <= 24): return 3;
        //        case int n when (n > 24 && n <= 36): return 5;
        //        default: return 0;
        //    }
        //}

        internal double CalculateClosingAmount(DateTime now)
        {
            if (AccountStatus == AccountStatus.Closed)
            {
                var months = ((now.Year - CreatedOn.Year) * 12) + now.Month - CreatedOn.Month;
                if (months <= 0) return 0;
                //var interestRate = GetFixedInterestRate(months);
                return MaturityAmountCalculator(DepositedAmount, InterestRate);
            }
            return 0;
        }
    }
}