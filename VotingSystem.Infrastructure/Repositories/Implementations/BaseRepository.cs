using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VotingSystem.Common.RequestModel;
using VotingSystem.Infrastructure.Data;
using VotingSystem.Infrastructure.Repositories.Interfaces;

namespace VotingSystem.Infrastructure.Repositories.Implementations
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly AppDbContext _dbContext;

        public BaseRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> Add(T entity)
        {
            var result = await _dbContext.Set<T>().AddAsync(entity);
            _dbContext.SaveChanges();
            return result.Entity;
        }

        public async Task<bool> Delete(T entity)
        {
            _dbContext.Remove(entity);
            var affectedRows = await _dbContext.SaveChangesAsync();
            return affectedRows > 0;
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
    }
}
