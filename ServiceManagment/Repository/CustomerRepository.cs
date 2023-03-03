using Microsoft.EntityFrameworkCore;
using ServiceManagment.Data;
using ServiceManagment.Interfaces;
using ServiceManagment.Models;

namespace ServiceManagment.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Customer customer)
        {
            _context.Add(customer);
            return Save();
        }

        public bool Delete(Customer customer)
        {
            _context.Remove(customer);
            return Save();
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            return await _context.Customers
                .Include(i => i.Address)
                .Include(j => j.Contact)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersByCustomerId(int id)
        {
            return await _context.Orders
                .Include(i => i.Product)
                .Include(j => j.Customer)
                .Where(x => x.CustomerId == id)
                .ToListAsync();
        }

        public async Task<Customer?> GetCustomerById(int id)
        {
            return await _context.Customers
                .Include(i => i.Address)
                .Include(j => j.Contact)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool Update(Customer customer)
        {
            _context.Update(customer);
            return Save();
        }
    }
}
