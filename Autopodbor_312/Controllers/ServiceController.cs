using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Telegram.Bot.Types;
using static System.Environment;

namespace Autopodbor_312.Controllers
{
	public class ServiceController : Controller
	{
		private readonly AutopodborContext _context;
		private IWebHostEnvironment _appEnvironment;

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
			if (servicePhotoFile != null)
			{
				string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/serviceImg/{servicePhotoFile.FileName}");
				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					await servicePhotoFile.CopyToAsync(fileStream);
				}
				service.Photo = $"/serviceImg/{servicePhotoFile.FileName}";
			}
			if (ModelState.IsValid)
			{
				service.IsAdditional = true;
				_context.Add(service);
				await _context.SaveChangesAsync();
				if (service.IsAdditional == true)
					return RedirectToAction("AdditionalServicesDetails", "Service");
				return RedirectToAction("IndexServices", "Service");
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
			if (servicePhotoFile == null)
			{
				var currentService = await _context.Services.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
				service.Photo = currentService.Photo;
			}
			else
			{
				string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/serviceImg/{servicePhotoFile.FileName}");
				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					await servicePhotoFile.CopyToAsync(fileStream);
				}
				service.Photo = $"/serviceImg/{servicePhotoFile.FileName}";
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
					return RedirectToAction("AdditionalServicesDetails", "Service");
				return RedirectToAction("Services", "Service");
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
			if (services == null)
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
			return RedirectToAction("AdditionalServicesDetails", "Service");
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
