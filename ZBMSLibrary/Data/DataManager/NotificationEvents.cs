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
        public static Action<RecurringAccountBObj> RecurringDepositCreated;
        public static Action<RecurringAccount,double> MonthlyInstallmentDeposited;
        public static Action<Deposit,double> DepositSettled;
        public static Action<TransactionSummary> UpdateSavingsAccountWithdrawTransaction;
        public static Action<TransactionSummary> UpdateCurrentAccountWithdrawTransaction;  
        public static Action<TransactionSummary> UpdateSavingsAccountDepositTransaction;
        public static Action<TransactionSummary> UpdateCurrentAccountDepositTransaction;
        public static Action<TransactionSummary> RdCreationSavingsTransaction;
        public static Action<TransactionSummary> RdCreationCurrentTransaction;
        public static Action<TransactionSummary> FdCreationSavingsTransaction;
        public static Action<TransactionSummary> FdCreationCurrentTransaction;
        public static Action<TransactionSummary> MonthlyRdCurrentTransaction;
        public static Action<TransactionSummary> MonthlyRdSavingsTransaction;
        public static Action<TransactionSummary> SettlementDepositTransaction;
        //public static Action<TransactionSummary> MonthlyInstallmentTransaction;

    }
}