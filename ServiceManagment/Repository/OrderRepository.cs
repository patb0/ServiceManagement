﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ServiceManagment.Data;
using ServiceManagment.Data.Enum;
using ServiceManagment.Interfaces;
using ServiceManagment.Models;

namespace ServiceManagment.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Order order)
        {
            _context.Orders.Add(order);
            return Save();
        }

        public bool Delete(Order order)
        {
            _context.Remove(order);
            return Save();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(i => i.Customer)
                .Include(j => j.Product)
                .Include(k => k.Payment)
                .OrderBy(y => y.OrderStatus)
                .ThenByDescending(x => x.OrderAdded.Date)
                .ThenByDescending(x => x.OrderAdded.TimeOfDay)
                .ToListAsync();
        }

        public async Task<Order> GetPaymentByOrderIdAsync(int id)
        {
            return await _context.Orders
                .Include(i => i.Payment)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(i => i.Customer)
                .Include(j => j.Product)
                .Include(k => k.Payment)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool Update(Order order)
        {
            _context.Update(order);
            return Save();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersBySearchKey(string searchKey)
        {
            return await _context.Orders
                .Include(i => i.Customer)
                .Include(j => j.Product)
                .Include(k => k.Payment)
                .Where(x => x.Customer.Name.Contains(searchKey)
                || x.Product.ProducerName.Contains(searchKey)
                || x.Product.Model.Contains(searchKey)
                || x.Product.Fault.Contains(searchKey))
                .OrderByDescending(x => x.OrderAdded.Date)
                .ThenByDescending(x => x.OrderAdded.TimeOfDay)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersByStatus(string status)
        {
            return await _context.Orders
                .Include(i => i.Customer)
                .Include(j => j.Product)
                .Include(k => k.Payment)
                .Where(x => x.OrderStatus == (OrderStatus)Enum.Parse(typeof(OrderStatus), status))
                .OrderByDescending(x => x.OrderAdded.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Service>> GetAllServicesByPaymentId(int id)
        {
            return await _context.Services
                .Where(i => i.Id == id)
                .ToListAsync();
        }

		public async Task<IEnumerable<Order>> GetAllOrdersByCustomerId(int id)
		{
			return await _context.Orders
				.Include(i => i.Product)
				.Include(j => j.Customer)
				.Include(k => k.Payment)
				.Where(x => x.CustomerId == id)
				.OrderBy(x => x.OrderStatus)
				.ToListAsync();
		}

		public async Task<IEnumerable<Order>> GetAllPaymentByCustomerId(int id)
		{
			return await _context.Orders
				.Include(i => i.Payment)
				.Where(x => x.CustomerId == id)
				.ToListAsync();
		}

		public async Task<IEnumerable<Order>> GetAllOrdersByWorkerId(string id)
		{
			return await _context.Orders
                .Include(i => i.Product)
                .Where(x => x.WorkerId == id)
                .ToListAsync();
        }
	}
}
