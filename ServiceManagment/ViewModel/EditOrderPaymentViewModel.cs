using ServiceManagment.Data.Enum;
using ServiceManagment.Models;
using System.ComponentModel.DataAnnotations;

namespace ServiceManagment.ViewModel
{
    public class EditOrderPaymentViewModel
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }

        [StringLength(40)]
        [Required(ErrorMessage = "Name cannot be empty")]
        public string Name { get; set; }

        [RegularExpression(@"(\d).?(\d{0,2})", ErrorMessage = "Wrong amount to pay!")]
        public double Price { get; set; }
        public ServiceStatus Status { get; set; }
        public double? ToPay { get; set; }
        public double? Paid { get; set; }
        public IEnumerable<Service>? Services { get; set; }
    }
}
