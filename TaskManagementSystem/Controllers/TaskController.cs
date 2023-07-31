using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Business.DTOs;
using TaskManagementSystem.Business.Managers.Interfaces;
using TaskManagementSystem.Common.Exceptions;

namespace TaskManagementSystem.Controllers;

[ApiController]
//[Authorize]
[Route("api/tasks")]

public class TaskController : ControllerBase
{
    private readonly ITaskManager _manager;

    public TaskController(ITaskManager manager) => _manager = manager;

    [HttpPost]
    //[Authorize(Roles = "User")]
    public async Task<IActionResult> CreateTask([FromBody] TaskDto taskDto)
    {
        try
        {
            var result = await _manager.CreateTask(taskDto);
            return Ok(new { Message = "Task created successfully!", Data = result });
        }
        catch (PlatformException e)
        {
            return StatusCode((int)e.StatusCode,
                new { e.ErrorMessage, InnerMessage = e.InnerException.Message });
        }

    }

    [HttpGet]
    //[Authorize(Roles = "Manager")]
    public async Task<IActionResult> GetTasks()
    {
        try
        {
            return Ok(await _manager.GetAll());
        }
        catch (PlatformException e)
        {
            return StatusCode((int)e.StatusCode,
                new { e.ErrorMessage, InnerMessage = e.InnerException.Message });
        }
    }


    [HttpGet]
    //[Authorize(Roles = "Manager")]
    [Route("{id:int:min(1)}")]
    public async Task<IActionResult> GetTaskById([FromRoute] int id)
    {
        try
        {
            return Ok(await _manager.GetById(id));
        }
        catch (PlatformException e)
        {
            return StatusCode((int)e.StatusCode, new { e.ErrorMessage });
        }
    }


    [HttpPut]
    //[Authorize(Roles = "Manager")]
    public async Task<IActionResult> UpdateTask([FromBody] TaskDto taskDto)
    {
        try
        {
            await _manager.Update(taskDto);
            return NoContent();
        }
        catch (PlatformException e)
        {
            return StatusCode((int)e.StatusCode, new { e.ErrorMessage });
        }
    }

    [HttpGet]
    //[Authorize(Roles = "User")]
    [Route("/GetTasksByUserId/{userId}")]
    public async Task<IActionResult> GetTasksByUserId([FromRoute] string userId)
    {
        try
        {
            return Ok(await _manager.GetTaskForUser(userId));
        }
        catch (PlatformException e)
        {
            return StatusCode((int)e.StatusCode, new { e.ErrorMessage });
        }
    }


    [HttpDelete]
    //[Authorize(Roles = "Admin")]
    [Route("/DeleteATask/{taskId}")]
    public async Task DeleTask([FromRoute] int taskId) => await _manager.DeleTask(taskId);

    [HttpPut]
    //[Authorize(Roles = "Manager")]
    [Route("/Assigne/{userId}/task/{taskId}")]
    public async Task<IActionResult> AssigneTask([FromRoute] int taskId, [FromRoute] string userId)
    {
        try
        {
            await _manager.AssigneTask(taskId, userId);
            return NoContent();
        }
        catch (PlatformException e)
        {
            return StatusCode((int)e.StatusCode, new { e.ErrorMessage });

        }
    }
}

