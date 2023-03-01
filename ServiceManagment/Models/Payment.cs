using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagment.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public double AllPayments { get; set; }
        public double OverduePayments { get; set; }
        public double CompletedPayments { get; set; }
    }
}
