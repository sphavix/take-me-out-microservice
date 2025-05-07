using Dinner.Api.Models;

namespace Dinner.Api.Services
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user);
    }
}