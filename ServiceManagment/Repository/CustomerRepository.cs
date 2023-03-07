using Microsoft.EntityFrameworkCore;
using ServiceManagment.Data;
using ServiceManagment.Interfaces;
using ServiceManagment.Models;

namespace ServiceManagment.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

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

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers
                .Include(i => i.Address)
                .Include(j => j.Contact)
                .ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersBySearchKeyAsync(string searchKey)
        {
            return await _context.Customers
                .Include(i => i.Address)
                .Include(j => j.Contact)
                .Where(x => x.Name.Contains(searchKey)
                || x.Address.City.Contains(searchKey)
                || x.Contact.PhoneNumber.Contains(searchKey)
                || x.Contact.EmailAddress.Contains(searchKey))
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersByCustomerIdAsync(int id)
        {
            return await _context.Orders
                .Include(i => i.Product)
                .Include(j => j.Customer)
                .Include(k => k.Payment)
                .Where(x => x.CustomerId == id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllPaymentByCustomerIdAsync(int id)
        {
            return await _context.Orders
                .Include(i => i.Payment)
                .Where(x => x.CustomerId == id)
                .ToListAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
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
