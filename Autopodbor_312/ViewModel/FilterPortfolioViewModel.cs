using Autopodbor_312.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autopodbor_312.ViewModel
{
	public class FilterPortfolioViewModel
	{
		public SelectList CarsBrands { get; set; }
		public SelectList CarsModels { get; set; }
		public SelectList CarsBodyTypes { get; set; }
		public PaginationList<Portfolio> Portfolios { get; set; }
	}
}
