using Autopodbor_312.Models;
using System.Collections.Generic;

namespace Autopodbor_312.ViewModel
{
	public class CreatePortfolioViewModel
	{
		public Portfolio Portfolio { get; set; }
		public IEnumerable<CarsBrands> CarsBrands { get; set; }
		public IEnumerable<CarsBrandsModel> CarsBrandsModel { get; set; }
		public IEnumerable<CarsBodyTypes> CarsBodyTypes { get; set; }
	}
}
