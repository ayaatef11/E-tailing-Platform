using Microsoft.AspNetCore.Identity;
using Models;
using Models.DTOS.Requests;
using UserService.DTOs.Configuration;

namespace UserService.services
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager);
        // void createRole(string roleName);
        Task<bool> AssignRoleToUser(string userEmail, string roleName);
        TokenModel GenerateTokens(ClaimsIdentity identity);
        string GenerateAccessToken(ClaimsIdentity identity);

        JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims);
        Task<AuthResult> GenerateJwtToken(AppUser user);
        Task<AuthResult> VerifyToken(TokenRequests tokenRequest);
    }
}
