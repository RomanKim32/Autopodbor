using Autopodbor_312.Controllers;
using Autopodbor_312.Interfaces;
using Autopodbor_312.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Autopodbor_312.Tests
{
    public class ContactInformationsControllerTests
    {
        [Fact]
        public void UpdateAndSaveChanges()
        {
            // Arrange
            var mock = new Mock<IContactInformationsRepository>();
            var controller = new ContactInformationsController(mock.Object);
            var contactInfoTest = GetContactInformation();
            mock.Setup(repo => repo.UpdateAndSaveChanges(contactInfoTest));

            // Act
            var result = controller.Edit(contactInfoTest.Id, contactInfoTest);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(viewResult);
        }

        [Fact]
        public void GetFirstContactInformationTest()
        {
            // Arrange
            var mock = new Mock<IContactInformationsRepository>();
            var controller = new ContactInformationsController(mock.Object);
            mock.Setup(repo => repo.GetFirstContactInformation()).Returns(GetContactInformation());

            // Act
            var resultIndex =  controller.Index();
            var resultEdit = controller.Edit();

            // Assert
            var viewResultIndex = Assert.IsType<ViewResult>(resultIndex);
            var modelIndex = Assert.IsAssignableFrom<ContactInformation>(viewResultIndex.Model);
            Assert.NotNull(modelIndex);
            Assert.IsType<ContactInformation>(modelIndex);

            var viewResultEdit = Assert.IsType<ViewResult>(resultEdit);
            var modelEdit = Assert.IsAssignableFrom<ContactInformation>(viewResultEdit.Model);
            Assert.NotNull(modelEdit);
            Assert.IsType<ContactInformation>(modelEdit);
            Assert.Equal(modelIndex, modelEdit);
        }
   
        private ContactInformation GetContactInformation()
        {
            var contactInformation = new ContactInformation()
            {
                Email = "test@test.com",
                LinkToInstagram = "https://test.com",
                LinkToTiktok = "https://test.com",
                PhoneNumber = "123123123"
            };
            return contactInformation;
        }
    }
}
