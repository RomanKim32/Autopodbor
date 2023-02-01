using Autopodbor_312.Interfaces;
using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Autopodbor_312.Controllers
{
    public class CalculatorController : Controller
    {
        private readonly ICalculatorRepository _calculatorRepository;

        public CalculatorController(ICalculatorRepository calculatorRepository)
        {
            _calculatorRepository = calculatorRepository;
        }

        public IActionResult Index()
        {
            return View(_calculatorRepository.CreateCalculatorViewModel());
        }

        [Authorize(Roles = "admin")]
        public IActionResult EditCalculator()
        {
            return View(_calculatorRepository.CreateCalculatorViewModel());
        }

        [Authorize(Roles = "admin")]
        public IActionResult AddParameter(string name, string key, string value)
        {
            if (key != null && value != null)
            {
                switch (name)
                {
                    case "brand":
                        try
                        {
                            return Ok(_calculatorRepository.CreateNewBrand(key, value));                          
                        }
                        catch
                        {
                            return BadRequest();
                        }
                    case "body":
                        try
                        {
                            return Ok(_calculatorRepository.CreateCarsBodyType(key, value));
                        }
                        catch
                        {
                            return BadRequest();
                        }
                    case "year":
                        try
                        {
                            return Ok(_calculatorRepository.CreateCarsYear(key, value));
                        }
                        catch
                        {
                            return BadRequest();
                        }                       
                    case "fuel":
                        try
                        {
                            return Ok(_calculatorRepository.CreateCarsFuels(key, value));
                        }
                        catch
                        {
                            return BadRequest();
                        }
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
        public IActionResult GetModel(int brandId)
        {
            var carsBrandsModel = _calculatorRepository.GetBrands(brandId);
            var calculatorViewModel = new CalculatorViewModel
            {
                CarsBrandsModels = carsBrandsModel
            };
            var brand = _calculatorRepository.GetBrand(brandId);
            ViewBag.Brand = brand;
            return Json(calculatorViewModel);
        }

        [Authorize(Roles = "admin")]
        public IActionResult DeleteParameter(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string[] nameAndId = id.Split('-');
                switch (nameAndId[0])
                {
                    case "brand":
                        try
                        {
                            return Ok(_calculatorRepository.GetCarsBrandsList(nameAndId));
                        }
                        catch
                        {
                            return BadRequest();
                        }
                    case "body":
                        try
                        {
                            return Ok(_calculatorRepository.GetCarsBodyTypes(nameAndId));
                        }
                        catch
                        {
                            return BadRequest();
                        }
                    case "year":
                        try
                        {
                            return Ok(_calculatorRepository.GetCarsYears(nameAndId));
                        }
                        catch
                        {
                            return BadRequest();
                        }
                    case "fuel":
                        try
                        {
                            return Ok(_calculatorRepository.GetCarsFuels(nameAndId));
                        }
                        catch
                        {
                            return BadRequest();
                        }
                    case "model":
                        _calculatorRepository.DeleteCarsBrandsModels(nameAndId);                      
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
        public IActionResult EditParameter(string id, string key, string value)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string[] nameAndId = id.Split('-');
                switch (nameAndId[0])
                {
                    case "brand":
                        try
                        {
                            return Ok(_calculatorRepository.EditCarsBrands(nameAndId, key, value));
                        }
                        catch
                        {
                            return BadRequest();
                        }
                    case "body":
                        try
                        {
                            return Ok(_calculatorRepository.EditCarsBodyTypes(nameAndId, key, value));
                        }
                        catch
                        {
                            return BadRequest();
                        }
                    case "year":
                        try
                        {
                            return Ok(_calculatorRepository.EditCarsYears(nameAndId, key, value));
                        }
                        catch
                        {
                            return BadRequest();
                        }                    
                    case "fuel":
                        try
                        {
                            return Ok(_calculatorRepository.EditCarsFuels(nameAndId, key, value));
                        }
                        catch
                        {
                            return BadRequest();
                        }                      
                    case "model":
                        ViewBag.Brand =  _calculatorRepository.EditCarsBrandsModel(nameAndId, key, value);
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
        public IActionResult AllModels(int brandId)
        {
            ViewBag.Brand = _calculatorRepository.GetBrand(brandId);                 
            return View(_calculatorRepository.AllModels(brandId));
        }

        [HttpGet]
        public IActionResult AddModels(int brandId)
        {
            ViewBag.Brand = _calculatorRepository.GetBrand(brandId);
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult AddModels(CarsBrandsModel carsBrandsModel)
        {
            CarsBrands brands = _calculatorRepository.GetBrand(carsBrandsModel.CarsBrandsId);
            if (brands == null)
            {
                return NotFound();
            }           
            _calculatorRepository.AddCarsBrandsModel(carsBrandsModel);
            return RedirectToAction("AllModels", new { brandId = brands.Id });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
