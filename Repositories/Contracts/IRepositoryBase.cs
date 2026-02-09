using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Repositories.Contracts
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> GetAll(bool trackChanges);
        IQueryable<T> GetAllByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
        Task<T?> FindByConditionAsync(Expression<Func<T, bool>> expression,
                                      bool trackChanges,
                                      Func<IQueryable<T>,
                                      IIncludableQueryable<T, object>>? include = null);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync(bool trackChanges);
        Task<IEnumerable<T>> GetAllByConditionAsync(Expression<Func<T, bool>> expression, bool trackChanges);
        Task SaveAsync();
    }
}
