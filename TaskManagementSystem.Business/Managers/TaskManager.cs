using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;
using TaskManagementSystem.Business.DTOs;
using TaskManagementSystem.Business.Managers.Interfaces;
using TaskManagementSystem.Common.Enums;
using TaskManagementSystem.Common.Exceptions;
using TaskManagementSystem.DataAccess.Interfaces;
using TaskManagementSystem.DataAccess.Models;

namespace TaskManagementSystem.Business.Managers;

public class TaskManager : BaseManager<TaskModel>, ITaskManager
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly CustomUserManager _customUserManager;
    public TaskManager(ITaskRepository repository, IMapper mapper, IHttpContextAccessor contextAccessor, CustomUserManager customUserManager) : base(repository, mapper)
    {
        _contextAccessor = contextAccessor;
        _customUserManager = customUserManager;
    }

    public async Task<TaskDto> GetById(int id)
    {
        var taskEntity = await GetByIdAsync(id);
        if (taskEntity is null)
            throw new PlatformExceptionBuilder().StatusCode((int)HttpStatusCode.NotFound)
                .ErrorMessage("Task does not Exist.").Build();

        var taskDto = Mapper.Map<TaskDto>(taskEntity);

        return taskDto;
    }

    public async Task<IEnumerable<TaskDto>> GetAll()
    {
        var entities = await GetAllAsync();
        return Mapper.Map<IEnumerable<TaskDto>>(entities);
    }

    public async Task<TaskDto> CreateTask(TaskDto taskDto)
    {
        taskDto.CreatedByUserId = GetUserId();
        taskDto.TaskStatus = TaskStatusEnum.NotStarted;
        taskDto.CreatedAt = DateTime.Now;

        var taskToCreate = Mapper.Map<TaskModel>(taskDto);
        var createTask = await AddAsync(taskToCreate);

        return Mapper.Map<TaskDto>(createTask);
    }

    public async Task Update(TaskDto taskDto)
    {
        if (taskDto.Id == 0)
            throw new PlatformExceptionBuilder().StatusCode((int)HttpStatusCode.NotFound)
                .ErrorMessage("Task does not Exist.").Build();


        await UpdateAsync(Mapper.Map<TaskModel>(taskDto));
    }

    public async Task<IEnumerable<TaskDto>> GetTaskForUser(string userId)
    {
        var entities = await QueryAsync(x => x.UserId == userId);
        return Mapper.Map<IEnumerable<TaskDto>>(entities);
    }

    public async Task DeleTask(int taskId)
    {
        var entityToBeDeleted = await GetByIdAsync(taskId);
        await DeleteAsync(entityToBeDeleted);

    }

    public async Task AssigneTask(int taskId, string userId)
    {
        var taskToBeAssigned = await GetById(taskId);
        if (taskToBeAssigned is null)
            throw new PlatformExceptionBuilder().StatusCode((int)HttpStatusCode.NotFound)
                .ErrorMessage("Task does not Exist.").Build();

        var userToBeAssigned = await _customUserManager.GetByIdAsync(userId);
        if (userToBeAssigned is null)
            throw new PlatformExceptionBuilder().StatusCode((int)HttpStatusCode.NotFound)
                .ErrorMessage("User does not Exist.").Build();

        taskToBeAssigned.UserId = userId;
        await Update(taskToBeAssigned);
    }


    //public async Task<TaskDto> AssigneTask(int taskId, string userId, DataContext contextFactory)
    //{
    //    var taskToBeAssigned = await GetById(taskId);
    //    if (taskToBeAssigned is null)
    //        throw new PlatformExceptionBuilder().StatusCode((int)HttpStatusCode.NotFound)
    //            .ErrorMessage("Task does not Exist.").Build();

    //    var userToBeAssigned = await _customUserManager.GetByIdAsync(userId);
    //    if (userToBeAssigned is null)
    //        throw new PlatformExceptionBuilder().StatusCode((int)HttpStatusCode.NotFound)
    //            .ErrorMessage("User does not Exist.").Build();

    //    taskToBeAssigned.UserId = userId;

    //    contextFactory.Update(taskToBeAssigned);
    //    await contextFactory.SaveChangesAsync();
    //    await Update(taskToBeAssigned);

    //    return taskToBeAssigned;
    //}


    #region Private Methods
    private string GetUserId()
    {
        ClaimsPrincipal user = _contextAccessor.HttpContext.User;
        Claim userIdClaim = user.FindFirst("UserId");

        if (userIdClaim != null)
            return userIdClaim.Value;

        return string.Empty;
    }
    #endregion
}

