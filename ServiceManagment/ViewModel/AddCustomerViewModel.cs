﻿using ServiceManagment.Data.Enum;
using ServiceManagment.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagment.ViewModel
{
    public class AddCustomerViewModel
    {
        [StringLength(40)]
        [Required(ErrorMessage = "Customer name cannot be empty!")]
        [RegularExpression("([a-zA-z].*[a-zA-z])", ErrorMessage = "Customer name can only have a leeters!")]
        public string Name { get; set; }


        [RegularExpression(@"\d{10}", ErrorMessage = "Wrong NIP number!")]
        public string? NIP { get; set; }
        public DateTime UserAdded { get; set; }

        [StringLength(100)]
        public string? Description { get; set; }
        public CustomerGroup CustomerGroup { get; set; }
        public CustomerType CustomerType { get; set; }
        public AddressViewModel Address { get; set; }
        public ContactViewModel Contact { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public string WorkerId { get; set; }
    }
}
