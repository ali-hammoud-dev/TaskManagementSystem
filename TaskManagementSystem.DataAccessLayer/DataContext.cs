using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.DataAccess.Models;

namespace TaskManagementSystem.DataAccess;

public class DataContext : IdentityDbContext<IdentityUser>
{
    public DbSet<TaskModel> Tasks { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
            new IdentityRole { Name = "Manager", ConcurrencyStamp = "2", NormalizedName = "Manager" },
            new IdentityRole { Name = "User", ConcurrencyStamp = "3", NormalizedName = "User" }
        );
    }
}

