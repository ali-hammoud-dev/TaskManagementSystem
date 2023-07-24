using AutoMapper;
using System.Linq.Expressions;
using TaskManagementSystem.Business.Managers.Interfaces;
using TaskManagementSystem.DataAccess.Interfaces;

namespace TaskManagementSystem.Business.Managers;

public class BaseManager<TEntity> : IBaseManager<TEntity> where TEntity : class
{
    private readonly IBaseRepository<TEntity> _repository;
    protected readonly IMapper Mapper;

    public BaseManager(IBaseRepository<TEntity> repository, IMapper mapper)
    {
        _repository = repository;
        Mapper = mapper;
    }

    public async Task<TEntity> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        return await _repository.AddAsync(entity);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        await _repository.UpdateAsync(entity);
    }

    public async Task DeleteAsync(TEntity entity)
    {
        await _repository.DeleteAsync(entity);
    }

    public async Task<TEntity> QueryItemAsync(Expression<Func<TEntity, bool>> predicate) => await _repository.QueryItemAsync(predicate);

    public async Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> predicate) => await _repository.QueryAsync(predicate);
}

