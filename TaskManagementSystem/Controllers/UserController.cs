﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagementSystem.Business.DTOs;
using TaskManagementSystem.Business.Managers;

namespace TaskManagementSystem.Controllers;

[ApiController]
[Authorize]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly CustomUserManager _userManager;

    public UserController(CustomUserManager userManager)
    {
        _userManager = userManager;
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("DeleteUser/{userId}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var user = await _userManager.GetByIdAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        await _userManager.DeleteAsync(user);

        return Ok(new { Message = "User deleted successfully." });
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
                token = tokenHandler.WriteToken(token)
            });
        }

        return Unauthorized();
    }


    [HttpPost]
    [AllowAnonymous]
    public async Task CreateUser([FromBody] LoginDto user, string password, string role)
    {
        await _userManager.CreateUserAsync(user, password, role);
    }

}
