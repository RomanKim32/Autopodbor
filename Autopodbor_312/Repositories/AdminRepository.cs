using Autopodbor_312.Interfaces;
using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;




namespace Autopodbor_312.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AutopodborContext _context;
        private readonly IServiceScopeFactory _serviceScopeFactory;


        public AdminRepository(AutopodborContext autopodborContext, IServiceScopeFactory serviceScopeFactory)
        {
            _context = autopodborContext;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public LoginViewModel Login(string returnUrl)
        {
            return new LoginViewModel { ReturnUrl = returnUrl };
        }

        public IEnumerable<User> Index()
        {
            var usersList = _context.Users.ToList();
            return usersList;
        }

        public List<IdentityRole<int>> GetAllRolesExceptAdmin()
        {
            List<IdentityRole<int>> roles = _context.Roles.Where(r => r.Name != "admin").ToList(); 
            return roles;
        }

        public List<User> DeleteUser(int id, int adminId)
        {
            var user = _context.Users.Find(id);
            _context.Users.Remove(user);
            _context.SaveChanges();
            var users = _context.Users.Where(u => u.Id != adminId).ToList();
            return users;
        }

        public void UpdateAndSaveUser(User user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }
    }
}
