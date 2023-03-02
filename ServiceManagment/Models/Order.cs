using ServiceManagment.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagment.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime OrderAdded { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
