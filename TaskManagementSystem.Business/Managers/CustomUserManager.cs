using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TaskManagementSystem.Business.DTOs;
using TaskManagementSystem.Business.Managers.Interfaces;
using TaskManagementSystem.Common.Exceptions;
using TaskManagementSystem.Logging.Interfaces;

namespace TaskManagementSystem.Business.Managers;

public class CustomUserManager : UserManager<IdentityUser>, ICustomUsermanager
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;
    private readonly ILoggerService _loggerService;
    private readonly ITaskManager _taskManager;

    public CustomUserManager(IUserStore<IdentityUser> store, IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<IdentityUser> passwordHasher, IEnumerable<IUserValidator<IdentityUser>> userValidators,
        IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators, ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<IdentityUser>> logger,
        RoleManager<IdentityRole> roleManager, IMapper mapper, ILoggerService loggerService, ITaskManager taskManager) :
        base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators,
            keyNormalizer, errors, services, logger)
    {
        _roleManager = roleManager;
        _mapper = mapper;
        _loggerService = loggerService;
        _taskManager = taskManager;
    }


    public async Task<IdentityUser> GetByIdAsync(string id)
    {
        try
        {
            var entity = await FindByIdAsync(id);
            _loggerService.LogInfo($"Retrieved entity by id = {id},of type {entity.GetType().FullName}");
            return entity;
        }
        catch (Exception e)
        {
            _loggerService.LogErrorException(e);
            throw new PlatformExceptionBuilder().StatusCode(HttpStatusCode.NotFound).ErrorMessage(e.Message).Build();
        }
    }

    public async Task DeleteAsync(IdentityUser entity)
    {
        try
        {
            await base.DeleteAsync(entity);
            _loggerService.LogInfo("The entity has been deleted");
        }
        catch (Exception e)
        {
            _loggerService.LogErrorException(e);
            throw new PlatformExceptionBuilder().StatusCode(HttpStatusCode.BadRequest).ErrorMessage(e.Message).Build();
        }
    }

    public async Task CreateUserAsync(LoginDto user, string password, string role)
    {
        try
        {
            var newUser = _mapper.Map<IdentityUser>(user);

            var result = await CreateAsync(newUser, password);
            if (result.Succeeded)
            {

                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole(role));

                await AddToRoleAsync(newUser, role);
                _loggerService.LogInfo($"Added successfully {newUser.GetType().FullName}");
            }
        }
        catch (Exception e)
        {
            _loggerService.LogErrorException(e);
            throw new PlatformExceptionBuilder().StatusCode(HttpStatusCode.NoContent).ErrorMessage(e.Message).Build();
        }
    }

    public async Task DeleteUserArrangeAsync(string userId)
    {
        try
        {
            var user = await FindByIdAsync(userId);
            var tasksByUser = await _taskManager.GetTaskForUser(userId);
            await _taskManager.DeleArrangeTask(tasksByUser, userId);
            await DeleteAsync(user);
        }
        catch (Exception e)
        {
            _loggerService.LogErrorException(e);
            throw new PlatformExceptionBuilder().ErrorMessage(e.Message).Build();
        }

    }

    public async Task<IdentityUser> FindByNameAsync(string name)
    {
        var user = await base.FindByNameAsync(name);
        return user;
    }

    public async Task<bool> CheckPasswordAsync(IdentityUser user, string password)
    {
        bool isPasswordValid = await base.CheckPasswordAsync(user, password);
        return isPasswordValid;
    }

    public async Task<IList<string>> GetRolesAsync(IdentityUser user)
    {
        IList<string> roles = await base.GetRolesAsync(user);
        return roles;
    }
}