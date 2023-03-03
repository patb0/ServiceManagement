using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepository.GetAllOrders();
            return View(orders);
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
                }
            };
            _orderRepository.Add(order);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(int id)
        {
            var order = await _orderRepository.GetOrderById(id);

            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderRepository.GetOrderById(id);
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditOrderViewModel orderViewModel)
        {
            var order = await _orderRepository.GetOrderById(id);
            if(order != null)
            {
                order.OrderStatus = orderViewModel.OrderStatus;
                order.OrderAdded = orderViewModel.OrderAdded;
                order.CustomerId = orderViewModel.CustomerId;
                order.Customer = orderViewModel.Customer;
                order.ProductId = orderViewModel.ProductId;
                order.Product = orderViewModel.Product;
                _orderRepository.Update(order);
                //return Index
            }
            //return Error
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ListOrdersByStatus (string status)
        {
            var orders = await _orderRepository.GetAllOrdersByStatus(status);

            return View(orders);
        }
    }
}
