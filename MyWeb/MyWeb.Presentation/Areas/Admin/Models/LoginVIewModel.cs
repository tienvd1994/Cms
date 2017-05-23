using System.ComponentModel.DataAnnotations;

namespace MyWeb.Presentation.Areas.Admin.Models
{
    public class LoginViewModel
    {
        public bool CheckoutAsGuest { get; set; }

        [Required]
        [Display(Name = "UserName")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string Email { get; set; }

        public bool UsernamesEnabled { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}