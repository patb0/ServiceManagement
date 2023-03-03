using ServiceManagment.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace ServiceManagment.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public ProductType ProductType { get; set; }
        public string ProducerName { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public string Fault { get; set; }
        public string? Description { get; set; }
    }
}