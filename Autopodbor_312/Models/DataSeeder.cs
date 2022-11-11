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
                var countries = new List<Services>
                {
                    new Services { Name = "Подбор под ключ", Description = "" },
                    new Services { Name = "Выездной осмотр", Description = "" }
                };
                context.AddRange(countries);
                context.SaveChanges();
            }
        }
    }
}
