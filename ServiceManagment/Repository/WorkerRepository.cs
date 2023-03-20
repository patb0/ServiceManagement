using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceManagment.Data;
using ServiceManagment.Interfaces;
using ServiceManagment.Models;

namespace ServiceManagment.Repository
{
	public class WorkerRepository : IWorkerRepository
	{
		private readonly ApplicationDbContext _context;
        public WorkerRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Worker worker)
		{
			throw new NotImplementedException();
		}

		public bool Delete(Worker worker)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<Worker>> GetAllWorkers()
		{
			return await _context.Users
				.ToListAsync();
		}

		public async Task<Worker> GetWorkerById(string id)
		{
			return await _context.Users.FindAsync(id);
		}

		public bool Save()
		{
			var saved = _context.SaveChanges();

			return saved > 0 ? true : false;
		}

		public bool Update(Worker worker)
		{
			_context.Update(worker);

			return Save();
		}
	}
}
