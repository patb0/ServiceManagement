using ServiceManagment.Models;

namespace ServiceManagment.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllOrdersByStatus(string status);
        Task<Order> GetPaymentByOrderIdAsync(int id);
        Task<IEnumerable<Order>> GetAllOrdersBySearchKey(string searchKey);
        //Task<string> GetWorkerById(string id);
        Task<IEnumerable<Order>> GetAllOrdersByCustomerId(int id);
		Task<IEnumerable<Order>> GetAllOrdersByWorkerId(string id);
		Task<IEnumerable<Order>> GetAllPaymentByCustomerId(int id);
		bool Add(Order order);
        bool Update(Order order);
        bool Delete(Order order);
        bool Save();
    }
}
