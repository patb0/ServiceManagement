using ServiceManagment.Data.Enum;
using ServiceManagment.Models;

namespace ServiceManagment.ViewModel
{
    public class DetailOrderViewModel
    {
        public int Id { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime OrderAdded { get; set; }
        public Customer Customer { get; set; }
        public Product Product { get; set; }
        public Payment Payment { get; set; }
        public string? WorkerId { get; set; }
        public string? WorkerName { get; set; }
    }
}
