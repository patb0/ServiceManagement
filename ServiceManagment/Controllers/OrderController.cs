using Microsoft.AspNetCore.Mvc;
using ServiceManagment.Common;
using ServiceManagment.Data.Enum;
using ServiceManagment.Interfaces;
using ServiceManagment.Models;
using ServiceManagment.ViewModel;

namespace ServiceManagment.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
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
                    return View(await _orderRepository.GetAllOrdersAsync());
                }
            }

            return View(await _orderRepository.GetAllOrdersAsync());
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            var orderViewModel = new CreateOrderViewModel { CustomerId = id };

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
                        ToPay = orderViewModel.Payment.ToPay,
                        Paid = 0,
                    }
                };

                _orderRepository.Add(order);

                return RedirectToAction("Index");
            }
            return View("Error");
        }

        public async Task<IActionResult> Detail(int id)
        {
            var orderDetail = await _orderRepository.GetOrderByIdAsync(id);

            return orderDetail == null ? View("Error") : View(orderDetail);
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
                    CustomerName = order.Customer.Name,
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
                        order.Payment.ToPay = 0;
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

        public async Task<IActionResult> DetailOrderPayment (int id)
        {
            var orderPayment = await _orderRepository.GetPaymentByOrderIdAsync(id);

            return orderPayment == null ? View("Error") : View(orderPayment);
        }

        [HttpGet]
        public async Task<IActionResult> EditOrderPayment (int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);

            if( order != null )
            {
                var editOrderPaymentVM = new EditOrderPaymentViewModel
                {
                    Paid = order.Payment.Paid,
                    ToPay = order.Payment.ToPay,
                };
                return View(editOrderPaymentVM);
            }

            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> EditOrderPayment (int id, EditOrderPaymentViewModel editOrderPaymentVM)
        {
            if(!ModelState.IsValid)
            {
                return View(editOrderPaymentVM);
            }
            else
            {
                var order = await _orderRepository.GetOrderByIdAsync(id);
                if (order != null)
                {
                    if (order.Payment.ToPay < editOrderPaymentVM.ToPay)
                    {
                        order.Payment.ToPay += editOrderPaymentVM.ToPay - order.Payment.ToPay;
                    }
                    else
                    {
                        order.Payment.Paid += order.Payment.ToPay - editOrderPaymentVM.ToPay;
                        order.Payment.ToPay = editOrderPaymentVM.ToPay;
                    }
                    _orderRepository.Update(order);

                    return RedirectToAction("Index");
                }
            }

            return View("Error");
        }
    }
}
