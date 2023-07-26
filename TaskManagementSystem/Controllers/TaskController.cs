using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Business.DTOs;
using TaskManagementSystem.Business.Managers.Interfaces;

namespace TaskManagementSystem.Controllers;

[ApiController]
[Authorize]
[Route("api/tasks")]

public class TaskController : ControllerBase
{
    private readonly ITaskManager _manager;

    public TaskController(ITaskManager manager) => _manager = manager;

    [HttpPost]
    [Authorize(Roles = "User")]
    public async Task<TaskDto> CreateTask([FromBody] TaskDto taskDto) => await _manager.CreateTask(taskDto);

    [HttpGet]
    [Authorize(Roles = "Manager")]
    public async Task<IEnumerable<TaskDto>> GetTasks() => await _manager.GetAll();


    [HttpGet]
    [Authorize(Roles = "Manager")]
    [Route("{id:int:min(1)}")]
    public async Task<TaskDto> GetTaskById([FromRoute] int id) => await _manager.GetById(id);


    [HttpPut]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> UpdateTask([FromBody] TaskDto taskDto)
    {
        await _manager.Update(taskDto);
        return NoContent();
    }

    [HttpGet]
    [Authorize(Roles = "User")]
    [Route("/GetTasksByUserId/{userId}")]
    public async Task<IEnumerable<TaskDto>> GetTasksByUserId([FromRoute] string userId) => await _manager.GetTaskForUser(userId);


    [HttpDelete]
    [Authorize(Roles = "Admin")]
    [Route("/DeleteATask/{taskId}")]
    public async Task DeleTask([FromRoute] int taskId) => await _manager.DeleTask(taskId);

    [HttpPut]
    [Authorize(Roles = "Manager")]
    [Route("/Assigne/{userId}/task/{taskId}")]
    public async Task<IActionResult> AssigneTask([FromRoute] int taskId, [FromRoute] string userId)
    {
        await _manager.AssigneTask(taskId, userId);
        return NoContent();
    }
}

