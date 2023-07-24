using TaskManagementSystem.Common.Enums;

namespace TaskManagementSystem.Business.DTOs;

public class TaskDto : BaseDto
{
    public string? UserId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public bool IsCompleted { get; set; }

    public PriorityEnum Priority { get; set; }

    public TaskStatusEnum TaskStatus { get; set; }

    public string CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime DueDate { get; set; }
}

