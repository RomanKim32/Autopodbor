using System.Collections.Generic;
using System.Linq;

namespace Autopodbor_312.Models
{
    public class DataSeeder
    {
        public static void SeedCountries(AutopodborContext context)
        {
            if (!context.Services.Any())
            {
                var services = new List<Services>
                {
                    new Services { Name = "Подбор под ключ", Description = "" },
                    new Services { Name = "Выездной осмотр", Description = "" }
                };
                context.AddRange(services);
                context.SaveChanges();
            }
            if (!context.CarsBrands.Any())
            {
                var brands = new List<CarsBrands>
                {
                    new CarsBrands{ Brand = "Toyota", Price = "300"},
                    new CarsBrands{ Brand = "Mercedes-Benz", Price = "500"},
                    new CarsBrands{ Brand = "Audi", Price = "400"}
                };
				context.AddRange(brands);
				context.SaveChanges();
			}
			if (!context.CarsYears.Any())
			{
				var years = new List<CarsYears>
				{
					new CarsYears{ ManufacturesYear = "До 2000", Price = "200"},
					new CarsYears{ ManufacturesYear = "До 2010", Price = "300"},
					new CarsYears{ ManufacturesYear = "До 2020", Price = "400"},
                    new CarsYears{ ManufacturesYear = "Выше 2020", Price = "500"}
                };
				context.AddRange(years);
				context.SaveChanges();
			}
            if (!context.CarsBodyTypes.Any())
            {
                var bodies = new List<CarsBodyTypes>
                {
                    new CarsBodyTypes{ BodyType = "Седан", Price = "300"},
					new CarsBodyTypes{ BodyType = "Хэтчбек", Price = "200"},
					new CarsBodyTypes{ BodyType = "Минивэн", Price = "500"},
					new CarsBodyTypes{ BodyType = "Внедорожник", Price = "500"},
					new CarsBodyTypes{ BodyType = "Купе", Price = "300"}
				};
				context.AddRange(bodies);
				context.SaveChanges();
			}
            if (!context.CarsFuels.Any())
            {
                var fuels = new List<CarsFuels>
                {
                    new CarsFuels{ FuelsType = "Гибрид", Price = "300"},
					new CarsFuels{ FuelsType = "Не гибрид", Price = "100"}
				};
				context.AddRange(fuels);
				context.SaveChanges();
			}
		}
    }
}
