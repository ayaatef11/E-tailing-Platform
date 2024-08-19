using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebApplication1.Models.DTOS.Requests;
using WebApplication1.Models.DTOS.Responses;
using WebApplication1.Models.DTOS;
using WebApplication1.Models;
namespace newProj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthManagementController(UserManager<IdentityUser> _userManager, TokenService tokenService) : ControllerBase
    {
        

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

                var jwtToken = await tokenService.GenerateJwtToken(newUser);
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

                var jwtToken = await tokenService.GenerateJwtToken(existingUser);
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
                var result = await tokenService. VerifyToken(tokenRequest);

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
        //send email
        //forget password
        //reset password 
    }
}

