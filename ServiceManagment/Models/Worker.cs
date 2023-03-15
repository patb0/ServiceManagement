using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagment.Models
{
    public class Worker : IdentityUser
    {
        public string? EmailAddress { get; set; }
        public ICollection<Customer>? Customers { get; set; }
    }
}
