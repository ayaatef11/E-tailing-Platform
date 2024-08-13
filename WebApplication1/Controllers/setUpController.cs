
namespace newProj.Controllers
{
    [Route("api/Roles")]
    [ApiController]
    public class setUpController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<setUpController> _logger;

        public setUpController(DbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<setUpController> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

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

        [HttpGet]
        [Route("GetAllUsers")]
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
    }
}
