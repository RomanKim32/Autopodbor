using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;

namespace Autopodbor_312.Interfaces
{
	public interface INewsRepository
	{
		IEnumerable<News> GetPublicatedNews();
		IEnumerable<News> GetAllNews();
		void CreateNews(News news, IFormFile mainPic, IFormFileCollection uploadFiles, string video);
		NewsDetailsViewModel DetailsNews(int? id);
		NewsDetailsViewModel EditNews(int? id);
		void EditNews(int? id, News news);
		PortfolioNewsFile EditMainPhoto(int? id, IFormFile newPhoto);
		PortfolioNewsFile EditMinorPhoto(int? id, IFormFile newPhoto);
		PortfolioNewsFile DeletePhotoOrVideo(int? id);
		void AddMinorPhoto(int? id, IFormFile newPhoto);
		PortfolioNewsFile EditVideo(int? id, string newVideoId);
		void AddVideo(int? id, string videoId);
		News DeleteNews(int? id);
		void DeleteConfirmedNews(int? id);
		void PublicNews(int? id);
	}
}
