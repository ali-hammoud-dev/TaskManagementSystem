using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagementSystem.DataAccess.Models;

namespace TaskManagementSystem.DataAccess.Configuration;

public class TaskConfigurations : IEntityTypeConfiguration<TaskModel>
{
    public void Configure(EntityTypeBuilder<TaskModel> builder)
    {

    }
}

