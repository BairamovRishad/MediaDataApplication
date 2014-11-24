using System.ComponentModel.DataAnnotations;

namespace MediaDataApplication.AspNetMvcClient.Models {

    public class LoginViewModel {
        [Required, StringLength(30)]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required, StringLength(30)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel {
        [Required, StringLength(30)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required, StringLength(30)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required, StringLength(30)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }

}