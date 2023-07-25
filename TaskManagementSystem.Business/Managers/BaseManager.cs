using AutoMapper;
using System.Linq.Expressions;
using TaskManagementSystem.Business.Managers.Interfaces;
using TaskManagementSystem.Common.Exceptions;
using TaskManagementSystem.DataAccess.Interfaces;
using TaskManagementSystem.Logging.Interfaces;

namespace TaskManagementSystem.Business.Managers;

public class BaseManager<TEntity> : IBaseManager<TEntity> where TEntity : class
{
    private readonly IBaseRepository<TEntity> _repository;
    private readonly ILoggerService _loggerService;
    protected readonly IMapper Mapper;

    public BaseManager(IBaseRepository<TEntity> repository, IMapper mapper, ILoggerService loggerService)
    {
        _repository = repository;
        _loggerService = loggerService;
        Mapper = mapper;
    }

    public async Task<TEntity> GetByIdAsync(int id)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(id);
            _loggerService.LogInfo($"Retrieved entity by id = {id},of type {entity.GetType().FullName}");
            return entity;
        }
        catch (Exception e)
        {

            _loggerService.LogErrorException(e);
            throw new PlatformExceptionBuilder().StatusCode(404).ErrorMessage(e.Message).Build();
        }
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        try
        {
            var entities = await _repository.GetAllAsync();
            _loggerService.LogInfo("Retrieving entities successfully");
            return entities;

        }
        catch (Exception e)
        {
            _loggerService.LogErrorException(e);
            throw new PlatformExceptionBuilder().StatusCode(204).ErrorMessage(e.Message).Build();
        }
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        try
        {
            var entityToBeCreated = await _repository.AddAsync(entity);
            _loggerService.LogInfo($"Added successfully {entityToBeCreated.GetType().FullName}");
            return entityToBeCreated;
        }
        catch (Exception e)
        {
            _loggerService.LogErrorException(e);
            throw new PlatformExceptionBuilder().StatusCode(204).ErrorMessage(e.Message).Build();
        }
    }

    public async Task UpdateAsync(TEntity entity)
    {
        try
        {
            await _repository.UpdateAsync(entity);
            _loggerService.LogInfo($"The update was completed successfully on the entity {entity.GetType().FullName}");
        }
        catch (Exception e)
        {
            _loggerService.LogErrorException(e);
            throw new PlatformExceptionBuilder().StatusCode(204).ErrorMessage(e.Message).Build();
        }
    }

    public async Task DeleteAsync(TEntity entity)
    {
        try
        {
            await _repository.DeleteAsync(entity);
            _loggerService.LogInfo("The entity has been deleted");
        }
        catch (Exception e)
        {
            _loggerService.LogErrorException(e);
            throw new PlatformExceptionBuilder().StatusCode(400).ErrorMessage(e.Message).Build();
        }
    }

    public async Task DeleteArrangeAsync(IEnumerable<TEntity> entities)
    {
        await _repository.DeleteArrangeAsync(entities);
    }

    public async Task<TEntity> QueryItemAsync(Expression<Func<TEntity, bool>> predicate) => await _repository.QueryItemAsync(predicate);

    public async Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> predicate) => await _repository.QueryAsync(predicate);
}

