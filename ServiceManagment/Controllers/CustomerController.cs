using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceManagment.Common;
using ServiceManagment.Data.Enum;
using ServiceManagment.Interfaces;
using ServiceManagment.Models;
using ServiceManagment.ViewModel;

namespace ServiceManagment.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IWorkerRepository _workerRepository;

        public CustomerController(ICustomerRepository customerRepository, IOrderRepository orderRepository, IWorkerRepository workerRepository)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _workerRepository = workerRepository;
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
                    ViewData["CurrentKey"] = searchKey;
					TempData["Error"] = "No results!";

					return View(await _customerRepository.GetAllCustomersAsync());
                }
            }

            return View(await _customerRepository.GetAllCustomersAsync());
        }

        [HttpGet]
        public IActionResult Add()
        {
            var currentUserId = HttpContext.User.GetUserId();
            var addCustomerViewModel = new AddCustomerViewModel { WorkerId = currentUserId};

            return View(addCustomerViewModel);
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
                    WorkerId = addCustomerVM.WorkerId,
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
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            var workerName = await _workerRepository.GetWorkerNameById(customer.WorkerId);

            if(customer != null)
            {
                var detailCustomerVM = new DetailCustomerViewModel()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    NIP = customer.NIP,
                    UserAdded = customer.UserAdded,
                    Description = customer.Description,
                    CustomerGroup = customer.CustomerGroup,
                    CustomerType = customer.CustomerType,
                    Address = new Address()
                    {
                        City = customer.Address.City,
                        PostalCode = customer.Address.PostalCode,
                        Street = customer.Address.Street,
                        FlatNumber = customer.Address.FlatNumber,
                    },
                    Contact = new Contact()
                    {
                        EmailAddress = customer.Contact.EmailAddress,
                        PhoneNumber = customer.Contact.PhoneNumber,
                        SecondPhoneNumber = customer.Contact.SecondPhoneNumber,
                    },
                    WorkerId = customer.WorkerId,
                    WorkerName = workerName,
                };

                return View(detailCustomerVM);
            }

            return View("Error");
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
        public async Task<IActionResult> Edit(int id, EditCustomerViewModel editCustomerVM)
        {
            if(!ModelState.IsValid)
            {
                return View(editCustomerVM);
            }
            else
            {
                var customer = await _customerRepository.GetCustomerByIdAsync(id);
                if(customer != null)
                {
                    customer.Id = editCustomerVM.Id;
                    customer.Name = editCustomerVM.Name;
                    customer.NIP = editCustomerVM.NIP;
                    customer.Description = editCustomerVM.Description;
                    customer.CustomerGroup = editCustomerVM.CustomerGroup;
                    customer.CustomerType = editCustomerVM.CustomerType;
                    customer.Address = editCustomerVM.Address;
                    customer.Contact = editCustomerVM.Contact;

					_customerRepository.Update(customer);

					return RedirectToAction("Index");
				}
                //var customer = new Customer
                //{
                //    Id = editCustomerVM.Id,
                //    Name = editCustomerVM.Name,
                //    NIP = editCustomerVM.NIP,
                //    Description = editCustomerVM.Description,
                //    CustomerGroup = editCustomerVM.CustomerGroup,
                //    CustomerType = editCustomerVM.CustomerType,
                //    Address = editCustomerVM.Address,
                //    Contact = editCustomerVM.Contact,
                //}; 
            }

            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);

            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customerToDelete = await _customerRepository.GetCustomerByIdAsync(id);

            if (customerToDelete != null)
            {
                _customerRepository.Delete(customerToDelete);

                return RedirectToAction("Index");
            }

            return View("Index");
        }

        public async Task<IActionResult> History(int id)
        {
            double? toPay = 0;
            double? paid = 0;

            var orders = await _orderRepository.GetAllOrdersByCustomerId(id);
            var payments = await _orderRepository.GetAllPaymentByCustomerId(id); 

            if(orders != null || payments != null) 
            {
				foreach (var payment in payments)
				{
					toPay += payment.Payment.ToPay;
					paid += payment.Payment.Paid;
				}

				var historyCustomerVM = new HistoryCustomerViewModel()
				{
					Orders = orders,
					ToPay = toPay,
					Paid = paid,
				};

                return View(historyCustomerVM);
			}

            return View("Error");
        }

        public async Task<IActionResult> ListCustomerByType(string customerType)
        {
            var customers = await _customerRepository.GetAllCustomersByTypeAsync(customerType);

            return View(customers);
        }
    }
}