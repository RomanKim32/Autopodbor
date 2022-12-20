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

        public async Task<IActionResult> IndexNews()
        {
            List<News> news = await _context.News.ToListAsync();
            foreach (var n in news)
            {
                if (n.Image == null)
                {
                    UploadFile mainPic = await _context.UploadFiles.Where(i => i.NewsId == n.Id && i.Type == "mainPic").FirstOrDefaultAsync();
                    n.Image = mainPic;
                    await _context.SaveChangesAsync();
                }                
            }
            return View(news);
        }

        public IActionResult CreateNews()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNews(News news, IFormFile mainPic, IFormFileCollection uploadFiles, string video)
        {
            if (ModelState.IsValid)
            {             
                news.CreatedDate = DateTime.Now;
                _context.Add(news);
                await _context.SaveChangesAsync();
                if (uploadFiles != null)
                {

                    foreach (var upload in uploadFiles)
                    {
                        string path = "/Files/Pictures" + upload.FileName;
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await upload.CopyToAsync(fileStream);
                        }
                        UploadFile update = new UploadFile {  Path = path, Type = "picture", NewsId = news.Id};
                        _context.Add(update);
                        await _context.SaveChangesAsync();
                    }
                    if (mainPic != null)
                    {
                        string path = "/Files/Pictures" + mainPic.FileName;
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await mainPic.CopyToAsync(fileStream);
                        }
                        UploadFile update = new UploadFile { Path = path, Type = "mainPic", NewsId = news.Id };
                        _context.Add(update);
                        await _context.SaveChangesAsync();
                    }
                    if (video != null)
                    {
                        string[] paths = video.Split(' ');
                        foreach (var path in paths)
                        {
                            UploadFile vid = new UploadFile { Path = "https://www.youtube.com/embed/" + path, Type = "video", NewsId = news.Id };
                            _context.Add(vid);
                            await _context.SaveChangesAsync();
                        }                                                
                    }
                    return RedirectToAction("IndexNews");
                }
            }
            return RedirectToAction("IndexNews");
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
            var imVid = await _context.UploadFiles.Where(iv => iv.NewsId == id).ToListAsync();
            foreach (var iv in imVid)
            {
                if (iv.Type == "picture" && iv.Type == "mainPic")
                {
                    _context.UploadFiles.Remove(iv);
                    System.IO.File.Delete(iv.Path);
                    await _context.SaveChangesAsync();
                }
                else if (iv.Type == "video")
                {
                    _context.UploadFiles.Remove(iv);
                    await _context.SaveChangesAsync();
                }
            }
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return RedirectToAction("IndexNews", "Media");

        }

        public async Task<IActionResult> EditNews(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var news = await _context.News.FindAsync(id);
            List<UploadFile> pics = await _context.UploadFiles.Where(i => i.NewsId == id && i.Type == "picture").ToListAsync();
            List<UploadFile> vids = await _context.UploadFiles.Where(v => v.NewsId == id && v.Type == "video").ToListAsync();
            UploadFile mainPic = await _context.UploadFiles.Where(m => m.NewsId == id && m.Type == "mainPic").FirstOrDefaultAsync();
            NewsAndUploadFileViewModel test = new NewsAndUploadFileViewModel { Pictures = pics, Videos = vids, News = news, MainPic = mainPic};
            if (news == null)
            {
                return NotFound();
            }
            return View(test);
        }

        [HttpPost]
        public async Task<IActionResult> EditNews(int? id, News news)
        {
            if (id != news.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(news);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsExists(news.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("EditNews", new { id = id });
            }
            return View(news);
        }        

        [HttpPost]
        public async Task<IActionResult> EditUploadFile(int id, IFormFile uploadFilePic, string news, string port, string type, string vid, string videoPath)
        {
            if(vid != null && videoPath != null && news != null)
            {
                UploadFile video = await _context.UploadFiles.FirstOrDefaultAsync(p => p.Id == id);
                UploadFile newVideo = new UploadFile { Path = "https://www.youtube.com/embed/" + videoPath, Type = "video", NewsId = video.NewsId };
                _context.Add(newVideo);
                _context.Remove(video);
                await _context.SaveChangesAsync();
                return RedirectToAction("EditNews", new { id = video.NewsId });
            }
            if (vid != null && videoPath != null && port != null)
            {
                UploadFile video = await _context.UploadFiles.FirstOrDefaultAsync(p => p.Id == id);
                UploadFile newVideo = new UploadFile { Path = "https://www.youtube.com/embed/" + videoPath, Type = "video", PortfolioId = video.PortfolioId };
                _context.Add(newVideo);
                _context.Remove(video);
                await _context.SaveChangesAsync();
                return RedirectToAction("EditPortfolio", new { id = video.PortfolioId });
            }
            UploadFile pic = await _context.UploadFiles.FirstOrDefaultAsync(p => p.Id == id);            
            string path = "/Files/Pictures" + uploadFilePic.FileName;
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                await uploadFilePic.CopyToAsync(fileStream);
            }
            if (news != null && type == "main")
            {
                UploadFile update = new UploadFile { Path = path, Type = "mainPic", NewsId = pic.NewsId };
                _context.Add(update);
                _context.Remove(pic);
                await _context.SaveChangesAsync();
                return RedirectToAction("EditNews", new { id = pic.NewsId });
            }
            if (port != null && type == "main")
            {
                UploadFile update = new UploadFile { Path = path, Type = "mainPic", PortfolioId = pic.PortfolioId };
                _context.Add(update);
                _context.Remove(pic);
                await _context.SaveChangesAsync();
                return RedirectToAction("EditPortfolio", new { id = pic.PortfolioId });
            }
            if (news != null && type == null)
            {
                UploadFile update = new UploadFile { Path = path, Type = "picture", NewsId = pic.NewsId };
                _context.Add(update);
                _context.Remove(pic);
                await _context.SaveChangesAsync();
                return RedirectToAction("EditNews", new { id = pic.NewsId });
            }
            if (port != null && type == null)
            {
                UploadFile update = new UploadFile { Path = path, Type = "picture", PortfolioId = pic.PortfolioId };
                _context.Add(update);
                _context.Remove(pic);
                await _context.SaveChangesAsync();
                return RedirectToAction("EditPortfolio", new { id = pic.PortfolioId });
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> DeleteUpload(int id, string type)
        {
            UploadFile upload = await _context.UploadFiles.FindAsync(id);
            if (upload.NewsId != null && type == null)
            {
                System.IO.File.Delete(_appEnvironment.WebRootPath + upload.Path);
                _context.Remove(upload);
                await _context.SaveChangesAsync();
                return RedirectToAction("EditNews", new { id = upload.NewsId });
            }
            if (upload.PortfolioId != null && type == null)
            {
                System.IO.File.Delete(_appEnvironment.WebRootPath + upload.Path);
                _context.Remove(upload);
                await _context.SaveChangesAsync();
                return RedirectToAction("EditPortfolio", new { id = upload.PortfolioId });
            }
            if (upload.NewsId != null && type == "vid")
            {
                _context.Remove(upload);
                await _context.SaveChangesAsync();
                return RedirectToAction("EditNews", new { id = upload.NewsId });
            }
            if (upload.PortfolioId != null && type == "vid")
            {                
                _context.Remove(upload);
                await _context.SaveChangesAsync();
                return RedirectToAction("EditPortfolio", new { id = upload.PortfolioId });
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> CreateUpload(int id, string type, string vid, IFormFile uploadFilePic, string video)
        {
            if (type == "news" && vid == null && uploadFilePic != null)
            {
                string path = "/Files/Pictures" + uploadFilePic.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadFilePic.CopyToAsync(fileStream);
                }
                UploadFile pic = new UploadFile { Path = path, Type = "picture", NewsId = id };
                _context.Add(pic);
                await _context.SaveChangesAsync();
                return RedirectToAction("EditNews", new { id = id });
            }
            if (type == "port" && vid == null && uploadFilePic != null)
            {
                string path = "/Files/Pictures" + uploadFilePic.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadFilePic.CopyToAsync(fileStream);
                }
                UploadFile pic = new UploadFile { Path = path, Type = "picture", PortfolioId = id };
                _context.Add(pic);
                await _context.SaveChangesAsync();
                return RedirectToAction("EditPortfolio", new { id = id });
            }
            if (type == "news" && vid != null && uploadFilePic == null)
            {
                UploadFile newVideo = new UploadFile { Path = "https://www.youtube.com/embed/" + video, Type = "video", NewsId = id };
                _context.Add(newVideo);
                await _context.SaveChangesAsync();
                return RedirectToAction("EditNews", new { id = id });
            }
            if (type == "port" && vid != null && uploadFilePic == null)
            {
                UploadFile newVideo = new UploadFile { Path = "https://www.youtube.com/embed/" + video, Type = "video", PortfolioId = id };
                _context.Add(newVideo);
                await _context.SaveChangesAsync();
                return RedirectToAction("EditPortfolio", new { id = id });
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult CreatePortfolio()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePortfolio(Portfolio port, IFormFile mainPic, IFormFileCollection uploadFiles, string video)
        {
            if (ModelState.IsValid)
            {
                port.CreatedDate = DateTime.Now;
                _context.Add(port);
                await _context.SaveChangesAsync();
                if (uploadFiles != null)
                {

                    foreach (var upload in uploadFiles)
                    {
                        string path = "/Files/Pictures" + upload.FileName;
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await upload.CopyToAsync(fileStream);
                        }
                        UploadFile update = new UploadFile { Path = path, Type = "picture", PortfolioId = port.Id };
                        _context.Add(update);
                        await _context.SaveChangesAsync();
                    }
                    if (mainPic != null)
                    {
                        string path = "/Files/Pictures" + mainPic.FileName;
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await mainPic.CopyToAsync(fileStream);
                        }
                        UploadFile update = new UploadFile { Path = path, Type = "mainPic", PortfolioId = port.Id };
                        _context.Add(update);
                        await _context.SaveChangesAsync();
                    }
                    if (video != null)
                    {
                        string[] paths = video.Split(' ');
                        foreach (var path in paths)
                        {
                            UploadFile vid = new UploadFile { Path = "https://www.youtube.com/embed/" + path, Type = "video", PortfolioId = port.Id };
                            _context.Add(vid);
                            await _context.SaveChangesAsync();
                        }
                    }
                    return RedirectToAction("IndexPortfolio");
                }
            }
            return RedirectToAction("IndexPortfolio");
        }

        public async Task<IActionResult> IndexPortfolio()
        {
            List<Portfolio> port = await _context.Portfolio.ToListAsync();
            foreach (var n in port)
            {
                if (n.Image == null)
                {
                    UploadFile mainPic = await _context.UploadFiles.Where(i => i.PortfolioId == n.Id && i.Type == "mainPic").FirstOrDefaultAsync();
                    n.Image = mainPic;
                    await _context.SaveChangesAsync();
                }
            }
            return View(port);
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
            var imVid = await _context.UploadFiles.Where(iv => iv.PortfolioId == id).ToListAsync();
            foreach (var iv in imVid)
            {
                if (iv.Type == "picture" && iv.Type == "mainPic")
                {
                    _context.UploadFiles.Remove(iv);
                    System.IO.File.Delete(iv.Path);
                    await _context.SaveChangesAsync();
                }
                else if (iv.Type == "video")
                {
                    _context.UploadFiles.Remove(iv);
                    await _context.SaveChangesAsync();
                }
            }
            _context.Remove(port);
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
            List<UploadFile> vids = await _context.UploadFiles.Where(v => v.PortfolioId == id && v.Type == "video").ToListAsync();
            UploadFile mainPic = await _context.UploadFiles.Where(m => m.PortfolioId == id && m.Type == "mainPic").FirstOrDefaultAsync();
            PortfolioAndUploadFileViewModel test = new PortfolioAndUploadFileViewModel { Pictures = pics, Videos = vids, Portfolio = port, MainPic = mainPic };
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

