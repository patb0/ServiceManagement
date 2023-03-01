using System.ComponentModel.DataAnnotations;

namespace ServiceManagment.Models
{
    public class Equipment
    {
        [Key]
        public int Id { get; set; }
        public string Fault { get; set; }
        public string Description { get; set; }
        public Product? Product { get; set; }
    }
}
