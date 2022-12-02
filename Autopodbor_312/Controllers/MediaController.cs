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

        public IActionResult IndexNews()
        {
            var newsList = _context.News.ToList();
            foreach (var news in newsList)
            {
                news.Images = _context.UploadFiles.Where(s => s.NewsId == news.Id && (s.Type.Contains("photo"))).ToList();
                news.Videos = _context.UploadFiles.Where(s => s.NewsId == news.Id && (s.Type.Contains("video"))).ToList();
            }
            
            return View(newsList.ToList());
        }
        public IActionResult CreateNews()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNews(News news, IFormFileCollection uploadFile, IFormFile video)
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


        public async Task<IActionResult> DeleteNews(int? id)
        {
            if (id == null)
                return NotFound();
            var news = await _context.News.FirstOrDefaultAsync(p => p.Id == id);
            if (news == null)
                return NotFound();
            return View(news);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var news = await _context.News.FindAsync(id);
            var imVid = await _context.UploadFiles.Where(iv => iv.NewsId == news.Id).ToListAsync();
            foreach (var iv in imVid)
            {
                _context.UploadFiles.Remove(iv);
                System.IO.File.Delete(iv.Path);
                await _context.SaveChangesAsync();
            }
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return RedirectToAction("IndexNews", "Media");

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
                else if (file.Length > 0 && (file.FileName.Contains(".jpg") || file.FileName.Contains(".png") || file.FileName.Contains(".jpeg") || file.FileName.Contains(".img")))
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
                System.IO.File.Delete(iv.Path);
                await _context.SaveChangesAsync();
            }            
            _context.Portfolio.Remove(port);
            await _context.SaveChangesAsync();
            return RedirectToAction("IndexPortfolio", "Media");            
        }        

        public async Task<IActionResult> EditPortfolio(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var port = await _context.Portfolio.FindAsync(id);
            List<UploadFile> pics = await _context.UploadFiles.Where(i => i.PortfolioId == id && i.Type == "picture").ToListAsync();
            List<UploadFile> vids = await _context.UploadFiles.Where(i => i.PortfolioId == id && i.Type == "video").ToListAsync();
            PortfolioAndUploadFileViewModel test = new PortfolioAndUploadFileViewModel { Pictures = pics, Videos = vids, Portfolio = port };
            if (port == null)
            {
                return NotFound();
            }
            return View(test);
        }

        [HttpPost]
        public async Task<IActionResult> EditPortfolio(int? id, Portfolio portfolio)
        {
            if (id != portfolio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(portfolio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PortfolioExists(portfolio.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("EditPortfolio", new { id = id });
            }
            return View(portfolio);
        }        

        private bool PortfolioExists(int id)
        {
            return _context.Portfolio.Any(e => e.Id == id);
        }

        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUpload(int? id)
        {
            var upload = await _context.UploadFiles.FindAsync(id);
            _context.Remove(upload);
            System.IO.File.Delete(upload.Path);
            await _context.SaveChangesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditUploadFilePortfolio(IFormFile uploadFilePic, IFormFile uploadFileVid, int id)
        {
            var upload = await _context.UploadFiles.FindAsync(id);
            string strDateTime = DateTime.Now.ToString("ddMMyyyyHHMMss");
            if ((uploadFilePic == null && uploadFileVid != null) || (uploadFilePic != null && uploadFileVid == null))
            {
                if (upload.Type == "picture")
                {
                    string finalPathVid = "/Files/Pictures/" + strDateTime + uploadFilePic.FileName;
                    UploadFile picture = new UploadFile
                    {
                        Path = finalPathVid,
                        PortfolioId = upload.PortfolioId,
                        Type = "picture"
                    };
                    _context.Add(picture);
                    using (Stream fileStream = new FileStream(_appEnvironment.WebRootPath + finalPathVid, FileMode.Create))
                    {
                        await uploadFilePic.CopyToAsync(fileStream);
                    }
                }
                if (upload.Type == "video")
                {
                    string finalPathVid = "/Files/Videos/" + strDateTime + uploadFileVid.FileName;
                    UploadFile video = new UploadFile
                    {
                        Path = finalPathVid,
                        PortfolioId = upload.PortfolioId,
                        Type = "video"
                    };
                    _context.Add(video);
                    using (Stream fileStream = new FileStream(_appEnvironment.WebRootPath + finalPathVid, FileMode.Create))
                    {
                        await uploadFileVid.CopyToAsync(fileStream);
                    }
                }
                _context.Remove(upload);
                System.IO.File.Delete(_appEnvironment.WebRootPath + upload.Path);
                await _context.SaveChangesAsync();
                return RedirectToAction("EditPortfolio", new { id = upload.PortfolioId });
            }
            else            
                return RedirectToAction("EditPortfolio", new { id = upload.PortfolioId });            
        }

        public async Task<IActionResult> DetailsPortfolio(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var port = await _context.Portfolio.FindAsync(id);
            List<UploadFile> pics = await _context.UploadFiles.Where(i => i.PortfolioId == id && i.Type == "picture").ToListAsync();
            List<UploadFile> vids = await _context.UploadFiles.Where(i => i.PortfolioId == id && i.Type == "video").ToListAsync();
            PortfolioAndUploadFileViewModel test = new PortfolioAndUploadFileViewModel { Pictures = pics, Videos = vids, Portfolio = port };
            if (port == null)
            {
                return NotFound();
            }
            return View(test);
        }
    }
}

