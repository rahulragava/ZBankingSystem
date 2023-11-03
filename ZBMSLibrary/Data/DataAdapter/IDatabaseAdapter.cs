using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Gaming.Input;

namespace ZBMSLibrary.Data.DataAdapter
{
    public interface IDatabaseAdapter
    {
        Task InsertInTableAsync<T>(T obj) where T : new();
        Task InsertMultipleObjectInTableAsync<T>(List<T> objList) where T : new();
        Task RemoveObjectFromTableAsync<T>(string id) where T : new();
        Task UpdateObjectInTableAsync<T>(T obj) where T : new();
        Task<T> GetObjectFromTableAsync<T>(string id) where T : new();
        Task<IEnumerable<T>> GetAllObjectsInTableAsync<T>() where T : new();
        Task<IEnumerable<T>> Query<T>(string query, params object[] args) where T : new();
        Task<T> QueryFetchObject<T>(string query) where T : new();
        Task<IEnumerable<T>> GetSpecificObjectsInTableAsync<T>(int takeAmount, int skipAmount) where T : new();
        Task RunInTransactionAsync(Func<Task> action);
    }
}