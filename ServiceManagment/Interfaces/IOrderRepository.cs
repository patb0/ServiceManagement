using ServiceManagment.Models;

namespace ServiceManagment.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrders();
        Task<Order> GetOrderById(int id);
        Task<IEnumerable<Order>> GetAllOrdersByStatus(string status);
        bool Add(Order order);
        bool Update(Order order);
        bool Delete(Order order);
        bool Save();
    }
}
