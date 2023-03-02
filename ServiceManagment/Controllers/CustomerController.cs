using Microsoft.AspNetCore.Mvc;
using ServiceManagment.Interfaces;
using ServiceManagment.Models;

namespace ServiceManagment.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _customerRepository.GetAllCustomers();
            return View(customers);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Customer customer)
        {
            customer.UserAdded = DateTime.Now;
            _customerRepository.Add(customer);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(int id)
        {
            var customerDetail = await _customerRepository.GetCustomerById(id);
            return View(customerDetail);
        }

        public async Task<IActionResult> Edit(int id)
        {

            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            var customerToDelete = await _customerRepository.GetCustomerById(id);
            if (customerToDelete != null)
            {
                _customerRepository.Delete(customerToDelete);
                return RedirectToAction("Index");
            }
            return View("Index");
        }
    }
}
