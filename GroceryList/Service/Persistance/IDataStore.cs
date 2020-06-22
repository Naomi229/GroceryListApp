using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GroceryList.Service.Persistance
{
    public interface IDataStore<TEntity>
    {

        Task<bool> AddAsync(TEntity item);
        Task<bool> UpdateAsync(TEntity item);
        Task<bool> DeleteAsync(TEntity item);
        Task<TEntity> GetAsync(string id);
        Task<IEnumerable<TEntity>> GetAllAsync(bool forceRefresh = false);
        Task<int> ExecuteScalarAsync(string query, params object[] args);
        Task<List<TEntity>> ExecuteQueryAsync(string query, params object[] args);
        Task<TEntity> FindAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : new();

    }
}
