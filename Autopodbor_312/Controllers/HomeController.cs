using Autopodbor_312.Interfaces;
using Autopodbor_312.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace Autopodbor_312.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeRepository _homeRepository;

        public HomeController(IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
        }

        public IActionResult Index()
        {

            return View(_homeRepository.GetMainPageViewModel());
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
            _homeRepository.CreateTool(MainPage, file);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Edit()
        {
            return View(_homeRepository.GetMainPageViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult EditBanner(MainPage FirstBanner, MainPage SecondBanner, MainPage item, IFormFile newPhoto)
        {
            _homeRepository.EditBanner(FirstBanner, SecondBanner, item, newPhoto);
            return RedirectToAction("Edit");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var mainPage = _homeRepository.Delete(id);
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
            _homeRepository.DeleteConfirmed(id);
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
