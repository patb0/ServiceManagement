using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceManagment.Common;
using ServiceManagment.Data;
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
        private readonly ApplicationDbContext _context;

        public CustomerController(ICustomerRepository customerRepository,
            IOrderRepository orderRepository,
            IWorkerRepository workerRepository,
            ApplicationDbContext context)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _workerRepository = workerRepository;
            _context = context;
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
            
            using(var transaction = _context.Database.BeginTransaction())
            {
                try
                {
					var customer = new Customer
					{
						Name = addCustomerVM.Name,
						NIP = addCustomerVM.NIP,
						UserAdded = DateTime.Now,
						Description = addCustomerVM.Description,
						CustomerGroup = addCustomerVM.CustomerGroup,
						CustomerType = addCustomerVM.CustomerType,
						Address = new Address
						{
							City = addCustomerVM.Address.City,
							PostalCode = addCustomerVM.Address.PostalCode,
							Street = addCustomerVM.Address.Street,
							FlatNumber = addCustomerVM.Address.FlatNumber,

						},
						Contact = new Contact
						{
							PhoneNumber = addCustomerVM.Contact.PhoneNumber,
							SecondPhoneNumber = addCustomerVM.Contact.SecondPhoneNumber,
							EmailAddress = addCustomerVM.Contact.EmailAddress,
						},
						Orders = addCustomerVM.Orders,
						WorkerId = addCustomerVM.WorkerId,
					};
					_customerRepository.Add(customer);

                    transaction.Commit();

					return operation == (uint)CustomerConstans.ONLY_ADD_CUSTOMER
						? RedirectToAction("Index")
						: RedirectToAction("Create", "Order", new { @id = customer.Id });
				}
                catch(Exception)
                {
                    transaction.Rollback();

                    TempData["Result"] = "Error occured while adding customer to database";
				}

                return View();
                //if (operation == ((uint)CustomerConstans.ONLY_ADD_CUSTOMER))
                //{
                //	return RedirectToAction("Index");
                //}

                //return RedirectToAction("Create", "Order", new { @id = customer.Id });
            }
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
                    Address = new AddressViewModel
                    {
                        City = customer.Address.City,
                        PostalCode = customer.Address.PostalCode,
                        Street = customer.Address.Street,
                        FlatNumber = customer.Address.FlatNumber,
                    },
                    Contact = new ContactViewModel
                    {
                        PhoneNumber = customer.Contact.PhoneNumber,
                        SecondPhoneNumber = customer.Contact.SecondPhoneNumber,
                        EmailAddress = customer.Contact.EmailAddress,
                    },
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

            using(var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var customer = await _customerRepository.GetCustomerByIdAsync(id);

                    customer.Name = editCustomerVM.Name;
                    customer.NIP = editCustomerVM.NIP;
                    customer.Description = editCustomerVM.Description;
                    customer.CustomerGroup = editCustomerVM.CustomerGroup;
                    customer.CustomerType = editCustomerVM.CustomerType;

                    customer.Address.City = editCustomerVM.Address.City;
                    customer.Address.PostalCode = editCustomerVM.Address.PostalCode;
                    customer.Address.Street = editCustomerVM.Address.Street;
                    customer.Address.FlatNumber = editCustomerVM.Address.FlatNumber;

                    customer.Contact.PhoneNumber = editCustomerVM.Contact.PhoneNumber;
                    customer.Contact.SecondPhoneNumber = editCustomerVM.Contact.SecondPhoneNumber;
                    customer.Contact.EmailAddress = editCustomerVM.Contact.EmailAddress;

                    _customerRepository.Update(customer);

                    transaction.Commit();

                    return RedirectToAction("Index");
                }
                catch(Exception)
                {
                    transaction.Rollback();
                }

                return View(editCustomerVM);
            }
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