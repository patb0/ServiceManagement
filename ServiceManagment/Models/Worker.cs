using Azure.Identity;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagment.Models
{
    public class Worker : IdentityUser
    {
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<Customer>? Customers { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
