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
        bool Add(Order order);
        bool Update(Order order);
        bool Delete(Order order);
        bool Save();
    }
}
