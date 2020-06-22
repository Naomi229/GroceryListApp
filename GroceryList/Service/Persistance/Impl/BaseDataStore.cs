using SQLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GroceryList.Service.Persistance.Impl
{
    public class BaseDataStore<T> : IDataStore<T> where T : new()
    {

        private readonly SQLiteAsyncConnection _connection;

        public BaseDataStore(SQLiteAsyncConnection connection)
        {
            _connection = connection;
            _connection.CreateTableAsync<T>();
        }

        public virtual async Task<bool> AddAsync(T item)
        {
            await _connection.InsertAsync(item);
            return true;
        }
        public virtual async Task<bool> UpdateAsync(T item)
        {
            await _connection.UpdateAsync(item);
            return true;
        }

        public virtual async Task<bool> DeleteAsync(T item)
        {
            await _connection.DeleteAsync(item);
            return true;
        }
        public virtual async Task<T> GetAsync(string id)
        {
            return await _connection.FindAsync<T>(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(bool forceRefresh = false)
        {
            return await _connection.Table<T>().ToListAsync();
        }

        public virtual async Task<List<T>> ExecuteQueryAsync(string query, params object[] args)
        {
            return await _connection.QueryAsync<T>(query, args);
        }

        public virtual async Task<int> ExecuteScalarAsync(string query, params object[] args)
        {
            return await _connection.ExecuteScalarAsync<int>(query, args);
        }

        public virtual async Task<TEntity> FindAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : new()
        {
            return await _connection.FindAsync<TEntity>(predicate);
        }
    }
}
