using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagementSystem.Business.DTOs;
using TaskManagementSystem.Business.Managers.Interfaces;
using TaskManagementSystem.Common.Enums;
using Xunit;

namespace TaskManagementSystem.Controllers;

public class TaskControllerTests
{
    private readonly Mock<ITaskManager> _mockTaskManager = new Mock<ITaskManager>();
    private readonly TaskController _taskController;

    public TaskControllerTests() => _taskController = new TaskController(_mockTaskManager.Object);

    [Fact]
    public async Task CreateTaskValidTaskReturnsCreatedTask()
    {
        var taskDto = new TaskDto
        {
            Title = "task 1",
            Description = "description",
            IsCompleted = true,
            Priority = PriorityEnum.Low,
            TaskStatus = TaskStatusEnum.NotStarted,
        };
        var createdTaskDto = new TaskDto
        {
            UserId = null,
            Title = "string",
            Description = "string",
            IsCompleted = false,
            Priority = PriorityEnum.Low,
            TaskStatus = TaskStatusEnum.NotStarted,
            CreatedByUserId = "3f8182d9-c0fd-4fc1-a83b-d5b1eb01c1cb",
            CreatedAt = DateTime.Parse("2023-07-26T01:56:12.6884496+03:00"),
            UpdatedAt = DateTime.Parse("0001-01-01T00:00:00"),
            DueDate = DateTime.Parse("0001-01-01T00:00:00"),
            Id = 2005
        };
        _mockTaskManager.Setup(manager => manager.CreateTask(taskDto)).ReturnsAsync(createdTaskDto);

        var result = await _taskController.CreateTask(taskDto);
        var okObjectResult = Assert.IsType<TaskDto>(result);
        var returnedTaskDto = Assert.IsType<TaskDto>(okObjectResult);
        Assert.Equal(createdTaskDto, returnedTaskDto);
    }

    [Fact]
    public async Task GetTasksReturnsListOfTasks()
    {
        var tasks = new List<TaskDto>
        {
            new TaskDto
            {
                UserId = null,
                Title = "string",
                Description = "string",
                IsCompleted = false,
                Priority = PriorityEnum.Low,
                TaskStatus = TaskStatusEnum.NotStarted,
                CreatedByUserId = "3f8182d9-c0fd-4fc1-a83b-d5b1eb01c1cb",
                CreatedAt = DateTime.Parse("2023-07-26T01:56:12.6884496+03:00"),
                UpdatedAt = DateTime.Parse("0001-01-01T00:00:00"),
                DueDate = DateTime.Parse("0001-01-01T00:00:00"),
                Id = 2005
            }
        };
        _mockTaskManager.Setup(manager => manager.GetAll()).ReturnsAsync(tasks);

        var result = await _taskController.GetTasks();

        var okObjectResult = Assert.IsType<List<TaskDto>>(result);
        var returnedTasks = Assert.IsType<List<TaskDto>>(okObjectResult);
        Assert.Equal(tasks, returnedTasks);
    }

    [Fact]
    public async Task GetTaskByIdValidIdReturnsTask()
    {
        int taskId = 1005;
        var taskDto = new TaskDto
        {
            UserId = null,
            Title = "string",
            Description = "string",
            IsCompleted = false,
            Priority = PriorityEnum.Low,
            TaskStatus = TaskStatusEnum.NotStarted,
            CreatedByUserId = "3f8182d9-c0fd-4fc1-a83b-d5b1eb01c1cb",
            CreatedAt = DateTime.Parse("2023-07-26T01:56:12.6884496+03:00"),
            UpdatedAt = DateTime.Parse("0001-01-01T00:00:00"),
            DueDate = DateTime.Parse("0001-01-01T00:00:00"),
            Id = 1005
        };
        _mockTaskManager.Setup(manager => manager.GetById(taskId)).ReturnsAsync(taskDto);

        var result = await _taskController.GetTaskById(taskId);

        var okObjectResult = Assert.IsType<TaskDto>(result);
        var returnedTaskDto = Assert.IsType<TaskDto>(okObjectResult);
        Assert.Equal(taskDto, returnedTaskDto);
    }

    [Fact]
    public async Task UpdateTaskValidTaskReturnsNoContent()
    {
        var taskDto = new TaskDto
        {
            UserId = null,
            Title = "string",
            Description = "hello world",
            IsCompleted = false,
            Priority = PriorityEnum.Low,
            TaskStatus = TaskStatusEnum.NotStarted,
            CreatedByUserId = "3f8182d9-c0fd-4fc1-a83b-d5b1eb01c1cb",
            CreatedAt = DateTime.Parse("2023-07-26T01:56:12.6884496+03:00"),
            UpdatedAt = DateTime.Parse("0001-01-01T00:00:00"),
            DueDate = DateTime.Parse("0001-01-01T00:00:00"),
            Id = 2005
        };
        _mockTaskManager.Setup(manager => manager.Update(taskDto)).Returns(Task.CompletedTask);
        var result = await _taskController.UpdateTask(taskDto);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task GetTasksByUserIdValidUserIdReturnsTasks()
    {
        string userId = "3f8182d9-c0fd-4fc1-a83b-d5b1eb01c1cb"; // A valid user ID for testing
        var tasks = new List<TaskDto>
        {
            new TaskDto
            {
                UserId = "3f8182d9-c0fd-4fc1-a83b-d5b1eb01c1cb",
                Title = "hello siki",
                Description = "nawnaw",
                IsCompleted = true,
                Priority = PriorityEnum.High,
                TaskStatus = TaskStatusEnum.NotStarted,
                CreatedByUserId = "a51794a0-6575-4f70-88bd-d6fafae88923",
                CreatedAt = DateTime.Parse("2023-07-26T00:25:30.5924828"),
                UpdatedAt = DateTime.Parse("2023-07-25T21:25:18.412"),
                DueDate = DateTime.Parse("2023-07-25T21:25:18.412"),
                Id = 2002
            },
            new TaskDto
            {
                UserId = "3f8182d9-c0fd-4fc1-a83b-d5b1eb01c1cb",
                Title = "string",
                Description = "testdescription",
                IsCompleted = true,
                Priority = PriorityEnum.High,
                TaskStatus = TaskStatusEnum.NotStarted,
                CreatedByUserId = "string",
                CreatedAt = DateTime.Parse("2023-07-25T23:44:01.348"),
                UpdatedAt = DateTime.Parse("2023-07-25T23:44:01.348"),
                DueDate = DateTime.Parse("2023-07-25T23:44:01.348"),
                Id = 2005
            }
        };


        _mockTaskManager.Setup(manager => manager.GetTaskForUser(userId)).ReturnsAsync(tasks);

        var result = await _taskController.GetTasksByUserId(userId);

        var okObjectResult = Assert.IsType<List<TaskDto>>(result);
        var returnedTasks = Assert.IsType<List<TaskDto>>(okObjectResult);
        Assert.Equal(tasks, returnedTasks);
    }

    [Fact]
    public async Task AssigneTaskValidTaskIdAndUserIdReturnsNoContent()
    {
        int taskId = 2004;
        string userId = "3f8182d9-c0fd-4fc1-a83b-d5b1eb01c1cb";

        _mockTaskManager.Setup(manager => manager.AssigneTask(taskId, userId)).Returns(Task.CompletedTask);

        var result = await _taskController.AssigneTask(taskId, userId);
        Assert.IsType<NoContentResult>(result);
    }
}

