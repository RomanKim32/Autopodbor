using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Autopodbor_312.Interfaces
{
    public interface ICalculatorRepository
    {
        CalculatorViewModel CreateCalculatorViewModel();

        List<CarsBrands> CreateNewBrand(string key, string value);

        List<CarsBodyTypes> CreateCarsBodyType(string key, string value);

        List<CarsYears> CreateCarsYear(string key, string value);

        List<CarsFuels> CreateCarsFuels(string key, string value);

        List<CarsBrandsModel> GetBrands(int brandId);

        CarsBrands GetBrand(int brandId);

        List<CarsBrands> GetCarsBrandsList(string[] nameAndId);

        List<CarsBodyTypes> GetCarsBodyTypes(string[] nameAndId);

        List<CarsYears> GetCarsYears(string[] nameAndId);

        List<CarsFuels> GetCarsFuels(string[] nameAndId);

        void DeleteCarsBrandsModels(string[] nameAndId);

        List<CarsBrands> EditCarsBrands(string[] nameAndId, string key, string value);

        List<CarsBodyTypes> EditCarsBodyTypes(string[] nameAndId, string key, string value);

        List<CarsYears> EditCarsYears(string[] nameAndId, string key, string value);

        List<CarsFuels> EditCarsFuels(string[] nameAndId, string key, string value);

        CarsBrands EditCarsBrandsModel(string[] nameAndId, string key, string value);

        List<CarsBrandsModel> AllModels(int brandId);

        CarsBrandsModel GetCarsBrandsModel(CarsBrandsModel model);

        void AddCarsBrandsModel(CarsBrandsModel carsBrandsModel);

    }
}
