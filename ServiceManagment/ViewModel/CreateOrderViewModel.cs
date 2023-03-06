using ServiceManagment.Data.Enum;
using ServiceManagment.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagment.ViewModel
{
    public class CreateOrderViewModel
    {
        public OrderStatus OrderStatus { get; set; }
        public DateTime OrderAdded { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [ForeignKey("Payment")]
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }
    }
}
