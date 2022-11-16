using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Autopodbor_312.Models
{
    public class AutopodborContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<News> News { get; set; }
        public DbSet<Portfolio> Portfolio { get; set; }
        public DbSet<Services> Services { get; set; }
		public DbSet<CarsBodyTypes> CarsBodyTypes { get; set; }
		public DbSet<CarsBrands> CarsBrands { get; set; }
		public DbSet<CarsFuels> CarsFuels { get; set; }
		public DbSet<CarsYears> CarsYears { get; set; }
		public DbSet<Orders> Orders { get; set; }


		public AutopodborContext(DbContextOptions<AutopodborContext> options) : base(options)
        {

        }
    }
}
