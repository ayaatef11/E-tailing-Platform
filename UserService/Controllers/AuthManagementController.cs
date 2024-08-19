using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Configuration;
using WebApplication1.Models.DTOS.Requests;
using WebApplication1.Models.DTOS.Responses;
using WebApplication1.Models.DTOS;
using WebApplication1.Models;
using UserService.Data;

namespace newProj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthManagementController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly jwtConfig _jwtConfig;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IdentityContext _dbContext;
        private IConfiguration _jwtSettings;

        public AuthManagementController(UserManager<IdentityUser> userManager, IOptionsMonitor<jwtConfig> optionsMonitor
            , TokenValidationParameters tokenValidationParameters
            , IdentityContext apidbcontext)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
            _tokenValidationParameters = tokenValidationParameters;
            _dbContext = apidbcontext;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequests user)
        {

            if (ModelState.IsValid)
            {
                //check if repeated email
                var existingUser = await _userManager.FindByEmailAsync(user.Email);
                if (existingUser != null)
                {
                    return BadRequest(new RegistrationResponseDTO()
                    {
                        Errors = new List<string>()
                        {
                            "Email is already in use"
                        },
                        Success = false
                    });
                }

                var newUser = new IdentityUser()
                {
                    Email = user.Email,
                    UserName = user.UserName
                };

                var isCreated = await _userManager.CreateAsync(newUser, user.Password);
                if (!isCreated.Succeeded)
                {
                    return BadRequest(new RegistrationResponseDTO()
                    {
                        Errors = isCreated.Errors.Select(x => x.Description).ToList(),
                        Success = false
                    });
                }

                var jwtToken = await GenerateJwtToken(newUser);
                return Ok(jwtToken);
            }
            //if not valid
            return BadRequest(new RegistrationResponseDTO()
            {
                Errors = new List<string>()
                {
                    "Invalid payload"
                },
                Success = false
            });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequests user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);
                if (existingUser == null)
                {
                    return BadRequest(new RegistrationResponseDTO()
                    {
                        Errors = new List<string>()
                        {
                            "Invalid login request"
                        },
                        Success = false
                    });
                }

                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

                if (!isCorrect)
                {
                    return BadRequest(new RegistrationResponseDTO()
                    {
                        Errors = new List<string>()
                        {
                            "Invalid login request"
                        },
                        Success = false
                    });
                }

                var jwtToken = await GenerateJwtToken(existingUser);
                return Ok(jwtToken);
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
        private async Task<AuthResult> GenerateJwtToken(IdentityUser user)
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

        private async Task<AuthResult> VerifyToken(TokenRequests tokenRequest)
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
                _dbContext._refreshTokens.Update(storedToken);
                await _dbContext.SaveChangesAsync();

                var dbUser = await _userManager.FindByIdAsync(storedToken.userId);
                return await GenerateJwtToken(dbUser);

            }
            catch (Exception ex)
            {
                return null;
            }
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
        //send email
        //forget password
        //reset password 
    }
}

