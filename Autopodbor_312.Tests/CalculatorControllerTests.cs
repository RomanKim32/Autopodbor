using Autopodbor_312.Controllers;
using Autopodbor_312.Interfaces;
using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Autopodbor_312.Tests
{
    public class CalculatorControllerTests
    {

        [Fact]
        public void AddCarsFuelsTest()
        {
            // Arrange
            var mock = new Mock<ICalculatorRepository>();
            var controller = new CalculatorController(mock.Object);
            var carsFuels = "newCarsFuels";
            var price = "500";
            mock.Setup(repo => repo.CreateCarsYear(carsFuels, price));

            // Act
            var result = controller.AddParameter("fuel", carsFuels, price);
            var obj = result as ObjectResult;

            // Assert
            Assert.Equal(200, obj.StatusCode);
            Assert.NotNull(obj);
            Assert.NotNull(result);
        }

        [Fact]
        public void AddCarsYearTest()
        {
            // Arrange
            var mock = new Mock<ICalculatorRepository>();
            var controller = new CalculatorController(mock.Object);
            var carsYear = "newCarsYear";
            var price = "500";
            mock.Setup(repo => repo.CreateCarsYear(carsYear, price));

            // Act
            var result = controller.AddParameter("year", carsYear, price);
            var obj = result as ObjectResult;

            // Assert
            Assert.Equal(200, obj.StatusCode);
            Assert.NotNull(obj);
            Assert.NotNull(result);
        }

        [Fact]
        public void AddCarsBodyTypeTest()
        {
            // Arrange
            var mock = new Mock<ICalculatorRepository>();
            var controller = new CalculatorController(mock.Object);
            var bodyType = "newBodyType";
            var price = "500";
            mock.Setup(repo => repo.CreateCarsBodyType(bodyType, price));

            // Act
            var result = controller.AddParameter("body", bodyType, price);
            var obj = result as ObjectResult;

            // Assert
            Assert.Equal(200, obj.StatusCode);
            Assert.NotNull(obj);
            Assert.NotNull(result);
        }

        [Fact]
        public void AddBrandTest()
        {
            // Arrange
            var mock = new Mock<ICalculatorRepository>();
            var controller = new CalculatorController(mock.Object);
            var brand = "newBrand";
            var price = "500";
            mock.Setup(repo => repo.CreateNewBrand(brand, price));

            // Act
            var result = controller.AddParameter("brand", brand, price);
            var obj = result as ObjectResult;

            // Assert
            Assert.Equal(200, obj.StatusCode);
            Assert.NotNull(obj);
            Assert.NotNull(result);
        }

        [Fact]
        public void AddModelsTestExeption()
        {
            // Arrange
            var mock = new Mock<ICalculatorRepository>();
            var controller = new CalculatorController(mock.Object);
            var brand = GetBrandsListTest().FirstOrDefault();
            const int brandId = 2;
            mock.Setup(repo => repo.GetBrand(brandId)).Returns(GetCarsBrand());

            // Act
            var result = controller.AddModels(brand);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetBrandTest()
        {
            // Arrange
            var mock = new Mock<ICalculatorRepository>();
            var controller = new CalculatorController(mock.Object);
            const int brandId = 2;
            mock.Setup(repo => repo.GetBrand(brandId)).Returns(GetCarsBrand());

            // Act
            var result = controller.AddModels(brandId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ViewDataDictionary>(viewResult.ViewData);
            Assert.NotNull(model);
            Assert.True(model is ViewDataDictionary);
            Assert.Single(model.Values);
        }

        [Fact]
        public void GetBrandsTest()
        {
            // Arrange
            var mock = new Mock<ICalculatorRepository>();
            var controller = new CalculatorController(mock.Object);
            const int brandId = 2;
            mock.Setup(repo => repo.GetBrands(brandId)).Returns(GetBrandsListTest().Where(b => b.CarsBrandsId == brandId).ToList());

            // Act
            var result = controller.GetModel(brandId);

            // Assert
            var viewResult = Assert.IsType<JsonResult>(result);
            var json = viewResult.Value;
            Assert.NotNull(json);
            Assert.IsAssignableFrom<JsonResult>(viewResult);
        }

        [Fact]
        public void CreateCalculatorViewModelTest()
        {
            // Arrange
            var mock = new Mock<ICalculatorRepository>();
            var controller = new CalculatorController(mock.Object);
            mock.Setup(repo => repo.CreateCalculatorViewModel()).Returns(GetCalculatorViewModelTest());

            // Act
            var resultIndex = controller.Index();
            var resultEdit = controller.EditCalculator();

            // Assert
            var viewResultIndex = Assert.IsType<ViewResult>(resultIndex);
            var modelIndex = Assert.IsAssignableFrom<CalculatorViewModel>(viewResultIndex.Model);
            Assert.NotNull(modelIndex);

            var viewResultEdit = Assert.IsType<ViewResult>(resultEdit);
            var modelEdit = Assert.IsAssignableFrom<CalculatorViewModel>(viewResultEdit.Model);
            Assert.NotNull(modelEdit);
            Assert.Equal(resultIndex.GetType(), resultEdit.GetType());
            Assert.Equal(modelEdit, modelIndex);
        }

        private CarsBrands GetCarsBrand()
        {
            var result = new CarsBrands() { Id = 1, Brand = "test", Price = "500" };
            return result; 
        }

        private List<CarsBrandsModel> GetBrandsListTest()
        {
            var brands = new List<CarsBrandsModel>
            {
                new CarsBrandsModel {Id = 1, CarsBrandsId = 2, Model = "test", Price = "100"},
                new CarsBrandsModel {Id = 2, CarsBrandsId = 2, Model = "test2", Price = "200"},
                new CarsBrandsModel {Id = 3, CarsBrandsId = 2, Model = "test3", Price = "300"},
                new CarsBrandsModel {Id = 4, CarsBrandsId = 1, Model = "test4", Price = "400"},
            };
            return brands;
        }

        private CalculatorViewModel GetCalculatorViewModelTest()
        {
            CalculatorViewModel calculatorViewModel = new CalculatorViewModel
            {
                CarsBodyTypes = new List<CarsBodyTypes> { new CarsBodyTypes(), new CarsBodyTypes() },
                CarsBrands= new List<CarsBrands> { new CarsBrands(),new CarsBrands() },
                CarsBrandsModels = new List<CarsBrandsModel> { new CarsBrandsModel(), new CarsBrandsModel() },
                CarsFuels = new List<CarsFuels> { new CarsFuels(), new CarsFuels() },
                CarsYears = new List<CarsYears> { new CarsYears(), new CarsYears()}
            };
            return calculatorViewModel;
        }
    }
}
