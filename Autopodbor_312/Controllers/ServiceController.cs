using Autopodbor_312.Models;
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
        private readonly AutopodborContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public ServiceController(AutopodborContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _appEnvironment = webHost;
        }

        [HttpGet]
        public async Task<IActionResult> Services()
        {
            var sercices = await _context.Services.Where(s => s.NameRu != "Обратный звонок").Where(s => s.IsAdditional == false).OrderBy(s => s.Id).ToListAsync();
            return View(sercices);
        }
        public async Task<IActionResult> ForAdminServices()
        {
            var sercices = await _context.Services.Where(s => s.NameRu != "Дополнительные услуги").OrderBy(s => s.Id).ToListAsync();
            return View(sercices);
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
        public async Task<IActionResult> CreateServices(Services service, IFormFile servicePhotoFile)
        {
            if (ModelState.IsValid)
            {
                service.IsAdditional = true;
                _context.Services.Add(service);
                await _context.SaveChangesAsync();

                string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/serviceImg/Id={service.Id}&{servicePhotoFile.FileName}");
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await servicePhotoFile.CopyToAsync(fileStream);
                }
                service.Photo = $"/serviceImg/Id={service.Id}&{servicePhotoFile.FileName}";
                await _context.SaveChangesAsync();
                if (service.IsAdditional == true)
                    return RedirectToAction("ForAdminServices", "Service");
                return RedirectToAction("ForAdminServices", "Service");
            };
            return View(service);
        }


        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditServices(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var service = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditServices(IFormFile servicePhotoFile, int id, [Bind("Id,NameRu,DescriptionRu,NameKy,DescriptionKy,IsAdditional,Photo")] Services service)
        {
            if (servicePhotoFile != null)
            {
                string oldFilePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot{service.Photo}");
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
                string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/serviceImg/Id={service.Id}&{servicePhotoFile.FileName}");
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await servicePhotoFile.CopyToAsync(fileStream);
                }
                service.Photo = $"/serviceImg/Id={service.Id}&{servicePhotoFile.FileName}";
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicesExists(service.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (service.IsAdditional == true)
                    return RedirectToAction("ForAdminServices", "Service");
                return RedirectToAction("ForAdminServices", "Service");
            }
            return View(service);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteServices(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var services = await _context.Services.FirstOrDefaultAsync(m => m.Id == id);
            if (services == null || services.IsAdditional == false)
            {
                return NotFound();
            }

            return View(services);
        }

        [HttpPost, ActionName("DeleteServices")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmedServices(int id)
        {
            var services = await _context.Services.FindAsync(id);
            string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot{services.Photo}");
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            _context.Services.Remove(services);
            await _context.SaveChangesAsync();
            return RedirectToAction("ForAdminServices", "Service");
        }

        [HttpGet]
        public async Task<IActionResult> AdditionalServicesDetails()
        {
            var additionalServicesList = await _context.Services.Where(s => s.IsAdditional == true).ToListAsync();
            return View(additionalServicesList);
        }

        private bool ServicesExists(int id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    }
}
