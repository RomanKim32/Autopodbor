using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace Autopodbor_312.Models
{
	public class Orders
	{
		public int Id { get; set; }
		public int ServicesId { get; set; }
		public Services Services { get; set; }
		public DateTime OrderTime { get; set; }
		#nullable enable
        public string? UserName { get; set; }
		#nullable disable
        [Required(ErrorMessage = "please enter username")]
        public string PhoneNumber { get; set; }
		#nullable enable
        public string? Email { get; set; }
		public int? CarsBrandsId { get; set; }
		public CarsBrands? CarsBrands { get; set; }
		public int? CarsYearsId { get; set; }
		public CarsYears? CarsYears { get; set; }
		public int? CarsBodyTypesId { get; set; }
		public CarsBodyTypes? CarsBodyTypes { get; set; }
		public int? CarsFuelsId { get; set; }
		public CarsFuels? CarsFuels { get; set; }
		public string? Comment { get; set; }
		public string? Price { get; set; }
	}
}
