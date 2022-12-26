using Autopodbor_312.Models;
using System.Collections.Generic;

namespace Autopodbor_312.ViewModel
{
	public class CalculatorViewModel
	{
		public IEnumerable<CarsBodyTypes> CarsBodyTypes { get; set; }
		public IEnumerable<CarsBrands> CarsBrands { get; set; }
		public IEnumerable<CarsFuels> CarsFuels { get; set; }
		public IEnumerable<CarsYears> CarsYears { get; set; }
		public IEnumerable<CarsBrandsModel> CarsBrandsModels { get; set; }
	}
}
