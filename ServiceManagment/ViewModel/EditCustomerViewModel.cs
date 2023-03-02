using ServiceManagment.Data.Enum;
using ServiceManagment.Models;

namespace ServiceManagment.ViewModel
{
    public class EditCustomerViewModel
    {
        public string? Name { get; set; }
        public string? NIP { get; set; }
        public DateTime UserAdded { get; set; }
        public string? Description { get; set; }
        public CustomerGroup CustomerGroup { get; set; }
        public CustomerType CustomerType { get; set; }
        public Address Address { get; set; }
        public Contact Contact { get; set; }
    }
}
