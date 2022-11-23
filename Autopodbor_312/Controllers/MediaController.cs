using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Autopodbor_312.Controllers
{
    public class MediaController : Controller
    {
        AutopodborContext _context;
        IWebHostEnvironment _appEnvironment;
        private readonly UserManager<User> _userManager;

        public MediaController(AutopodborContext context, IWebHostEnvironment appEnvironment, UserManager<User> userManager)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var newsList = _context.News.ToList();
            foreach (var news in newsList)
            {
                news.Images = _context.UploadFiles.Where(s => s.NewsId == news.Id && (s.Type.Contains("photo"))).ToList();
                news.Videos = _context.UploadFiles.Where(s => s.NewsId == news.Id && (s.Type.Contains("video"))).ToList();
            }
            
            return View(newsList.ToList());
        }
        public IActionResult CreatedNews()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatedNews(News news, IFormFileCollection uploadFile, IFormFile video)
        {
            if (ModelState.IsValid)
            {
                news.CreatedDate = DateTime.Now;
                _context.News.Add(news);
                await _context.SaveChangesAsync();
                News newsData = _context.News.FirstOrDefault(p => p.Name == news.Name);

                if (uploadFile != null)
                {

                    foreach (var upload in uploadFile)
                    {
                        // путь к папке Files
                        string path = "/Files/Pictures" + upload.FileName;
                        // сохраняем файл в папку Files в каталоге wwwroot
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await upload.CopyToAsync(fileStream);
                        }
                        UploadFile uploads = new UploadFile {  Path = path, Type = "picture", NewsId = newsData.Id};
                        _context.UploadFiles.Add(uploads);
                        await _context.SaveChangesAsync();
                    }
                    if (video != null)
                    {
                        string path = "/Files/Videos" + video.FileName;
                        // сохраняем файл в папку Files в каталоге wwwroot
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await video.CopyToAsync(fileStream);
                        }
                        UploadFile uploads = new UploadFile { Path = path, Type = "video", NewsId = newsData.Id };
                        _context.UploadFiles.Add(uploads);
                        await _context.SaveChangesAsync();
                    }

                }
            }

                return RedirectToAction("Index", "Media");
         }
        

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var news = await _context.News.FindAsync(id);
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return Ok();
        }

        public IActionResult CreatePortfolio()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePortfolio(Portfolio portfolio, IFormFileCollection uploadFiles)
        {
            if (ModelState.IsValid)
            {
                portfolio.CreatedDate = DateTime.Now;
                _context.Add(portfolio);
                await _context.SaveChangesAsync();
            }
            string strDateTime = DateTime.Now.ToString("ddMMyyyyHHMMss");

            foreach (var file in uploadFiles)
            {
                if (file.Length > 0 && file.FileName.Contains(".mp4"))
                {
                    string finalPathVid = "/Files/Videos/" + strDateTime + file.FileName;
                    UploadFile video = new UploadFile
                    {
                        Path = finalPathVid,
                        PortfolioId = portfolio.Id,
                        Type = "video"
                    };
                    _context.Add(video);
                    _context.SaveChanges();
                    using (Stream fileStream = new FileStream(_appEnvironment.WebRootPath + finalPathVid, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
                else if (file.Length > 0 && (file.FileName.Contains(".jpg") || file.FileName.Contains(".png") || file.FileName.Contains(".jpeg")))
                {
                    string finalPathPic = "/Files/Pictures/" + strDateTime + file.FileName;
                    UploadFile pic = new UploadFile
                    {
                        Path = finalPathPic,
                        PortfolioId = portfolio.Id,
                        Type = "picture"
                    };
                    _context.Add(pic);
                    _context.SaveChanges();
                    using (Stream fileStream = new FileStream(_appEnvironment.WebRootPath + finalPathPic, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }
            return View();
        }

        public async Task<IActionResult> IndexPortfolio()
        {
            var ports = await _context.Portfolio.ToListAsync();            
            foreach (var port in ports)
            {
                var Image= await _context.UploadFiles.FirstOrDefaultAsync(i => i.PortfolioId == port.Id && i.Type == "picture");
                if (Image != null)
                {
                    port.Image = Image;
                }
            }
            return View(ports);
        }

        public async Task<IActionResult> DeletePortfolio(int? id)
        {
            if (id == null)            
                return NotFound();
            var port = await _context.Portfolio.FirstOrDefaultAsync(p => p.Id == id);
            if (port == null)
                return NotFound();
            return View(port);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePortfolio(int id)
        {
            var port = await _context.Portfolio.FindAsync(id);
            var imVid = await _context.UploadFiles.Where(iv => iv.PortfolioId == port.Id).ToListAsync();
            foreach (var iv in imVid)
            {
                _context.UploadFiles.Remove(iv);
                await _context.SaveChangesAsync();
            }            
            _context.Portfolio.Remove(port);
            await _context.SaveChangesAsync();
            return RedirectToAction("IndexPortfolio", "Media");
            
        }
    }
}

