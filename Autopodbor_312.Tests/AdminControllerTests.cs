using Autopodbor_312.Controllers;
using Autopodbor_312.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System.Collections.Generic;
using Xunit;
using User = Autopodbor_312.Models.User;

namespace Autopodbor_312.Tests
{
    public class AdminControllerTests
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IWebHostEnvironment _appEnvironment;

        [Fact]
        public void GetAllRolesExceptAdminTest()
        {

            // Arrange
            var mock = new Mock<IAdminRepository>();
            var controller = new AdminController(mock.Object, _userManager, _signInManager, _appEnvironment);
            mock.Setup(repo => repo.GetAllRolesExceptAdmin()).Returns(GetAllRolesExceptAdmin());

            // Act
            var result = controller.Register();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ViewDataDictionary>(viewResult.ViewData);
            Assert.NotNull(model);
            Assert.Single(model);
        }

        [Fact]
        public void LoginTest()
        {
            // Arrange
            var mock = new Mock<IAdminRepository>();
            var controller = new AdminController(mock.Object, _userManager, _signInManager, _appEnvironment);
            string url = "testUrl";
            mock.Setup(repo => repo.Login(url));

            // Act
            var result = controller.Login(url);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(viewResult);
        }

        private List<IdentityRole<int>> GetAllRolesExceptAdmin()
        {
            var result = new List<IdentityRole<int>>
            {
                new IdentityRole<int>(),
                new IdentityRole<int>()
            };
            return result;
        }
    }
}
