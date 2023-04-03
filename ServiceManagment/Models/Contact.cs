using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagment.Models
{
    public class Contact
    {
        [Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string? SecondPhoneNumber { get; set; }
    }
}
