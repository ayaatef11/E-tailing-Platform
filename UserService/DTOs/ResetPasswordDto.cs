namespace UserService.DTOs
{
    public class ResetPasswordDto
    {
            public string Email { get; set; }=string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }=string.Empty;

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }=string.Empty;

            public string Token { get; set; } = string.Empty;
        }

    }

