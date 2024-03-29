﻿using System;
using System.Threading.Tasks;
using ZBMSLibrary.Data.DataHandler.Contract;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Enums;
using ZBMSLibrary.Entities.Model;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager
{
    public class DepositSettlementManager : IDepositSettlementManager
    {
        private readonly IDbHandler _dbHandler;

        public DepositSettlementManager(IDbHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }

        public async Task DepositSettlementAsync(DepositSettlementRequest depositSettlementRequest,
            DepositSettlementUseCaseCallBack depositSettlementUseCaseCallBack)
        {
            try
            {
                foreach (var deposit in depositSettlementRequest.Deposits)
                {
                    var maturityAmount = deposit.DepositedAmount;
                    //deposit.DepositedAmount = 0;
                    //deposit.AccountStatus = AccountStatus.Closed;
                    TransactionSummary transactionSummary = new TransactionSummary()
                    {
                        Amount = deposit.DepositedAmount,
                        TransactionOn = DateTime.Now,
                        TransactionType = TransactionType.Credit,
                        
                    };
                    var userName = await _dbHandler.GetUserNameAsync(deposit.UserId);


                    if (deposit is RecurringAccountBObj recurringAccount)
                    {
                        maturityAmount = recurringAccount.MaturityAmountCalculator(recurringAccount.DepositedAmount,
                            recurringAccount.InterestRate);
                        var recurringDeposit= new RecurringAccount()
                        {
                            UserId = recurringAccount.UserId,
                            AccountStatus = AccountStatus.Closed,
                            DepositedAmount = recurringAccount.DepositedAmount,
                            IfscCode = recurringAccount.IfscCode,
                            CreatedOn = recurringAccount.CreatedOn,
                            AccountNumber = recurringAccount.AccountNumber,
                            InterestRate = recurringAccount.InterestRate,
                            Tenure = recurringAccount.Tenure,
                            FromAccountId = recurringAccount.FromAccountId,
                            SavingsAccountId = recurringAccount.SavingsAccountId,
                            NextDueDate = DateTime.Now,
                            MonthlyInstallment = recurringAccount.MonthlyInstallment,
                        };
                        transactionSummary.Description = "Recurring Deposit Closed";
                        transactionSummary.SenderAccountNumber = recurringDeposit.AccountNumber;
                        transactionSummary.ReceiverAccountNumber = recurringDeposit.SavingsAccountId;
                        await _dbHandler.InsertTransactionAsync(transactionSummary);
                        await _dbHandler.UpdateRecurringAccountAsync(recurringDeposit);

                    }
                    else if (deposit is FixedDepositBObj fixedDeposit)
                    {
                        maturityAmount = fixedDeposit.MaturityAmountCalculator(fixedDeposit.DepositedAmount,
                            fixedDeposit.InterestRate);
                        var fd= new FixedDeposit()
                        {
                            UserId = fixedDeposit.UserId,
                            AccountStatus = AccountStatus.Active,
                            DepositedAmount = fixedDeposit.DepositedAmount,
                            IfscCode = fixedDeposit.IfscCode,
                            CreatedOn = fixedDeposit.CreatedOn,
                            AccountNumber = fixedDeposit.AccountNumber,
                            InterestRate = fixedDeposit.InterestRate,
                            Tenure = fixedDeposit.Tenure,
                            FromAccountId = fixedDeposit.FromAccountId,
                            SavingsAccountId = fixedDeposit.SavingsAccountId
                            
                        };
                        transactionSummary.Description = "Fixed Deposit ReOpened";
                        transactionSummary.SenderAccountNumber = fixedDeposit.AccountNumber;
                        transactionSummary.ReceiverAccountNumber = fixedDeposit.SavingsAccountId;
                        await _dbHandler.InsertTransactionAsync(transactionSummary);
                        await _dbHandler.UpdateFixedDepositAsync(fd);
                    }
                    var account = await _dbHandler.GetSavingsAccountAsync(deposit.SavingsAccountId);
                    account.Balance += maturityAmount;
                    await _dbHandler.UpdateSavingsAccountAsync(account);
                    TransactionSummaryVObj transactionSummaryVObj = new TransactionSummaryVObj()
                    {
                        Amount = transactionSummary.Amount,
                        Description = transactionSummary.Description,
                        ReceiverAccountNumber = transactionSummary.ReceiverAccountNumber,
                        TransactionOn = transactionSummary.TransactionOn,
                        TransactionType = transactionSummary.TransactionType,
                        SenderAccountNumber = transactionSummary.SenderAccountNumber,
                        Id = transactionSummary.Id,
                        UserName = userName,
                    };
                    NotificationEvents.DepositSettled?.Invoke(deposit, maturityAmount);
                    NotificationEvents.SettlementDepositTransaction?.Invoke(transactionSummaryVObj);
                }
                //NotifyEvents
                //NotificationEvents.

                depositSettlementUseCaseCallBack?.OnSuccess(new DepositSettlementResponse());
            }
            catch (Exception e)
            {
               depositSettlementUseCaseCallBack?.OnError(e);
            }
        }
    }
}