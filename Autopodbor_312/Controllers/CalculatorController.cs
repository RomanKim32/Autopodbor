using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Autopodbor_312.Controllers
{
    public class CalculatorController : Controller
    {
        private readonly AutopodborContext _autodborContext;


        public CalculatorController(AutopodborContext autopodborContext)
        {
            _autodborContext = autopodborContext;
        }

        private async Task<CalculatorViewModel> GetCalculatorViewModel()
        {
            var carsBodyTypes = await _autodborContext.CarsBodyTypes.ToListAsync();
            var carsBrands = await _autodborContext.CarsBrands.ToListAsync();
            var carsFuels = await _autodborContext.CarsFuels.ToListAsync();
            var carsYears = await _autodborContext.CarsYears.ToListAsync();
            var calculatorViewModel = new CalculatorViewModel
            {
                CarsBodyTypes = carsBodyTypes,
                CarsBrands = carsBrands,
                CarsYears = carsYears,
                CarsFuels = carsFuels
            };
            return calculatorViewModel;
        }

        public async Task<IActionResult> Index()
        {
            return View(await GetCalculatorViewModel());
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditCalculator()
        {
            return View(await GetCalculatorViewModel());
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddParameter(string name, string key, string value)
        {

            if (key != null && value != null)
            {
                switch (name)
                {
                    case "brand":
                        CarsBrands newBrand = new CarsBrands();
                        newBrand.Brand = key;
                        newBrand.Price = value;
                        _autodborContext.Add(newBrand);
                        await _autodborContext.SaveChangesAsync();
                        var carsBrands = await _autodborContext.CarsBrands.ToListAsync();
                        return PartialView("CarBrandsPar", carsBrands);

                    case "body":
                        CarsBodyTypes newBody = new CarsBodyTypes();
                        newBody.BodyType = key;
                        newBody.Price = value;
                        _autodborContext.Add(newBody);
                        await _autodborContext.SaveChangesAsync();
                        var model1 = await _autodborContext.CarsBodyTypes.ToListAsync();
                        return PartialView("CarBodyTypePar", model1);

                    case "year":
                        CarsYears newYear = new CarsYears();
                        newYear.ManufacturesYear = key;
                        newYear.Price = value;
                        _autodborContext.Add(newYear);
                        await _autodborContext.SaveChangesAsync();
                        var model2 = await _autodborContext.CarsYears.ToListAsync();
                        return PartialView("CarYearPar", model2);

                    case "fuel":
                        CarsFuels carsFuel = new CarsFuels();
                        carsFuel.FuelsType = key;
                        carsFuel.Price = value;
                        _autodborContext.Add(carsFuel);
                        await _autodborContext.SaveChangesAsync();
                        var model3 = await _autodborContext.CarsFuels.ToListAsync();
                        return PartialView("CarFuelsPar", model3);
                    default:
                        return View();
                }

            }
            else
            {
                return Error();
            }

        }

        public async Task<IActionResult> GetModel(int brandId)
        {
            var carsBrandsModel = _autodborContext.CarsBrandsModels.Where(c => c.CarsBrandsId == brandId).ToList();
            var calculatorViewModel = new CalculatorViewModel
            {
                CarsBrandsModels = carsBrandsModel

            };
            return Json(calculatorViewModel);
        }


        [Authorize(Roles = "admin")]

        public async Task<IActionResult> DeleteParameter(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string[] nameAndId = id.Split('-');
                switch (nameAndId[0])
                {
                    case "brand":
                        var brand = await _autodborContext.CarsBrands.FirstOrDefaultAsync(b => b.Id == Convert.ToInt32(nameAndId[1]));
                        _autodborContext.CarsBrands.Remove(brand);
                        await _autodborContext.SaveChangesAsync();
                        var model = await _autodborContext.CarsBrands.ToListAsync();
                        return PartialView("CarBrandsPar", model);
                    case "body":
                        var body = await _autodborContext.CarsBodyTypes.FirstOrDefaultAsync(b => b.Id == Convert.ToInt32(nameAndId[1]));
                        _autodborContext.CarsBodyTypes.Remove(body);
                        await _autodborContext.SaveChangesAsync();
                        var model1 = await _autodborContext.CarsBodyTypes.ToListAsync();
                        return PartialView("CarBodyTypePar", model1);
                    case "year":
                        var year = await _autodborContext.CarsYears.FirstOrDefaultAsync(y => y.Id == Convert.ToInt32(nameAndId[1]));
                        _autodborContext.CarsYears.Remove(year);
                        await _autodborContext.SaveChangesAsync();
                        var model2 = await _autodborContext.CarsYears.ToListAsync();
                        return PartialView("CarYearPar", model2);
                    case "fuel":
                        var fuel = await _autodborContext.CarsFuels.FirstOrDefaultAsync(f => f.Id == Convert.ToInt32(nameAndId[1]));
                        _autodborContext.CarsFuels.Remove(fuel);
                        await _autodborContext.SaveChangesAsync();
                        var model3 = await _autodborContext.CarsFuels.ToListAsync();
                        return PartialView("CarFuelsPar", model3);
                    case "model":
                        var brandsmodel = await _autodborContext.CarsBrandsModels.FirstOrDefaultAsync(f => f.Id == Convert.ToInt32(nameAndId[1]));
                       _autodborContext.CarsBrandsModels.Remove(brandsmodel);
                        await _autodborContext.SaveChangesAsync();
                        return Ok();

                    default:
                        return View();
                }
            }
            else
            {
                return Error();
            }
        }

        [Authorize(Roles = "admin")]

        public async Task<IActionResult> EditParameter(string id, string key, string value)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string[] nameAndId = id.Split('-');
                switch (nameAndId[0])
                {
                    case "brand":
                        var brand = await _autodborContext.CarsBrands.FirstOrDefaultAsync(b => b.Id == Convert.ToInt32(nameAndId[1]));
                        brand.Brand = key;
                        brand.Price = value;
                        _autodborContext.Update(brand);
                        await _autodborContext.SaveChangesAsync();
                        var model = await _autodborContext.CarsBrands.ToListAsync();
                        return PartialView("CarBrandsPar", model);
                    case "body":
                        var body = await _autodborContext.CarsBodyTypes.FirstOrDefaultAsync(b => b.Id == Convert.ToInt32(nameAndId[1]));
                        body.BodyType = key;
                        body.Price = value;
                        _autodborContext.Update(body);
                        await _autodborContext.SaveChangesAsync();
                        var model1 = await _autodborContext.CarsBodyTypes.ToListAsync();
                        return PartialView("CarBodyTypePar", model1);
                    case "year":
                        var year = await _autodborContext.CarsYears.FirstOrDefaultAsync(y => y.Id == Convert.ToInt32(nameAndId[1]));
                        year.ManufacturesYear = key;
                        year.Price = value;
                        _autodborContext.Update(year);
                        await _autodborContext.SaveChangesAsync();
                        var model2 = await _autodborContext.CarsYears.ToListAsync();
                        return PartialView("CarYearPar", model2);
                    case "fuel":
                        var fuel = await _autodborContext.CarsFuels.FirstOrDefaultAsync(f => f.Id == Convert.ToInt32(nameAndId[1]));
                        fuel.FuelsType = key;
                        fuel.Price = value;
                        _autodborContext.Update(fuel);
                        await _autodborContext.SaveChangesAsync();
                        var model3 = await _autodborContext.CarsFuels.ToListAsync();
                        return PartialView("CarFuelsPar", model3);
                    case "model":
                        var brandsmodel = await _autodborContext.CarsBrandsModels.FirstOrDefaultAsync(f => f.Id == Convert.ToInt32(nameAndId[1]));
                        brandsmodel.Model = key;
                        brandsmodel.Price = value;
                        _autodborContext.Update(brandsmodel);
                        await _autodborContext.SaveChangesAsync();
                        CarsBrands brandView = _autodborContext.CarsBrands.FirstOrDefault(b => b.Id == brandsmodel.CarsBrandsId);
                        ViewBag.Brand = brandView;
                        return Ok();
                    default:
                        return View();
                }

            }
            else
            {
                return Error();
            }
        }

        public async Task<IActionResult> AllModels(int brandId)
        {
            CarsBrands brand = _autodborContext.CarsBrands.FirstOrDefault(b => b.Id == brandId);
            var autopodborContext = _autodborContext.CarsBrandsModels.Where(a => a.CarsBrandsId == brandId).Include(c => c.CarsBrands);
            ViewBag.Brand = brand;
            return View(await autopodborContext.ToListAsync());
        }
        [HttpGet]
        public IActionResult AddModels(int brandId)
        {
            CarsBrands brand = _autodborContext.CarsBrands.FirstOrDefault(b => b.Id == brandId);
            ViewBag.Brand = brand;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddModels(CarsBrandsModel carsBrandsModel)
        {
            CarsBrands car = _autodborContext.CarsBrands.FirstOrDefault(c => c.Id == carsBrandsModel.CarsBrandsId);
            if (car == null)
            {
                return NotFound();
            }
            else
            {
                CarsBrandsModel modelExist = _autodborContext.CarsBrandsModels.FirstOrDefault(c => c.Model == carsBrandsModel.Model);
                if (modelExist == null)
                {
                    _autodborContext.CarsBrandsModels.Add(carsBrandsModel);
                    _autodborContext.SaveChanges();

                    return RedirectToAction("AllModels", new { brandId = car.Id });
                }
                return BadRequest("Model is Exist!");

            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }




    }
}
