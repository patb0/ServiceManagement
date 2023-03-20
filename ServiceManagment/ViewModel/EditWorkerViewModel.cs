using System.ComponentModel.DataAnnotations;

namespace ServiceManagment.ViewModel
{
	public class EditWorkerViewModel
	{
        public string Id { get; set; }		
        [Required(ErrorMessage = "Email address cannot be empty!")]
		public string EmailAddress { get; set; }
		public string? Name { get; set; }
		public DateTime CreatedAt { get; set; }

		[Required(ErrorMessage = "Phone number cannot be empty!")]
		[RegularExpression(@"(\d{3}).?(\d{3}).?(\d{3})", ErrorMessage = "Wrong phone number!")]
		public string PhoneNumber { get; set; }
	}
}
