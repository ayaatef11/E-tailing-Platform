
using Microsoft.AspNetCore.Identity;
using WebApplication1.Models;

namespace Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager);
    }
}