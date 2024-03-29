﻿using ServiceManagment.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagment.Models
{
    public class Service
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public double? Price { get; set; }
        public ServiceStatus Status { get; set; }

        [ForeignKey("Payment")]
        public int PaymentId { get; set; }
        public Payment? Payment { get; set; }
    }
}
