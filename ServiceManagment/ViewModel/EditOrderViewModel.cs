using ServiceManagment.Data.Enum;
using ServiceManagment.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagment.ViewModel
{
    public class EditOrderViewModel
    {
        public int Id { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string CustomerName { get; set; }
        public Product Product { get; set; }
    }
}
