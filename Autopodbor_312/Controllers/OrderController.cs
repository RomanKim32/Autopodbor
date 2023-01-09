using Autopodbor_312.Models;
using Autopodbor_312.OrderMailing;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
			var service = await _autodborContext.Services.FirstOrDefaultAsync(s => s.NameRu == serviceName);
			Orders order = new Orders { Services = service, ServicesId = service.Id };
			var carsBodyTypes = await _autodborContext.CarsBodyTypes.ToListAsync();
			var carsBrands = await _autodborContext.CarsBrands.ToListAsync();
			var carsFuels = await _autodborContext.CarsFuels.ToListAsync();
			var carsYears = await _autodborContext.CarsYears.ToListAsync();
			var carsBrandsModel = await _autodborContext.CarsBrandsModels.ToListAsync();
			var orderViewModel = new OrderViewModel
			{
				Order = order,
				CarsBodyTypes = carsBodyTypes,
				CarsBrands = carsBrands,
				CarsYears = carsYears,
				CarsFuels = carsFuels,
				CarsBrandsModels = carsBrandsModel
			};
			return View(orderViewModel);
		}

		[HttpPost]
		public async Task<IActionResult> CreateOrder(string userName, string phoneNumber, string email, string comment, string carsBrandsId, string carsBodyTypesId, string carsYearsId,string carsFuelsId, string serviceId, string modelId)
		{
			try
			{
				Orders order = new Orders();
				order.Email = email;
				order.OrderTime = DateTime.Now;
				order.PhoneNumber = phoneNumber;
				order.UserName = userName;
				order.ServicesId = Convert.ToInt32(serviceId);
				order.Services = await _autodborContext.Services.FirstOrDefaultAsync(s => s.Id == Convert.ToInt32(serviceId));
				order.Comment = comment;
				order.CarsBodyTypesId = Convert.ToInt32(carsBodyTypesId);
				order.CarsBodyTypes = await _autodborContext.CarsBodyTypes.FirstOrDefaultAsync(c => c.Id == order.CarsBodyTypesId);
				order.CarsBrandsId = Convert.ToInt32(carsBrandsId);
				order.CarsBrands = await _autodborContext.CarsBrands.FirstOrDefaultAsync(c => c.Id == order.CarsBrandsId);
				order.CarsFuelsId = Convert.ToInt32(carsFuelsId);
				order.CarsFuels = await _autodborContext.CarsFuels.FirstOrDefaultAsync(c => c.Id == order.CarsFuelsId);
				order.CarsYearsId = Convert.ToInt32(carsYearsId);
				order.CarsYears = await _autodborContext.CarsYears.FirstOrDefaultAsync(c => c.Id == order.CarsYearsId);
				if (modelId != null)
				{
					order.CarsBrandsModelsId = Convert.ToInt32(modelId);
					order.CarsBrandsModels = await _autodborContext.CarsBrandsModels.FirstOrDefaultAsync(c => c.Id == order.CarsBrandsModelsId);
				}
				_autodborContext.Add(order);
				await _autodborContext.SaveChangesAsync();
				Program.Bot.SendInfo(order);
				EmailService emailService = new EmailService();
				await emailService.SendEmailAsync($"<p>{GetOrderIfo(order)}</p>");
			}
			catch
			{
				return BadRequest();
			}
			return Ok();
		}

		[HttpPost]
		public async Task<IActionResult> CreateCallBackAndAdditionalService(string userName, string phoneNumber, string email, string comment, string serviceName)
		{
			try
			{
				Services service = new Services();
				if (serviceName == null)
					service = await _autodborContext.Services.FirstOrDefaultAsync(s => s.NameRu == "Обратный звонок");
				else
					service = await _autodborContext.Services.FirstOrDefaultAsync(s => s.NameRu == serviceName);

				Orders order = new Orders();
				order.Email = email;
				order.OrderTime = DateTime.Now;
				order.PhoneNumber = phoneNumber;
				order.UserName = userName;
				order.ServicesId = service.Id;
				order.Services = service;
				order.Comment = comment;
				_autodborContext.Add(order);
				await _autodborContext.SaveChangesAsync();
				Program.Bot.SendInfo(order);
				EmailService emailService = new EmailService();
				await emailService.SendEmailAsync($"<p>{GetOrderIfo(order)}</p>");
			}
			catch
			{
				return BadRequest();
			}
			return Ok();
		}

		private StringBuilder GetOrderIfo(Orders order)
		{
			StringBuilder info = new StringBuilder(
			  $"Название заказа: {order.Services.NameRu}.\n" +
			  $"Номер телефона: {order.PhoneNumber}.\n");
			if (order.UserName != null)
				info.Append($"Имя пользователя - {order.UserName}.\n");
			if (order.Email != null)
				info.Append($"Почта: {order.Email}.\n");
			if (order.CarsBrands != null)
				info.Append($"Марка: {order.CarsBrands.Brand}.\n");
			if (order.CarsBrandsModels != null)
				info.Append($"Модель: {order.CarsBrandsModels.Model}.\n");
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
