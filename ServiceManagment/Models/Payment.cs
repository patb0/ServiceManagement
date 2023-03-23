using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagment.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [RegularExpression(@"(\d).?(\d{0,2})", ErrorMessage = "Wrong amount to pay!")]
        public double? ToPay { get; set; }
        public double? Paid { get; set; } = 0;
        public ICollection<Service>? Services { get; set; }
    }
}
