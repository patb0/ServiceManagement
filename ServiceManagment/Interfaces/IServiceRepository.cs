using ServiceManagment.Models;

namespace ServiceManagment.Interfaces
{
    public interface IServiceRepository
    {
        Task<IEnumerable<Service>> GetAllServicesByPaymentId(int id);
        Task<Service> GetServiceById(int id);
        bool Add(Service payService);
        bool Delete(Service payService);
        bool Update(Service payService);
        bool Save();
    }
}
