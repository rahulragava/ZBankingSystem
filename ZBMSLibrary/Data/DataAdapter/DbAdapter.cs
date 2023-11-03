using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZBMSLibrary.Entities.BusinessObject;

namespace ZBMSLibrary.Data.DataAdapter
{
    public class DbAdapter : IDatabaseAdapter
    {
        public SQLiteAsyncConnection Connection;
        public DbAdapter()
        {
            DatabaseInitializer.Instance.InitializeDatabase();
            SQLiteConnectionString connectionString = new SQLiteConnectionString(DatabaseInitializer.Instance.DbPath);
            Connection = new SQLiteAsyncConnection(connectionString);
        }

        public async Task InsertInTableAsync<T>(T obj) where T : new()
        {
            await DatabaseInitializer.Instance.Db.InsertAsync(obj);
        }
        public async Task InsertMultipleObjectInTableAsync<T>(List<T> objList) where T : new()
        {
            await DatabaseInitializer.Instance.Db.InsertAllAsync(objList, true);
        }

        public async Task RemoveObjectFromTableAsync<T>(string id) where T : new()
        {
            await DatabaseInitializer.Instance.Db.DeleteAsync<T>(id);
        }



        public async Task UpdateObjectInTableAsync<T>(T obj) where T : new()
        {
            await DatabaseInitializer.Instance.Db.UpdateAsync(obj);
        }

        public async Task<T> GetObjectFromTableAsync<T>(string id) where T : new()
        {
            return await DatabaseInitializer.Instance.Db.GetAsync<T>(id);
        }

        public async Task<IEnumerable<T>> Query<T>(string query, params object[] args) where T : new()
        {
            return await Connection.QueryAsync<T>(query, args).ConfigureAwait(false);
        }

        public async Task<T> QueryFetchObject<T>(string query) where T : new()
        {
            return await Connection.FindWithQueryAsync<T>(query).ConfigureAwait(false);
        }
        public async Task<IEnumerable<T>> GetAllObjectsInTableAsync<T>() where T : new()
        {
            DatabaseInitializer.Instance.InitializeDatabase();
            return (await DatabaseInitializer.Instance.Db.Table<T>().ToListAsync());
        }
       
        public async Task<IEnumerable<T>> GetSpecificObjectsInTableAsync<T>(int takeAmount, int skipAmount) where T : new()
        {
            DatabaseInitializer.Instance.InitializeDatabase();
            return await DatabaseInitializer.Instance.Db.Table<T>().Skip(skipAmount).Take(takeAmount).ToListAsync();
        }

        public async Task RunInTransactionAsync(Func<Task> action)
        {
            SQLiteConnectionWithLock conn = Connection.GetConnection();
            conn.BeginTransaction();
            try
            {
                await action().ConfigureAwait(false);
                conn.Commit();
            }
            catch (Exception e)
            {
                conn.Rollback();
                throw e;
            }
        }

    }
}