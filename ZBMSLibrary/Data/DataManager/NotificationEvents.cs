using System;
using System.Collections.Generic;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Model;

namespace ZBMSLibrary.Data.DataManager
{
    public static class NotificationEvents
    {
        public static Action<double> DepositSavingsAccountAmountUpdation;
        public static Action<double> DepositCurrentAmountUpdation;
        public static Action<double> WithdrawSavingsAccountAmountUpdation;
        public static Action<double> WithdrawCurrentAccountAmountUpdation;
        public static Action<CurrentAccountBObj> CurrentAccountCreated;
        public static Action<SavingsAccountBObj> SavingsAccountCreated;
        public static Action<FixedDepositBObj> FixedDepositCreated;
        public static Action<FixedDepositBObj> FixedDepositUpdated;
        public static Action<RecurringAccountBObj> RecurringDepositCreated;
        public static Action<RecurringAccountBObj> RecurringDepositUpdated;
        public static Action<PersonalLoanBObj> PersonalLoanCreated;
        public static Action<PersonalLoanBObj> PersonalLoanUpdated;
        public static Action<RecurringAccount,double> MonthlyInstallmentDeposited;
        public static Action<Deposit,double> DepositSettled;
        public static Action<TransactionSummaryVObj> UpdateSavingsAccountWithdrawTransaction;
        public static Action<TransactionSummaryVObj> SavingsLoanDuePaidNotification;
        public static Action<TransactionSummaryVObj> UpdateCurrentAccountWithdrawTransaction; 
        public static Action<TransactionSummaryVObj> CurrentAccountLoanDuePaidNotification;
        public static Action<TransactionSummaryVObj> UpdateSavingsAccountDepositTransaction;
        public static Action<TransactionSummaryVObj> UpdateCurrentAccountDepositTransaction;
        public static Action<TransactionSummaryVObj> RdCreationSavingsTransaction;
        public static Action<TransactionSummaryVObj> RdCreationCurrentTransaction;
        public static Action<TransactionSummaryVObj> FdCreationSavingsTransaction;
        public static Action<TransactionSummaryVObj> FdCreationCurrentTransaction;
        public static Action<TransactionSummaryVObj> LoanCreationUsingSavingsAccountTransaction;
        public static Action<TransactionSummaryVObj> LoanCreationUsingCurrentAccountTransaction;
        public static Action<TransactionSummaryVObj> MonthlyRdCurrentTransaction;
        public static Action<TransactionSummaryVObj> MonthlyRdSavingsTransaction;
        public static Action<TransactionSummaryVObj> SettlementDepositTransaction;
        //public static Action<TransactionSummary> MonthlyInstallmentTransaction;

    }
}