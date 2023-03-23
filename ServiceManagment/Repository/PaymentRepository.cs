using Microsoft.EntityFrameworkCore;
using ServiceManagment.Data;
using ServiceManagment.Interfaces;
using ServiceManagment.Models;

namespace ServiceManagment.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;
        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Payment payment)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Payment payment)
        {
            throw new NotImplementedException();
        }

        public async Task<Payment>? GetPaymentById(int id)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();

            return saved > 0 ? true : false;
        }

        public bool Update(Payment payment)
        {
            _context.Update(payment);

            return Save();
        }
    }
}
