using ServiceManagment.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace ServiceManagment.ViewModel
{
	public class ProductViewModel
	{
		[StringLength(20)]
		[Required(ErrorMessage = "Producer name cannot be empty!")]
		public string ProducerName { get; set; }
        public ProductType ProductType { get; set; }

        [StringLength(10)]
		[Required(ErrorMessage = "Model cannot be empty!")]
		public string Model { get; set; }

		[StringLength(15)]
		[Required(ErrorMessage = "Serial number cannot be empty!")]
		public string SerialNumber { get; set; }

		[StringLength(100)]
		[Required(ErrorMessage = "Fault cannot be empty!")]
		public string Fault { get; set; }
		public string? Description { get; set; }
	}
}
