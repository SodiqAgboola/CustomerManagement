using CustomerManagementSystem.Data;
using CustomerManagementSystem.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CustomerManagementSystem.Repository
{
    public class EfRepository<T> : IAsyncRepository<T> where T : class
    {
        private readonly ApplicationDBContext _dbContext;
        public DbSet<T> Table;
        public EfRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
            Table = _dbContext.Set<T>();
        }
        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<List<T>> AddRangeAsync(List<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();

            return entities;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);

            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<T> GetAll()
        {
            return GetAllIncluding();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> GetByIdAsync<TId>(TId id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            _ = entities.Select(x => { _dbContext.Entry(x).State = EntityState.Modified; return x; });
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] propertySelectors)
        {
            var query = Table.AsQueryable();

            return propertySelectors.Aggregate(query, (current, propertySelector) => current.Include(propertySelector));
        }
    }
}
