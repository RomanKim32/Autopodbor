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
	public class PortfolioController : Controller
	{
		private readonly AutopodborContext _context;
		private readonly IWebHostEnvironment _appEnvironment;

		public PortfolioController(AutopodborContext context, IWebHostEnvironment appEnvironment)
		{
			_context = context;
			_appEnvironment = appEnvironment;
		}

		public async Task<IActionResult> Index()
		{
            List<Portfolio> portfoliosPublished = await _context.Portfolio.Where(p => p.Publicate == true).OrderByDescending(p => p.CreatedDate).ToListAsync();
			return View(portfoliosPublished);
		}

		[Authorize(Roles = "admin,portfolioManager")]
		public async Task<IActionResult> Portfolio()
		{
			List<Portfolio> portfolios = await _context.Portfolio.OrderByDescending(p => p.CreatedDate).ToListAsync();
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
				portfolio.MainImagePath = $"/newsPortfolioFiles/portfolioFiles/{mainPic.FileName}";
				_context.Portfolio.Add(portfolio);
				await _context.SaveChangesAsync();
				if (mainPic != null)
				{
					string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/newsPortfolioFiles/portfolioFiles/{mainPic.FileName}");
					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						await mainPic.CopyToAsync(fileStream);
					}
					PortfolioNewsFile update = new PortfolioNewsFile { Path = $"/newsPortfolioFiles/portfolioFiles/{mainPic.FileName}", Type = "mainPic", PortfolioId = portfolio.Id };
					_context.PortfolioNewsFiles.Add(update);
					await _context.SaveChangesAsync();
				}

				if (uploadFiles != null)
				{
					foreach (var upload in uploadFiles)
					{
						string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/newsPortfolioFiles/portfolioFiles/{upload.FileName}");
						using (var fileStream = new FileStream(filePath, FileMode.Create))
						{
							await upload.CopyToAsync(fileStream);
						}
						PortfolioNewsFile update = new PortfolioNewsFile { Path = $"/newsPortfolioFiles/portfolioFiles/{upload.FileName}", Type = "picture", PortfolioId = portfolio.Id };
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

		public async Task<IActionResult> DetailsPortfolio(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			Portfolio portfolio = await _context.Portfolio.FirstOrDefaultAsync(p => p.Id == id);
			List<PortfolioNewsFile> minorImg = await _context.PortfolioNewsFiles.Where(i => i.PortfolioId == id && i.Type == "picture").ToListAsync();
			List<PortfolioNewsFile> videos = await _context.PortfolioNewsFiles.Where(v => v.PortfolioId == id && v.Type == "video").ToListAsync();
			PortfolioDetailsViewModel portfolioDetailsViewModel = new PortfolioDetailsViewModel { Portfolio = portfolio, MinorPictures = minorImg, Videos = videos };
			return View(portfolioDetailsViewModel);
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
			PortfolioDetailsViewModel portfolioDetailsViewModel = new PortfolioDetailsViewModel { MinorPictures = pics, Videos = vids, Portfolio = port, MainPic = mainPic };
			if (port == null)
			{
				return NotFound();
			}
			return View(portfolioDetailsViewModel);
		}

		[HttpPost]
		[Authorize(Roles = "admin,portfolioManager")]
		public async Task<IActionResult> EditPortfolio(int? id, Portfolio portfolio)
		{
			if (id != portfolio.Id)
			{
				return NotFound();
			}
			portfolio.CreatedDate= DateTime.Now;
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

		[HttpPost]
        [Authorize(Roles = "admin,portfolioManager")]
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
                    string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/newsPortfolioFiles/portfolioFiles/{newPhoto.FileName}");
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await newPhoto.CopyToAsync(fileStream);
                    }
                    portfolioNewsFile.Path = $"/newsPortfolioFiles/portfolioFiles/{newPhoto.FileName}";
                    Portfolio portfolio = await _context.Portfolio.FirstOrDefaultAsync(p => p.Id == portfolioNewsFile.PortfolioId);
                    portfolio.MainImagePath = $"/newsPortfolioFiles/portfolioFiles/{newPhoto.FileName}";
                    _context.Portfolio.Update(portfolio);
                    await _context.SaveChangesAsync();
                    _context.PortfolioNewsFiles.Update(portfolioNewsFile);
                    await _context.SaveChangesAsync();
                    UpdateCreationDate(portfolioNewsFile.PortfolioId);
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
			return RedirectToAction("EditPortfolio", new { id = portfolioNewsFile.PortfolioId });
        }

        [HttpPost]
        [Authorize(Roles = "admin,portfolioManager")]
        public async Task<IActionResult> EditMinorPhoto(int? id ,IFormFile newPhoto)
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
                    string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/newsPortfolioFiles/portfolioFiles/{newPhoto.FileName}");
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await newPhoto.CopyToAsync(fileStream);
                    }
                    portfolioNewsFile.Path = $"/newsPortfolioFiles/portfolioFiles/{newPhoto.FileName}";
                    _context.PortfolioNewsFiles.Update(portfolioNewsFile);
                    await _context.SaveChangesAsync();
                    UpdateCreationDate(portfolioNewsFile.PortfolioId);
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
            return RedirectToAction("EditPortfolio", new { id = portfolioNewsFile.PortfolioId });
        }

		[HttpPost]
        [Authorize(Roles = "admin,portfolioManager")]
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
			_context.PortfolioNewsFiles.Remove(portfolioNewsFile);
            await _context.SaveChangesAsync();
            UpdateCreationDate(portfolioNewsFile.PortfolioId);
            return RedirectToAction("EditPortfolio", new { id = portfolioNewsFile.PortfolioId});
        }

		[HttpPost]
        [Authorize(Roles = "admin,portfolioManager")]
        public async Task<IActionResult> AddMinorPhoto(int? id, IFormFile newPhoto)
		{
			if (id == null || newPhoto == null)
			{
                return NotFound();
            }
            string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/newsPortfolioFiles/portfolioFiles/{newPhoto.FileName}");
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await newPhoto.CopyToAsync(fileStream);
            }
            PortfolioNewsFile portfolioNewsFile = new PortfolioNewsFile { Path = $"/newsPortfolioFiles/portfolioFiles/{newPhoto.FileName}", Type = "picture", PortfolioId = id };
            _context.PortfolioNewsFiles.Add(portfolioNewsFile);
            await _context.SaveChangesAsync();
            UpdateCreationDate(id);
            return RedirectToAction("EditPortfolio", new { id = id });
        }

		[HttpPost]
        [Authorize(Roles = "admin,portfolioManager")]
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
            UpdateCreationDate(portfolioNewsFile.PortfolioId);
            return RedirectToAction("EditPortfolio", new { id = portfolioNewsFile.PortfolioId });
        }

        [HttpPost]
        [Authorize(Roles = "admin,portfolioManager")]
        public async Task<IActionResult> AddVideo(int? id, string videoId)
        {
            if (id == null || videoId == null)
            {
                return NotFound();
            }
            PortfolioNewsFile portfolioNewsFile = new PortfolioNewsFile { Path = "https://www.youtube.com/embed/" + videoId, Type = "video", PortfolioId = id };
            _context.PortfolioNewsFiles.Add(portfolioNewsFile);
            await _context.SaveChangesAsync();
            UpdateCreationDate(id);
            return RedirectToAction("EditPortfolio", new { id = id });
        }

        [Authorize(Roles = "admin,portfolioManager")]
        public async Task<IActionResult> DeletePortfolio(int? id)
        {
            if (id == null)
                return NotFound();
            var portfolio = await _context.Portfolio.FirstOrDefaultAsync(p => p.Id == id);
            if (portfolio == null)
                return NotFound();
            return View(portfolio);
        }

        [HttpPost]
        [Authorize(Roles = "admin,portfolioManager")]
        public async Task<IActionResult> DeletePortfolio(int id)
        {
            Portfolio portfolio = await _context.Portfolio.FirstOrDefaultAsync(p => p.Id == id);
            List<PortfolioNewsFile> portfolioNewsFiles = await _context.PortfolioNewsFiles.Where(p => p.PortfolioId == id).ToListAsync();
            foreach (var p in portfolioNewsFiles)
            {
                _context.PortfolioNewsFiles.Remove(p);
            }
            await _context.SaveChangesAsync();
            _context.Remove(portfolio);
            await _context.SaveChangesAsync();
            return RedirectToAction("Portfolio", "Portfolio");

        }

        private bool PortfolioNewsFilesExists(int id)
		{
			return _context.PortfolioNewsFiles.Any(e => e.Id == id);
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
            UpdateCreationDate(portfolio.Id);
            return Ok();
        }

        private void UpdateCreationDate(int? id)
        {
            Portfolio portfolio = _context.Portfolio.FirstOrDefault(p => p.Id == id);
            portfolio.CreatedDate = DateTime.Now;
            _context.Portfolio.Update(portfolio);
            _context.SaveChangesAsync();
        }
    }
}

