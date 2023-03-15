using System.ComponentModel.DataAnnotations;

namespace ServiceManagment.ViewModel
{
    public class RegisterWorkerViewModel
    {
        [Required(ErrorMessage = "Email address cannot be empty!")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password cannot be empty!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password cannot be empty!")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords are different!")]
        public string ConfirmPassword { get; set; }
    }
}
