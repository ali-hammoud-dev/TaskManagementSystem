using TaskManagementSystem.Business.Managers;
using TaskManagementSystem.Business.Managers.Interfaces;
using TaskManagementSystem.DataAccess.Interfaces;
using TaskManagementSystem.DataAccess.Repositories;
using TaskManagementSystem.Logging;
using TaskManagementSystem.Logging.Interfaces;

namespace TaskManagementSystem;

public static class RegisterIdentityBusinessServicesExtensions
{
    public static IServiceCollection RegisterServiceCollection(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddScoped(typeof(IBaseManager<>), typeof(BaseManager<>));
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<ILoggerService, LoggerService>();
        services.AddScoped<ICustomUsermanager, CustomUserManager>();
        services.AddScoped<ITaskManager, TaskManager>();

        services.Configure<Program>(configuration.GetSection("Program"));

        return services;
    }
}

