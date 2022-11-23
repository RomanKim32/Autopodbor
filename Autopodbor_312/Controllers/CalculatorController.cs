using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Diagnostics;
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

		public async Task<IActionResult> Index()
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
			return View(calculatorViewModel);
		}

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditCalculator()
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
			return View(calculatorViewModel);
		}

        [Authorize(Roles = "admin")]
        [HttpPost]
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
                        break;
					case "body":
						CarsBodyTypes newBody = new CarsBodyTypes();
						newBody.BodyType = key;
						newBody.Price = value;
                        _autodborContext.Add(newBody);
                        break;
					case "year":
						CarsYears newYear = new CarsYears();
                        newYear.ManufacturesYear = key;
                        newYear.Price = value;
						_autodborContext.Add(newYear);
						break;
					case "fuel":
						CarsFuels carsFuel = new CarsFuels();
                        carsFuel.FuelsType = key;
                        carsFuel.Price = value;
						_autodborContext.Add(carsFuel);
						break;
				}
                await _autodborContext.SaveChangesAsync();
            }
			else
			{
				return Error();
            }
			return View();
		}

        [Authorize(Roles = "admin")]
        [HttpPost]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			if (!string.IsNullOrEmpty(id))
			{
				string[] nameAndId = id.Split('-');
				switch (nameAndId[0])
				{
					case "brand":
                        var brand = await _autodborContext.CarsBrands.FirstOrDefaultAsync(b => b.Id == Convert.ToInt32(nameAndId[1]));
                        _autodborContext.CarsBrands.Remove(brand);
                        break;
					case "body":
						var body = await _autodborContext.CarsBodyTypes.FirstOrDefaultAsync(b => b.Id == Convert.ToInt32(nameAndId[1]));
                        _autodborContext.CarsBodyTypes.Remove(body);
						break;
					case "year":
						var year = await _autodborContext.CarsYears.FirstOrDefaultAsync(y => y.Id == Convert.ToInt32(nameAndId[1]));
                        _autodborContext.CarsYears.Remove(year);
						break;
					case "fuel":
						var fuel = await _autodborContext.CarsFuels.FirstOrDefaultAsync(f => f.Id == Convert.ToInt32(nameAndId[1]));
                        _autodborContext.CarsFuels.Remove(fuel);
						break;
				}
                await _autodborContext.SaveChangesAsync();

            }
            return View();
		}

        [Authorize(Roles = "admin")]
        [HttpPost]
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
                        break;
                    case "body":
                        var body = await _autodborContext.CarsBodyTypes.FirstOrDefaultAsync(b => b.Id == Convert.ToInt32(nameAndId[1]));
                        body.BodyType = key;
                        body.Price = value;
                        _autodborContext.Update(body);
                        break;
                    case "year":
                        var year = await _autodborContext.CarsYears.FirstOrDefaultAsync(y => y.Id == Convert.ToInt32(nameAndId[1]));
                        year.ManufacturesYear = key;
                        year.Price = value;
                        _autodborContext.Update(year);
                        break;
                    case "fuel":
                        var fuel = await _autodborContext.CarsFuels.FirstOrDefaultAsync(f => f.Id == Convert.ToInt32(nameAndId[1]));
                        fuel.FuelsType = key;
                        fuel.Price = value;
                        _autodborContext.Update(fuel);
                        break;
                }
                await _autodborContext.SaveChangesAsync();
            }
                return View();
		}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
