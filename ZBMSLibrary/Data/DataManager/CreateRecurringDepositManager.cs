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
    public class CreateRecurringDepositManager : ICreateRecurringDepositManager
    {
        private readonly IDbHandler _dbHandler;

        public CreateRecurringDepositManager(IDbHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }

        public async Task CreateRecurringDepositAsync(CreateRecurringDepositRequest createRecurringDepositRequest,
            CreateRecurringDepositUseCaseCallBack createRecurringDepositUseCaseCallBack)
        {
            try
            {
                TransactionSummary transactionSummary = new TransactionSummary()
                {
                    Amount = createRecurringDepositRequest.RecurringAccount.DepositedAmount,
                    Description = "Deposit",
                    ReceiverAccountNumber = createRecurringDepositRequest.RecurringAccount.AccountNumber,
                    TransactionOn = DateTime.Now,
                    TransactionType = TransactionType.Debit,
                };
                try
                {
                    var account = await _dbHandler.GetSavingsAccountAsync(createRecurringDepositRequest.RecurringAccount.FromAccountId);
                    account.Balance -= createRecurringDepositRequest.RecurringAccount.DepositedAmount;
                    await _dbHandler.UpdateSavingsAccountAsync(account);
                    transactionSummary.SenderAccountNumber = account.AccountNumber;
                    await _dbHandler.InsertTransactionAsync(transactionSummary);
                    NotificationEvents.RdCreationSavingsTransaction?.Invoke(transactionSummary);
                }
                catch (Exception ex)
                {
                    var account = await _dbHandler.GetCurrentAccountAsync(createRecurringDepositRequest.RecurringAccount.FromAccountId);
                    account.Balance -= createRecurringDepositRequest.RecurringAccount.DepositedAmount;
                    await _dbHandler.UpdateCurrentAccountAsync(account);
                    transactionSummary.SenderAccountNumber = account.AccountNumber;
                    await _dbHandler.InsertTransactionAsync(transactionSummary);
                    NotificationEvents.RdCreationCurrentTransaction?.Invoke(transactionSummary);
                }
                await _dbHandler.CreateRecurringDepositAsync(createRecurringDepositRequest.RecurringAccount);

                var recurringDepositBObj = new RecurringAccountBObj()
                {
                    UserId = createRecurringDepositRequest.RecurringAccount.UserId,
                    AccountStatus = createRecurringDepositRequest.RecurringAccount.AccountStatus,
                    DepositedAmount = createRecurringDepositRequest.RecurringAccount.DepositedAmount,
                    IfscCode = createRecurringDepositRequest.RecurringAccount.IfscCode,
                    CreatedOn = createRecurringDepositRequest.RecurringAccount.CreatedOn,
                    AccountNumber = createRecurringDepositRequest.RecurringAccount.AccountNumber,
                    InterestRate = createRecurringDepositRequest.RecurringAccount.InterestRate,
                    Tenure = createRecurringDepositRequest.RecurringAccount.Tenure,
                    FromAccountId = createRecurringDepositRequest.RecurringAccount.FromAccountId,
                    SavingsAccountId = createRecurringDepositRequest.RecurringAccount.SavingsAccountId,
                    LastPaidDate = createRecurringDepositRequest.RecurringAccount.LastPaidDate,
                    MonthlyInstallment = createRecurringDepositRequest.RecurringAccount.MonthlyInstallment,
                };
                var userName = await _dbHandler.GetUserNameAsync(createRecurringDepositRequest.RecurringAccount.UserId);
                var branchName =
                    await _dbHandler.GetBranchNameAsync(createRecurringDepositRequest.RecurringAccount.IfscCode);
                recurringDepositBObj.SetDefault();
                recurringDepositBObj.BranchName = branchName;
                recurringDepositBObj.UserName = userName;
                NotificationEvents.RecurringDepositCreated?.Invoke(recurringDepositBObj);
                createRecurringDepositUseCaseCallBack?.OnSuccess(new CreateRecurringDepositResponse());
            }
            catch (Exception e)
            {   
                createRecurringDepositUseCaseCallBack?.OnError(e);
            }
        }
    }
}