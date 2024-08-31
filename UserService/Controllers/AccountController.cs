using API.Dtos;
using Models;
using OrdersAndItemsService.Controllers;
using UserService.DTOs;
using UserService.services;
using Models.DTOS.Responses;
using API.Errors;


namespace API.Controllers
{
    public class AccountController(UserManager<AppUser> _userManager, IMapper _mapper,
            SignInManager<AppUser> _signInManager, 
            IEmailService _emailService, /*IConfigurationSection _jwtSettings,*/ITokenService _tokenService) : BaseApiController
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
        public async Task<ActionResult<AppUserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
                return BadRequest(new RegistrationResponseDTO()
                {
                    Errors =
                        ["Invalid login request"],
                    Success = false
                });

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (result.Succeeded is false)
                return BadRequest(new RegistrationResponseDTO()
                {
                    Errors =
                        ["Invalid login request"],
                    Success = false
                });
           
            if (await _userManager.GetTwoFactorEnabledAsync(user))
            {
               
                var twoFactorCode = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                await _emailService.SendEmailAsync(user.Email, "Your Two-Factor Code", $"Your code is {twoFactorCode}");

                
                return Ok(new { RequiresTwoFactor = true });
            }
            ///var jwtToken = await tokenService.GenerateJwtToken(existingUser);
            // return Ok(jwtToken);
            /* var signinCredentials = GetSigningCredentials();
           var Claims = GetClaims(user);
           var tokenOptions = GenerateTokenOptions(signinCredentials, Claims);
           var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);*/
            return Ok(new AppUserDto
            {
                DisplayName = user.DisplayName,
                Email = model.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
        }
        
        //[ServiceFilter(typeof(ApiKeyAuthenticationFilter))]
        [HttpPost("register")]       
        public async Task<ActionResult<AppUserDto>> Register(RegisterDto model)
        {
            if (CheckEmailExist(model.Email).Result.Value)
                return BadRequest(new RegistrationResponseDTO()
                    {
                     Errors =
                        ["Email is already in use"],
                        Success = false
                    });

            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName =/*model.userName,//*/model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
           var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationLink = Url.Action("ConfirmEmail", "Account",
                new { userId = user.Id, token }, Request.Scheme);
      /*      var message = new Messager(new List<string> { user.Email }, "Confirm your email" ,$"Please confirm your email by clicking this link: {confirmationLink}"){ };
           await _emailService.SendEmail(message);*/
            await _emailService.SendEmailAsync(user.Email, "Confirm your email",
                $"Please confirm your email by clicking this link: {confirmationLink}");

            await _tokenService.AssignRoleToUser(user.Email, "Admin");

            if (result.Succeeded is false)
                 return BadRequest(new RegistrationResponseDTO()
                {
                    Errors = result.Errors.Select(x => x.Description).ToList(),
                    Success = false
                });

             //var jwtToken = await _tokenService.GenerateJwtToken(user);

            return Ok(new AppUserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager),
                //JwtToken = jwtToken
            });
        }


        [HttpPost("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);//get the user id from the current user claims
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
        [HttpPost("forgetPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(401,"Not allowed"));
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)  return BadRequest(new ApiResponse(400, "Not found"));
            
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetLink = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);

            await _emailService.SendEmailAsync(user.Email!, "Reset Password", $"Please reset your password by clicking here: <a href='{resetLink}'>link</a>");

            return Ok();
        }


        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(400));
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest(new ApiResponse(400));
            

            // Reset the user's password
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Ok(model);
        }

        [HttpPost("VerifyTwoFactorCode")]
        public async Task<IActionResult> VerifyTwoFactorCode([FromBody] TwoFactorDto model)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return Unauthorized("Unable to load user.");
            }

            var result = await _signInManager.TwoFactorSignInAsync("Email", model.Code, model.RememberMe, model.RememberClient);
            if (result.Succeeded)
            {
                return Ok("Login successful");
            }
            else if (result.IsLockedOut)
            {
                return Unauthorized("User account locked out.");
            }
            else
            {
                return Unauthorized("Invalid two-factor authentication code");
            }
        }
        [HttpGet("changeEmail")]
        public async Task<IActionResult> ChangeEmail(string newEmail)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var confirmationLink = Url.Action("ConfirmEmailChange", "Account",
                new { userId = user.Id, email = newEmail, code = encodedToken }, Request.Scheme);

            // Send email with confirmation link
            await _emailService.SendEmailAsync(newEmail, "Confirm your email change",
                $"Please confirm your email change by clicking this link: {confirmationLink}");

            return Ok("Confirmation email sent. Please check your new email address.");
        }

        [HttpGet("ConfirmEmailChange")]
        public async Task<IActionResult> ConfirmEmailChange(string userId, string email, string code)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(code))
            {
                return BadRequest("Invalid parameters.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var result = await _userManager.ChangeEmailAsync(user, email, decodedCode);
            if (!result.Succeeded)
            {
                return BadRequest("Error changing email.");
            }

            var setUserNameResult = await _userManager.SetUserNameAsync(user, email);
            if (!setUserNameResult.Succeeded)
            {
                return BadRequest("Error updating username.");
            }

            return Ok("Email change confirmed.");
        }


        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<AppUserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email!);
            return Ok(new AppUserDto()
            {
                DisplayName = user!.DisplayName,
                Email = user.Email!,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
        }

        [Authorize]
        [HttpGet("Address")]
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
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);// It logs out the user by removing their authentication cookies,
                                                                                              // //effectively ending their session and invalidating their current authentication.
            return Ok();
        }
    }
}