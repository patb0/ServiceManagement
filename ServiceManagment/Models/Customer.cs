using ServiceManagment.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagment.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [StringLength(40)]
        [Required(ErrorMessage = "Customer name cannot be empty!")]
        [RegularExpression("([a-zA-z].*[a-zA-z])", ErrorMessage = "Customer name can only have a leeters!")]
        public string Name { get; set; }

        [RegularExpression(@"\d{10}", ErrorMessage = "Wrong NIP number!")]
        public string? NIP { get; set; }
        public DateTime UserAdded { get; set; }
        public string? Description { get; set; }
        public CustomerGroup CustomerGroup { get; set; }
        public CustomerType CustomerType { get; set; }
        public Address Address { get; set; }
        public Contact Contact { get; set; }
        public ICollection<Order>? Orders { get; set; } 
    }
}
