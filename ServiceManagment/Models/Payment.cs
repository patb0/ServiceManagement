using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagment.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(1000)]
        public double? ToPay { get; set; }
        public double? Paid { get; set; } = 0;
    }
}
