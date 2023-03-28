using ServiceManagment.Data.Enum;
using ServiceManagment.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagment.ViewModel
{
    public class CreateOrderViewModel
    {
        public int CustomerId { get; set; }
        public Product Product { get; set; }
        public Payment Payment { get; set; }
        public string WorkerId { get; set; }
    }
}
