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
            MaturityAmount = MaturityAmountCalculator(DepositedAmount, InterestRate);
            MaturityDate = MaturityDateCalculator(CreatedOn, Tenure*12);
        }

        public double MaturityAmountCalculator(double amount, double interestRate)
        {
            //P x(1 + r / 100)^nt === formula to calculate the maturity amount of the fixed deposit

            var val = (4 * Tenure);
            var interest = interestRate / 400;
            var estimatedValue = DepositedAmount * (Math.Pow(1 + (interest), val));
            return Math.Round(estimatedValue, 2);
        }
        public double ManualMaturityAmountCalculator(double amount,int tenure)
        {
            //P x(1 + r / 100)^nt

            var interestRate = GetFixedInterestRate(tenure);
            var val = (4 * Tenure);
            var interest = interestRate / 400;
            var estimatedValue = DepositedAmount * (Math.Pow(1 + (interest), val));
            return Math.Round(estimatedValue, 2);
        }

        private DateTime MaturityDateCalculator(DateTime date, int months)
        {
            return date.AddMonths(months);
        }

        private double GetFixedInterestRate(int tenureInYears)
        {

            switch (tenureInYears)
            {
                case 0: return 0;
                case 1: return 6;
                case 2: 
                case 3: return 6.8;
                case 4: return 7.2;
                default: return InterestRate;
            }
        }

        internal double CalculateClosingAmount(DateTime now)
        {
            if (AccountStatus == AccountStatus.Closed)
            {
                //TenureInMonths = ((now.Year - CreatedOn.Year) * 12) + now.Month - CreatedOn.Month;
                var months = ((now.Year - CreatedOn.Year) * 12) + now.Month - CreatedOn.Month;
                if (months <= 0) return DepositedAmount;
                var interestRate = GetFixedInterestRate(months);
                return MaturityAmountCalculator(DepositedAmount, interestRate);
            }
            return 0;
        }
    }
}