using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagment.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Phone]
        public string? SecondPhoneNumber { get; set; }
    }
}
