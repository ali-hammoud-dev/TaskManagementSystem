using TaskManagementSystem.Business.DTOs;
using TaskManagementSystem.DataAccess.Models;

namespace TaskManagementSystem.Business.Managers.Interfaces;

public interface ITaskManager : IBaseManager<TaskModel>
{
    Task<TaskDto?> GetById(int id);

    Task<IEnumerable<TaskDto>> GetAll();

    Task<TaskDto> CreateTask(TaskDto taskDto);

    Task Update(TaskDto taskDto);

    Task<IEnumerable<TaskDto>> GetTaskForUser(string userId);

    Task DeleTask(int taskId);

    Task DeleArrangeTask(IEnumerable<TaskDto> tskDtos, string userId);

    Task AssigneTask(int taskId, string userId);
}

