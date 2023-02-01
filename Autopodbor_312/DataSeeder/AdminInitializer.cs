using Autopodbor_312.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Autopodbor_312.DataSeeder
{
    public class AdminInitializer
    {
        public static async Task SeedAdminUser(
            RoleManager<IdentityRole<int>> _roleManager,
            UserManager<User> _userManager)
        {
            string adminEmail = "mgaldobin@mail.ru";
            string adminPassword = "123123";
            var roles = new[] { "admin", "mediaManager", "portfolioManager" };
            foreach (var r in roles)
            {
                if (await _roleManager.FindByNameAsync(r) is null)
                    await _roleManager.CreateAsync(new IdentityRole<int>(r));
            }
            if (await _userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await _userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(admin, "admin");
            }
        }
    }
}
