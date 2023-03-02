using Microsoft.AspNetCore.Mvc;
using ServiceManagment.Interfaces;
using ServiceManagment.Models;

namespace ServiceManagment.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public IActionResult Index()
        {
            var orders = _orderRepository.GetAllOrders();
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Order order)
        {
            order.OrderAdded = DateTime.Now;
            order.OrderStatus = Data.Enum.OrderStatus.New;
            _orderRepository.Add(order);
            return RedirectToAction("Index");
        }
    }
}
