using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace Autopodbor_312.Models
{
	public class Orders
	{
		public int Id { get; set; }
		public int ServicesId { get; set; }
		public virtual Services Services { get; set; }
		public DateTime OrderTime { get; set; }
#nullable enable
		
		public string? UserName { get; set; }
#nullable disable
	
		public string PhoneNumber { get; set; }
		#nullable enable
        public string? Email { get; set; }
		
		public int? CarsBrandsId { get; set; }
		public virtual CarsBrands? CarsBrands { get; set; }
		
		public int? CarsYearsId { get; set; }
		public virtual CarsYears? CarsYears { get; set; }
		
		public int? CarsBodyTypesId { get; set; }
		public virtual CarsBodyTypes? CarsBodyTypes { get; set; }
		
		public int? CarsFuelsId { get; set; }
		public virtual CarsFuels? CarsFuels { get; set; }

		public int? CarsBrandsModelsId { get; set; }
		public virtual CarsBrandsModel? CarsBrandsModels { get; set; }
		public string? Comment { get; set; }
	}
}
