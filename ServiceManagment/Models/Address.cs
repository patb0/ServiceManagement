using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagment.Models
{
    public class Address
    {
        [Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)] //auto-increment
		public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string FlatNumber { get; set; } 
        public string PostalCode { get; set; }
    }
}
