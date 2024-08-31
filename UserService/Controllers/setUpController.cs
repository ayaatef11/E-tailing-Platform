

using Models.DTOS.Requests;
using Models.DTOS.Responses;
using UserService.services;

namespace Controllers
{
    [Route("api/Roles")]
    [ApiController]
    public class SetUpController( UserManager<IdentityUser> _userManager, RoleManager<IdentityRole> _roleManager, ILogger<SetUpController> _logger,IPhotoService _cloudinaryService,TokenService tokenService)
 : ControllerBase
    {
  
        [HttpGet]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string name)
        {
            // Check if the role exists
            var roleExist = await _roleManager.RoleExistsAsync(name);
            if (!roleExist)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(name));

                if (roleResult.Succeeded)
                {
                    _logger.LogInformation($"The Role {name} has been added successfully");
                    return Ok(new
                    {
                        result = $"The role {name} has been added successfully"
                    });
                }
                else
                {
                    _logger.LogInformation($"The Role {name} has not been added successfully");
                    return Ok(new
                    {
                        result = $"The role {name} has not been added successfully"
                    });
                }
            }

            return BadRequest(new { error = "Role already exists" });
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }

        [HttpPost]
        [Route("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogInformation($"The user with the email {email} doesn't exist");
                return BadRequest(new
                {
                    error = "User does not exist"
                });
            }

            // Check if the role exists
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                _logger.LogInformation($"The role {roleName} does not exist");
                return BadRequest(new
                {
                    error = "Role does not exist"
                });
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);

            // Check if the user is assigned to the role successfully
            if (result.Succeeded)
            {
                return Ok(new
                {
                    result = $"The user {email} has been added to the role {roleName} successfully"
                });
            }
            else
            {
                _logger.LogInformation($"The user {email} was not able to be added to the role {roleName}");
                return BadRequest(new
                {
                    error = $"The user {email} was not able to be added to the role {roleName}"
                });
            }
        }

        // Add a method to remove a user from a role
        [HttpPost]
        [Route("RemoveUserFromRole")]
        public async Task<IActionResult> RemoveUserFromRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogInformation($"The user with the email {email} doesn't exist");
                return BadRequest(new
                {
                    error = "User does not exist"
                });
            }

            // Check if the role exists
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                _logger.LogInformation($"The role {roleName} does not exist");
                return BadRequest(new
                {
                    error = "Role does not exist"
                });
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            // Check if the user is removed from the role successfully
            if (result.Succeeded)
            {
                return Ok(new
                {
                    result = $"The user {email} has been removed from the role {roleName} successfully"
                });
            }
            else
            {
                _logger.LogInformation($"The user {email} was not able to be removed from the role {roleName}");
                return BadRequest(new
                {
                    error = $"The user {email} was not able to be removed from the role {roleName}"
                });
            }
        }
        [HttpGet("uploadfiles")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var result = await _cloudinaryService.UploadImageAsync(file);
            return Ok(result);
        }

        [HttpGet("RefreshTokens")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequests tokenRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await tokenService.VerifyToken(tokenRequest);

                if (result == null)
                {
                    return BadRequest(new RegistrationResponseDTO()
                    {
                        Errors =
                            [ "invalid tokens "],
                        Success = false
                    });
                }
                return Ok(result);
            }

            return BadRequest(new RegistrationResponseDTO()
            {
                Errors =
                [ "Invalid payload" ],
                Success = false
            });
        }

    }
}
