

namespace UserService.DTOs
{
    public class RegisterDto
    {
        [Display(Name ="Email address")]
        [Required(ErrorMessage ="Email address is required")]
        public string Email{ get; set; }
        [Required]
       // [DataType(DataType.Password)]
        public string Password { get; set; }
       // public string userName { get; set; } = string.Empty;

        [Display(Name ="Confirm password")]
        [Required(ErrorMessage ="confirm password is required")]
        //[DataType(DataType.Password)]//it is of type string in the identity user
        [Compare("Password",ErrorMessage ="Password do not match")]
        public string confirmPassword { get; set; } =string.Empty;
        public string DisplayName {  get; set; }=string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
