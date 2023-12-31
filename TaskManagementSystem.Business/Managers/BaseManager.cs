﻿using AutoMapper;
using System.Linq.Expressions;
using System.Net;
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
            if (entity is null)
            {
                _loggerService.LogInfo($"Cannot Get Entity By Id{id}");
                throw new PlatformExceptionBuilder().StatusCode(HttpStatusCode.BadRequest).ErrorMessage($"Cannot Get Entity By Id{id}").Build();
            }
            _loggerService.LogInfo("Retrieving entity successfully");
            return entity;
        }
        catch (PlatformException e)
        {

            _loggerService.LogErrorException(e);
            throw new PlatformExceptionBuilder().StatusCode(e.StatusCode).ErrorMessage(e.ErrorMessage).Build();
        }
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        try
        {
            var entities = await _repository.GetAllAsync();

            if (!entities.Any())
            {
                _loggerService.LogInfo("No entities found.");
                throw new PlatformExceptionBuilder()
                    .StatusCode(HttpStatusCode.NotFound)
                    .ErrorMessage("No entities found.")
                    .Build();
            }

            _loggerService.LogInfo("Entities retrieved successfully.");
            return entities;
        }
        catch (PlatformException e)
        {
            _loggerService.LogErrorException(e);

            throw new PlatformExceptionBuilder()
                .StatusCode(HttpStatusCode.InternalServerError)
                .InnerException(e)
                .ErrorMessage(e.ErrorMessage)
                .Build();
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
        catch (PlatformException e)
        {
            _loggerService.LogErrorException(e);
            throw new PlatformExceptionBuilder().StatusCode(HttpStatusCode.NoContent).ErrorMessage(e.ErrorMessage).Build();
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
            throw new PlatformExceptionBuilder().StatusCode(HttpStatusCode.NoContent).ErrorMessage(e.Message).Build();
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
            throw new PlatformExceptionBuilder().StatusCode(HttpStatusCode.NotFound).ErrorMessage(e.Message).Build();
        }
    }

    public async Task DeleteArrangeAsync(IEnumerable<TEntity> entities)
    {
        await _repository.DeleteArrangeAsync(entities);
    }

    public async Task<TEntity> QueryItemAsync(Expression<Func<TEntity, bool>> predicate) => await _repository.QueryItemAsync(predicate);

    public async Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> predicate) => await _repository.QueryAsync(predicate);
}

