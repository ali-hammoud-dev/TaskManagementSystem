using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.DataAccess.Interfaces;
using TaskManagementSystem.DataAccess.Models;

namespace TaskManagementSystem.DataAccess.Repositories;

public class TaskRepository : BaseRepository<TaskModel>, ITaskRepository
{
    public TaskRepository(DataContext context) : base(context)
    {
    }
}

