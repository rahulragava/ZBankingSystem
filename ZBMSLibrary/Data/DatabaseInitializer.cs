using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using SQLite;
using ZBMSLibrary.Entities.Enums;
using ZBMSLibrary.Entities.Model;

namespace ZBMSLibrary.Data
{
    public sealed class DatabaseInitializer
    {
        private static DatabaseInitializer _dbInstance;
        private static readonly object PadLock = new object();

        private DatabaseInitializer()
        {
        }

        public static DatabaseInitializer Instance
        {
            get
            {
                if (_dbInstance == null)
                    lock (PadLock)
                    {
                        if (_dbInstance == null)
                        {
                            _dbInstance = new DatabaseInitializer();
                        }
                    }

                return _dbInstance;
            }
        }

        public SQLiteAsyncConnection Db { get; set; }
        public string DbPath { get; set; }
        public void InitializeDatabase()
        {
            if (Db == null)
            {
                DbPath =
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "ZBMS.db3");
                Db = new SQLiteAsyncConnection(DbPath);
                Debug.WriteLine(DbPath);
                //await CreateAllTablesAsync();
            }
        }

        public async Task CreateAllTablesAsync()
        {
            await Db.CreateTableAsync<User>();
            await Db.CreateTableAsync<Branch>();
            await Db.CreateTableAsync<CurrentAccount>();
            await Db.CreateTableAsync<TransactionSummary>();
            await Db.CreateTableAsync<SavingsAccount>();
            await Db.CreateTableAsync<RecurringAccount>();
            await Db.CreateTableAsync<FixedDeposit>();
            await Db.CreateTableAsync<PersonalLoan>();
            await Db.CreateTableAsync<AccountStatus>();
        }
    }
}