using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security.Claims;
using TaskManagementSystem.Business.DTOs;
using TaskManagementSystem.Business.Managers.Interfaces;
using TaskManagementSystem.Business.Validator.Interfaces;
using TaskManagementSystem.Common.Enums;
using TaskManagementSystem.Common.Exceptions;
using TaskManagementSystem.DataAccess.Interfaces;
using TaskManagementSystem.DataAccess.Models;
using TaskManagementSystem.Logging.Interfaces;

namespace TaskManagementSystem.Business.Managers;

public class TaskManager : BaseManager<TaskModel>, ITaskManager
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ITaskValidator _taskValidator;

    public TaskManager(ITaskRepository repository,
        IMapper mapper, ILoggerService loggerService,
        IHttpContextAccessor contextAccessor,
        ITaskValidator taskValidator) : base(repository, mapper, loggerService)
    {
        _contextAccessor = contextAccessor;
        _taskValidator = taskValidator;
    }

    public async Task<TaskDto> GetById(int id)
    {
        var taskEntity = await GetByIdAsync(id);
        if (taskEntity is null)
            throw new PlatformExceptionBuilder().StatusCode(HttpStatusCode.NotFound)
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
        _taskValidator.ValidateUpdate(taskDto);
        var task = await QueryItemAsync(x => x.Id == taskDto.Id && x.UserId == taskDto.UserId);
        if (task != null)
            await UpdateAsync(Mapper.Map<TaskModel>(taskDto));
        else
            throw new PlatformExceptionBuilder().StatusCode(HttpStatusCode.BadRequest)
                .ErrorMessage("User Should Be Assigned To a User.").Build();

    }

    public async Task<IEnumerable<TaskDto>> GetTaskForUser(string userId)
    {
        var entities = await QueryAsync(x => x.UserId == userId);
        if (entities.IsNullOrEmpty())

            throw new PlatformExceptionBuilder()
                .StatusCode(HttpStatusCode.NotFound)
                .ErrorMessage("No task for this user").Build();

        return Mapper.Map<IEnumerable<TaskDto>>(entities);
    }

    public async Task DeleTask(int taskId)
    {
        var entityToBeDeleted = await GetByIdAsync(taskId);
        await DeleteAsync(entityToBeDeleted);

    }

    public async Task DeleArrangeTask(IEnumerable<TaskDto> taskDtos, string userId)
    {
        var entities = await QueryAsync(x => x.UserId == userId);
        await DeleteArrangeAsync(entities);
    }

    public async Task AssigneTask(int taskId, string userId)
    {
        var taskToBeAssigned = await QueryItemAsync(x => x.Id == taskId);
        if (taskToBeAssigned is null)
            throw new PlatformExceptionBuilder().StatusCode(HttpStatusCode.NotFound)
                .ErrorMessage("Task does not Exist.").Build();

        var usermanager = _contextAccessor.HttpContext.RequestServices.GetService<ICustomUsermanager>();
        var userToBeAssigned = await usermanager.GetByIdAsync(userId);
        if (userToBeAssigned is null)
            throw new PlatformExceptionBuilder().StatusCode(HttpStatusCode.NotFound)
                .ErrorMessage("User does not Exist.").Build();

        taskToBeAssigned.UserId = userId;
        await UpdateAsync(taskToBeAssigned);
    }

    #region Private Methods
    private string GetUserId()
    {
        ClaimsPrincipal user = _contextAccessor.HttpContext.User;
        Claim? userIdClaim = user.FindFirst("UserId");

        if (userIdClaim != null)
            return userIdClaim.Value;

        return string.Empty;
    }
    #endregion
}