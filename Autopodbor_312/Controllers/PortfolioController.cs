using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Autopodbor_312.Controllers
{
	public class PortfolioController : Controller
	{
		private readonly AutopodborContext _context;
		private readonly IWebHostEnvironment _appEnvironment;
		private readonly UserManager<User> _userManager;

		public PortfolioController(AutopodborContext context, IWebHostEnvironment appEnvironment, UserManager<User> userManager)
		{
			_context = context;
			_appEnvironment = appEnvironment;
			_userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			List<Portfolio> portfoliosPublished = await _context.Portfolio.Where(p => p.Publicate == true).ToListAsync();
			return View(portfoliosPublished);
		}

		[Authorize(Roles = "admin,portfolioManager")]
		public async Task<IActionResult> Portfolio()
		{
			List<Portfolio> portfolios = await _context.Portfolio.ToListAsync();
			return View(portfolios);
		}


		[Authorize(Roles = "admin,portfolioManager")]
		public IActionResult CreatePortfolio()
		{
			return View();
		}

		[HttpPost]
		[Authorize(Roles = "admin,portfolioManager")]
		public async Task<IActionResult> CreatePortfolio(Portfolio portfolio, IFormFile mainPic, IFormFileCollection uploadFiles, string video)
		{
			if (ModelState.IsValid)
			{
				portfolio.CreatedDate = DateTime.Now;
				portfolio.Publicate = false;
				portfolio.MainImagePath = $"/newsPortfolioFiles/{mainPic.FileName}";
				_context.Portfolio.Add(portfolio);
				await _context.SaveChangesAsync();
				if (mainPic != null)
				{
					string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/newsPortfolioFiles/{mainPic.FileName}");
					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						await mainPic.CopyToAsync(fileStream);
					}
					PortfolioNewsFile update = new PortfolioNewsFile { Path = $"/newsPortfolioFiles/{mainPic.FileName}", Type = "mainPic", PortfolioId = portfolio.Id };
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
						PortfolioNewsFile update = new PortfolioNewsFile { Path = $"/newsPortfolioFiles/{upload.FileName}", Type = "picture", PortfolioId = portfolio.Id };
						_context.PortfolioNewsFiles.Add(update);
						await _context.SaveChangesAsync();
					}
					if (video != null)
					{
						string[] paths = video.Split(' ');
						foreach (var path in paths)
						{
							PortfolioNewsFile vid = new PortfolioNewsFile { Path = "https://www.youtube.com/embed/" + path, Type = "video", PortfolioId = portfolio.Id };
							_context.Add(vid);
							await _context.SaveChangesAsync();
						}
					}
					return RedirectToAction("Portfolio");
				}
			}
			return RedirectToAction("Portfolio");
		}

		[Authorize(Roles = "admin,portfolioManager")]
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
		[Authorize(Roles = "admin,portfolioManager")]
		public async Task<IActionResult> DeletePortfolio(int id)
		{
			var port = await _context.Portfolio.FindAsync(id);
			var imVid = await _context.PortfolioNewsFiles.Where(iv => iv.PortfolioId == id).ToListAsync();
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
			_context.Remove(port);
			await _context.SaveChangesAsync();
			return RedirectToAction("Portfolio", "Media");

		}

		[Authorize(Roles = "admin,portfolioManager")]
		public async Task<IActionResult> EditPortfolio(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var port = await _context.Portfolio.FindAsync(id);
			List<PortfolioNewsFile> pics = await _context.PortfolioNewsFiles.Where(i => i.PortfolioId == id && i.Type == "picture").ToListAsync();
			List<PortfolioNewsFile> vids = await _context.PortfolioNewsFiles.Where(v => v.PortfolioId == id && v.Type == "video").ToListAsync();
			PortfolioNewsFile mainPic = await _context.PortfolioNewsFiles.Where(m => m.PortfolioId == id && m.Type == "mainPic").FirstOrDefaultAsync();
			PortfolioAndUploadFileViewModel test = new PortfolioAndUploadFileViewModel { Pictures = pics, Videos = vids, Portfolio = port, MainPic = mainPic };
			if (port == null)
			{
				return NotFound();
			}
			return View(test);
		}

		[HttpPost]
		[Authorize(Roles = "admin,portfolioManager")]
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

		public async Task<IActionResult> PublicPortfolio(int id)
		{
			Portfolio portfolio = _context.Portfolio.FirstOrDefault(p => p.Id == id);
			if (portfolio.Publicate == false)
				portfolio.Publicate = true;
			else
				portfolio.Publicate = false;
			_context.Update(portfolio);
			await _context.SaveChangesAsync();
			return Ok();
		}

		public async Task<IActionResult> DetailsPortfolio(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var port = await _context.Portfolio.FindAsync(id);
			List<PortfolioNewsFile> pics = await _context.PortfolioNewsFiles.Where(i => i.PortfolioId == id && i.Type == "picture").ToListAsync();
			List<PortfolioNewsFile> vids = await _context.PortfolioNewsFiles.Where(i => i.PortfolioId == id && i.Type == "video").ToListAsync();
			PortfolioAndUploadFileViewModel test = new PortfolioAndUploadFileViewModel { Pictures = pics, Videos = vids, Portfolio = port };
			if (port == null)
			{
				return NotFound();
			}
			return View(test);
		}

		[HttpPost]
		public async Task<IActionResult> EditUploadFile(int id, IFormFile uploadFilePic, string news, string port, string type, string vid, string videoPath)
		{
			if (vid != null && videoPath != null && news != null)
			{
				PortfolioNewsFile video = await _context.PortfolioNewsFiles.FirstOrDefaultAsync(p => p.Id == id);
				PortfolioNewsFile newVideo = new PortfolioNewsFile { Path = "https://www.youtube.com/embed/" + videoPath, Type = "video", NewsId = video.NewsId };
				_context.Add(newVideo);
				_context.Remove(video);
				await _context.SaveChangesAsync();
				return RedirectToAction("EditNews", new { id = video.NewsId });
			}
			if (vid != null && videoPath != null && port != null)
			{
				PortfolioNewsFile video = await _context.PortfolioNewsFiles.FirstOrDefaultAsync(p => p.Id == id);
				PortfolioNewsFile newVideo = new PortfolioNewsFile { Path = "https://www.youtube.com/embed/" + videoPath, Type = "video", PortfolioId = video.PortfolioId };
				_context.Add(newVideo);
				_context.Remove(video);
				await _context.SaveChangesAsync();
				return RedirectToAction("EditPortfolio", new { id = video.PortfolioId });
			}
			PortfolioNewsFile pic = await _context.PortfolioNewsFiles.FirstOrDefaultAsync(p => p.Id == id);
			string path = "/Files/Pictures" + uploadFilePic.FileName;
			using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
			{
				await uploadFilePic.CopyToAsync(fileStream);
			}
			if (news != null && type == "main")
			{
				PortfolioNewsFile update = new PortfolioNewsFile { Path = path, Type = "mainPic", NewsId = pic.NewsId };
				_context.Add(update);
				_context.Remove(pic);
				await _context.SaveChangesAsync();
				return RedirectToAction("EditNews", new { id = pic.NewsId });
			}
			if (port != null && type == "main")
			{
				PortfolioNewsFile update = new PortfolioNewsFile { Path = path, Type = "mainPic", PortfolioId = pic.PortfolioId };
				_context.Add(update);
				_context.Remove(pic);
				await _context.SaveChangesAsync();
				return RedirectToAction("EditPortfolio", new { id = pic.PortfolioId });
			}
			if (news != null && type == null)
			{
				PortfolioNewsFile update = new PortfolioNewsFile { Path = path, Type = "picture", NewsId = pic.NewsId };
				_context.Add(update);
				_context.Remove(pic);
				await _context.SaveChangesAsync();
				return RedirectToAction("EditNews", new { id = pic.NewsId });
			}
			if (port != null && type == null)
			{
				PortfolioNewsFile update = new PortfolioNewsFile { Path = path, Type = "picture", PortfolioId = pic.PortfolioId };
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
			PortfolioNewsFile upload = await _context.PortfolioNewsFiles.FindAsync(id);
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
				PortfolioNewsFile pic = new PortfolioNewsFile { Path = path, Type = "picture", NewsId = id };
				_context.Add(pic);
				await _context.SaveChangesAsync();
				return RedirectToAction("EditNews", new { id = id });
			}
			if (type == "portfolio" && vid == null && uploadFilePic != null)
			{
				string path = "/Files/Pictures" + uploadFilePic.FileName;
				using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
				{
					await uploadFilePic.CopyToAsync(fileStream);
				}
				PortfolioNewsFile pic = new PortfolioNewsFile { Path = path, Type = "picture", PortfolioId = id };
				_context.Add(pic);
				await _context.SaveChangesAsync();
				return RedirectToAction("EditPortfolio", new { id = id });
			}
			if (type == "news" && vid != null && uploadFilePic == null)
			{
				PortfolioNewsFile newVideo = new PortfolioNewsFile { Path = "https://www.youtube.com/embed/" + video, Type = "video", NewsId = id };
				_context.Add(newVideo);
				await _context.SaveChangesAsync();
				return RedirectToAction("EditNews", new { id = id });
			}
			if (type == "portfolio" && vid != null && uploadFilePic == null)
			{
				PortfolioNewsFile newVideo = new PortfolioNewsFile { Path = "https://www.youtube.com/embed/" + video, Type = "video", PortfolioId = id };
				_context.Add(newVideo);
				await _context.SaveChangesAsync();
				return RedirectToAction("EditPortfolio", new { id = id });
			}
			else
			{
				return NotFound();
			}
		}
	}
}

