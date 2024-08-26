using API.Dtos;

using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Models;
using OrdersAndItemsService.API.Errors;
using OrdersAndItemsService.Controllers;
using UserService.DTOs;
using UserService.services;
using WebApplication1.Models.DTOS.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json.Linq;

namespace API.Controllers
{
    public class AccountController(UserManager<AppUser> _userManager, IMapper _mapper,
            SignInManager<AppUser> _signInManager, IAuthService _authService,IEmailService _emailService) : BaseApiController
    {


        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return BadRequest("Invalid email confirmation request");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest("User not found");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
                return Ok("Email confirmed successfully");
            else
                return BadRequest("Email confirmation failed");
        }


        [HttpPost("login")]
        [ProducesResponseType(typeof(AppUserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AppUserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
                return BadRequest(new RegistrationResponseDTO()
                {
                    Errors = new List<string>()
                        {
                            "Invalid login request"
                        },
                    Success = false
                });

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (result.Succeeded is false)
                return BadRequest(new RegistrationResponseDTO()
                {
                    Errors = new List<string>()
                        {
                            "Invalid login request"
                        },
                    Success = false
                });
            ///var jwtToken = await tokenService.GenerateJwtToken(existingUser);
           // return Ok(jwtToken);

            return Ok(new AppUserDto
            {
                DisplayName = user.DisplayName,
                Email = model.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }

        [HttpPost("register")]
        /*[ProducesResponseType(typeof(AppUserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]*/
        public async Task<ActionResult<AppUserDto>> Register(RegisterDto model)
        {
            if (CheckEmailExist(model.Email).Result.Value)
                return BadRequest(new RegistrationResponseDTO()
                    {
                     Errors = new List<string>()
                        {
                            "Email is already in use"
                        },
                        Success = false
                    });

            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName =model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Construct confirmation link (this is just an example, update with your actual URL)
            var confirmationLink = Url.Action("ConfirmEmail", "Account",
                new { userId = user.Id, token = token }, Request.Scheme);

            // Send email with the confirmation link (you would implement email sending here)
            await _emailService.SendEmailAsync(user.Email, "Confirm your email",
                $"Please confirm your email by clicking this link: {confirmationLink}");

            await _authService.AssignRoleToUser(user.Email, "Admin");

            if (result.Succeeded is false)
                 return BadRequest(new RegistrationResponseDTO()
                {
                    Errors = result.Errors.Select(x => x.Description).ToList(),
                    Success = false
                });
           // var jwtToken = await tokenService.GenerateJwtToken(newUser);
           //return ok(jwttoken);
            return Ok(new AppUserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<AppUserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email!);
            return Ok(new AppUserDto()
            {
                DisplayName = user!.DisplayName,
                Email = user.Email!,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<OrderAddressDto>> GetCurrentUserAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            //var user = await _userManager.FindByEmailAsync(email);
            // -- we don't use it because it load user without navigational property so I created extension method do that

            var user = await _userManager.Users.Include(x => x.Address).SingleOrDefaultAsync(u => u.Email == email);

            return Ok(_mapper.Map<Address, OrderAddressDto>(user.Address));
        }

        [Authorize]
        [HttpPut("address")]
        [ProducesResponseType(typeof(OrderAddressDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderAddressDto>> UpdateUserAddress(AddressDto updatedAddress)
        {
            var address = _mapper.Map<AddressDto, Address>(updatedAddress);

            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.Users.Include(x => x.Address).SingleOrDefaultAsync(u => u.Email == email);

            updatedAddress.Id = user.Address.Id;

            user.Address = address;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));

            return Ok(updatedAddress);
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExist(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }

        [HttpPost("logout")]
        //[ValidateAntiForgeryToken]//Anti-forgery tokens are used to prevent Cross-Site Request
                                  //Forgery (CSRF) attacks by ensuring that requests are coming from a trusted source.
        public async Task<IActionResult> Logout(string email)
        {
         /* var user=await   _userManager.FindByEmailAsync(email);
            if (user == null) return BadRequest(new ApiResponse(400));
            await _userManager.DeleteAsync(user);*/
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    return Ok();
                }
            }

            return BadRequest(new ApiResponse(500,"Error deleting account"));
        }
        //forget password
        //reset password 
        //delete account
    }
}