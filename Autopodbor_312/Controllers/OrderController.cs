using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
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

		public async Task<IActionResult> CreateOrder(string serviceName)
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
			EmailService emailService = new EmailService();
			await emailService.SendEmailAsync($"<p>{GetOrderIfo(model.Order)}</p>");
			return RedirectToAction("Index", "Home");
		}

        [HttpPost]
        public IActionResult CreateCallBackAndAdditionalService(string userName, string phoneNumber, string email, string comment, string serviceName)
        {
            Services service = new Services();
            if(serviceName == null)
                service = _autodborContext.Services.FirstOrDefault(s => s.Name == "Обратный звонок");
            else
                service = _autodborContext.Services.FirstOrDefault(s => s.Name == serviceName);
            Orders order = new Orders();
            order.Email = email;
            order.OrderTime = DateTime.Now;
            order.PhoneNumber = phoneNumber;
            order.UserName = userName;
            order.ServicesId = service.Id;
            order.Comment = comment;
            _autodborContext.Add(order);
            _autodborContext.SaveChanges();

			Program.Bot.SendInfo(order);
			EmailService emailService = new EmailService();
			await emailService.SendEmailAsync($"<p>{GetOrderIfo(order)}</p>");
            return RedirectToAction("AdditionalServicesDetails", "Admin");
        }

		private StringBuilder GetOrderIfo(Orders order)
		{
			StringBuilder info = new StringBuilder(
			  $"Название заказа: {order.Services.Name}.\n" +
			  $"Номер телефона: {order.PhoneNumber}.\n");
			if (order.UserName != null)
				info.Append($"Имя пользователя - {order.UserName}.\n");
			if (order.Email != null)
				info.Append($"Почта: {order.Email}.\n");
			if (order.CarsBrands != null)
				info.Append($"Марка: {order.CarsBrands.Brand}.\n");
			if (order.CarsBodyTypes != null)
				info.Append($"Тип кузова: {order.CarsBodyTypes.BodyType}.\n");
			if (order.CarsYears != null)
				info.Append($"Год выпуска: {order.CarsYears.ManufacturesYear}. \n");
			if (order.CarsFuels != null)
				info.Append($"Вид топлива: {order.CarsFuels.FuelsType}.\n");
			if (order.Comment != null)
				info.Append($"Дополнительная информация: {order.Comment}.");
			return info;
		}
    }
}
