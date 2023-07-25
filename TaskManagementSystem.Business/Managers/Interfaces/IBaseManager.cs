using System.Linq.Expressions;

namespace TaskManagementSystem.Business.Managers.Interfaces;

public interface IBaseManager<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(int id);

    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<TEntity> AddAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task DeleteAsync(TEntity entity);

    Task DeleteArrangeAsync(IEnumerable<TEntity> entities);

    Task<TEntity> QueryItemAsync(Expression<Func<TEntity, bool>> predicate);

    Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> predicate);
}