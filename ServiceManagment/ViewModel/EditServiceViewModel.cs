using System.ComponentModel.DataAnnotations;

namespace ServiceManagment.ViewModel
{
    public class EditServiceViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name cannot be empty!")]
        public string Name { get; set; }

        [RegularExpression(@"(\d).?(\d{0,2})", ErrorMessage = "Wrong amount to pay!")]
        public double? Price { get; set; }
    }
}
