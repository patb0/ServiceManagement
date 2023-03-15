using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceManagment.Data;
using ServiceManagment.Models;
using ServiceManagment.ViewModel;

namespace ServiceManagment.Controllers
{
    public class WorkerController : Controller
    {
        private readonly UserManager<Worker> _workerManager;
        private readonly SignInManager<Worker> _signInManager;
        private readonly ApplicationDbContext _dbContext;

        public WorkerController(UserManager<Worker> workerManager, SignInManager<Worker> signInManager,
            ApplicationDbContext dbContext)
        {
            _workerManager = workerManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterWorkerViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterWorkerViewModel registerWorkerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerWorkerVM);
            }

            var workerExist = await _workerManager.FindByEmailAsync(registerWorkerVM.EmailAddress);

            if (workerExist != null)
            {
                TempData["Error"] = "Worker about this email is now exist!";
                return View(registerWorkerVM);
            }

            var worker = new Worker()
            {
                UserName = registerWorkerVM.EmailAddress,
                EmailAddress = registerWorkerVM.EmailAddress,
            };

            var newWorker = await _workerManager.CreateAsync(worker, registerWorkerVM.Password);
            
            if(newWorker.Succeeded)
            {
                await _workerManager.AddToRoleAsync(worker, WorkerRoles.Worker);
            }

            return RedirectToAction("Index", "Customer");
        }
    }
}
