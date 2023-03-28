using ServiceManagment.Data.Enum;
using ServiceManagment.Models;

namespace ServiceManagment.ViewModel
{
	public class HistoryCustomerViewModel
	{
		public double? ToPay { get; set; }
		public double? Paid { get; set; }
		public IEnumerable<Order> Orders { get; set; }
    }
}
