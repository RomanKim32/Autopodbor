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
    public class PortfolioControllerTests
    {
        [Fact]
        public void CreatePortfolioPostMethodTest()
        {
            // Arrange
            var mock = new Mock<IPortfolioRepository>();
            Portfolio portfolio = new Portfolio();
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
            mock.Setup(repo => repo.CreatePortfolio(portfolio, testFile, formFiles, videoTest));
            var controller = new PortfolioController(mock.Object);

            // Act
            var result = controller.CreatePortfolio(portfolio, testFile, formFiles, videoTest);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.IsType<RedirectToActionResult>(viewResult);
            Assert.Equal(3, formFiles.Count());
            Assert.IsAssignableFrom<IFormFileCollection>(formFiles);
            Assert.IsAssignableFrom<IFormFile>(testFile);
        }

        [Fact]
        public void ReturnsAViewResultWithAListOfAllPortfolios()
        {
            // Arrange
            var mock = new Mock<IPortfolioRepository>();
            mock.Setup(repo => repo.GetAllPortfolioForAdmin()).Returns(GetTestPortfolio());
            var controller = new PortfolioController(mock.Object);
            var expectedList = GetTestPortfolio().OrderByDescending(n => n.CreatedDate);
            PaginationList<Portfolio> paginationList = new PaginationList<Portfolio>(GetTestPortfolio(), 1, 1, 1);

            // Act
            var result = controller.Portfolio();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Portfolio>>(viewResult.Model);
            Assert.Equal(model.Count(), GetTestPortfolio().Count());
            Assert.Equal(2, expectedList.ToArray()[0].Id);
            Assert.Equal(3, expectedList.ToArray()[1].Id);
            Assert.Equal(1, expectedList.ToArray()[2].Id);
            Assert.IsAssignableFrom<PaginationList<Portfolio>>(paginationList);
        }

        [Fact]
        public void DetailsPortfolioTest()
        {
            // Arrange
            var mock = new Mock<IPortfolioRepository>();
            const int id = 1;
            mock.Setup(x => x.DetailsPortfolio(id));
            var controller = new PortfolioController(mock.Object);
            PortfolioDetailsViewModel portfolioDetailsViewModel = new PortfolioDetailsViewModel
            {
                Portfolio = GetTestPortfolio().FirstOrDefault(n => n.Id == id),
                MinorPictures = GetTestPortfolioNewsFiles().Where(m => m.NewsId == id && m.Type == "picture").ToList(),
                Videos = GetTestPortfolioNewsFiles().Where(v => v.NewsId == id && v.Type == "video").ToList()
            };

            // Act
            var result = controller.DetailsPortfolio(id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, portfolioDetailsViewModel.MinorPictures.Count());
            Assert.Equal(3, portfolioDetailsViewModel.Videos.Count());
            Assert.True(portfolioDetailsViewModel.Portfolio.Id == id);
            Assert.True(portfolioDetailsViewModel.MinorPictures.All(n => n.Type == "picture"));
            Assert.True(portfolioDetailsViewModel.Videos.All(n => n.Type == "video"));
            Assert.IsAssignableFrom<PortfolioDetailsViewModel>(portfolioDetailsViewModel);
        }

        [Fact]
        public void EditPortfolioTestGetMethod()
        {
            // Arrange
            var mock = new Mock<IPortfolioRepository>();
            const int id = 2;
            mock.Setup(x => x.EditPortfolio(id));
            var controller = new PortfolioController(mock.Object);
            PortfolioDetailsViewModel portfolioDetailsViewModel = new PortfolioDetailsViewModel
            {
                Portfolio = GetTestPortfolio().FirstOrDefault(n => n.Id == id),
                MinorPictures = GetTestPortfolioNewsFiles().Where(m => m.NewsId == id && m.Type == "picture").ToList(),
                Videos = GetTestPortfolioNewsFiles().Where(v => v.NewsId == id && v.Type == "video").ToList(),
                MainPic = GetTestPortfolioNewsFiles().FirstOrDefault(m => m.NewsId == id && m.Type == "mainPic")
            };

            // Act
            var result = controller.EditPortfolio(id);

            //Assert
            Assert.NotNull(result);
            Assert.Single(portfolioDetailsViewModel.MinorPictures);
            Assert.Single(portfolioDetailsViewModel.Videos);
            Assert.True(portfolioDetailsViewModel.Portfolio.Id == id);
            Assert.True(portfolioDetailsViewModel.MinorPictures.All(n => n.Type == "picture"));
            Assert.True(portfolioDetailsViewModel.Videos.All(n => n.Type == "video"));
            Assert.True(portfolioDetailsViewModel.MainPic.Id == portfolioDetailsViewModel.Portfolio.Id);
            Assert.True(portfolioDetailsViewModel.MainPic.Type == "mainPic");
            Assert.IsAssignableFrom<PortfolioDetailsViewModel>(portfolioDetailsViewModel);
        }

        [Fact]
        public void EditMainPhotoTest()
        {
            // Arrange
            var mock = new Mock<IPortfolioRepository>();
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
            var controller = new PortfolioController(mock.Object);

            // Act
            var result = controller.EditMainPhoto(id, testFile);

            // Assert
            var viewResult = Assert.IsAssignableFrom<IActionResult>(result);
            Assert.IsAssignableFrom<IFormFile>(testFile);
            Assert.True(portfolioNewsFile.Id == id);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void PublicPortfolioTest()
        {
            // Arrange
            var mock = new Mock<IPortfolioRepository>();
            const int id = 3;
            mock.Setup(repo => repo.PublicPortfolio(id));
            var controller = new PortfolioController(mock.Object);
            var portfolio = GetTestPortfolio().FirstOrDefault(p => p.Id == id);
            portfolio.Publicate = true;

            // Act
            var result = controller.PublicPortfolio(id);

            // Assert
            var viewResult = Assert.IsAssignableFrom<IActionResult>(result);
            Assert.True(portfolio.Publicate == true && portfolio.Id == id);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void EditMinorPhotoTest()
        {
            // Arrange
            var mock = new Mock<IPortfolioRepository>();
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
            var controller = new PortfolioController(mock.Object);

            // Act
            var result = controller.EditMainPhoto(id, testFile);

            // Assert
            var viewResult = Assert.IsAssignableFrom<IActionResult>(result);
            Assert.IsAssignableFrom<IFormFile>(testFile);
            Assert.True(portfolioNewsFile.Id == id);
            Assert.NotNull(viewResult);
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

        private List<Portfolio> GetTestPortfolio()
        {
            var portfolios = new List<Portfolio>
               {
                    new Portfolio
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
                    new Portfolio
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
                    new Portfolio
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
            return portfolios;
        }
    }
}
