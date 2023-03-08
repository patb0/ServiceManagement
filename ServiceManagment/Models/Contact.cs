using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagment.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Phone number cannot be empty!")]
        [RegularExpression(@"(\d{3}).*(\d{3}).*(\d{3})", ErrorMessage = "Wrong phone number!")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "E-mail address cannot be empty!")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Phone]
        public string? SecondPhoneNumber { get; set; }
    }
}
