using ServiceManagment.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagment.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? NIP { get; set; }
        public DateTime UserAdded { get; set; }
        public string? Description { get; set; }
        public CustomerGroup CustomerGroup { get; set; }
        public CustomerType CustomerType { get; set; }
        public Address Address { get; set; }
        public Contact Contact { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public string? WorkerId { get; set; }
        public Worker Worker { get; set; }
    }
}
