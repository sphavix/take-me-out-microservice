using System.Security.Cryptography;
using System.Text;
using Dinner.Api.Models;
using Dinner.Api.Models.Dtos;
using Dinner.Api.Persistence;
using Dinner.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dinner.Api.Controllers;

public class AccountsController(
    ApplicationDbContext context,
    ITokenService tokenService) : BaseApiController
{
    // POST
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto dto)
    {

        if(await UserExists(dto.Username))
        {
            return BadRequest("Username is taken");
        }
        using var hmac = new HMACSHA512();

        var user = new ApplicationUser
        {
            UserName = dto.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
            PasswordSalt = hmac.Key
        };
        
        context.Users.Add(user);
        await context.SaveChangesAsync();

        return new UserDto
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user)
        };
    }

    // POST:: accounts/login
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto dto)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(x => x.UserName == dto.Username.ToLower());

        // Check if user is null
        if (user == null) 
            return Unauthorized("Invalid username");

        using var hmac = new HMACSHA512(user.PasswordSalt); // use the same key as when the password was hashed
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)); // compute the hash of the password

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
        }
        return new UserDto
        {
            Username = dto.Username.ToLower(),
            Token = tokenService.CreateToken(user)
        };
    }

    private async Task<bool> UserExists(string username)
    {
        return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
    }

}