using System.Linq.Expressions;

namespace Repositories.Contracts
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> GetAll(bool trackChanges);
        T? FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
        void Create(T entity);
        IQueryable<T> GetAllByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
        void Update(T entity);
        void Delete(T entity);
    }
}
