using ServiceManagment.Models;

namespace ServiceManagment.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllCustomers();
        Task<Customer> GetCustomerById(int id);
        Task<IEnumerable<Order>> GetAllOrdersByCustomerId(int id);
        Task<IEnumerable<Order>> GetAllPaymentByCustomerId(int id);
        bool Add(Customer customer);
        bool Update(Customer customer);
        bool Delete(Customer customer);
        bool Save();
    }
}
