using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Interfaces.IRepositories
{
    public interface IRepository<T> where T : class
    {
        // CRUD
        Task<bool> CreateAsync(T entity);
        Task<bool> CreateRangeAsync(IEnumerable<T> entity);

        Task<bool> UpdateAsync(T entity);

        Task<bool> DeleteAsync(T entity);
        Task<bool> DeleteRangeAsync(IEnumerable<T> entity);

        Task<List<T>> GetAsync(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>[]? include = null, bool tracked = true,
            Expression<Func<T, IOrderedQueryable>>? orderByExpression = null, int take = -1);

        Task<T?> GetOneAsync(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>[]? include = null, bool tracked = false);

        Task<List<T>> GetAsyncIncludes(Expression<Func<T, bool>>? condition = null, List<Func<IQueryable<T>, IQueryable<T>>>? includes = null, bool tracked = true);

        Task<T?> GetOneAsyncIncludes(Expression<Func<T, bool>>? condition = null, List<Func<IQueryable<T>, IQueryable<T>>>? includes = null, bool tracked = true);

        Task<bool> AnyAsync(Expression<Func<T, bool>>? condition = null);

        Task<bool> CommitAsync();
        void DetachEntity(T entity);
        Task<int> CountAsync(Expression<Func<T, bool>>? expression = null);
        Task<decimal> SumAsync(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>>? expression = null);
    }
}
