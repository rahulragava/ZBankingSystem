using System;
using ZBMSLibrary.Data.DataManager.Contract;
using ZBMSLibrary.Data.Dependencies;
using ZBMSLibrary.Data;
using ZBMSLibrary.Entities.Model;
using Microsoft.Extensions.DependencyInjection;
using ZBMSLibrary.Entities.BusinessObject;

namespace ZBMSLibrary.UseCase
{
    public class TransferUseCase : UseCaseBase<TransferResponse>
    {
        private readonly ITransferManager _transferManager = DependencyContainer.DiContainer.GetRequiredService<ITransferManager>();
        public readonly TransferRequest TransferRequest;
        public TransferUseCase(TransferRequest transferRequest, IPresenterCallBack<TransferResponse> presenterCallBack) : base(presenterCallBack)
        {
            TransferRequest = transferRequest;
        }

        public override void Action()
        {
            _transferManager.TransferAsync(TransferRequest, new TransferUseCaseCallBack(this));
        }
    }
    public class TransferUseCaseCallBack : IUseCaseCallBack<TransferResponse>
    {
        private readonly TransferUseCase _transferUseCase;
        public TransferUseCaseCallBack(TransferUseCase transferUseCase)
        {
            _transferUseCase = transferUseCase;
        }

        public void OnSuccess(TransferResponse responseObj)
        {
            _transferUseCase.PresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _transferUseCase.PresenterCallBack?.OnError(ex);
        }
    }

    public class TransferRequest
    {
        public Account Account;
        public string AccountNumber;
        public double Amount;
        public TransferRequest(Account account, string accountNumber, double amount)
        {
            Account = account;
            AccountNumber = accountNumber;
            Amount = amount;
        }
    }

    public class TransferResponse
    {
        public TransactionSummaryVObj TransactionSummaryVObj;
        public TransferResponse(TransactionSummaryVObj transactionSummaryVObj)
        {
            TransactionSummaryVObj = transactionSummaryVObj;
        }
    }
}