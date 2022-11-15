﻿using System;
using System.Security.Policy;

namespace Autopodbor_312.Models
{
	public class Orders
	{
		public int Id { get; set; }
		public int ServicesId { get; set; }
		public Services Services { get; set; }
		public DateTime OrderTime { get; set; }
		public string UserName { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public int CarsBrandsId { get; set; }
		public CarsBrands CarsBrands { get; set; }
		public int CarsYearsId { get; set; }
		public CarsYears CarsYears { get; set; }
		public int CarsBodyTypesId { get; set; }
		public CarsBodyTypes CarsBodyTypes { get; set; }
		public int CarsEnginesId { get; set; }
		public CarsEngines CarsEngines { get; set; }
		public int CarsFuelsId { get; set; }
		public CarsFuels CarsFuels { get; set; }
		public string Comment { get; set; }
	}
}