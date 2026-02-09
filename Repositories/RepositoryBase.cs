using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Repositories.Contracts;
using System.Linq.Expressions;

namespace Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class, new()
    {
        protected readonly RepositoryContext _repositoryContext;

        protected RepositoryBase(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IQueryable<T> GetAll(bool trackChanges)
        {
            return trackChanges ? _repositoryContext.Set<T>() :
                                  _repositoryContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> GetAllByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            return trackChanges ? _repositoryContext.Set<T>().Where(expression) :
                                  _repositoryContext.Set<T>().Where(expression).AsNoTracking();
        }

        public async Task<T?> FindByConditionAsync(Expression<Func<T, bool>> expression, 
                                                   bool trackChanges,
                                                   Func<IQueryable<T>, 
                                                   IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = _repositoryContext.Set<T>();

            if (!trackChanges)
                query = query.AsNoTracking();

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync(expression);
        }

        public async Task CreateAsync(T entity)
        {
            await _repositoryContext.Set<T>().AddAsync(entity);
        }

        public Task UpdateAsync(T entity)
        {
            _repositoryContext.Set<T>().Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(T entity)
        {
            await Task.Run(() => _repositoryContext.Set<T>().Remove(entity));
        }

        public async Task<IEnumerable<T>> GetAllAsync(bool trackChanges)
            => await (trackChanges
                ? _repositoryContext.Set<T>()
                : _repositoryContext.Set<T>().AsNoTracking())
                .ToListAsync();

        public async Task<IEnumerable<T>> GetAllByConditionAsync(Expression<Func<T, bool>> expression, bool trackChanges)
            => await (trackChanges
                ? _repositoryContext.Set<T>().Where(expression)
                : _repositoryContext.Set<T>().Where(expression).AsNoTracking())
                .ToListAsync();

        public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
    }
}
