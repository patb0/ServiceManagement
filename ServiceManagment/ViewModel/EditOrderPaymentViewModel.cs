using System.ComponentModel.DataAnnotations;

namespace ServiceManagment.ViewModel
{
    public class EditOrderPaymentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "To pay cannot be empty. Enter '0'")]
        [RegularExpression(@"(\d).?(\d{0,2})", ErrorMessage = "Wrong amount to pay!")]
        public double? ToPay { get; set; }
        public double? Paid { get; set; }
    }
}
