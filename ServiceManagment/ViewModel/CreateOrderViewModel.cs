using ServiceManagment.Data.Enum;
using ServiceManagment.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagment.ViewModel
{
    public class CreateOrderViewModel
    {
        public OrderStatus OrderStatus { get; set; }
        public DateTime OrderAdded { get; set; }
        public int CustomerId { get; set; }
        public Product Product { get; set; }
        public Payment Payment { get; set; }
    }
}
