using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Autopodbor_312.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AutopodborContext _autodborContext;

        public AdminController(UserManager<User> userManager, SignInManager<User> signInManager, AutopodborContext autopodborContext)
        {
            _userManager = userManager;
            _autodborContext = autopodborContext;
            _signInManager = signInManager;
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

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        //Services 
        [HttpGet, ActionName("IndexServices")]
        public async Task<IActionResult> IndexServices()
        {
            var sercices = await _autodborContext.Services.ToListAsync();
            return View( sercices);
        }

        // GET
        [HttpGet, ActionName("CreateServices")]
        public IActionResult CreateServices()
        {
            return View();
        }

        // POST
        [HttpPost,ActionName("CreateServices")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateServices([Bind("Id,Name,Description")] Services services)
        {
            if (ModelState.IsValid)
            {
                _autodborContext.Add(services);
                await _autodborContext.SaveChangesAsync();
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

            var dish = await _autodborContext.Services.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            return View(dish);
        }

        [HttpPost, ActionName("EditServices")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditServices(int id, [Bind("Id,Name,Description")] Services services)
        {
            if (id != services.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _autodborContext.Update(services);
                    await _autodborContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicesExists(services.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("IndexServices", "Admin");
            }
            return View(services);
        }

        // GET
        [HttpGet, ActionName("DeleteServices")]
        public async Task<IActionResult> DeleteServices(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var  services = await _autodborContext.Services.FirstOrDefaultAsync(m => m.Id == id);
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
            var services = await _autodborContext.Services.FindAsync(id);
            _autodborContext.Services.Remove(services);
            await _autodborContext.SaveChangesAsync();
            return RedirectToAction("IndexServices", "Admin");
        }

        private bool ServicesExists(int id)
        {
            return _autodborContext.Services.Any(e => e.Id == id);
        }
    }
}
