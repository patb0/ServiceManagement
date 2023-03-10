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
        public async Task<IActionResult> Add(AddCustomerViewModel addCustomerVM, int operation)
        {
            if (!ModelState.IsValid)
            {
                return View(addCustomerVM);
            }
            else
            {
                var customer = new Customer
                {
                    Id = addCustomerVM.Id,
                    Name = addCustomerVM.Name,
                    NIP = addCustomerVM.NIP,
                    UserAdded = DateTime.Now,
                    Description = addCustomerVM.Description,
                    CustomerGroup = addCustomerVM.CustomerGroup,
                    CustomerType = addCustomerVM.CustomerType,
                    Address = addCustomerVM.Address,
                    Contact = addCustomerVM.Contact,
                    Orders = addCustomerVM.Orders,
                };

                _customerRepository.Add(customer);

                if (operation == ((uint)CustomerConstans.ONLY_ADD_CUSTOMER))
                {
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Create", "Order", new { @id = customer.Id });
            }

            return View("Error");
        }

        public async Task<IActionResult> Detail(int id)
        {
            var customerDetail = await _customerRepository.GetCustomerByIdAsync(id);

            return customerDetail == null ? View("Error") : View(customerDetail);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if(customer != null)
            {
                var editCustomerVM = new EditCustomerViewModel
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    NIP = customer.NIP,
                    Description = customer.Description,
                    CustomerGroup = customer.CustomerGroup,
                    CustomerType = customer.CustomerType,
                    Address = customer.Address,
                    Contact = customer.Contact,
                };

                return View(editCustomerVM);
            }

            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCustomerViewModel editCustomerVM)
        {
            if(!ModelState.IsValid)
            {
                return View(editCustomerVM);
            }
            else
            {
                var customer = new Customer
                {
                    Id = editCustomerVM.Id,
                    Name = editCustomerVM.Name,
                    NIP = editCustomerVM.NIP,
                    Description = editCustomerVM.Description,
                    CustomerGroup = editCustomerVM.CustomerGroup,
                    CustomerType = editCustomerVM.CustomerType,
                    Address = editCustomerVM.Address,
                    Contact = editCustomerVM.Contact,
                };

                _customerRepository.Update(customer);

                return RedirectToAction("Index");
            }

            return View("Error");
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
