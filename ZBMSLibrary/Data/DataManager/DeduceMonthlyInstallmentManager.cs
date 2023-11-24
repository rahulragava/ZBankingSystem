using System;
using System.Threading.Tasks;
using ZBMSLibrary.Data.DataHandler.Contract;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Enums;
using ZBMSLibrary.Entities.Model;
using ZBMSLibrary.UseCase;

namespace ZBMSLibrary.Data.DataManager
{
    public class DeduceMonthlyInstallmentManager : IDeduceMonthlyInstallmentManager
    {
        private readonly IDbHandler _dbHandler;

        public DeduceMonthlyInstallmentManager(IDbHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }

        public async Task DeduceMonthlyInstallmentAsync(DeduceMonthlyInstallmentRequest deduceMonthlyInstallmentRequest,
            DeduceMonthlyInstallmentUseCaseCallBack deduceMonthlyInstallmentUseCaseCallBack)
        {
            try
            {
                foreach (var monthlyInstallment in deduceMonthlyInstallmentRequest.MonthlyInstallments)
                {
                    var dueMonths = monthlyInstallment.Value;
                    var dueAmount = monthlyInstallment.Key.MonthlyInstallment * dueMonths;
                    var a = monthlyInstallment.Key;
                    TransactionSummary transactionSummary = new TransactionSummary()
                    {
                        Amount = monthlyInstallment.Key.MonthlyInstallment,
                        TransactionOn = DateTime.Now,
                        TransactionType = TransactionType.Debit,
                        ReceiverAccountNumber = monthlyInstallment.Key.AccountNumber,
                        Description = "Monthly RD Installment",
                        
                    };
                    var userName = await _dbHandler.GetUserNameAsync(monthlyInstallment.Key.UserId);
                    try
                    {
                        var account = await _dbHandler.GetSavingsAccountAsync(monthlyInstallment.Key.FromAccountId);
                        account.Balance -= dueAmount;
                        transactionSummary.SenderAccountNumber = account.AccountNumber;
                        monthlyInstallment.Key.DepositedAmount += dueAmount;
                        await _dbHandler.UpdateSavingsAccountAsync(account);
                        await _dbHandler.InsertTransactionAsync(transactionSummary);
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
                        NotificationEvents.MonthlyRdSavingsTransaction?.Invoke(transactionSummaryVObj);
                    }
                    catch (InvalidOperationException ex)
                    {
                        var account = await _dbHandler.GetCurrentAccountAsync(monthlyInstallment.Key.FromAccountId);
                        account.Balance -= dueAmount;
                        transactionSummary.SenderAccountNumber = account.AccountNumber;
                        monthlyInstallment.Key.DepositedAmount += dueAmount;
                        await _dbHandler.UpdateCurrentAccountAsync(account);
                        await _dbHandler.InsertTransactionAsync(transactionSummary);
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
                        NotificationEvents.MonthlyRdCurrentTransaction?.Invoke(transactionSummaryVObj);
                    }
                   
                    await _dbHandler.UpdateRecurringAccountAsync(monthlyInstallment.Key);
                    NotificationEvents.MonthlyInstallmentDeposited?.Invoke(monthlyInstallment.Key,dueAmount);
                }
                deduceMonthlyInstallmentUseCaseCallBack?.OnSuccess(new DeduceMonthlyInstallmentResponse());
            }
            catch (Exception e)
            {
                deduceMonthlyInstallmentUseCaseCallBack?.OnError(e);
            }
        }
    }
}