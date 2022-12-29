using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Autopodbor_312.Controllers
{
	public class NewsController : Controller
	{
		private readonly AutopodborContext _context;
		private readonly IWebHostEnvironment _appEnvironment;

		public NewsController(AutopodborContext context, IWebHostEnvironment webHost)
		{
			_context = context;
			_appEnvironment = webHost;
		}

		public async Task<IActionResult> Index()
		{
			List<News> newsPublished = await _context.News.Where(n => n.Publicate == true).OrderByDescending(n => n.CreatedDate).ToListAsync();
			return View(newsPublished);
		}

		[Authorize(Roles = "admin,mediaManager")]
		public async Task<IActionResult> News()
		{
			List<News> news = await _context.News.OrderByDescending(n => n.CreatedDate).ToListAsync();
			return View(news);
		}

		[Authorize(Roles = "admin,mediaManager")]
		public IActionResult CreateNews()
		{
			return View();
		}

		[HttpPost]
		[Authorize(Roles = "admin,mediaManager")]
		public async Task<IActionResult> CreateNews(News news, IFormFile mainPic, IFormFileCollection uploadFiles, string video)
		{
            if (ModelState.IsValid)
			{
				news.CreatedDate = DateTime.Now;
				news.Publicate = false;
                _context.News.Add(news);
				await _context.SaveChangesAsync();
                news.MainImagePath = $"/newsPortfolioFiles/newsFiles/mainPicId={news.Id}&{mainPic.FileName}";
                if (mainPic != null)
                {
                    string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/newsPortfolioFiles/newsFiles/mainPicId={news.Id}&{mainPic.FileName}");
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await mainPic.CopyToAsync(fileStream);
                    }
                    PortfolioNewsFile update = new PortfolioNewsFile { Path = $"/newsPortfolioFiles/newsFiles/mainPicId={news.Id}&{mainPic.FileName}", Type = "mainPic", NewsId = news.Id };
                    _context.PortfolioNewsFiles.Add(update);
                    await _context.SaveChangesAsync();
                }
                if (uploadFiles != null)
				{
					foreach (var upload in uploadFiles)
					{
                        PortfolioNewsFile portfolioNewsFile = new PortfolioNewsFile {Type = "picture", NewsId = news.Id };
                        _context.PortfolioNewsFiles.Add(portfolioNewsFile);
                        await _context.SaveChangesAsync();
                        string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/newsPortfolioFiles/newsFiles/Id={portfolioNewsFile.Id}&{upload.FileName}");
                        portfolioNewsFile.Path = $"/newsPortfolioFiles/newsFiles/Id={portfolioNewsFile.Id}&{upload.FileName}";
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
						{
							await upload.CopyToAsync(fileStream);
						}
						
					}					
					if (video != null)
					{
						string[] paths = video.Split(' ');
						foreach (var path in paths)
						{
							PortfolioNewsFile vid = new PortfolioNewsFile { Path = "https://www.youtube.com/embed/" + path, Type = "video", NewsId = news.Id };
							_context.Add(vid);
							await _context.SaveChangesAsync();
						}
					}
					return RedirectToAction("News");
				}
			}
			return RedirectToAction("News");
		}

		public async Task<IActionResult> DetailsNews(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			News news = await _context.News.FirstOrDefaultAsync(p => p.Id == id);
			List<PortfolioNewsFile> minorImg = await _context.PortfolioNewsFiles.Where(i => i.NewsId == id && i.Type == "picture").ToListAsync();
			List<PortfolioNewsFile> videos = await _context.PortfolioNewsFiles.Where(v => v.NewsId == id && v.Type == "video").ToListAsync();
			NewsDetailsViewModel newsDetailsViewModel = new NewsDetailsViewModel { News = news, MinorPictures = minorImg, Videos = videos };
			return View(newsDetailsViewModel);
		}

        [Authorize(Roles = "admin,mediaManager")]
        public async Task<IActionResult> EditNews(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var news = await _context.News.FindAsync(id);
            List<PortfolioNewsFile> pics = await _context.PortfolioNewsFiles.Where(i => i.NewsId == id && i.Type == "picture").ToListAsync();
            List<PortfolioNewsFile> vids = await _context.PortfolioNewsFiles.Where(v => v.NewsId == id && v.Type == "video").ToListAsync();
            PortfolioNewsFile mainPic = await _context.PortfolioNewsFiles.Where(m => m.NewsId == id && m.Type == "mainPic").FirstOrDefaultAsync();
            NewsDetailsViewModel newsDetailsViewModel = new NewsDetailsViewModel { MinorPictures = pics, Videos = vids, News = news, MainPic = mainPic };
            if (news == null)
            {
                return NotFound();
            }
            return View(newsDetailsViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "admin,mediaManager")]
        public async Task<IActionResult> EditNews(int? id, News news)
        {
            if (id != news.Id)
            {
                return NotFound();
            }
            news.CreatedDate = DateTime.Now;
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
        [Authorize(Roles = "admin,mediaManager")]
        public async Task<IActionResult> EditMainPhoto(int? id, IFormFile newPhoto)
        {
            if (id == null || newPhoto == null)
            {
                return NotFound();
            }
            PortfolioNewsFile portfolioNewsFile = await _context.PortfolioNewsFiles.FirstOrDefaultAsync(p => p.Id == id);
            if (ModelState.IsValid)
            {
                try
                {
                    News news = await _context.News.FirstOrDefaultAsync(p => p.Id == portfolioNewsFile.NewsId);
                    string oldFilePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot{news.MainImagePath}");
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                    string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/newsPortfolioFiles/newsFiles/mainPicId={news.Id}&{newPhoto.FileName}");
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await newPhoto.CopyToAsync(fileStream);
                    }
                    portfolioNewsFile.Path = $"/newsPortfolioFiles/newsFiles/mainPicId={news.Id}&{newPhoto.FileName}";
                    news.MainImagePath = $"/newsPortfolioFiles/newsFiles/mainPicId={news.Id}&{newPhoto.FileName}";
                    _context.News.Update(news);
                    await _context.SaveChangesAsync();
                    _context.PortfolioNewsFiles.Update(portfolioNewsFile);
                    await _context.SaveChangesAsync();
                    UpdateCreationDate(portfolioNewsFile.NewsId);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PortfolioNewsFilesExists(portfolioNewsFile.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction("EditNews", new { id = portfolioNewsFile.NewsId});
        }

        [HttpPost]
        [Authorize(Roles = "admin,mediaManager")]
        public async Task<IActionResult> EditMinorPhoto(int? id, IFormFile newPhoto)
        {
            if (id == null || newPhoto == null)
            {
                return NotFound();
            }
            PortfolioNewsFile portfolioNewsFile = await _context.PortfolioNewsFiles.FirstOrDefaultAsync(p => p.Id == id);
            if (portfolioNewsFile == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    string oldFilePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot{portfolioNewsFile.Path}");
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                    string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/newsPortfolioFiles/newsFiles/Id={id}&{newPhoto.FileName}");
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await newPhoto.CopyToAsync(fileStream);
                    }
                    portfolioNewsFile.Path = $"/newsPortfolioFiles/newsFiles/Id={id}&{newPhoto.FileName}";
                    _context.PortfolioNewsFiles.Update(portfolioNewsFile);
                    await _context.SaveChangesAsync();
                    UpdateCreationDate(portfolioNewsFile.NewsId);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PortfolioNewsFilesExists(portfolioNewsFile.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction("EditNews", new { id = portfolioNewsFile.NewsId });
        }

        [HttpPost]
        [Authorize(Roles = "admin,mediaManager")]
        public async Task<IActionResult> DeletePhotoOrVideo(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            PortfolioNewsFile portfolioNewsFile = await _context.PortfolioNewsFiles.FirstOrDefaultAsync(p => p.Id == id);
            if (portfolioNewsFile == null)
            {
                return NotFound();
            }
            string FilePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot{portfolioNewsFile.Path}");
            if (System.IO.File.Exists(FilePath))
            {
                System.IO.File.Delete(FilePath);
            }
            _context.PortfolioNewsFiles.Remove(portfolioNewsFile);
            await _context.SaveChangesAsync();
            UpdateCreationDate(portfolioNewsFile.NewsId);
            return RedirectToAction("EditNews", new { id = portfolioNewsFile.NewsId });
        }

        [HttpPost]
        [Authorize(Roles = "admin,mediaManager")]
        public async Task<IActionResult> AddMinorPhoto(int? id, IFormFile newPhoto)
        {
            if (id == null || newPhoto == null)
            {
                return NotFound();
            }
            PortfolioNewsFile portfolioNewsFile = new PortfolioNewsFile {Type = "picture", NewsId = id };
            _context.PortfolioNewsFiles.Add(portfolioNewsFile);
            await _context.SaveChangesAsync();
            portfolioNewsFile.Path = $"/newsPortfolioFiles/newsFiles/Id={portfolioNewsFile.Id}&{newPhoto.FileName}";
            _context.PortfolioNewsFiles.Update(portfolioNewsFile);
            await _context.SaveChangesAsync();
            string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/newsPortfolioFiles/newsFiles/Id={portfolioNewsFile.Id}&{newPhoto.FileName}");
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await newPhoto.CopyToAsync(fileStream);
            }
            UpdateCreationDate(id);
            return RedirectToAction("EditNews", new { id = id });
        }

        [HttpPost]
        [Authorize(Roles = "admin,mediaManager")]
        public async Task<IActionResult> EditVideo(int? id, string newVideoId)
        {
            if (id == null || newVideoId == null)
            {
                return NotFound();
            }
            PortfolioNewsFile portfolioNewsFile = await _context.PortfolioNewsFiles.FirstOrDefaultAsync(p => p.Id == id);
            if (portfolioNewsFile == null)
            {
                return NotFound();
            }
            portfolioNewsFile.Path = "https://www.youtube.com/embed/" + newVideoId;
            _context.PortfolioNewsFiles.Update(portfolioNewsFile);
            await _context.SaveChangesAsync();
            UpdateCreationDate(portfolioNewsFile.NewsId);
            return RedirectToAction("EditNews", new { id = portfolioNewsFile.NewsId });
        }

        [HttpPost]
        [Authorize(Roles = "admin,mediaManager")]
        public async Task<IActionResult> AddVideo(int? id, string videoId)
        {
            if (id == null || videoId == null)
            {
                return NotFound();
            }
            PortfolioNewsFile portfolioNewsFile = new PortfolioNewsFile { Path = "https://www.youtube.com/embed/" + videoId, Type = "video", NewsId = id };
            _context.PortfolioNewsFiles.Add(portfolioNewsFile);
            await _context.SaveChangesAsync();
            UpdateCreationDate(id);
            return RedirectToAction("EditNews", new { id = id });
        }

        [Authorize(Roles = "admin,mediaManager")]
        public async Task<IActionResult> DeleteNews(int? id)
        {
            if (id == null)
                return NotFound();
            News news = await _context.News.FirstOrDefaultAsync(p => p.Id == id);
            if (news == null)
                return NotFound();
            return View(news);
        }

        [HttpPost]
		[Authorize(Roles = "admin,mediaManager")]
		public async Task<IActionResult> DeleteNews(int id)
		{
            News news = await _context.News.FirstOrDefaultAsync(p => p.Id == id);
            List<PortfolioNewsFile> portfolioNewsFiles = await _context.PortfolioNewsFiles.Where(p => p.NewsId == id).ToListAsync();
            foreach (var n in portfolioNewsFiles)
            {
                string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot{n.Path}");
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                _context.PortfolioNewsFiles.Remove(n);
            }
            await _context.SaveChangesAsync();
            _context.Remove(news);
            await _context.SaveChangesAsync();
            return RedirectToAction("News", "News");
        }

        public async Task<IActionResult> PublicNews(int id)
        {
            News news = _context.News.FirstOrDefault(n => n.Id == id);
            if (news.Publicate == false)
                news.Publicate = true;
            else
                news.Publicate = false;
            _context.Update(news);
            await _context.SaveChangesAsync();
            UpdateCreationDate(id);
            return Ok();
        }

        private bool NewsExists(int id)
		{
			return _context.News.Any(e => e.Id == id);
		}

        private bool PortfolioNewsFilesExists(int id)
        {
            return _context.PortfolioNewsFiles.Any(e => e.Id == id);
        }

        private void UpdateCreationDate(int? id)
        {
            News news = _context.News.FirstOrDefault(p => p.Id == id);
            news.CreatedDate = DateTime.Now;
            _context.News.Update(news);
            _context.SaveChangesAsync();
        }
    }
}
