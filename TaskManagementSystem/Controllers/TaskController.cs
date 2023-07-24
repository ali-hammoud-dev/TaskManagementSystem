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
    [Authorize(Roles = "User,Admin,Manager")]
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
    public async Task UpdateTask([FromBody] TaskDto taskDto) => await _manager.Update(taskDto);

    [HttpGet]
    [Authorize(Roles = "Manager,User")]
    [Route("/GetTasksByUserId/{userId}")]
    public async Task<IEnumerable<TaskDto>> GetTasksByUserId([FromRoute] string userId) => await _manager.GetTaskForUser(userId);

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    [Route("/DeleteATask{taskId}")]
    public async Task DeleTask([FromRoute] int taskId)
    {
        await _manager.DeleTask(taskId);
        HttpContext.Response.StatusCode = 204;
    }

    [HttpPut]
    [Authorize(Roles = "Manager")]
    [Route("/AssigneTask")]
    public async Task AssigneTask([FromBody] TaskDto taskDto)
    {
        await _manager.Update(taskDto);
        HttpContext.Response.StatusCode = 204;
    }

    //[HttpPut]
    //[Authorize(Roles = "Manager")]
    //[Route("/AssigneTask/{taskid}/user/{userId}")]
    //public async Task<TaskDto> AssigneTask(int taskid, string userId, DataContext contextfactory)
    //{
    //    var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
    //        .UseSqlServer("Server=ECOMZ-D-AH-L;Database=TaskManagement.db;User ID=sa;Password=p@ssw0rd;TrustServerCertificate=True")
    //        .Options;

    //    var contextFactory = new DataContextFactory(dbContextOptions);

    //    return await _manager.AssigneTask(taskid, userId, contextfactory);

    //}
}

