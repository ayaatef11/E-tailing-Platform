using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Configuration;
using WebApplication1.Models;
using WebApplication1.Models.DTOS.Requests;
using WebApplication1.Models.DTOS.Responses;
using WebApplication1.Models.DTOS;
using System.Web.Mvc;
using UserService.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public class TokenService(IdentityDbContext _dbContext, jwtConfig _jwtConfig, IConfiguration _jwtSettings,TokenValidationParameters _tokenValidationParameters, )
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

    public string GenerateAccessToken(ClaimsIdentity identity)
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

    public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken(
            issuer: _jwtSettings["validIssuer"],
            audience: _jwtSettings["validAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings["expiryInMinutes"])),
            signingCredentials: signingCredentials);
        return tokenOptions;
    }

    private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
    {
        var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToLocalTime();
        return dateTimeVal;

    }
    private string RandomString(int length)
    {

        var random = new Random();
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(x => x[random.Next(x.Length)]).ToArray());
    }

    private List<Claim> GetClaims(AppUser user)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email)
            };
        return claims;
    }


    [HttpPost]
    [Route("RefreshToken")]

    public async Task<IActionResult> RefreshToken([FromBody] TokenRequests tokenRequest)
    {
        if (ModelState.IsValid)
        {
            var result = await VerifyToken(tokenRequest);

            if (result == null)
            {
                return BadRequest(new RegistrationResponseDTO()
                {
                    Errors = new List<string>()
                        {
                            "invalid tokens "
                        },
                    Success = false
                });
            }
            return Ok(result);
        }

        return BadRequest(new RegistrationResponseDTO()
        {
            Errors = new List<string>()
            {
                    "Invalid payload"
                },
            Success = false
        });
    }
    public async Task<AuthResult> GenerateJwtToken(IdentityUser user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email,  user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
            Expires = DateTime.UtcNow.AddSeconds(30),//it is preferable to be from 5 to 10 minutes 
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token);
        var refreshToken = new RefreshTokens()
        {
            jwtId = token.Id,
            isUsed = false,
            isRevorked = false,
            userId = user.Id,
            AddedDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddMonths(6),
            token = RandomString(35) + Guid.NewGuid
        };

        await _dbContext.SaveChangesAsync();
        return new AuthResult()
        {
            Token = jwtToken,
            Success = true,
            RefreshToken = refreshToken.token
        };
    }

    public async Task<AuthResult> VerifyToken(TokenRequests tokenRequest)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        try
        {
            //validation 1
            var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters, out var validatedToken);
            //validation 2
            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                if (result == false) return null;

            }

            //validation 3

            var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);
            if (expiryDate > DateTime.UtcNow)
            {
                return new AuthResult()
                {
                    Success = false,
                    Errors = new List<string>()
                        {
                            "Token has not yet expired"
                        }
                };
            }
            //validatipn 4
            var storedToken = await _dbContext._refreshTokens.FirstOrDefaultAsync(x => x.token == tokenRequest.Token);

            if (storedToken == null)
            {
                return new AuthResult()
                {
                    Success = false,
                    Errors = new List<string>()
                        {
                            "Token doesn't exist "
                        }
                };
            }
            //validation 5

            if (storedToken.isUsed)
            {
                return new AuthResult()
                {
                    Success = false,
                    Errors = new List<string>()
                        {
                            "token hasn been used  "
                        }
                };
            }
            //vlaidation 6
            if (storedToken.isRevorked)
            {
                return new AuthResult()
                {
                    Success = false,
                    Errors = new List<string>()
                        {
                            "Token has been revoked"
                        }
                };
            }
            //validation 7
            var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            if (storedToken.jwtId != jti)
            {
                return new AuthResult()
                {
                    Success = false,
                    Errors = new List<string>()
                    {
                        "token doesn't match"
                        }
                };
            }
            //update current token

            storedToken.isUsed = true;
            _dbContext._re.Update(storedToken);
            await _dbContext.SaveChangesAsync();

            var dbUser = await _userManager.FindByIdAsync(storedToken.userId);
            return await GenerateJwtToken(dbUser);

        }
        catch (Exception ex)
        {
            return null;
        }
    }




}
