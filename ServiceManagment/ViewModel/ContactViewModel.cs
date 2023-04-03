using System.ComponentModel.DataAnnotations;

namespace ServiceManagment.ViewModel
{
	public class ContactViewModel
	{
		[Required(ErrorMessage = "Phone number cannot be empty!")]
		[RegularExpression(@"(\d{3})[-]?(\d{3})[-]?(\d{3})", ErrorMessage = "Wrong phone number!")]
		public string PhoneNumber { get; set; }

		[Required(ErrorMessage = "E-mail address cannot be empty!")]
		[RegularExpression(@"(([a-zA-Z0-9\\_\\-\\.]+)@([a-zA-Z0-9]+).(.+))", ErrorMessage = "Wrong email address!")]
		public string EmailAddress { get; set; }

		[RegularExpression(@"(\d{3})[-]?(\d{3})[-]?(\d{3})", ErrorMessage = "Wrong phone number!")]
		public string? SecondPhoneNumber { get; set; }
	}
}
