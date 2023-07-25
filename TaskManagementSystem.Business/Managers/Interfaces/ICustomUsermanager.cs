using Microsoft.AspNetCore.Identity;
using TaskManagementSystem.Business.DTOs;

namespace TaskManagementSystem.Business.Managers.Interfaces;

public interface ICustomUsermanager
{
    Task DeleteAsync(IdentityUser entity);

    Task<IdentityUser> GetByIdAsync(string id);

    Task CreateUserAsync(LoginDto user, string password, string Role);

    Task DeleteUserArrangeAsync(string userId);

    Task<IdentityUser> FindByNameAsync(string name);

    Task<bool> CheckPasswordAsync(IdentityUser user, string password);

    Task<IList<string>> GetRolesAsync(IdentityUser user);

}
