using Autopodbor_312.Enums;
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Autopodbor_312.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AutopodborContext _context;
        private readonly UserManager<User> _userManager;
        IWebHostEnvironment _appEnvironment;

        public HomeController(ILogger<HomeController> logger, AutopodborContext context, UserManager<User> userManager, IWebHostEnvironment appEnvironment)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _appEnvironment = appEnvironment;
        }


        public IActionResult Index()
        {
            var bannerList = new List<MainPage>();
            MainPage temp = _context.MainPage.Where(h => h.Banner == "big").FirstOrDefault();
            if (temp != null) bannerList.Add(temp);
            temp = _context.MainPage.Where(h => h.Banner == "medium").FirstOrDefault();
            if (temp != null) bannerList.Add(temp);
            temp = _context.MainPage.Where(h => h.Banner == "small").FirstOrDefault();
            if (temp != null) bannerList.Add(temp);
            foreach (var banner in bannerList)
            {

                banner.Images = _context.MainPageFiles.Where(s => s.MainPageId == banner.Id && (s.Type.Contains("picture"))).OrderBy(n => n.Id).ToList();

            }
            return View(bannerList);
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


        [HttpPost]
        public IActionResult CultureManagement(string culture, string returnUrl)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30) }
                );
            return LocalRedirect(returnUrl);
        }
        [Authorize(Roles = "admin")]
        public IActionResult CreateBanner()
        {
            List<Banners> banners = new List<Banners>();
            foreach (Banners i in Enum.GetValues(typeof(Banners)))
                banners.Add(i);
            ViewData["BannersList"] = banners;
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateBanner(MainPage mainPage, IFormFileCollection uploadFile)
        {
            if (ModelState.IsValid)
            {
                mainPage.CreatedDate = DateTime.Now;

                _context.MainPage.Add(mainPage);
                await _context.SaveChangesAsync();
                MainPage HomePageData = _context.MainPage.FirstOrDefault(p => p.Title == mainPage.Title);

                if (uploadFile != null)
                {
                    foreach (var upload in uploadFile)
                    {
                        // путь к папке Files
                        string path = "/Files/MainPage/" + upload.FileName;
                        // сохраняем файл в папку Files в каталоге wwwroot
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await upload.CopyToAsync(fileStream);
                        }
                        MainPageFile mainPageFile = new MainPageFile { Path = path, Type = "picture", MainPageId = HomePageData.Id };
                        _context.MainPageFiles.Add(mainPageFile);
                        await _context.SaveChangesAsync();
                    }
                }

            }

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var port = await _context.MainPage.FirstOrDefaultAsync(p => p.Id == id);
            if (port == null)
                return NotFound();
            return View(port);
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var port = await _context.MainPage.FindAsync(id);
            var imVid = await _context.MainPageFiles.Where(iv => iv.MainPageId == port.Id).ToListAsync();
            foreach (var iv in imVid)
            {
                _context.MainPageFiles.Remove(iv);
                await _context.SaveChangesAsync();
            }
            _context.MainPage.Remove(port);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");

        }



        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var homePage = await _context.MainPage.FindAsync(id);
            List<MainPageFile> pics = await _context.MainPageFiles.Where(i => i.MainPageId == id && i.Type == "picture").OrderBy(n => n.Id).ToListAsync();

            MainPageFile mainPic = await _context.MainPageFiles.Where(m => m.MainPageId == id && m.Type == "mainPic").FirstOrDefaultAsync();
            MainPageViewModel homePageViewModel = new MainPageViewModel { MainPageFiles = pics, MainPage = homePage, MainPic = mainPic };
            if (homePage == null)
            {
                return NotFound();
            }
            return View(homePageViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id, MainPage mainPage)
        {
            if (id != mainPage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mainPage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomePageExists(mainPage.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { id = id });
            }
            return View(mainPage);
        }

        private bool HomePageExists(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditUploadFile(int id, IFormFile uploadFilePic,  string description, string title)
        {

            MainPageFile pic = await _context.MainPageFiles.FirstOrDefaultAsync(p => p.Id == id);
            if (uploadFilePic == null)
            {
                pic.Description = description;
                pic.Title = title;
                _context.Update(pic);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", new { id = pic.MainPageId });
            }
            string path = "/Files/MainPage/" + uploadFilePic.FileName;

            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                await uploadFilePic.CopyToAsync(fileStream);
            }
            if (uploadFilePic != null)
            {
                pic.Path = path;
                pic.Description = description;
                pic.Title = title;
                _context.Update(pic);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", new { id = pic.MainPageId });
            }

            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateUpload(int id, string description, IFormFile uploadFilePic)
        {
            if (uploadFilePic != null)
            {
                string path = "/Files/MainPage/" + uploadFilePic.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadFilePic.CopyToAsync(fileStream);
                }
                MainPageFile pic = new MainPageFile { Path = path, Type = "picture", MainPageId = id, Description = description };
                _context.Add(pic);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", new { id = id });
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUpload(int id)
        {
            MainPageFile upload = await _context.MainPageFiles.FindAsync(id);
            if (upload.MainPageId != null)
            {
                string FilePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot{upload.Path}");
                System.IO.File.Delete(FilePath);
                _context.Remove(upload);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { id = upload.MainPageId });
            }
            else
            {
                return NotFound();
            }
        }


    }
}
