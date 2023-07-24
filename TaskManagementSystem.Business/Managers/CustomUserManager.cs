using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TaskManagementSystem.Business.DTOs;
using TaskManagementSystem.Business.Managers.Interfaces;

namespace TaskManagementSystem.Business.Managers;

public class CustomUserManager : UserManager<IdentityUser>, ICustomUsermanager
{

    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;


    public CustomUserManager(IUserStore<IdentityUser> store, IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<IdentityUser> passwordHasher, IEnumerable<IUserValidator<IdentityUser>> userValidators,
        IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators, ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<IdentityUser>> logger, RoleManager<IdentityRole> roleManager, IMapper mapper) :
        base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators,
        keyNormalizer, errors, services, logger)
    {
        _roleManager = roleManager;
        _mapper = mapper;
    }


    public async Task<IdentityUser> GetByIdAsync(string id)
    {
        return await FindByIdAsync(id);
    }

    public async Task DeleteAsync(IdentityUser entity)
    {
        await base.DeleteAsync(entity);
    }

    public async Task CreateUserAsync(LoginDto user, string password, string role)
    {
        var newUser = _mapper.Map<IdentityUser>(user);

        var result = await CreateAsync(newUser, password);
        if (result.Succeeded)
        {

            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

        }

        await AddToRoleAsync(newUser, role);
    }

}
