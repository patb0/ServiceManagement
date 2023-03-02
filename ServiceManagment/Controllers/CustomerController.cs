using Microsoft.AspNetCore.Mvc;
using ServiceManagment.Interfaces;
using ServiceManagment.Models;
using ServiceManagment.ViewModel;

namespace ServiceManagment.Controllers
{
    public class CustomerController : Controller
    {
        private ICustomerRepository _customerRepository;

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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _customerRepository.GetCustomerById(id);
            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditCustomerViewModel viewCustomer)
        {
            var customer = await _customerRepository.GetCustomerById(id);
            if(customer != null)
            {
                customer.Name = viewCustomer.Name;
                customer.NIP = viewCustomer.NIP;
                customer.Description = viewCustomer.Description;
                customer.CustomerGroup = viewCustomer.CustomerGroup;
                customer.CustomerType = viewCustomer.CustomerType;
                customer.Address = viewCustomer.Address;
                customer.Contact = viewCustomer.Contact;

                _customerRepository.Update(customer);
            }
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
