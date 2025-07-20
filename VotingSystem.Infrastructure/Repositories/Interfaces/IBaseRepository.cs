using System;
using System.Linq.Expressions;
using VotingSystem.Common.RequestModel;

namespace VotingSystem.Infrastructure.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<bool> Delete(T entity);
        Task<IEnumerable<T>> GetAll(GetRequest<T>? request);
        Task<T>? GetById(object entityId);
        Task SaveChangesAsync();
    }
}
