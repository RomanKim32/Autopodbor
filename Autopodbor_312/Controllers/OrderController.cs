using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Autopodbor_312.Controllers
{
    public class OrderController : Controller
    {
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly AutopodborContext _autodborContext;

		public OrderController(UserManager<User> userManager, SignInManager<User> signInManager, AutopodborContext autopodborContext)
		{
			_userManager = userManager;
			_autodborContext = autopodborContext;
			_signInManager = signInManager;
		}

		public async Task<IActionResult> Index(string serviceName)
        {
			var service = await _autodborContext.Services.FirstOrDefaultAsync(s => s.Name == serviceName);
			Orders order = new Orders { Services = service, ServicesId = service.Id };
			var carsBodyTypes = await _autodborContext.CarsBodyTypes.ToListAsync();
			var carsBrands = await _autodborContext.CarsBrands.ToListAsync();
			var carsFuels = await _autodborContext.CarsFuels.ToListAsync();
			var carsYears = await _autodborContext.CarsYears.ToListAsync();
            var orderViewModel = new OrderViewModel 
			{ 
				Order = order, 
				CarsBodyTypes = carsBodyTypes,
				CarsBrands = carsBrands,
				CarsYears = carsYears,
				CarsFuels = carsFuels
			};
			return View(orderViewModel);
        }

		[HttpPost]
		public async Task<IActionResult> CreateOrder(OrderViewModel model)
		{
			model.Order.OrderTime = DateTime.Now;
			_autodborContext.Add(model.Order);
			await _autodborContext.SaveChangesAsync();
			model.Order.Services = await _autodborContext.Services.FirstOrDefaultAsync(s => s.Id == model.Order.ServicesId);
			model.Order.CarsFuels = await _autodborContext.CarsFuels.FirstOrDefaultAsync(f => f.Id == model.Order.CarsFuelsId);
			model.Order.CarsBodyTypes = await _autodborContext.CarsBodyTypes.FirstOrDefaultAsync(b => b.Id == model.Order.CarsBodyTypesId);
			model.Order.CarsBrands = await _autodborContext.CarsBrands.FirstOrDefaultAsync(b => b.Id == model.Order.CarsBrandsId);
			model.Order.CarsYears = await _autodborContext.CarsYears.FirstOrDefaultAsync(y => y.Id == model.Order.CarsYearsId);
			Program.Bot.SendInfo(model.Order);
            return RedirectToAction("Index", "Home");
		}

        [HttpPost]
        public IActionResult CallBack(string userName, string phoneNumber, string email)
        {
            var service = _autodborContext.Services.FirstOrDefault(s => s.Name == "Обратный звонок");
            Orders order = new Orders();
            order.Email = email;
            order.OrderTime = DateTime.Now;
            order.PhoneNumber = phoneNumber;
            order.UserName = userName;
            order.ServicesId = service.Id;
            _autodborContext.Add(order);
            _autodborContext.SaveChanges();
			Program.Bot.SendInfo(order);
            return RedirectToAction("Index", "Home");
        }
    }
}
