using Autopodbor_312.Controllers;
using Autopodbor_312.Interfaces;
using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Autopodbor_312.Tests
{
	public class NewsControllerTests
	{
		[Fact]
		public void PublicNewsTest()
		{
			// Arrange
			var mock = new Mock<INewsRepository>();
			const int id = 3;
			mock.Setup(repo => repo.PublicNews(id));
			var controller = new NewsController(mock.Object);
			var news = GetTestNews().FirstOrDefault(n => n.Id == id);
			news.Publicate = true;

			// Act
			var result = controller.PublicNews(id);

			// Assert
			var viewResult = Assert.IsAssignableFrom<IActionResult>(result);
			Assert.True(news.Publicate == true && news.Id == id);
			Assert.NotNull(viewResult);
		}

		[Fact]
		public void EditMinorPhotoTest()
		{
			// Arrange
			var mock = new Mock<INewsRepository>();
			const int id = 2;
			var content = "Hello World from a Fake File";
			var fileName = "test.pdf";
			var stream = new MemoryStream();
			var writer = new StreamWriter(stream);
			writer.Write(content);
			writer.Flush();
			stream.Position = 0;
			IFormFile testFile = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);
			var portfolioNewsFile = GetTestPortfolioNewsFiles().FirstOrDefault(p => p.Id == id);
			mock.Setup(repo => repo.EditMinorPhoto(id, testFile));
			var controller = new NewsController(mock.Object);

			// Act
			var result = controller.EditMainPhoto(id, testFile);

			// Assert
			var viewResult = Assert.IsAssignableFrom<IActionResult>(result);
			Assert.IsAssignableFrom<IFormFile>(testFile);
			Assert.True(portfolioNewsFile.Id == id);
			Assert.NotNull(viewResult);
		}

		[Fact]
		public void EditMainPhotoTest()
		{
			// Arrange
			var mock = new Mock<INewsRepository>();
			const int id = 2;
			var content = "Hello World from a Fake File";
			var fileName = "test.pdf";
			var stream = new MemoryStream();
			var writer = new StreamWriter(stream);
			writer.Write(content);
			writer.Flush();
			stream.Position = 0;
			IFormFile testFile = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);
			var portfolioNewsFile = GetTestPortfolioNewsFiles().FirstOrDefault(p => p.Id == id);
			mock.Setup(repo => repo.EditMainPhoto(id, testFile));
			var controller = new NewsController(mock.Object);

			// Act
			var result = controller.EditMainPhoto(id, testFile);

			// Assert
			var viewResult = Assert.IsAssignableFrom<IActionResult>(result);
			Assert.IsAssignableFrom<IFormFile>(testFile);
			Assert.True(portfolioNewsFile.Id == id);
			Assert.NotNull(viewResult);
		}

		[Fact]
		public void EditNewsTestGetMethod()
		{
			// Arrange
			var mock = new Mock<INewsRepository>();
			const int id = 2;
			mock.Setup(x => x.EditNews(id));
			var controller = new NewsController(mock.Object);
			NewsDetailsViewModel newsDetailsViewModel = new NewsDetailsViewModel
			{
				News = GetTestNews().FirstOrDefault(n => n.Id == id),
				MinorPictures = GetTestPortfolioNewsFiles().Where(m => m.NewsId == id && m.Type == "picture").ToList(),
				Videos = GetTestPortfolioNewsFiles().Where(v => v.NewsId == id && v.Type == "video").ToList(),
				MainPic = GetTestPortfolioNewsFiles().FirstOrDefault(m => m.NewsId == id && m.Type == "mainPic")
			};

			// Act
			var result = controller.EditNews(id);

			//Assert
			Assert.NotNull(result);
			Assert.Single(newsDetailsViewModel.MinorPictures);
			Assert.Single(newsDetailsViewModel.Videos);
			Assert.True(newsDetailsViewModel.News.Id == id);
			Assert.True(newsDetailsViewModel.MinorPictures.All(n => n.Type == "picture"));
			Assert.True(newsDetailsViewModel.Videos.All(n => n.Type == "video"));
			Assert.True(newsDetailsViewModel.MainPic.Id == newsDetailsViewModel.News.Id);
			Assert.True(newsDetailsViewModel.MainPic.Type == "mainPic");
			Assert.IsAssignableFrom<NewsDetailsViewModel>(newsDetailsViewModel);
		}

		[Fact]
		public void DetailsNewsTest()
		{
			// Arrange
			var mock = new Mock<INewsRepository>();
			const int id = 1;
			mock.Setup(x => x.DetailsNews(id));
			var controller = new NewsController(mock.Object);
			NewsDetailsViewModel newsDetailsViewModel = new NewsDetailsViewModel
			{
				News = GetTestNews().FirstOrDefault(n => n.Id == id),
				MinorPictures = GetTestPortfolioNewsFiles().Where(m => m.NewsId == id && m.Type == "picture").ToList(),
				Videos = GetTestPortfolioNewsFiles().Where(v => v.NewsId == id && v.Type == "video").ToList()
			};

			// Act
			var result = controller.DetailsNews(id);

			//Assert
			Assert.NotNull(result);
			Assert.Equal(2, newsDetailsViewModel.MinorPictures.Count());
			Assert.Equal(3, newsDetailsViewModel.Videos.Count());
			Assert.True(newsDetailsViewModel.News.Id == id);
			Assert.True(newsDetailsViewModel.MinorPictures.All(n => n.Type == "picture"));
			Assert.True(newsDetailsViewModel.Videos.All(n => n.Type == "video"));
			Assert.IsAssignableFrom<NewsDetailsViewModel>(newsDetailsViewModel);
		}

		[Fact]
		public void ReturnsAViewResultWithAListOfPublicatedNews()
		{
			// Arrange
			var mock = new Mock<INewsRepository>();
			mock.Setup(repo => repo.GetPublicatedNews()).Returns(GetTestNews());
			var controller = new NewsController(mock.Object);
			const int newsCount = 2;
			var expectedList = GetTestNews().OrderByDescending(n => n.CreatedDate).Where(n => n.Publicate == true);
			PaginationList<News> paginationList = new PaginationList<News>(GetTestNews(), 1, 1, 1);

			// Act
			var result = controller.Index();

			// Assert
			var viewResult = Assert.IsType<ViewResult>(result);
			var model = Assert.IsAssignableFrom<IEnumerable<News>>(viewResult.Model);
			Assert.Equal(newsCount, GetTestNews().Where(n => n.Publicate == true).Count());
			Assert.Equal(2, expectedList.ToArray()[0].Id);
			Assert.Equal(1, expectedList.ToArray()[1].Id);
			Assert.IsAssignableFrom<PaginationList<News>>(paginationList);
		}

		[Fact]
		public void ReturnsAViewResultWithAListOfAllNews()
		{
			// Arrange
			var mock = new Mock<INewsRepository>();
			mock.Setup(repo => repo.GetAllNews()).Returns(GetTestNews());
			var controller = new NewsController(mock.Object);
			var expectedList = GetTestNews().OrderByDescending(n => n.CreatedDate);
			PaginationList<News> paginationList = new PaginationList<News>(GetTestNews(), 1, 1, 1);

			// Act
			var result = controller.News();

			// Assert
			var viewResult = Assert.IsType<ViewResult>(result);
			var model = Assert.IsAssignableFrom<IEnumerable<News>>(viewResult.Model);
			Assert.Equal(model.Count(), GetTestNews().Count());
			Assert.Equal(2, expectedList.ToArray()[0].Id);
			Assert.Equal(3, expectedList.ToArray()[1].Id);
			Assert.Equal(1, expectedList.ToArray()[2].Id);
			Assert.IsAssignableFrom<PaginationList<News>>(paginationList);
		}

		[Fact]
		public void CreateNewsPostMethodTest()
		{
			// Arrange
			var mock = new Mock<INewsRepository>();
			News news = new News();
			var content = "Hello World from a Fake File";
			var fileName = "test.pdf";
			var stream = new MemoryStream();
			var writer = new StreamWriter(stream);
			writer.Write(content);
			writer.Flush();
			stream.Position = 0;
			IFormFile testFile = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);
			IFormFileCollection formFiles = new FormFileCollection { testFile, testFile, testFile };
			string videoTest = "";
			mock.Setup(repo => repo.CreateNews(news, testFile, formFiles, videoTest));
			var controller = new NewsController(mock.Object);

			// Act
			var result = controller.CreateNews(news, testFile, formFiles, videoTest);

			// Assert
			var viewResult = Assert.IsType<RedirectToActionResult>(result);
			Assert.IsType<RedirectToActionResult>(viewResult);
			Assert.Equal(3, formFiles.Count());
			Assert.IsAssignableFrom<IFormFileCollection>(formFiles);
			Assert.IsAssignableFrom<IFormFile>(testFile);
		}

		private List<PortfolioNewsFile> GetTestPortfolioNewsFiles()
		{
			var portrolioNewsFiles = new List<PortfolioNewsFile>
			{
				new PortfolioNewsFile
				{
					Id = 1,
					NewsId = 1,
					PortfolioId = null,
					Path = "",
					Type = "mainPic"
				},
				new PortfolioNewsFile
				{
					Id = 2,
					NewsId = 2,
					PortfolioId = null,
					Path = "",
					Type = "mainPic"
				},
				new PortfolioNewsFile
				{
					Id = 3,
					NewsId = 1,
					PortfolioId = null,
					Path = "",
					Type = "picture"
				},
				new PortfolioNewsFile
				{
					Id = 4,
					NewsId = 1,
					PortfolioId = null,
					Path = "",
					Type = "picture"
				},
				new PortfolioNewsFile
				{
					Id = 5,
					NewsId = 2,
					PortfolioId = null,
					Path = "",
					Type = "picture"
				},
				new PortfolioNewsFile
				{
					Id = 6,
					NewsId = 1,
					PortfolioId = null,
					Path = "",
					Type = "video"
				},
				new PortfolioNewsFile
				{
					Id = 7,
					NewsId = 1,
					PortfolioId = null,
					Path = "",
					Type = "video"
				},
				new PortfolioNewsFile
				{
					Id = 8,
					NewsId = 2,
					PortfolioId = null,
					Path = "",
					Type = "video"
				},
				new PortfolioNewsFile
				{
					Id = 9,
					NewsId = 1,
					PortfolioId = null,
					Path = "",
					Type = "video"
				},


			};
			return portrolioNewsFiles;
		}

		private List<News> GetTestNews()
		{
			var news = new List<News>
			   {
					new News
					{
						Id = 1,
						NameRu = "Название новости на русском",
						BodyRu = "Описание новости на русском",
						NameKy = "Название новости на кыргызском",
						BodyKy = "Описание новости на кыргызском",
						Publicate = true,
						CreatedDate= DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0)),
						MainImagePath = ""
					},
					new News
					{
						Id = 2,
						NameRu = "Название новости на русском",
						BodyRu = "Описание новости на русском",
						NameKy = "Название новости на кыргызском",
						BodyKy = "Описание новости на кыргызском",
						Publicate = true,
						CreatedDate= DateTime.Now,
						MainImagePath = ""
					},
					new News
					{
						Id = 3,
						NameRu = "Название новости на русском",
						BodyRu = "Описание новости на русском",
						NameKy = "Название новости на кыргызском",
						BodyKy = "Описание новости на кыргызском",
						Publicate = false,
						CreatedDate= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)),
						MainImagePath = ""
					}
			   };
			return news;
		}
	}
}
