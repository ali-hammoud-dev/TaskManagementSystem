using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.DataAccess.Models;

public class BaseModel
{
    [Key]
    public int Id { get; set; }
}

