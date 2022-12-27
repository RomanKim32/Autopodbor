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

		public async Task<IActionResult> News()
		{
			List<News> news = await _context.News.ToListAsync();
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
                news.MainImagePath = $"/newsPortfolioFiles/{mainPic.FileName}";
                _context.News.Add(news);
				await _context.SaveChangesAsync();
                if (mainPic != null)
                {
                    string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/newsPortfolioFiles/{mainPic.FileName}");
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await mainPic.CopyToAsync(fileStream);
                    }
                    PortfolioNewsFile update = new PortfolioNewsFile { Path = $"/newsPortfolioFiles/{mainPic.FileName}", Type = "mainPic", NewsId = news.Id };
                    _context.PortfolioNewsFiles.Add(update);
                    await _context.SaveChangesAsync();
                }
                if (uploadFiles != null)
				{
					foreach (var upload in uploadFiles)
					{
						string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/newsPortfolioFiles/{upload.FileName}");
						using (var fileStream = new FileStream(filePath, FileMode.Create))
						{
							await mainPic.CopyToAsync(fileStream);
						}
						PortfolioNewsFile update = new PortfolioNewsFile { Path = $"/newsPortfolioFiles/{upload.FileName}", Type = "picture", NewsId = news.Id };
						_context.PortfolioNewsFiles.Add(update);
						await _context.SaveChangesAsync();
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

		[Authorize(Roles = "admin,mediaManager")]
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
		[Authorize(Roles = "admin,mediaManager")]
		public async Task<IActionResult> DeleteNews(int id)
		{
			var news = await _context.News.FindAsync(id);
			var imVid = await _context.PortfolioNewsFiles.Where(iv => iv.NewsId == id).ToListAsync();
			foreach (var iv in imVid)
			{
				if (iv.Type == "picture" && iv.Type == "mainPic")
				{
					_context.PortfolioNewsFiles.Remove(iv);
					System.IO.File.Delete(iv.Path);
					await _context.SaveChangesAsync();
				}
				else if (iv.Type == "video")
				{
					_context.PortfolioNewsFiles.Remove(iv);
					await _context.SaveChangesAsync();
				}
			}
			_context.News.Remove(news);
			await _context.SaveChangesAsync();
			return RedirectToAction("IndexNews", "Media");
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
			NewsAndUploadFileViewModel test = new NewsAndUploadFileViewModel { Pictures = pics, Videos = vids, News = news, MainPic = mainPic };
			if (news == null)
			{
				return NotFound();
			}
			return View(test);
		}

		[HttpPost]
		[Authorize(Roles = "admin,mediaManager")]
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

		private bool NewsExists(int id)
		{
			return _context.News.Any(e => e.Id == id);
		}
	}
}
