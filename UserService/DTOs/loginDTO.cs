using System.ComponentModel.DataAnnotations;

namespace UserService.DTOs
{
    public class loginDTO
    {
        [Display(Name ="Email Address")]
        [Required(ErrorMessage ="Email address is required")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}
