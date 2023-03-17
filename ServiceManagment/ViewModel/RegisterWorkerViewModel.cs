using System.ComponentModel.DataAnnotations;

namespace ServiceManagment.ViewModel
{
    public class RegisterWorkerViewModel
    {
        [Required(ErrorMessage = "Email address cannot be empty!")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password cannot be empty!")]
        [StringLength(20, ErrorMessage = "{0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password cannot be empty!")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords are different!")]
        public string ConfirmPassword { get; set; }
    }
}
