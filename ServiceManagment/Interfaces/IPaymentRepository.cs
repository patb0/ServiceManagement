using ServiceManagment.Models;

namespace ServiceManagment.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment> GetPaymentById(int id);
        bool Add(Payment payment);
        bool Delete(Payment payment);
        bool Update(Payment payment);
        bool Save();
    }
}
