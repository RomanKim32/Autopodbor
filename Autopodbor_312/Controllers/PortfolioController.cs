using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Numerics;
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

		public IActionResult IndexTurnkeySelection(int? bodyType, int? brand, int? model)
		{
			IQueryable<Portfolio> portfolios = _context.Portfolio.Where(p => p.Publicate == true && p.IsFieldInspection == false).OrderByDescending(p => p.CreatedDate)
				.Include(p => p.CarsBodyTypes)
				.Include(p => p.CarsBrands)
				.Include(p => p.CarsBrandsModel);
			return View(GetFilterPortfolioViewModel(portfolios, bodyType, brand, model));
		}

		public IActionResult IndexFieldInspection(int? bodyType, int? brand, int? model)
		{
			IQueryable<Portfolio> portfolios =  _context.Portfolio.Where(p => p.Publicate == true && p.IsFieldInspection == true).OrderByDescending(p => p.CreatedDate)
                .Include(p => p.CarsBodyTypes)
                .Include(p => p.CarsBrands)
                .Include(p => p.CarsBrandsModel);
			return View(GetFilterPortfolioViewModel(portfolios, bodyType, brand, model));
		}

        private FilterPortfolioViewModel GetFilterPortfolioViewModel(IQueryable<Portfolio> portfolios, int? bodyType, int? brand, int? model, int pageNumber = 1)
        {
			if (bodyType != null && bodyType != 0)
			{
				portfolios = portfolios.Where(p => p.CarsBodyTypes.Id == bodyType);
			}
			if (brand != null && brand != 0)
			{
				portfolios = portfolios.Where(p => p.CarsBrands.Id == brand);
			}
			if (model != null && model != 0)
			{
				portfolios = portfolios.Where(p => p.CarsBrandsModel.Id == model);
			}

			List<CarsBodyTypes> carsBodyTypes = _context.CarsBodyTypes.ToList();
			List<CarsBrands> carsBrands = _context.CarsBrands.ToList();
			List<CarsBrandsModel> carsBrandsModels = _context.CarsBrandsModels.ToList();
			carsBodyTypes.Insert(0, new CarsBodyTypes { BodyType = "Все", Id = 0 });
			carsBrands.Insert(0, new CarsBrands { Brand = "Все", Id = 0 });
			carsBrandsModels.Insert(0, new CarsBrandsModel { Model = "Все", Id = 0 });

			var fpvm = new FilterPortfolioViewModel
			{
				Portfolios = PaginationList<Portfolio>.CreateAsync(portfolios.ToList(), pageNumber, 5),
				CarsBodyTypes = new SelectList(carsBodyTypes, "Id", "BodyType"),
				CarsBrands = new SelectList(carsBrands, "Id", "Brand"),
				CarsModels = new SelectList(carsBrandsModels, "Id", "Model"),
			};
            return fpvm;
		}

		[Authorize(Roles = "admin,portfolioManager")]
        public async Task<IActionResult> Portfolio(int pageNumber = 1)
		{
			List<Portfolio> portfolios = await _context.Portfolio.OrderByDescending(p => p.CreatedDate).ToListAsync();
			return View(PaginationList<Portfolio>.CreateAsync(portfolios, pageNumber, 5));
		}


		[Authorize(Roles = "admin,portfolioManager")]
		public IActionResult CreatePortfolio()
		{
            Portfolio portfolio = new Portfolio();
            List<CarsBodyTypes> carsBodyTypes = _context.CarsBodyTypes.ToList();
            List<CarsBrands> carsBrands = _context.CarsBrands.ToList();
            List<CarsBrandsModel> carsBrandsModels = _context.CarsBrandsModels.ToList();
            var createPortfolioViewModel = new CreatePortfolioViewModel
            {
                Portfolio = portfolio,
                CarsBodyTypes = carsBodyTypes,
                CarsBrandsModel= carsBrandsModels,
                CarsBrands = carsBrands,
            };
			return View(createPortfolioViewModel);
		}

		[HttpPost]
		[Authorize(Roles = "admin,portfolioManager")]
		public async Task<IActionResult> CreatePortfolio(Portfolio portfolio, IFormFile mainPic, IFormFileCollection uploadFiles, string video)
		{
            string newsPortfolioFolderPath = Path.Combine(_appEnvironment.ContentRootPath, "wwwroot/newsPortfolioFiles");
            DirectoryInfo newsPortfolioInfo = new DirectoryInfo(newsPortfolioFolderPath);
            if (!newsPortfolioInfo.Exists)
            {
                newsPortfolioInfo.Create();
            }
            string newsFolderPath = Path.Combine(_appEnvironment.ContentRootPath, "wwwroot/newsPortfolioFiles/portfolioFiles");
            DirectoryInfo newsInfo = new DirectoryInfo(newsFolderPath);
            if (!newsInfo.Exists)
            {
                newsInfo.Create();
            }
/*            if (ModelState.IsValid)
			{*/
				portfolio.CreatedDate = DateTime.Now;
				portfolio.Publicate = false;
				_context.Portfolio.Add(portfolio);
				await _context.SaveChangesAsync();
                portfolio.MainImagePath = $"/newsPortfolioFiles/portfolioFiles/mainPicId={portfolio.Id}&{mainPic.FileName}";
                await _context.SaveChangesAsync();
                if (mainPic != null)
				{
					string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/newsPortfolioFiles/portfolioFiles/mainPicId={portfolio.Id}&{mainPic.FileName}");
					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						await mainPic.CopyToAsync(fileStream);
					}
					PortfolioNewsFile update = new PortfolioNewsFile { Path = $"/newsPortfolioFiles/portfolioFiles/mainPicId={portfolio.Id}&{mainPic.FileName}", Type = "mainPic", PortfolioId = portfolio.Id };
					_context.PortfolioNewsFiles.Add(update);
					await _context.SaveChangesAsync();
				}

				if (uploadFiles != null)
				{
					foreach (var upload in uploadFiles)
					{
                        PortfolioNewsFile portfolioNewsFile = new PortfolioNewsFile {Type = "picture", PortfolioId = portfolio.Id };
                        _context.PortfolioNewsFiles.Add(portfolioNewsFile);
                        await _context.SaveChangesAsync();
                        string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/newsPortfolioFiles/portfolioFiles/Id={portfolioNewsFile.Id}&{upload.FileName}");
                        portfolioNewsFile.Path = $"/newsPortfolioFiles/portfolioFiles/Id={portfolioNewsFile.Id}&{upload.FileName}";
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
							PortfolioNewsFile vid = new PortfolioNewsFile { Path = "https://www.youtube.com/embed/" + path, Type = "video", PortfolioId = portfolio.Id };
							_context.Add(vid);
							await _context.SaveChangesAsync();
						}
					}
					return RedirectToAction("Portfolio");
				}
/*			}*/
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
            var port = await _context.Portfolio.Include(p => p.CarsBodyTypes).Include(p => p.CarsBrands).Include(p => p.CarsBrandsModel).FirstOrDefaultAsync(p => p.Id == id);
			List<PortfolioNewsFile> pics = await _context.PortfolioNewsFiles.Where(i => i.PortfolioId == id && i.Type == "picture").ToListAsync();
			List<PortfolioNewsFile> vids = await _context.PortfolioNewsFiles.Where(v => v.PortfolioId == id && v.Type == "video").ToListAsync();
			PortfolioNewsFile mainPic = await _context.PortfolioNewsFiles.Where(m => m.PortfolioId == id && m.Type == "mainPic").FirstOrDefaultAsync();
			PortfolioDetailsViewModel portfolioDetailsViewModel = new PortfolioDetailsViewModel 
            { 
                MinorPictures = pics,
                Videos = vids,
                Portfolio = port, 
                MainPic = mainPic,
            };
			if (port == null)
			{
				return NotFound();
			}
            ViewData["Brands"] = new SelectList(_context.CarsBrands, "Id", "Brand");
            ViewData["Models"] = new SelectList(_context.CarsBrandsModels, "Id", "Model");
            ViewData["BodyTypes"] = new SelectList(_context.CarsBodyTypes, "Id", "BodyType");
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
                    Portfolio portfolio = await _context.Portfolio.FirstOrDefaultAsync(p => p.Id == portfolioNewsFile.PortfolioId);
                    string oldFilePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot{portfolio.MainImagePath}");
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                    string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/newsPortfolioFiles/portfolioFiles/mainPicId={portfolio.Id}&{newPhoto.FileName}");
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await newPhoto.CopyToAsync(fileStream);
                    }
                    portfolioNewsFile.Path = $"/newsPortfolioFiles/portfolioFiles/mainPicId={portfolio.Id}&{newPhoto.FileName}";
                    portfolio.MainImagePath = $"/newsPortfolioFiles/portfolioFiles/mainPicId={portfolio.Id}&{newPhoto.FileName}";
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
                    string oldFilePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot{portfolioNewsFile.Path}");
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                    string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/newsPortfolioFiles/portfolioFiles/Id={id}&{newPhoto.FileName}");
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await newPhoto.CopyToAsync(fileStream);
                    }
                    portfolioNewsFile.Path = $"/newsPortfolioFiles/portfolioFiles/Id={id}&{newPhoto.FileName}";
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
            string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot{portfolioNewsFile.Path}");
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
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
            PortfolioNewsFile portfolioNewsFile = new PortfolioNewsFile {Type = "picture", PortfolioId = id };
            _context.PortfolioNewsFiles.Add(portfolioNewsFile);
            await _context.SaveChangesAsync();
            portfolioNewsFile.Path = $"/newsPortfolioFiles/portfolioFiles/Id={portfolioNewsFile.Id}&{newPhoto.FileName}";
            _context.PortfolioNewsFiles.Update(portfolioNewsFile);
            await _context.SaveChangesAsync();
            string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/newsPortfolioFiles/portfolioFiles/Id={portfolioNewsFile.Id}&{newPhoto.FileName}");
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await newPhoto.CopyToAsync(fileStream);
            }
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
                string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot{p.Path}");
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
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

