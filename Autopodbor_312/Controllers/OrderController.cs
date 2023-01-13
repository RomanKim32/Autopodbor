﻿using Autopodbor_312.Interfaces;
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
		private readonly IOrderRepository _orderRepository;

		public OrderController(IOrderRepository orderRepository)
		{
			_orderRepository = orderRepository;
		}

		public IActionResult CreateOrder(string serviceName)
		{
			if (serviceName == null)
			{
				return NotFound();
			}
			return View(_orderRepository.CreateOrder(serviceName));
		}

		[HttpPost]
		public IActionResult CreateOrder(string userName, string phoneNumber, string email, string comment, string carsBrandsId, string carsBodyTypesId, string carsYearsId,string carsFuelsId, string serviceId, string modelId)
		{
			try
			{
				_orderRepository.CreateOrder(userName, phoneNumber, email, comment, carsBrandsId, carsBodyTypesId, carsYearsId, carsFuelsId, serviceId, modelId);
			}
			catch
			{
				return BadRequest();
			}
			return Ok();
		}

		[HttpPost]
		public IActionResult CreateCallBackAndAdditionalService(string userName, string phoneNumber, string email, string comment, string serviceName)
		{
			try
			{
				 _orderRepository.CreateCallBackAndAdditionalService(userName, phoneNumber, email, comment, serviceName);
			}
			catch
			{
				return BadRequest();
			}
			return Ok();
		}
	}
}
