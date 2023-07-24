using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagementSystem.DataAccess.Interfaces;

namespace TaskManagementSystem.DataAccess.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly DbContext Context;
    private readonly DbSet<TEntity> _dbSet;

    public BaseRepository(DbContext context)
    {
        Context = context;
        _dbSet = Context.Set<TEntity>();
    }

    public async Task<TEntity> GetByIdAsync(int id) => await _dbSet.FindAsync(id);


    public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbSet.ToListAsync();
    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        await Context.SaveChangesAsync();
    }

    public async Task<TEntity> QueryItemAsync(Expression<Func<TEntity, bool>> predicate) => await _dbSet.FirstOrDefaultAsync(predicate);

    public async Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> predicate) => await _dbSet.Where(predicate).ToListAsync();
}

