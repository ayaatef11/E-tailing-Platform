using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Models;
public class TokenService
{
    private readonly string _accessTokenSecret = "your_access_token_secret"; // Use a secure key
    private readonly string _refreshTokenSecret = "your_refresh_token_secret"; // Use a secure key

    public TokenModel GenerateTokens(ClaimsIdentity identity)
    {
        var accessToken = GenerateAccessToken(identity);
        var refreshToken = GenerateRefreshToken();

        return new TokenModel
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiration = DateTime.UtcNow.AddMinutes(30), // Access token expiry
            RefreshTokenExpiration = DateTime.UtcNow.AddMonths(1) // Refresh token expiry
        };
    }

    private string GenerateAccessToken(ClaimsIdentity identity)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Convert.FromBase64String(_accessTokenSecret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = identity,
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        // Generate a unique refresh token (e.g., GUID)
        return Guid.NewGuid().ToString();
    }

}
