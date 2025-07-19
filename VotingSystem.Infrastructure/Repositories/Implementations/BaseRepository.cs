using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VotingSystem.Common.RequestModel;
using VotingSystem.Infrastructure.Data;
using VotingSystem.Infrastructure.Repositories.Interfaces;

namespace VotingSystem.Infrastructure.Repositories.Implementations
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly AppDbContext _dbContext;
        protected readonly DbSet<T> _dbSet; // for reaction repo

        public BaseRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public async Task<T> Add(T entity)
        {
            var result = await _dbContext.Set<T>().AddAsync(entity);
            _dbContext.SaveChanges();
            return result.Entity;
        }

        public async Task Delete(T entity)
        {
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<T> FindSingleByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(expression);
        }

        public async Task<IEnumerable<T>> GetAll(GetRequest<T>? request)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            // Apply filter if provided
            if (request?.Filter != null)
            {
                query = query.Where(request.Filter);
            }
            // Apply ordering if provided
            if (request?.OrderBy != null)
            {
                query = request.OrderBy(query);
            }

            // Execute the query and return the results
            var result = await query.ToListAsync();
            return result;
        }

        public async Task<T>? GetById(object entityId)
        {
            var result = await _dbContext.FindAsync<T>(entityId);
            return result;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<T> Update(T entity)
        {
            var result = _dbContext.Update(entity);
            await SaveChangesAsync();
            return result.Entity;
        }

        public async Task<IEnumerable<T>> FindAllByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>()
                .Where(expression)
                .ToListAsync();
        }
    }
}
