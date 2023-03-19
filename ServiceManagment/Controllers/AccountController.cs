using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServiceManagment.Data;
using ServiceManagment.Models;
using ServiceManagment.ViewModel;

namespace ServiceManagment.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Worker> _userManager;
        private readonly SignInManager<Worker> _signInManager;
        private readonly ApplicationDbContext _dbContext;

        public AccountController(UserManager<Worker> userManager, SignInManager<Worker> signInManager,
            ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }

        [Authorize(Roles = "Admin")]
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

            var workerExist = await _userManager.FindByEmailAsync(registerWorkerVM.EmailAddress);

            if (workerExist != null)
            {
                TempData["Error"] = "Worker about this email is now exist!";
                return View(registerWorkerVM);
            }


            var worker = new Worker()
            {
                UserName = registerWorkerVM.EmailAddress,
                Email = registerWorkerVM.EmailAddress,
            };

            var newWorker = await _userManager.CreateAsync(worker, registerWorkerVM.Password);
          
            if(newWorker.Succeeded)
            {
                await _userManager.AddToRoleAsync(worker, WorkerRoles.Worker);
            }

            return RedirectToAction("Index", "Customer");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginWorkerViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginWorkerViewModel loginWorkerVM, string? returnUrl)
        {
            if(!ModelState.IsValid) 
            {
                return View(loginWorkerVM);
            }

            var worker = await _userManager.FindByEmailAsync(loginWorkerVM.EmailAddress);

            if(worker != null)
            {
                var workerPasswordCheck = await _userManager.CheckPasswordAsync(worker, loginWorkerVM.Password);

                if (workerPasswordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(worker, loginWorkerVM.Password, false, false);

                    if(result.Succeeded) 
                    {
                        if(!String.IsNullOrEmpty(returnUrl))
                        {
                            return LocalRedirect(returnUrl);
                        }

                        return RedirectToAction("Index", "Customer");
                    }
                }

                TempData["Error"] = "Password is incorrect!";
                return View(loginWorkerVM);
            }

            TempData["Error"] = "Worker with this email address doesn't exist";
            return View(loginWorkerVM);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }
    }
}
