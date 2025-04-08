using System.Security.Cryptography;
using System.Text;
using Dinner.Api.Models;
using Dinner.Api.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Dinner.Api.Controllers;

public class AccountsController(ApplicationDbContext context) : BaseApiController
{
    // POST
    [HttpPost("register")]
    public async Task<ActionResult<ApplicationUser>> Register(string username, string password)
    {
        using var hmac = new HMACSHA512();

        var user = new ApplicationUser
        {
            UserName = username,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
            PasswordSalt = hmac.Key
        };
        
        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user;
    }

}