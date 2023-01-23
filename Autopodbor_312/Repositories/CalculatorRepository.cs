using Autopodbor_312.Interfaces;
using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace Autopodbor_312.Repositories
{
    public class CalculatorRepository : ICalculatorRepository
    {
        private readonly AutopodborContext _context;


        public CalculatorRepository(AutopodborContext autopodborContext)
        {
            _context = autopodborContext;
        }

        public CalculatorViewModel CreateCalculatorViewModel()
        {
            var carsBodyTypes = _context.CarsBodyTypes.ToList();
            var carsBrands = _context.CarsBrands.ToList();
            var carsFuels = _context.CarsFuels.ToList();
            var carsYears = _context.CarsYears.ToList();
            var calculatorViewModel = new CalculatorViewModel
            {
                CarsBodyTypes = carsBodyTypes,
                CarsBrands = carsBrands,
                CarsYears = carsYears,
                CarsFuels = carsFuels
            };
            return calculatorViewModel;
        }

        public List<CarsBrands> CreateNewBrand(string key, string value)
        {
            CarsBrands newBrand = new CarsBrands();
            newBrand.Brand = key;
            newBrand.Price = value;
            _context.Add(newBrand);
            _context.SaveChanges();
            var carsBrands = _context.CarsBrands.ToList();
            return carsBrands;
        }

        public List<CarsBodyTypes> CreateCarsBodyType(string key, string value)
        {
            CarsBodyTypes newBody = new CarsBodyTypes();
            newBody.BodyType = key;
            newBody.Price = value;
            _context.Add(newBody);
            _context.SaveChanges();
            var bodyTypes = _context.CarsBodyTypes.ToList();
            return bodyTypes;
        }

        public List<CarsYears> CreateCarsYear(string key, string value)
        {
            CarsYears newYear = new CarsYears();
            newYear.ManufacturesYear = key;
            newYear.Price = value;
            _context.Add(newYear);
            _context.SaveChanges();
            var carsYears = _context.CarsYears.ToList();
            return carsYears;
        }

        public List<CarsFuels> CreateCarsFuels(string key, string value)
        {
            CarsFuels carsFuel = new CarsFuels();
            carsFuel.FuelsType = key;
            carsFuel.Price = value;
            _context.Add(carsFuel);
            _context.SaveChanges();
            var carsFuels = _context.CarsFuels.ToList();
            return carsFuels;
        }

        public List<CarsBrandsModel> GetBrands(int brandId)
        {
            var carsBrandsModelList = _context.CarsBrandsModels.Where(c => c.CarsBrandsId == brandId).ToList();
            return carsBrandsModelList;
        }

        public CarsBrands GetBrand(int brandId)
        {
            var carsBrand = _context.CarsBrands.FirstOrDefault(b => b.Id == brandId);
            return carsBrand;
        }

        public List<CarsBrands> GetCarsBrandsList(string[] nameAndId)
        { 
            var brand = _context.CarsBrands.FirstOrDefault(b => b.Id == Convert.ToInt32(nameAndId[1]));
            _context.CarsBrands.Remove(brand);
            _context.SaveChanges();
            var model = _context.CarsBrands.ToList();
            return model;
        }

        public List<CarsBodyTypes> GetCarsBodyTypes(string[] nameAndId)
        {
            var carsBodyTypes = _context.CarsBodyTypes.FirstOrDefault(b => b.Id == Convert.ToInt32(nameAndId[1]));
            _context.CarsBodyTypes.Remove(carsBodyTypes);
            _context.SaveChanges();
            var model = _context.CarsBodyTypes.ToList();
            return model;
        }

        public List<CarsYears> GetCarsYears(string[] nameAndId)
        {
            var carsYears = _context.CarsYears.FirstOrDefault(y => y.Id == Convert.ToInt32(nameAndId[1]));
            _context.CarsYears.Remove(carsYears);
            _context.SaveChanges();
            var model = _context.CarsYears.ToList();
            return model;
        }

        public List<CarsFuels> GetCarsFuels(string[] nameAndId)
        {
            var carsYears = _context.CarsFuels.FirstOrDefault(f => f.Id == Convert.ToInt32(nameAndId[1]));
            _context.CarsFuels.Remove(carsYears);
            _context.SaveChanges();
            var model = _context.CarsFuels.ToList();
            return model;
        }

        public void DeleteCarsBrandsModels(string[] nameAndId)
        {
            var brandsmodel = _context.CarsBrandsModels.FirstOrDefault(f => f.Id == Convert.ToInt32(nameAndId[1]));
            _context.CarsBrandsModels.Remove(brandsmodel);
            _context.SaveChanges();
        }

        public List<CarsBrands> EditCarsBrands(string[] nameAndId, string key, string value)
        {
            var brand = _context.CarsBrands.FirstOrDefault(b => b.Id == Convert.ToInt32(nameAndId[1]));
            brand.Brand = key;
            brand.Price = value;
            _context.Update(brand);
            _context.SaveChanges();
            var model = _context.CarsBrands.ToList();
            return model;
        }

        public List<CarsBodyTypes> EditCarsBodyTypes(string[] nameAndId, string key, string value)
        {
            var body = _context.CarsBodyTypes.FirstOrDefault(b => b.Id == Convert.ToInt32(nameAndId[1]));
            body.BodyType = key;
            body.Price = value;
            _context.Update(body);
            _context.SaveChanges();
            var model = _context.CarsBodyTypes.ToList();
            return model;
        }

        public List<CarsYears> EditCarsYears(string[] nameAndId, string key, string value)
        {
            var year = _context.CarsYears.FirstOrDefault(y => y.Id == Convert.ToInt32(nameAndId[1]));
            year.ManufacturesYear = key;
            year.Price = value;
            _context.Update(year);
            _context.SaveChanges();
            var model = _context.CarsYears.ToList();
            return model;
        }

        public List<CarsFuels> EditCarsFuels(string[] nameAndId, string key, string value)
        {
            var fuel = _context.CarsFuels.FirstOrDefault(f => f.Id == Convert.ToInt32(nameAndId[1]));
            fuel.FuelsType = key;
            fuel.Price = value;
            _context.Update(fuel);
            _context.SaveChanges();
            var model = _context.CarsFuels.ToList();
            return model;
        }

        public CarsBrands EditCarsBrandsModel(string[] nameAndId, string key, string value)
        {
            var brandsmodel = _context.CarsBrandsModels.FirstOrDefault(f => f.Id == Convert.ToInt32(nameAndId[1]));
            brandsmodel.Model = key;
            brandsmodel.Price = value;
            _context.Update(brandsmodel);
            _context.SaveChanges();
            var brandView = _context.CarsBrands.FirstOrDefault(b => b.Id == brandsmodel.CarsBrandsId);
            return brandView;
        }

        public List<CarsBrandsModel> AllModels(int brandId)
        {
            var carBrandsAndModels = _context.CarsBrandsModels.Where(a => a.CarsBrandsId == brandId).Include(c => c.CarsBrands).ToList();
            return carBrandsAndModels;
        }

        public CarsBrandsModel GetCarsBrandsModel(CarsBrandsModel model)
        {
            CarsBrandsModel carBrandsModel = _context.CarsBrandsModels.FirstOrDefault(c => c.Model == model.Model);
            return carBrandsModel;
        }

        public void AddCarsBrandsModel(CarsBrandsModel carsBrandsModel)
        {
            _context.CarsBrandsModels.Add(carsBrandsModel);
            _context.SaveChanges();
        }
    }
}
