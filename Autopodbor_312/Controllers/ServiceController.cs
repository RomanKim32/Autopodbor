using Autopodbor_312.Interfaces;
using Autopodbor_312.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Autopodbor_312.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IServiceRepository _serviceRepository;

        public ServiceController(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        [HttpGet]
        public IActionResult Services()
        {          
            return View(_serviceRepository.GetMainServices());
        }

        public IActionResult ForAdminServices()
        {   
            return View(_serviceRepository.ForAdminServices());
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult CreateServices()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public IActionResult CreateServices(Services service, IFormFile servicePhotoFile)
        {
            _serviceRepository.CreateServices(service, servicePhotoFile);
            return RedirectToAction("ForAdminServices", "Service");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult EditServices(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var service = _serviceRepository.GetService(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public IActionResult EditServices(IFormFile servicePhotoFile, int id, [Bind("Id,NameRu,DescriptionRu,NameKy,DescriptionKy,IsAdditional,Photo")] Services service)
        {
            _serviceRepository.EditServices(servicePhotoFile, id, service);
            return RedirectToAction("ForAdminServices", "Service");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteServices(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var service = _serviceRepository.DeleteServices(id);
            if (service == null || service.IsAdditional == false)
            {
                return NotFound();
            }
            return View(service);
        }

        [HttpPost, ActionName("DeleteServices")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteConfirmedServices(int id)
        {
            _serviceRepository.DeleteConfirmedServices(id);
            return RedirectToAction("ForAdminServices", "Service");
        }

        [HttpGet]
        public IActionResult AdditionalServicesDetails()
        {
            return View(_serviceRepository.AdditionalServicesDetails());
        }
    }
}
