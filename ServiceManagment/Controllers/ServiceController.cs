using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceManagment.Interfaces;
using ServiceManagment.Models;
using ServiceManagment.ViewModel;

namespace ServiceManagment.Controllers
{
    [Authorize]
    public class ServiceController : Controller
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IPaymentRepository _paymentRepository;
        public ServiceController(IServiceRepository serviceRepository, IPaymentRepository paymentRepository)
        {
            _serviceRepository = serviceRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<IActionResult> Paid(int id)
        {
            var service = await _serviceRepository.GetServiceById(id);
            var payment = await _paymentRepository.GetPaymentById(service.PaymentId);

            if (service != null) 
            {
                payment.ToPay -= service.Price;
                payment.Paid += service.Price;
                service.Status = Data.Enum.ServiceStatus.Paid;

                _serviceRepository.Update(service);
                _paymentRepository.Update(payment);

                return RedirectToAction("EditOrderPayment", "Order", new {@id = service.Payment.OrderId});
            }

            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var service = await _serviceRepository.GetServiceById(id);

            if(service != null) 
            {
                var editServiceVM = new EditServiceViewModel()
                {
                    Id = id,
                    Name = service.Name,
                    Price = service.Price,
                };
                return View(editServiceVM);
            }

            return View("Error"); 
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditServiceViewModel editServiceVM)
        {
            var service = await _serviceRepository.GetServiceById(id);
            var payment = await _paymentRepository.GetPaymentById(service.PaymentId);

            if (!ModelState.IsValid) 
            {
                return View(editServiceVM);
            }
            else
            {
                if (editServiceVM != null)
                {
                    if(service.Price > editServiceVM.Price)
                    {
                        payment.ToPay -= service.Price - editServiceVM.Price;
                    }
                    else if(service.Price < editServiceVM.Price)
                    {
                        payment.ToPay += editServiceVM.Price - service.Price;
                    }

                    service.Price = editServiceVM.Price;
                    service.Name = editServiceVM.Name;

                    _serviceRepository.Update(service);
                    _paymentRepository.Update(payment);

                    return RedirectToAction("EditOrderPayment", "Order", new {@id = service.Payment.OrderId});
                }
            }

            return View("Error");
        }

        public async Task<IActionResult> Delete(int id) 
        { 
            var service = await _serviceRepository.GetServiceById(id);
            var payment = await _paymentRepository.GetPaymentById(service.PaymentId);

            if (service != null) 
            {
                if(service.Status == Data.Enum.ServiceStatus.NotPaid)
                {
					payment.ToPay -= service.Price;
				}
                else if(service.Status == Data.Enum.ServiceStatus.Paid)
                {
                    payment.Paid -= service.Price;
                }

                _serviceRepository.Delete(service);

                return RedirectToAction("EditOrderPayment", "Order", new { @id = payment.OrderId});
            }

            return View("Error");
        }
    }
}
