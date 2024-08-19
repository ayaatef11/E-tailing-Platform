using System.ComponentModel.DataAnnotations;

namespace BookShop.ViewModel
{
    public class RegiserDTO
    {
        [Display(Name ="Email address")]
        [Required(ErrorMessage ="Email address is required")]
        public string EmailAddressd { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Display(Name ="Confirm password")]
        [Required(ErrorMessage ="confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("password",ErrorMessage ="Password do not match")]
        public string confirmPassword { get; set; } 
    }
}
