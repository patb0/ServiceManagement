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
            //var orderViewModel = new CreateOrderViewModel();
            //if (Int32.TryParse(HttpContext.User.GetUserId(), out int currentlyCustomerId))
            //{
            //    orderViewModel.CustomerId = currentlyCustomerId;
            //}

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
    }
}
