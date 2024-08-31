using UserService.Data;
using Models;
using Models.DTOS.Requests;
using UserService.DTOs.Configuration;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace UserService.services;
public class TokenService(UserManager<AppUser> _userManager, IdentityContext _dbContext, jwtConfig _jwtConfig, IConfiguration _configuration, RoleManager<IdentityRole> _roleManager,TokenValidationParameters _tokenValidationParameters):ITokenService
{
    private readonly string _accessTokenSecret = "your_access_token_secret"; 
    private readonly string _refreshTokenSecret = "your_refresh_token_secret";
    public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager)
    {
        //all the informatios of the user meaning the identity of the user
        var authClaims = new List<Claim>()
            {
                new (ClaimTypes.GivenName, user.UserName!),
                new (ClaimTypes.Email, user.Email),
                new (ClaimTypes.NameIdentifier,user.Id)
            };

        var userRoles = await userManager.GetRolesAsync(user);

        foreach (var roleName in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, roleName));
            // Find the role by name
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                // Check if the role already has the claim to avoid duplication
                var existingClaims = await _roleManager.GetClaimsAsync(role);
                if (!existingClaims.Any(c => c.Type == ClaimTypes.Role && c.Value == roleName))
                {
                    // Add the claim to the role
                    var roleClaimResult = await _roleManager.AddClaimAsync(role, new Claim(ClaimTypes.Role, roleName));
                    if (!roleClaimResult.Succeeded)
                    {
                        throw new Exception($"Failed to add claim to role: {roleName}");
                    }
                }
            }
                }
        //save claims to database

        foreach (var claim in authClaims)
        {
            var result = await userManager.AddClaimAsync(user, claim);
        }

        //this command  generates a random secret key : node -e "console.log(require('crypto').randomBytes(32).toString('hex'))"
        var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]!));//create the auth key from the jwt secret key
        var token = new JwtSecurityToken(//create the token contains the identity of the user and the configurations of the jwt token
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:DurationInDays"]!)),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)//add the credentials of the user
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async void createRole(string roleName)
    {////note very important: staic function can't call variables defined in the priimary constructor
        var roleExist = await _roleManager.RoleExistsAsync(roleName);
        if (roleExist) return;
        await _roleManager.CreateAsync(new IdentityRole(roleName));
    }
    public async Task<bool> AssignRoleToUser(string userEmail, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user == null)
            return false;

        var roleExist = await _roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
            createRole(roleName);
        if (await _userManager.IsInRoleAsync(user, roleName))
            return true;

        var result = await _userManager.AddToRoleAsync(user, roleName);
        
        if (!result.Succeeded)
            return false;

        return true;
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]!);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }
    private async Task<IList<Claim>> GetClaims(AppUser user)
    {
        return await _userManager.GetClaimsAsync(user);


    }
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
        return Guid.NewGuid().ToString();
    }

    public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken(
            issuer: _configuration["validIssuer"],
            audience: _configuration["validAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["expiryInMinutes"])),
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


    public async Task<AuthResult> GenerateJwtToken(AppUser user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        //var jwtConfig = Configuration.GetSection("JwtConfig").Get<jwtConfig>();
        var key = Encoding.ASCII.GetBytes(_configuration["Secret"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email,  user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
            Expires = DateTime.UtcNow.AddSeconds(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token);
        var refreshToken = new RefreshTokens()
        {
            JwtId = token.Id,
            IsUsed = false,
            IsRevorked = false,
            AppUserId = user.Id,
            AddedDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddMonths(6),
            Token = RandomString(35) + Guid.NewGuid
        };

        await _dbContext.SaveChangesAsync();
        return new AuthResult()
        {
            Token = jwtToken,
            Success = true,
            RefreshToken = refreshToken.Token
        };
    }

    public async Task<AuthResult> VerifyToken(TokenRequests tokenRequest)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

            var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters, out var validatedToken);
          
            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                if (result == false) return null;

            }

           

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
            var storedToken = await _dbContext._refreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.Token);

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

            if (storedToken.IsUsed)
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
            
            if (storedToken.IsRevorked)
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
            var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            if (storedToken.JwtId != jti)
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
            storedToken.IsUsed = true;
            object value = _dbContext._refreshTokens.Update(storedToken);
            await _dbContext.SaveChangesAsync();

            var dbUser = await _userManager.FindByIdAsync(storedToken.AppUserId);
            return await GenerateJwtToken(dbUser);

        }

  
}





