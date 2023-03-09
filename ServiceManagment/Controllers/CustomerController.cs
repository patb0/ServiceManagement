using Microsoft.AspNetCore.Mvc;
using ServiceManagment.Common;
using ServiceManagment.Interfaces;
using ServiceManagment.Models;
using ServiceManagment.ViewModel;

namespace ServiceManagment.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IActionResult> Index(string searchKey)
        {
            if(!String.IsNullOrEmpty(searchKey))
            {
                var customers = await _customerRepository.GetAllCustomersBySearchKeyAsync(searchKey);
                if(customers.Count() != 0)
                {
                    ViewData["CurrentKey"] = searchKey;
                    return View(customers);
                }
                else
                {
                    return View(await _customerRepository.GetAllCustomersAsync());
                }
            }

            return View(await _customerRepository.GetAllCustomersAsync());
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Customer customer, int operation)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }
            else
            {
                customer.UserAdded = DateTime.Now;
                _customerRepository.Add(customer);

                if (operation == ((uint)OrderConstans.ONLY_ADD_CUSTOMER))
                {
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Create", "Order", new { @id = customer.Id });
            }
        }

        public async Task<IActionResult> Detail(int id)
        {
            var customerDetail = await _customerRepository.GetCustomerByIdAsync(id);

            return View(customerDetail);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);

            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditCustomerViewModel viewCustomer)
        {
            var customer = new Customer
            {
                Id = id,
                Name = viewCustomer.Name,
                NIP = viewCustomer.NIP,
                Description = viewCustomer.Description,
                CustomerGroup = viewCustomer.CustomerGroup,
                CustomerType = viewCustomer.CustomerType,
                Address = viewCustomer.Address,
                Contact = viewCustomer.Contact,
            };

            _customerRepository.Update(customer);

            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            var customerToDelete = await _customerRepository.GetCustomerByIdAsync(id);
            if (customerToDelete != null)
            {
                _customerRepository.Delete(customerToDelete);
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        public async Task<IActionResult> Orders(int id)
        {
            var userOrders = await _customerRepository.GetAllOrdersByCustomerIdAsync(id);

            return View(userOrders);
        }

        public async Task<IActionResult> Payment(int id)
        {
            double? userToPay = 0;
            var userPayments = await _customerRepository.GetAllPaymentByCustomerIdAsync(id);

            foreach(var payment in userPayments)
            {
                userToPay += payment.Payment.ToPay;
            }

            return View(userToPay);
        }
    }
}
