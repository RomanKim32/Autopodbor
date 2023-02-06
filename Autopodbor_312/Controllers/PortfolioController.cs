using Autopodbor_312.Interfaces;
using Autopodbor_312.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Autopodbor_312.Controllers
{
	public class PortfolioController : Controller
	{
        private readonly IPortfolioRepository _portfolioRepository;

        public PortfolioController(IPortfolioRepository portfolioRepository)
        {
            _portfolioRepository = portfolioRepository;
        }

        public IActionResult IndexTurnkeySelection(int? bodyType, int? brand, int? model,int pageIndex=1)
		{
			
			return View(_portfolioRepository.GetTurnkeySelectionPortfolio( bodyType, brand, model, pageIndex));
		}

		public IActionResult IndexFieldInspection(int? bodyType, int? brand, int? model, int pageIndex = 1)
		{
            return View(_portfolioRepository.GetFieldInspectionPortfolio(bodyType, brand, model,pageIndex));
		}

		[Authorize(Roles = "admin,portfolioManager")]
		public IActionResult Portfolio(int pageNumber = 1)
		{
			return View(PaginationList<Portfolio>.Create(_portfolioRepository.GetAllPortfolioForAdmin().ToList(), pageNumber, 5));
		}


		[Authorize(Roles = "admin,portfolioManager")]
		public IActionResult CreatePortfolio()
		{
			return View(_portfolioRepository.CreatePortfolio());
		}

		[HttpPost]
		[Authorize(Roles = "admin,portfolioManager")]
		public IActionResult CreatePortfolio(Portfolio portfolio, IFormFile mainPic, IFormFileCollection uploadFiles, string video)
		{
			_portfolioRepository.CreatePortfolio(portfolio, mainPic, uploadFiles, video);	
			return RedirectToAction("Portfolio");
		}

		public IActionResult DetailsPortfolio(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var portfolioDetailsViewModel = _portfolioRepository.DetailsPortfolio(id);
			if (id == null)
			{
                return NotFound();
            }
            return View(portfolioDetailsViewModel);
		}

		[Authorize(Roles = "admin,portfolioManager")]
		public IActionResult EditPortfolio(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var portfolioDetailsViewModel = _portfolioRepository.EditPortfolio(id);
			if (portfolioDetailsViewModel == null)
			{
                return NotFound();
            }

			List<SelectListItem> models = new SelectList(_portfolioRepository.GetAllCarsBrandsModel(), "Id", "Model").ToList();
			models.Insert(0, (new SelectListItem { Text = "Без модели", Value = null }));

			List<SelectListItem> brands = new SelectList(_portfolioRepository.GetAllCarsBrands(), "Id", "Brand").ToList();
			brands.Insert(0, (new SelectListItem { Text = "Без бренда", Value = null }));

			ViewData["Brands"] = brands;
			ViewData["Models"] = models;
			ViewData["BodyTypes"] = new SelectList(_portfolioRepository.GetAllCarsBodyTypes(), "Id", "BodyType");
			return View(portfolioDetailsViewModel);
		}

		[HttpPost]
		[Authorize(Roles = "admin,portfolioManager")]
		public IActionResult EditPortfolio(int? id, Portfolio portfolio)
		{
			if (id != portfolio.Id)
			{
				return NotFound();
			}
			_portfolioRepository.EditPortfolio(id, portfolio);
			return RedirectToAction("EditPortfolio", new { id = id });
		}

		[HttpPost]
		[Authorize(Roles = "admin,portfolioManager")]
		public IActionResult EditMainPhoto(int? id, IFormFile newPhoto)
		{
			if (id == null || newPhoto == null)
			{
				return NotFound();
			}
			var portfolioNewsFile = _portfolioRepository.EditMainPhoto(id, newPhoto);
			if (portfolioNewsFile == null)
			{
                return NotFound();
            }
            return RedirectToAction("EditPortfolio", new { id = portfolioNewsFile.PortfolioId });
		}

		[HttpPost]
		[Authorize(Roles = "admin,portfolioManager")]
		public IActionResult EditMinorPhoto(int? id, IFormFile newPhoto)
		{
			if (id == null || newPhoto == null)
			{
				return NotFound();
			}
			var portfolioNewsFile = _portfolioRepository.EditMinorPhoto(id, newPhoto);
			if(portfolioNewsFile == null)
			{
				return NotFound();
			}
            return RedirectToAction("EditPortfolio", new { id = portfolioNewsFile.PortfolioId });
		}

		[HttpPost]
		[Authorize(Roles = "admin,portfolioManager")]
		public IActionResult DeletePhotoOrVideo(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var portfolioNewsFile = _portfolioRepository.DeletePhotoOrVideo(id);
			if (portfolioNewsFile == null)
			{
				return NotFound();
			}
            return RedirectToAction("EditPortfolio", new { id = portfolioNewsFile.PortfolioId });
		}

		[HttpPost]
		[Authorize(Roles = "admin,portfolioManager")]
		public IActionResult AddMinorPhoto(int? id, IFormFile newPhoto)
		{
			if (id == null || newPhoto == null)
			{
				return NotFound();
			}
			_portfolioRepository.AddMinorPhoto(id, newPhoto);
			return RedirectToAction("EditPortfolio", new { id = id });
		}

		[HttpPost]
		[Authorize(Roles = "admin,portfolioManager")]
		public IActionResult EditVideo(int? id, string newVideoId)
		{
			if (id == null || newVideoId == null)
			{
				return NotFound();
			}
			var portfolioNewsFile = _portfolioRepository.EditVideo(id, newVideoId);
			if (portfolioNewsFile == null)
			{
				return NotFound();
			}
            return RedirectToAction("EditPortfolio", new { id = portfolioNewsFile.PortfolioId });
		}

		[HttpPost]
		[Authorize(Roles = "admin,portfolioManager")]
		public IActionResult AddVideo(int? id, string videoId)
		{
			if (id == null || videoId == null)
			{
				return NotFound();
			}
			_portfolioRepository.AddVideo(id, videoId);
			return RedirectToAction("EditPortfolio", new { id = id });
		}

		[Authorize(Roles = "admin,portfolioManager")]
		public IActionResult DeletePortfolio(int? id)
		{
			if (id == null)
			{
                return NotFound();
            }
			var portfolio = _portfolioRepository.DeletePortfolio(id);
			if (portfolio == null)
			{
                return NotFound();
            }
			return View(portfolio);
		}

        [HttpPost, ActionName("DeletePortfolio")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,portfolioManager")]
		public IActionResult DeleteConfirmedPortfolio(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			_portfolioRepository.DeleteConfirmedPortfolio(id);
			return RedirectToAction("Portfolio", "Portfolio");

		}

		public IActionResult PublicPortfolio(int id)
		{
			_portfolioRepository.PublicPortfolio(id);
			return Ok();
		}
	}
}

