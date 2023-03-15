using System.ComponentModel.DataAnnotations;

namespace ServiceManagment.ViewModel
{
    public class LoginWorkerViewModel
    {
        [Required(ErrorMessage = "Email address cannot be empty!")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password cannot be empty!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
