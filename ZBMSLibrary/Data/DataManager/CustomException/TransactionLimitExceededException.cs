using System;

namespace ZBMSLibrary.Data.DataManager.CustomException
{
    public class TransactionLimitExceededException : Exception
    {
        public TransactionLimitExceededException()
        { }

        public TransactionLimitExceededException(string message, Exception inner)
            : base(message, inner) { }

        public TransactionLimitExceededException(string message) : base(message)
        {
        }
    }
}