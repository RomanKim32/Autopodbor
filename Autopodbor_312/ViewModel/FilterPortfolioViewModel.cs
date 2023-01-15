using Autopodbor_312.Models;
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
		public PaginationList<Portfolio> Portfolios { get; set; }
	}
}
