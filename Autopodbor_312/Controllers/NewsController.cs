using Autopodbor_312.Interfaces;
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

		private readonly INewsRepository _newsRepository;

		public NewsController(INewsRepository newsRepository)
		{
			_newsRepository = newsRepository;
		}

		public IActionResult Index(int pageNumber = 1)
		{
			var newsPublished = _newsRepository.GetPublicatedNews();
			return View(PaginationList<News>.Create(newsPublished.ToList(), pageNumber, 5));
		}

		[Authorize(Roles = "admin,mediaManager")]
		public IActionResult News(int pageNumber = 1)
		{
            var news = _newsRepository.GetAllNews();
			return View(PaginationList<News>.Create(news.ToList(), pageNumber, 5));
		}

		[Authorize(Roles = "admin,mediaManager")]
		public IActionResult CreateNews()
		{
			return View();
		}

		[HttpPost]
		[Authorize(Roles = "admin,mediaManager")]
		public IActionResult CreateNews(News news, IFormFile mainPic, IFormFileCollection uploadFiles, string video)
		{
            _newsRepository.CreateNews(news, mainPic, uploadFiles, video);
            return RedirectToAction("News");
        }

		public IActionResult DetailsNews(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			return View( _newsRepository.DetailsNews(id));
		}

        [Authorize(Roles = "admin,mediaManager")]
        public IActionResult EditNews(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return View(_newsRepository.EditNews(id));
        }

        [HttpPost]
        [Authorize(Roles = "admin,mediaManager")]
        public IActionResult EditNews(int? id, News news)
        {
            if (id != news.Id)
            {
                return NotFound();
            }
            _newsRepository.EditNews(id, news);
			return RedirectToAction("EditNews", new { id = id });
		}

        [HttpPost]
        [Authorize(Roles = "admin,mediaManager")]
        public IActionResult EditMainPhoto(int? id, IFormFile newPhoto)
        {
            if (id == null || newPhoto == null)
            {
                return NotFound();
            }
            var portfolioNewsFile = _newsRepository.EditMainPhoto(id, newPhoto);
            if (portfolioNewsFile == null)
            {
				return NotFound();
			}
			return RedirectToAction("EditNews", new { id = portfolioNewsFile.NewsId});
        }

        [HttpPost]
        [Authorize(Roles = "admin,mediaManager")]
        public IActionResult EditMinorPhoto(int? id, IFormFile newPhoto)
        {
            if (id == null || newPhoto == null)
            {
                return NotFound();
            }
            var portfolioNewsFile = _newsRepository.EditMinorPhoto(id, newPhoto);
            if (portfolioNewsFile == null)
            {
				return NotFound();
			}
			return RedirectToAction("EditNews", new { id = portfolioNewsFile.NewsId });
        }

        [HttpPost]
        [Authorize(Roles = "admin,mediaManager")]
        public IActionResult DeletePhotoOrVideo(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var portfolioNewsFile = _newsRepository.DeletePhotoOrVideo(id);
            if (portfolioNewsFile == null)
            {
				return NotFound();
			}
			return RedirectToAction("EditNews", new { id = portfolioNewsFile.NewsId });
        }

        [HttpPost]
        [Authorize(Roles = "admin,mediaManager")]
        public IActionResult AddMinorPhoto(int? id, IFormFile newPhoto)
        {
            if (id == null || newPhoto == null)
            {
                return NotFound();
            }
            _newsRepository.AddMinorPhoto(id, newPhoto);
            return RedirectToAction("EditNews", new { id = id });
        }

        [HttpPost]
        [Authorize(Roles = "admin,mediaManager")]
        public IActionResult EditVideo(int? id, string newVideoId)
        {
            if (id == null || newVideoId == null)
            {
                return NotFound();
            }
            var portfolioNewsFile = _newsRepository.EditVideo(id, newVideoId);
            if (portfolioNewsFile == null)
            {
				return NotFound();
			}
			return RedirectToAction("EditNews", new { id = portfolioNewsFile.NewsId });
        }

        [HttpPost]
        [Authorize(Roles = "admin,mediaManager")]
        public IActionResult AddVideo(int? id, string videoId)
        {
            if (id == null || videoId == null)
            {
                return NotFound();
            }
            _newsRepository.AddVideo(id, videoId);
            return RedirectToAction("EditNews", new { id = id });
        }

        [Authorize(Roles = "admin,mediaManager")]
        public IActionResult DeleteNews(int? id)
        {
            if (id == null)
                return NotFound();
            var news = _newsRepository.DeleteNews(id);
            if (news == null)
                return NotFound();
            return View(news);
        }


		[HttpPost, ActionName("DeleteNews")]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "admin,mediaManager")]
		public IActionResult DeleteConfirmedNews(int? id)
		{
            if (id == null)
            {
                return NotFound();
            }
            _newsRepository.DeleteConfirmedNews(id);
			return RedirectToAction("News", "News");
        }

        public IActionResult PublicNews(int? id)
        {
            if (id == null)
            {
				return NotFound();
			}
            _newsRepository.PublicNews(id);
            return Ok();
        }


    }
}
