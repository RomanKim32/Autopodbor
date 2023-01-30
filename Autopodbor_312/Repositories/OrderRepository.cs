using Autopodbor_312.Interfaces;
using Autopodbor_312.Models;
using Autopodbor_312.OrderMailing;
using Autopodbor_312.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text;

namespace Autopodbor_312.Repositories
{
	public class OrderRepository : IOrderRepository
	{
		private readonly AutopodborContext _context;
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public OrderRepository(AutopodborContext autopodborContext, IServiceScopeFactory serviceScopeFactory)
		{
			_context = autopodborContext;
			_serviceScopeFactory = serviceScopeFactory;
		}

		public async void CreateCallBackAndAdditionalService(string userName, string phoneNumber, string email, string comment, string serviceName)
		{
			Orders order = new Orders();
			using (var scope = _serviceScopeFactory.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetRequiredService<AutopodborContext>();
				Services service = new Services();
				if (serviceName == null)
					service = dbContext.Services.FirstOrDefault(s => s.NameRu == "Обратный звонок");
				else
					service = dbContext.Services.FirstOrDefault(s => s.NameRu == serviceName || s.NameKy == serviceName);

				order.Email = email;
				order.OrderTime = DateTime.Now;
				order.PhoneNumber = phoneNumber;
				order.UserName = userName;
				order.ServicesId = service.Id;
				order.Services = service;
				order.Comment = comment;
				dbContext.Orders.Add(order);
				dbContext.SaveChanges();
			}
			Program.Bot.SendInfo(order);
			EmailService emailService = new EmailService();
			await emailService.SendEmailAsync($"<p>{GetOrderIfo(order)}</p>");
		}

		public OrderViewModel CreateOrder(string serviceName)
		{
			var service = _context.Services.FirstOrDefault(s => s.NameRu == serviceName);
			Orders order = new Orders { Services = service, ServicesId = service.Id };
			var carsBodyTypes = _context.CarsBodyTypes.ToList();
			var carsBrands = _context.CarsBrands.ToList();
			var carsFuels = _context.CarsFuels.ToList();
			var carsYears = _context.CarsYears.ToList();
			var orderViewModel = new OrderViewModel
			{
				Order = order,
				CarsBodyTypes = carsBodyTypes,
				CarsBrands = carsBrands,
				CarsYears = carsYears,
				CarsFuels = carsFuels,
			};
			return orderViewModel;
		}

		public async void CreateOrder(string userName, string phoneNumber, string email, string comment, string carsBrandsId, string carsBodyTypesId, string carsYearsId, string carsFuelsId, string serviceId, string modelId)
		{
			Orders order = new Orders();
			using (var scope = _serviceScopeFactory.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetRequiredService<AutopodborContext>();
				order.Email = email;
				order.OrderTime = DateTime.Now;
				order.PhoneNumber = phoneNumber;
				order.UserName = userName;
				order.ServicesId = Convert.ToInt32(serviceId);
				order.Services = dbContext.Services.FirstOrDefault(s => s.Id == Convert.ToInt32(serviceId));
				order.Comment = comment;
				order.CarsBodyTypesId = Convert.ToInt32(carsBodyTypesId);
				order.CarsBodyTypes = dbContext.CarsBodyTypes.FirstOrDefault(c => c.Id == order.CarsBodyTypesId);
				order.CarsBrandsId = Convert.ToInt32(carsBrandsId);
				order.CarsBrands = dbContext.CarsBrands.FirstOrDefault(c => c.Id == order.CarsBrandsId);
				order.CarsFuelsId = Convert.ToInt32(carsFuelsId);
				order.CarsFuels = dbContext.CarsFuels.FirstOrDefault(c => c.Id == order.CarsFuelsId);
				order.CarsYearsId = Convert.ToInt32(carsYearsId);
				order.CarsYears = dbContext.CarsYears.FirstOrDefault(c => c.Id == order.CarsYearsId);
				if (modelId != null)
				{
					order.CarsBrandsModelsId = Convert.ToInt32(modelId);
					order.CarsBrandsModels = dbContext.CarsBrandsModels.FirstOrDefault(c => c.Id == order.CarsBrandsModelsId);
				}
				dbContext.Add(order);
				dbContext.SaveChanges();
			}
			Program.Bot.SendInfo(order);
			EmailService emailService = new EmailService();
			await emailService.SendEmailAsync($"<p>{GetOrderIfo(order)}</p>");
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
