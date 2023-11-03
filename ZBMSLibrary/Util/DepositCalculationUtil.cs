//using System;
//using ZBMSLibrary.Entities.Enums;

//namespace ZBMSLibrary.Util
//{
//    public static class DepositCalculationUtil
//    {
//        public static double MaturityAmountCalculator(double amount, double interestRate)
//        {
//            //P x(1 + r / 100)^nt
//            return amount + (amount * (interestRate / 100));
//        }

//        public static DateTime MaturityDateCalculator(DateTime date, int months)
//        {
//            return date.AddMonths(months);
//        }

//        public static double GetFixedInterestRate(int tenureInMonths)
//        {

//            switch (tenureInMonths)
//            {
//                case int n when (n > 0 && n <= 3): return 1;
//                case int n when (n > 3 && n <= 6): return 1.3;
//                case int n when (n == 9): return 1.69;
//                case int n when (n == 10): return 1.86;
//                case int n when (n > 10 && n <= 24): return 3;
//                case int n when (n > 24 && n <= 36): return 5;
//                default: return 0;
//            }
//        }

//        internal static double CalculateClosingAmount(DateTime now)
//        {
//            if (AccountStatus == AccountStatus.Closed)
//            {
//                var months = ((now.Year - CreatedOn.Year) * 12) + now.Month - CreatedOn.Month;
//                if (months <= 0) return 0;
//                var interestRate = GetFixedInterestRate(months);
//                return MaturityAmountCalculator(Balance, interestRate);
//            }
//            return 0;
//        }
//    }
//} 