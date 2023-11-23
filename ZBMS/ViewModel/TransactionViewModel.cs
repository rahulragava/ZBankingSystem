using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Transactions;
using Windows.UI.ViewManagement;
using ZBMS.Util.VObj;
using ZBMSLibrary.Entities.BusinessObject;
using ZBMSLibrary.Entities.Enums;
using ZBMSLibrary.Entities.Model;

namespace ZBMS.ViewModel
{
    public class TransactionViewModel : INotifyPropertyChanged
    {
        //public ObservableCollection<TransactionSummaryVObj> TransactionSummaryVObjects;
        public readonly ObservableCollection<GroupInfoCollectionVObj<TransactionSummaryVObj>> TransactionSummaries = new ObservableCollection<GroupInfoCollectionVObj<TransactionSummaryVObj>>();
        public TransactionViewModel()
        {
            //    TransactionSummaryVObjects = new ObservableCollection<TransactionSummaryVObj>();
        }

        private int _lastItemIndex;

        public int LastItemIndex
        {
            get => _lastItemIndex;
            set => SetField(ref _lastItemIndex, value);
        }

        public TransactionSummary TransactionSummary;

        private string _id;

        public string Id
        {
            get => _id;
            set => SetField(ref _id, value);
        }

        private string _senderAccountNumber;

        public string SenderAccountNumber
        {
            get => _senderAccountNumber;
            set => SetField(ref _senderAccountNumber, value);
        }

        private string _userName;

        public string UserName
        {
            get => _userName;
            set => SetField(ref _userName, value);
        }

        private string _receiverAccountNumber;

        public string ReceiverAccountNumber
        {
            get => _receiverAccountNumber;
            set => SetField(ref _receiverAccountNumber, value);
        }

        private DateTime _transactionOn;

        public DateTime TransactionOn
        {
            get => _transactionOn;
            set => SetField(ref _transactionOn, value);
        }

        private double _amount;

        public double Amount
        {
            get => _amount;
            set => SetField(ref _amount, value);
        }

        private TransactionType _transactionType;

        public TransactionType TransactionType
        {
            get => _transactionType;
            set => SetField(ref _transactionType, value);
        }

        private string _description;

        public string Description
        {
            get => _description;
            set => SetField(ref _description, value);
        }


        //public void SetTransactionList(ObservableCollection<TransactionSummaryVObj> transactionSummary)
        //{
        //    TransactionSummaryVObjects.Clear();
        //    foreach (var transactionSummaryVObj in transactionSummary)
        //    {
        //        TransactionSummaryVObjects.Add(transactionSummaryVObj);
        //    }
        //}

        public void GenerateTransactionByGroup(ObservableCollection<TransactionSummaryVObj> transactionSummary)
        {
            var query = from item in transactionSummary
                        group item by GroupDateHelper(item.TransactionOn) into g
                        orderby g.Key
                        select new { GroupName = g.Key, Items = g };

            foreach (var g in query)
            {
                var info = new GroupInfoCollectionVObj<TransactionSummaryVObj>();
                info.Key = GetExactGroup(g.GroupName);


                foreach (var item in g.Items)
                {
                    info.Add(item);
                }

                TransactionSummaries.Add(info);
            }
        }

        private string GetExactGroup(GroupHelperType g)
        {
            switch (g)
            {
                case GroupHelperType.A:
                    return "Today";
                case GroupHelperType.B:
                    return "Yesterday";
                case GroupHelperType.C:
                    return "This week";
                case GroupHelperType.D:
                    return "This month";
                case GroupHelperType.E:
                    return "others";
                default:
                    return "others";
            }
        }

        public enum GroupHelperType
        {
            A,
            B,
            C,
            D,
            E,
        }

        public GroupHelperType GroupDateHelper(DateTime inputDate)
        {
            DateTime currentDate = DateTime.Now.Date;

            if (inputDate.Date == currentDate)
            {
                //today
                return GroupHelperType.A;
            }
            else if (inputDate.Date == currentDate.AddDays(-1))
            {
                //yesterday
                return GroupHelperType.B;
            }
            else if (inputDate.Year == currentDate.Year && inputDate.DayOfYear >= currentDate.DayOfYear - (int)currentDate.DayOfWeek &&
                     inputDate.DayOfYear <= currentDate.DayOfYear + (6 - (int)currentDate.DayOfWeek))
            {
                //this week
                return GroupHelperType.C;
            }
            else if (inputDate.Year == currentDate.Year && inputDate.Month == currentDate.Month)
            {
                //this month
                return GroupHelperType.D;
            }
            else
            {
                //others
                return GroupHelperType.E;
            }
        }

        public void ListPropertyChanged(TransactionSummaryVObj transaction)
        {
            var type = GroupDateHelper(transaction.TransactionOn);
            var key = GetExactGroup(type);

            var groupToInsert = TransactionSummaries.FirstOrDefault(group => (string)group.Key == key);
            if (groupToInsert != null)
            {
                groupToInsert.Insert(0, transaction);
            }
            else
            {
                var groupInfoCollection = new GroupInfoCollectionVObj<TransactionSummaryVObj>()
                {
                    Key = key,
                };
                groupInfoCollection.Insert(0, transaction);
                TransactionSummaries.Insert(0,groupInfoCollection);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }



    }
}