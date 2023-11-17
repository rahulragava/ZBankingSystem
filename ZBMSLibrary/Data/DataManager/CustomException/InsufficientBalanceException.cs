using System;

namespace ZBMSLibrary.Data.DataManager.CustomException
{
    public class InsufficientBalanceException : Exception
    {
        public InsufficientBalanceException()
        { }

        public InsufficientBalanceException(string message, Exception inner)
            : base(message, inner) { }

        public InsufficientBalanceException(string message) : base(message)
        {
        }
    }
}