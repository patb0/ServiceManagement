using ServiceManagment.Models;

namespace ServiceManagment.Interfaces
{
	public interface IWorkerRepository
	{
		Task<IEnumerable<Worker>> GetAllWorkers();
		Task<Worker> GetWorkerById(string id);
		Task<IEnumerable<Customer>> GetAllCustomersByWorkerId(string id);
		Task<IEnumerable<Order>> GetAllOrdersByWorkerId(string id);
		bool Add(Worker worker);
		bool Update(Worker worker);
		bool Delete(Worker worker);
		bool Save();
	}
}
