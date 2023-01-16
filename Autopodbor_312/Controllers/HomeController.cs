using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Autopodbor_312.Controllers
{
    public class HomeController : Controller
    {
        private readonly AutopodborContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public HomeController(AutopodborContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public IActionResult Index()
        {
            MainPageViewModel mainPageViewModel = new MainPageViewModel
            {
                FirstBanner = _context.MainPage.FirstOrDefault(b => b.Banner == "first"),
                SecondBanner = _context.MainPage.FirstOrDefault(b => b.Banner == "second"),
                ThirdBanner = _context.MainPage.Where(b => b.Banner == "third").ToList()
            };
            return View(mainPageViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult CreateTool()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult CreateTool(MainPage MainPage, IFormFile file)
        {
            MainPage.Banner = "third";
            string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/mainPageFiles/{file.FileName}");
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            MainPage.Path = $"/mainPageFiles/{file.FileName}";
            _context.Add(MainPage);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Edit()
        {
            MainPageViewModel mainPageViewModel = new MainPageViewModel
            {
                FirstBanner = _context.MainPage.FirstOrDefault(b => b.Banner == "first"),
                SecondBanner = _context.MainPage.FirstOrDefault(b => b.Banner == "second"),
                ThirdBanner = _context.MainPage.Where(b => b.Banner == "third").ToList()
            };
            return View(mainPageViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult EditTool(MainPage item, IFormFile newPhoto)
        {
            EditBanner(item, newPhoto);
            return RedirectToAction("Edit");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult EditBanner(MainPage FirstBanner, MainPage SecondBanner ,IFormFile newPhoto)
        {
            if (FirstBanner.Id != 0)
            {
                EditBanner(FirstBanner, newPhoto);
            }
            else if (SecondBanner.Id != 0)
            {
                EditBanner(SecondBanner, newPhoto);
            }
            return RedirectToAction("Edit");
        }

        private void EditBanner(MainPage banner, IFormFile file)
        {
            if (file != null)
            {
                string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/mainPageFiles/{file.FileName}");
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                banner.Path = $"/mainPageFiles/{file.FileName}";
            }
            _context.Update(banner);
            _context.SaveChanges();
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var mainPage = await _context.MainPage.FirstOrDefaultAsync(p => p.Id == id);
            if (mainPage == null)
                return NotFound();
            return View(mainPage);
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var mainPage = _context.MainPage.FirstOrDefault(m => m.Id == id);
            _context.Remove(mainPage);
            _context.SaveChanges();
            return RedirectToAction("Edit");
        }

        [HttpPost]
        public IActionResult CultureManagement(string culture, string returnUrl)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30) }
                );
            return LocalRedirect(returnUrl);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
