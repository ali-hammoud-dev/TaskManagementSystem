using System.Linq.Expressions;

namespace TaskManagementSystem.DataAccess.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(int id);

    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<TEntity> AddAsync(TEntity entity);

    Task<TEntity> UpdateAsync(TEntity entity);

    Task DeleteAsync(TEntity entity);

    Task DeleteArrangeAsync(IEnumerable<TEntity> entitys);

    Task<TEntity> QueryItemAsync(Expression<Func<TEntity, bool>> predicate);

    Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> predicate);
}

