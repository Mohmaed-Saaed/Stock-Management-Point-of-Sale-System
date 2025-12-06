using InfrastructureLayer.Data;
using InfrastructureLayer.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> _db { set; get; }
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _db = _context.Set<T>();
        }

        public async Task<bool> CreateAsync(T entity)
        {
            try
            {
                await _db.AddAsync(entity);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ex: {ex}");
                return false;
            }
        }

        public async Task<bool> CreateRangeAsync(IEnumerable<T> entity)
        {
            try
            {
                await _db.AddRangeAsync(entity);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ex: {ex}");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                var result = _db.Update(entity);

                var result2 = await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ex: {ex}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            try
            {
                _db.Attach(entity);
                _db.Remove(entity);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ex: {ex}");
                return false;
            }
        }

        public async Task<bool> DeleteRangeAsync(IEnumerable<T> entity)
        {
            try
            {
                _db.RemoveRange(entity);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ex: {ex}");
                return false;
            }
        }
        public async Task<List<T>> GetAsync(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>[]? include = null, bool tracked = true,
            Expression<Func<T, IOrderedQueryable>>? orderByExpression = null, int take = -1)
        {

            IQueryable<T> entities = _db;

            if (expression is not null)
            {
                entities = entities.Where(expression);
            }

            if (include is not null)
            {
                foreach (var item in include)
                {
                    entities = entities.Include(item);
                }
            }

            if (orderByExpression is not null)
            {
                entities = entities.OrderBy(orderByExpression);
            }

            if (take > -1)
            {
                entities = entities.Take(take);
            }

            if (!tracked)
            {
                entities = entities.AsNoTracking();
            }

            return await entities.ToListAsync();
        }

        public async Task<T?> GetOneAsync(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>[]? include = null, bool tracked = true)
        {
            return (await GetAsync(expression, include, tracked)).SingleOrDefault();
        }

        public async Task<List<T>> GetAsyncIncludes(Expression<Func<T, bool>>? condition = null, List<Func<IQueryable<T>, IQueryable<T>>>? includes = null, bool tracked = true)
        {
            IQueryable<T> entities = _db;

            if (condition is not null)
            {
                entities = entities.Where(condition);
            }

            if (includes is not null)
            {
                foreach (var item in includes)
                {
                    entities = item(entities);
                }
            }

            if (!tracked)
            {
                entities = entities.AsNoTracking();
            }

            return await entities.ToListAsync();
        }

        public async Task<T?> GetOneAsyncIncludes(Expression<Func<T, bool>>? condition = null, List<Func<IQueryable<T>, IQueryable<T>>>? includes = null, bool tracked = true)
        {
            return (await GetAsyncIncludes(condition, includes, tracked)).SingleOrDefault();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>>? condition = null)
        {
            IQueryable<T> entities = _db;
            if (condition is not null)
            {
                return await entities.AnyAsync(condition);
            }
            return await entities.AnyAsync();
        }


        public async Task<bool> CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ex: {ex}");
                return false;
            }
        }

        public void DetachEntity(T entity)
        {
            _db.Entry(entity).State = EntityState.Detached;
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? expression = null)
        {
            IQueryable<T> entities = _db;

            if (expression is not null)
            {
                entities = entities.Where(expression);
            }

            return await entities.CountAsync();
        }

        public async Task<decimal> SumAsync(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>>? expression = null)
        {
            IQueryable<T> entities = _db;

            if (expression is not null)
            {
                entities = entities.Where(expression);
            }

            return await entities.SumAsync(selector);
        }
    }
}
