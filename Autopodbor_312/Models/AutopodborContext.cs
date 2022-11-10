using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Autopodbor_312.Models
{
    public class AutopodborContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<News> News { get; set; }
        public DbSet<Portfolio> Portfolio { get; set; }
        public DbSet<Services> Services { get; set; }

        public AutopodborContext(DbContextOptions<AutopodborContext> options) : base(options)
        {

        }
    }
}
