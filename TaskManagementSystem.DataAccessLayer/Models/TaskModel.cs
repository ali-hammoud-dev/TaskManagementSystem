using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManagementSystem.Common.Enums;

namespace TaskManagementSystem.DataAccess.Models;

public class TaskModel : BaseModel
{
    public string? UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual IdentityUser? User { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Title { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [Required]
    public bool IsCompleted { get; set; }

    public PriorityEnum Priority { get; set; }

    public TaskStatusEnum TaskStatus { get; set; }

    public string CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime DueDate { get; set; }

}

