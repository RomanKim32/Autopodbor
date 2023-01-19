using Autopodbor_312.Controllers;
using Autopodbor_312.Interfaces;
using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Autopodbor_312.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void DeleteConfirmedTest()
        {
            // Arrange
            var mock = new Mock<IHomeRepository>();
            var controller = new HomeController(mock.Object);
            const int id = 1;
            mock.Setup(repo => repo.DeleteConfirmed(id));

            // Act
            var result = controller.DeleteConfirmed(id);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(viewResult);
            Assert.IsType<RedirectToActionResult>(viewResult);
        }

        [Fact]
        public void GetMainPageViewModel()
        {
            // Arrange
            var mock = new Mock<IHomeRepository>();
            var controller = new HomeController(mock.Object);
            mock.Setup(repo => repo.GetMainPageViewModel()).Returns(GetMainPageViewModelTest());

            // Act
            var resultIndex = controller.Index();
            var resultEdit = controller.Edit();

            // Assert
            var viewResultIndex = Assert.IsType<ViewResult>(resultIndex);
            var modelIndex = Assert.IsAssignableFrom<MainPageViewModel>(viewResultIndex.Model);
            Assert.NotNull(modelIndex);

            var viewResultEdit = Assert.IsType<ViewResult>(resultEdit);
            var modelEdit = Assert.IsAssignableFrom<MainPageViewModel>(viewResultEdit.Model);
            Assert.NotNull(modelEdit);
            Assert.Equal(resultIndex.GetType(), resultEdit.GetType());
            Assert.Equal(modelEdit, modelIndex);
        }

        [Fact]
        public void CreateToolTest()
        {
            // Arrange
            var mock = new Mock<IHomeRepository>();
            var controller = new HomeController(mock.Object);
            MainPage mainPageTest = new MainPage();
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;
            IFormFile testFile = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);
            mock.Setup(repo => repo.CreateTool(mainPageTest, testFile));

            // Act
            var result = controller.CreateTool(mainPageTest, testFile);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.IsType<RedirectToActionResult>(viewResult);
        }

        [Fact]
        public void DeleteTest()
        {
            // Arrange
            var mock = new Mock<IHomeRepository>();
            var controller = new HomeController(mock.Object);
            const int id = 2;
            mock.Setup(repo => repo.Delete(id)).Returns(GetMainPage());

            // Act
            var result = controller.Delete(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<MainPage>(viewResult.Model);
            Assert.NotNull(model);
            Assert.True(model is MainPage);
        }

        private MainPageViewModel GetMainPageViewModelTest()
        {
            MainPageViewModel mainPageViewModel = new MainPageViewModel
            {
                FirstBanner = new MainPage(),
                SecondBanner = new MainPage(),
                ThirdBanner = new List<MainPage> { new MainPage(), new MainPage() }
            };
            return mainPageViewModel;
        }

        private MainPage GetMainPage()
        {
            var mainPage = new MainPage
            {
                Id = 2,
                TitleRu = "Title",
                TitleKy = "Title",
                DescriptionKy = "Description",
                DescriptionRu= "Description",
                Banner = "first",
                Path = ""
            };
            return mainPage;
        }
        
    }
}
