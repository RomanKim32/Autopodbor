﻿using Autopodbor_312.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autopodbor_312.ViewModel
{
	public class FilterPortfolioViewModel
	{
		public List<CarsBrands> CarsBrands { get; set; }
		public List<CarsBrandsModel> CarsModels { get; set; }
		public List<CarsBodyTypes> CarsBodyTypes { get; set; }
		public IEnumerable<CarsBrandsModel> CarsBrandsModels { get; set; }
		public IEnumerable<CarsBrands> CarsBrands1 { get; set; }
		public PaginationList<Portfolio> Portfolios { get; set; }
		#nullable enable
        public int? SelectedBrand { get; set; }
		public string? SelectedBrandString { get; set; }
        public int? SelectedModel { get; set; }
		public string? SelectedModelString { get; set; }
		public int? SelectedBodyType { get; set; }
		public string? SelectedBodyTypeString { get; set; }
	}
}
