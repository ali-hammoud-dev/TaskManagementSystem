using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagementSystem.Business.DTOs;
using TaskManagementSystem.Business.Managers.Interfaces;

namespace TaskManagementSystem.Controllers;

[ApiController]
[Authorize]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly ICustomUsermanager _userManager;

    public UserController(ICustomUsermanager userManager) => _userManager = userManager;

    [Authorize(Roles = "Admin")]
    [HttpDelete]
    [Route("DeleteUser/{userId}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        await _userManager.DeleteUserArrangeAsync(userId);
        return Ok("User and associated records deleted.");
    }


    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser([FromBody] LoginDto user, string role)
    {
        await _userManager.CreateUserAsync(user, user.Password, role);
        return Ok("User Created");
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("LoginUser")]
    public async Task<IActionResult> LoginUser([FromBody] LoginDto loginUser)
    {
        var user = await _userManager.FindByNameAsync(loginUser.Username);

        if (user != null && await _userManager.CheckPasswordAsync(user, loginUser.Password))
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("UserId",user.Id)
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.
                GetBytes("super secret key"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(authClaims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);


            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                Expires = DateTime.Now.AddDays(1)
            });
        }

        return Unauthorized();
    }
}