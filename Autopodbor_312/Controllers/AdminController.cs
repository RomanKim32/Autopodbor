using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Autopodbor_312.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AutopodborContext _context;
        private IWebHostEnvironment _appEnvironment;

        public AdminController(UserManager<User> userManager, SignInManager<User> signInManager, AutopodborContext context, IWebHostEnvironment webHost)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _appEnvironment = webHost;  
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                    return View(model);
                }
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(
                    user,
                    model.Password,
                    model.RememberMe,
                    false
                    );
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Неправильный логин и (или) пароль");
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            var usersList = _context.Users.Where(u => u.Id != Convert.ToInt32(_userManager.GetUserId(User))).ToList();
            return View(usersList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet, ActionName("IndexServices")]
        public async Task<IActionResult> IndexServices()
        {
            var sercices = await _context.Services.Where(s => s.Name != "Обратный звонок").Where(s => s.isAdditional == false).ToListAsync();
            return View( sercices);
        }

        [HttpGet, ActionName("CreateServices")]
        public IActionResult CreateServices()
        {
            return View();
        }

        [HttpPost,ActionName("CreateServices")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateServices(IFormFile servicePhotoFile, [Bind("Id,Name,Description")] Services services)
        {
            if (servicePhotoFile != null)
            {
                string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/serviceImg/{servicePhotoFile.FileName}");
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await servicePhotoFile.CopyToAsync(fileStream);
                }
                services.Photo = $"/serviceImg/{servicePhotoFile.FileName}";
            }
            if (ModelState.IsValid)
            {
                services.isAdditional = true;
                _context.Add(services);
                await _context.SaveChangesAsync();
                if (services.isAdditional == true)
                    return RedirectToAction("AdditionalServicesDetails", "Admin");
                return RedirectToAction("IndexServices", "Admin");
            };
            return View(services);
        }


        [HttpGet, ActionName("EditServices")]
        public async Task<IActionResult> EditServices(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        [HttpPost, ActionName("EditServices")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditServices(IFormFile servicePhotoFile, int id, [Bind("Id,Name,Description,isAdditional,Photo")] Services service)
        {
            if (servicePhotoFile == null)
            { 
                var currentService = await _context.Services.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
                service.Photo = currentService.Photo;         
            }
            else 
            {
                string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/serviceImg/{servicePhotoFile.FileName}");
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await servicePhotoFile.CopyToAsync(fileStream);
                }
                service.Photo = $"/serviceImg/{servicePhotoFile.FileName}";
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicesExists(service.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if(service.isAdditional == true)
                    return RedirectToAction("AdditionalServicesDetails", "Admin");
                return RedirectToAction("IndexServices", "Admin");
            }
            return View(service);
        }

        [HttpGet, ActionName("DeleteServices")]
        public async Task<IActionResult> DeleteServices(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var  services = await _context.Services.FirstOrDefaultAsync(m => m.Id == id);
            if (services == null)
            {
                return NotFound();
            }

            return View(services);
        }

        [HttpPost, ActionName("DeleteServices")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedServices(int id)
        {
            var services = await _context.Services.FindAsync(id);
            _context.Services.Remove(services);
            await _context.SaveChangesAsync();
            return RedirectToAction("AdditionalServicesDetails", "Admin");
        }

        private bool ServicesExists(int id)
        {
            return _context.Services.Any(e => e.Id == id);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Register()
        {
            ViewData["Role"] = _context.Roles.Where(r => r.Name != "admin").ToList();
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email };


                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                    return RedirectToAction("Index", "Admin");                  
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        error.Description.Take(1).ToList()[0].ToString();
                    }
                }
            }
            ViewData["Role"] = _context.Roles.Where(r => r.Name != "admin").ToList();
            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.Email,
                Role = userRoles.FirstOrDefault(),
            };
            ViewData["Role"] = _context.Roles.Where(r => r.Name != "admin").ToList();
            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Email = model.Email;
                user.UserName = model.Email;
                var roles = await _userManager.GetRolesAsync(user);
                string userRole = roles.FirstOrDefault();
                var result = await _userManager.UpdateAsync(user);
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, model.Password);
                await _userManager.RemoveFromRoleAsync(user, userRole);
                await _userManager.AddToRoleAsync(user, model.Role);
                _context.Update(user);
                await _context.SaveChangesAsync();

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                ViewData["Role"] = _context.Roles.Where(r => r.Name != "admin").ToList();
                return View(model);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public IActionResult AdminArea()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AdditionalServicesDetails()
        {
            var additionalServicesList = await _context.Services.Where(s => s.isAdditional == true).ToListAsync();
            return View(additionalServicesList);
        }
    }
}
