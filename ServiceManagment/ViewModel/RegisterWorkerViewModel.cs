using ServiceManagment.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

		[Required(ErrorMessage = "Worker name cannot be empty!")]
		public string? Name { get; set; }
		public DateTime CreatedAt { get; set; }

		[Required(ErrorMessage = "Phone number cannot be empty!")]
		[RegularExpression(@"(\d{3}).?(\d{3}).?(\d{3})", ErrorMessage = "Wrong phone number!")]
		public string PhoneNumber { get; set; }
    }
}
