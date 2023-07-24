using Microsoft.AspNetCore.Identity;
using TaskManagementSystem.Business.DTOs;

namespace TaskManagementSystem.Business.Managers.Interfaces;

public interface ICustomUsermanager
{
    Task DeleteAsync(IdentityUser entity);

    Task<IdentityUser> GetByIdAsync(string id);

    Task CreateUserAsync(LoginDto user, string password, string Role);

}
