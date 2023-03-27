using Microsoft.EntityFrameworkCore;
using ServiceManagment.Data;
using ServiceManagment.Interfaces;
using ServiceManagment.Models;

namespace ServiceManagment.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly ApplicationDbContext _context;
        public ServiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Service payService)
        {
            _context.Add(payService);

            return Save();
        }

        public bool Delete(Service payService)
        {
            _context.Remove(payService);

            return Save();
        }

        public async Task<IEnumerable<Service>> GetAllServicesByPaymentId(int id)
        {
            return await _context.Services
                .Where(x => x.PaymentId == id)
                .OrderBy(i => i.Status)
                .ToListAsync();
        }

        public async Task<Service> GetServiceById(int id)
        {
            return await _context.Services
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();

            return saved > 0 ? true : false;
        }

        public bool Update(Service payService)
        {
            _context.Update(payService);

            return Save();
        }
    }
}
