using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceManagment.Interfaces;
using ServiceManagment.Models;
using ServiceManagment.ViewModel;

namespace ServiceManagment.Controllers
{
	[Authorize]
	public class WorkerController : Controller
	{
		private readonly IWorkerRepository _workerRepository;
		private readonly UserManager<Worker> _userManager;

        public WorkerController(IWorkerRepository workerRepository, UserManager<Worker> userManager)
        {
            _workerRepository = workerRepository;
			_userManager = userManager;
        }

		[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
		{
			var workers = await _workerRepository.GetAllWorkers();

			return View(workers);
		}

		public async Task<IActionResult> Detail(string id)
		{
			var worker = await _workerRepository.GetWorkerById(id);

			var detailWorkerVM = new DetailWorkerViewModel()
			{
				Id = worker.Id,
				Name = worker.Name,
				EmailAddress = worker.Email,
				CreatedAt = worker.CreatedAt,
				PhoneNumber = worker.PhoneNumber,
			};

			return View(detailWorkerVM);
		}

        [Authorize(Roles = "Admin")]
        [HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			var worker = await _workerRepository.GetWorkerById(id);
			
			if(worker != null) 
			{
				var editWorkerViewModel = new EditWorkerViewModel()
				{
					Id = worker.Id,
					Name = worker.Name,
					EmailAddress = worker.Email,
					PhoneNumber = worker.PhoneNumber,
				};

				return View(editWorkerViewModel);
			}

			return View("Error");
		}

		[HttpPost]
		public async Task<IActionResult> Edit(EditWorkerViewModel editWorkerVM)
		{
			if(!ModelState.IsValid)
			{
				return RedirectToAction("Index");
			}
			else
			{
				var worker = await _userManager.FindByIdAsync(editWorkerVM.Id);

				if (worker != null)
				{
					worker.Name = editWorkerVM.Name;
					worker.UserName = editWorkerVM.EmailAddress;
					worker.Email = editWorkerVM.EmailAddress;
					worker.PhoneNumber = editWorkerVM.PhoneNumber;

					await _userManager.UpdateAsync(worker);

					return RedirectToAction("Index");
				}

				return View("Error");
			}

			return RedirectToAction("Index");
		}

        [Authorize(Roles = "Admin")]
        [HttpGet]
		public async Task<IActionResult> Delete(string id)
		{
			var worker = await _workerRepository.GetWorkerById(id);
			if(worker != null) 
			{
				var deleteWorkerVM = new DeleteWorkerViewModel()
				{
					Name = worker.Name,
					CreatedAt = worker.CreatedAt,
				};

				return View(deleteWorkerVM);
			}

			return View("Error");
		}

        [Authorize(Roles = "Admin")]
        [HttpPost]
		public async Task<IActionResult> DeleteWorker(string id)
		{
			var worker = await _userManager.FindByIdAsync(id);

			if (worker != null) 
			{
				await _userManager.DeleteAsync(worker);

				return RedirectToAction("Index");
			}

			return View("Error");
		}


		public async Task<IActionResult> GetCustomers(string id)
		{
			var customers = await _workerRepository.GetAllCustomersByWorkerId(id);

			return View(customers);
		}

		public async Task<IActionResult> GetOrders(string id)
		{
			var orders = await _workerRepository.GetAllOrdersByWorkerId(id);

			return View(orders);
		}
	}
}
