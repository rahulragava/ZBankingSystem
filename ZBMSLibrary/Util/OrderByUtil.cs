using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ZBMSLibrary.Entities.Model;

namespace ZBMS.Util
{
    public static class OrderByUtil
    {
        public static List<T> OrderByDescending<T>(List<T> transactions) where T : TransactionSummary
        {
            var temporaryCollection = new List<T>(transactions.OrderByDescending(transaction => transaction.TransactionOn));
            transactions.Clear();
            foreach (T transactionSummary in temporaryCollection)
            {
                transactions.Add(transactionSummary);
            }
            return transactions;
        }
    }
}