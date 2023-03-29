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
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IWorkerRepository _workerRepository;

        public OrderController(IOrderRepository orderRepository, IServiceRepository serviceRepository, IPaymentRepository paymentRepository,
            IWorkerRepository workerRepository)
        {
            _orderRepository = orderRepository;
            _serviceRepository = serviceRepository;
            _paymentRepository = paymentRepository;
            _workerRepository = workerRepository;
        }

        public async Task<IActionResult> Index(string searchKey)
        {
            if(!String.IsNullOrEmpty(searchKey))
            {
                var orders = await _orderRepository.GetAllOrdersBySearchKey(searchKey);

                if(orders.Count() != 0)
                {
                    ViewData["CurrentKey"] = searchKey;
                    return View(orders);
                }
                else
                {
                    ViewData["CurrentKey"] = searchKey;
                    TempData["Error"] = "No results!";

                    return View(await _orderRepository.GetAllOrdersAsync());
                }
            }

            return View(await _orderRepository.GetAllOrdersAsync());
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            var currentWorkerId = HttpContext.User.GetUserId();
            var orderViewModel = new CreateOrderViewModel 
            {
                CustomerId = id,
                WorkerId = currentWorkerId,
            };

            return View(orderViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderViewModel orderViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(orderViewModel);
            }
            else
            {
                var order = new Order()
                {
                    OrderStatus = Data.Enum.OrderStatus.New,
                    OrderAdded = DateTime.Now,
                    CustomerId = orderViewModel.CustomerId,
                    Product = new Product
                    {
                        ProductType = orderViewModel.Product.ProductType,
                        ProducerName = orderViewModel.Product.ProducerName,
                        Model = orderViewModel.Product.Model,
                        SerialNumber = orderViewModel.Product.SerialNumber,
                        Fault = orderViewModel.Product.Fault,
                        Description = orderViewModel.Product.Description,
                    },
                    Payment = new Payment
                    {
                        ToPay = orderViewModel.Payment.ToPay > 0 ? orderViewModel.Payment.ToPay : 0,
                        Paid = 0,
                    },
                    WorkerId = orderViewModel.WorkerId,
                };

                _orderRepository.Add(order);

                return RedirectToAction("Index");
            }
            return View("Error");
        }

        public async Task<IActionResult> Detail(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            var workerName = await _workerRepository.GetWorkerNameById(order.WorkerId);

            if(order != null)
            {
                var detailOrderVM = new DetailOrderViewModel()
                {
                    Id = order.Id,
                    OrderStatus = order.OrderStatus,
                    OrderAdded = order.OrderAdded,
                    Customer = order.Customer,
                    Product = order.Product,
                    Payment = order.Payment,
                    WorkerId = order.WorkerId,
                    WorkerName = workerName,
                };

                return View(detailOrderVM);
            }

            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);

            if(order != null)
            {
                var editOrderVM = new EditOrderViewModel 
                {
                    Id = order.Id,
                    OrderStatus = order.OrderStatus,
                    Product = order.Product,
                };

                return View(editOrderVM);
            }

            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditOrderViewModel orderViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(orderViewModel);
            }
            else
            {
                var order = await _orderRepository.GetOrderByIdAsync(id);

                if (order != null)
                {
                    order.OrderStatus = orderViewModel.OrderStatus;
                    order.Product = orderViewModel.Product;

                    if (order.OrderStatus == (OrderStatus)Enum.Parse(typeof(OrderStatus), OrderConstans.FINISH_STATUS))
                    {
                        var payment = await _paymentRepository.GetPaymentByOrderId(id);

                        payment.Paid += order.Payment.ToPay;
                        payment.ToPay = 0;

						_paymentRepository.Update(payment);
					}

                    _orderRepository.Update(order);

                    return RedirectToAction("Index");
                }
            }

            return View("Error");
        }

        public async Task<IActionResult> ListOrdersByStatus (string status, string searchKey)
        {
            var orders = await _orderRepository.GetAllOrdersByStatus(status);
            if(orders != null)
            {
                return View(orders);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditOrderPayment (int id)
        {
            var orderPayment = await _orderRepository.GetPaymentByOrderIdAsync(id);
            var services = await _serviceRepository.GetAllServicesByPaymentId(orderPayment.Payment.Id);
            
            if( orderPayment != null)
            {
				if (orderPayment.OrderStatus == OrderStatus.Finished)
				{
					foreach (var service in services)
					{
						service.Status = ServiceStatus.Paid;
					}
				}

				var editOrderPaymentVM = new EditOrderPaymentViewModel()
                {
                    PaymentId = orderPayment.Payment.Id,
                    OrderId = id,
                    ToPay = orderPayment.Payment.ToPay,
                    Paid = orderPayment.Payment.Paid,
                    Services = services,
                };

                return View(editOrderPaymentVM);
            }

            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> EditOrderPayment (int id, EditOrderPaymentViewModel editOrderPaymentVM)
        {
            //payment repo?
            if (!ModelState.IsValid)
            {
                return View(editOrderPaymentVM);
            }
            else
            {
                var order = await _orderRepository.GetPaymentByOrderIdAsync(id);

                if (order != null)
                {
                    var service = new Service()
                    {
                        PaymentId = editOrderPaymentVM.PaymentId,
                        Name = editOrderPaymentVM.Name,
                        Price = editOrderPaymentVM.Price,
                        Status = ServiceStatus.NotPaid,
                    };

                    order.Payment.ToPay += service.Price;

                    _orderRepository.Update(order);
                    _serviceRepository.Update(service);

                    return RedirectToAction("EditOrderPayment", new {@id = id});
                }
            }

            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);

            if(order != null)
            {
                var deleteOrderVM = new DeleteOrderViewModel()
                {
                    Id = order.Id,
                    CustomerName = order.Customer.Name,
                    PoducerName = order.Product.ProducerName,
                    ModelName = order.Product.Model,
                    OrderAdded = order.OrderAdded,
                };

                return View(deleteOrderVM);
            }

            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);

            if(order != null) 
            {
                _orderRepository.Delete(order);

                return RedirectToAction("Index");
            }

            return View("Error");
        }
    }
}