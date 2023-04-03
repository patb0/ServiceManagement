using Microsoft.EntityFrameworkCore;
using ServiceManagment.Data;
using ServiceManagment.Data.Enum;
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
				.OrderByDescending(y => y.UserAdded.Date)
				.ThenByDescending(y => y.UserAdded.TimeOfDay)
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
                .OrderByDescending(y => y.UserAdded.Date)
                .ThenByDescending(y => y.UserAdded.TimeOfDay)
                .ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersByTypeAsync(string customerType)
        {
            return await _context.Customers
                .Include(i => i.Address)
                .Include (j => j.Contact)
                .Where(x => x.CustomerType == (CustomerType)Enum.Parse(typeof(CustomerType), customerType))
                .OrderByDescending(y => y.UserAdded.Date)
				.ThenByDescending(y => y.UserAdded.TimeOfDay)
				.ToListAsync();
        }

		public async Task<IEnumerable<Customer>> GetAllCustomersByWorkerId(string id)
		{
		    return await _context.Customers
				.Where(x => x.WorkerId == id)
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
