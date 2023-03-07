using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Create(int id, CreateOrderViewModel orderViewModel)
        {
            var order = new Order()
            {
                OrderStatus = Data.Enum.OrderStatus.New,
                OrderAdded = DateTime.Now,
                CustomerId = id,
                ProductId = orderViewModel.ProductId,
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

        public async Task<IActionResult> Detail(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);

            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditOrderViewModel orderViewModel)
        {
            //add validation
            string finishStatus = "Finished";
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if(order != null)
            {
                order.OrderStatus = orderViewModel.OrderStatus;
                order.Product.ProductType = orderViewModel.Product.ProductType;
                order.Product.Model = orderViewModel.Product.Model;
                order.Product.SerialNumber = orderViewModel.Product.SerialNumber;
                order.Product.Fault = orderViewModel.Product.Fault;
                order.Product.Description = orderViewModel.Product.Description;
                if(order.OrderStatus == (OrderStatus)Enum.Parse(typeof(OrderStatus), finishStatus))
                {
                    order.Payment.ToPay = 0;
                }

                _orderRepository.Update(order);
            }
            //return Index

            //return Error
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ListOrdersByStatus (string status)
        {
            var orders = await _orderRepository.GetAllOrdersByStatusAsync(status);

            return View(orders);
        }

        public async Task<IActionResult> DetailOrderPayment (int id)
        {
            var orderPayment = await _orderRepository.GetPaymentByOrderIdAsync(id);

            return View(orderPayment);
        }

        [HttpGet]
        public async Task<IActionResult> EditOrderPayment (int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if( order != null )
                return View(order.Payment);
            //return Error?
            return RedirectToAction("DetailOrderPayment");
        }

        [HttpPost]
        public async Task<IActionResult> EditOrderPayment (int id, EditOrderPaymentViewModel editOrderPaymentVM)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if(order != null)
            {
                if(order.Payment.ToPay < editOrderPaymentVM.ToPay)
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
            return RedirectToAction("Index");
        }
    }
}
