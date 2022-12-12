using Autopodbor_312.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

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
			var sercices = await _context.Services.Where(s => s.Name != "Обратный звонок").Where(s => s.IsAdditional == false).ToListAsync();
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
        public async Task<IActionResult> CreateServices(IFormFile servicePhotoFile, [Bind("Id,Name,Description")] Services services)
		{
			if (servicePhotoFile != null)
			{
				string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/serviceImg/{servicePhotoFile.FileName}");
				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					await servicePhotoFile.CopyToAsync(fileStream);
				}
				services.Photo = $"/serviceImg/{servicePhotoFile.FileName}";
			}
			if (ModelState.IsValid)
			{
				services.IsAdditional = true;
				_context.Add(services);
				await _context.SaveChangesAsync();
				if (services.IsAdditional == true)
					return RedirectToAction("AdditionalServicesDetails", "Service");
				return RedirectToAction("IndexServices", "Service");
			};
			return View(services);
		}

		public void AddToResourcesFile(string name, string valueRu, string valueKy)
		{
			//string name = "sdadasd"; "D:/ESDP/Autopodbor_312/Autopodbor_312/Resources/Views/Home/Index.ky.resx"; 
			string path = "Resources/Views/Service/Services.ru.resx"; // путь относительный 
			//string path = "D:/ESDP/Autopodbor_312/Autopodbor_312/Resources/Views/Home/Index.ky.resx";
			var file = Path.Combine(path);
			XDocument document = XDocument.Load(file);
			var c = document.Root.Elements("data");
			c.Last().AddAfterSelf(new XElement("data", new XAttribute("name", name), new XAttribute(XNamespace.Xml + "space", "preserve"), new XElement("value", valueRu)));
			document.Save(file);
		}

/*		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "admin")]
		public async Task<IActionResult> CreateServices(IFormFile servicePhotoFile, [Bind("Id,Name,Description")] Services services)
		{
			if (servicePhotoFile != null)
			{
				string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/serviceImg/{servicePhotoFile.FileName}");
				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					await servicePhotoFile.CopyToAsync(fileStream);
				}
				services.Photo = $"/serviceImg/{servicePhotoFile.FileName}";
			}
			if (ModelState.IsValid)
			{
				services.IsAdditional = true;

				_context.Add(services);
				services.KeyForNameInResourcesFiles = $"serviceName{services.Id}";
				//Test(services.KeyForNameInResourcesFiles, services.Name, ser);
				services.KeyForDescriptionInResourcesFiles = $"serviceDescription{services.Id}";
				_context.Update(services);

				await _context.SaveChangesAsync();
				if (services.IsAdditional == true)
					return RedirectToAction("AdditionalServicesDetails", "Service");
				return RedirectToAction("IndexServices", "Service");
			};
			return View(services);
		}*/


		[HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditServices(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var service = await _context.Services.FindAsync(id);
			if (service == null)
			{
				return NotFound();
			}
			return View(service);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditServices(IFormFile servicePhotoFile, int id, [Bind("Id,Name,Description,isAdditional,Photo")] Services service)
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
				return RedirectToAction("IndexServices", "Service");
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
