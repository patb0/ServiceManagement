using ServiceManagment.Models;

namespace ServiceManagment.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer> GetCustomerByIdAsync(int id);
        Task<IEnumerable<Customer>> GetAllCustomersBySearchKeyAsync(string searchKey);
        Task<IEnumerable<Customer>> GetAllCustomersByTypeAsync(string customerType);
		Task<IEnumerable<Customer>> GetAllCustomersByWorkerId(string id);
		//Task<string> GetWorkerNameById(string id);
        bool Add(Customer customer);
        bool Update(Customer customer);
        bool Delete(Customer customer);
        bool Save();
    }
}
