using System.ComponentModel.DataAnnotations;

namespace ServiceManagment.ViewModel
{
	public class AddressViewModel
	{
		[StringLength(20)]
		[Required(ErrorMessage = "City cannot be empty!")]
		[RegularExpression("([a-zA-z].*[a-zA-z])", ErrorMessage = "City can have only leeters!")]
		public string City { get; set; }

		[Required(ErrorMessage = "Street cannot be empty!")]
		[RegularExpression("([a-zA-z0-9].*[a-zA-z0-9])", ErrorMessage = "Wrong street name!")]
		public string Street { get; set; }

		[Required(ErrorMessage = "Flat number cannot be empty!")]
		[RegularExpression(@"([0-9]{1,2}).?(.{1,3})", ErrorMessage = "Wrong flat number!")]
		public string FlatNumber { get; set; }

		[Required(ErrorMessage = "Postal code cannot be empty!")]
		[RegularExpression(@"((\d{2})+)-(\d{3})", ErrorMessage = "Wrong postal code!")]
		public string PostalCode { get; set; }
	}
}
