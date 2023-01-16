using Autopodbor_312.Interfaces;
using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;


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
            
        public void DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            _context.Users.Remove(user);
            _context.SaveChangesAsync();
        }
    }
}
