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

    public TaskController(ITaskManager manager)
    {
        _manager = manager;
    }

    [HttpPost]
    [Authorize(Roles = "User,Admin,Manager")]
    public async Task<IActionResult> CreateTask([FromBody] TaskDto taskDto)
    {
        try
        {
            var createdTask = await _manager.CreateTask(taskDto);
            return Ok(createdTask);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> GetTasks()
    {
        try
        {
            var allTasks = await _manager.GetAll();
            return Ok(allTasks);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Authorize(Roles = "Manager")]
    [Route("{id:int:min(1)}")]
    public async Task<IActionResult> GetTaskById([FromRoute] int id)
    {
        try
        {
            var task = await _manager.GetById(id);
            return Ok(task);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> UpdateTask([FromBody] TaskDto taskDto)
    {
        try
        {
            await _manager.Update(taskDto);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Authorize(Roles = "Manager,User")]
    [Route("/GetTasksByUserId/{userId}")]
    public async Task<IActionResult> GetTasksByUserId([FromRoute] string userId)
    {
        try
        {
            var tasks = await _manager.GetTaskForUser(userId);
            return Ok(tasks);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    [Route("/DeleteATask/{taskId}")]
    public async Task<IActionResult> DeleTask([FromRoute] int taskId)
    {
        await _manager.DeleTask(taskId);
        return NoContent();
    }



    [HttpPut]
    [Authorize(Roles = "Manager")]
    [Route("/Assigne/{userId}/task/{taskId}")]
    public async Task<IActionResult> AssigneTask([FromRoute] int taskId, [FromRoute] string userId)
    {
        try
        {
            return Ok(_manager.AssigneTask(taskId, userId));
        }
        catch (Exception e)
        {

            return BadRequest(e.Message);
        }
    }
}

