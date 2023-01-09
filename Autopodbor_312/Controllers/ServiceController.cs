using Autopodbor_312.Interfaces;
using Autopodbor_312.Models;
using Autopodbor_312.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            return View(_serviceRepository.Services());
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
            return View(_serviceRepository.EditServices(id));
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
            return View(_serviceRepository.DeleteServices(id));
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
