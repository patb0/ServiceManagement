using Microsoft.AspNetCore.Mvc;
using ServiceManagment.Interfaces;
using ServiceManagment.Models;

namespace ServiceManagment.Controllers
{
    public class CustomerController : Controller
    {
        public CustomerController(ICustomerRepository customerRepository)
        {

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        
        //public async Task<IActionResult> Add(Customer customer)
        //{

        //}
    }
}
