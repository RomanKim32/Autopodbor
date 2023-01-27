using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Autopodbor_312.Interfaces
{
    public interface IAdminRepository
    {
        LoginViewModel Login(string returnUrl);

        IEnumerable<User> Index();

        List<IdentityRole<int>> GetAllRolesExceptAdmin();

        List<User> DeleteUser(int id, int adminId);

        void UpdateAndSaveUser(User user);
    }
}
