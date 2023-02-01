using Autopodbor_312.Interfaces;
using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Autopodbor_312.Controllers
{
	public class AdminController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly IAdminRepository _adminRepository;

		public AdminController(IAdminRepository adminRepository, UserManager<User> userManager, SignInManager<User> signInManager)
		{
			_adminRepository = adminRepository;
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[HttpGet]
		public IActionResult Login(string returnUrl = null)
		{
			return View(_adminRepository.Login(returnUrl));
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

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> LogOff()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

		[Authorize(Roles = "admin")]
		public IActionResult Index()
		{
			var usersList = _adminRepository.Index().Where(u => u.Id != Convert.ToInt32(_userManager.GetUserId(User))).ToList();
			return View(usersList);
		}

		[Authorize(Roles = "admin")]
		[HttpGet]
		public IActionResult Register()
		{
			ViewData["Role"] = _adminRepository.GetAllRolesExceptAdmin();
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
			ViewData["Role"] = _adminRepository.GetAllRolesExceptAdmin();
			return View(model);
		}

		[Authorize(Roles = "admin")]
		[HttpGet]
		public async Task<IActionResult> EditUser(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			var userRoles = await _userManager.GetRolesAsync(user);
			var model = new EditUserViewModel
			{
				Id = user.Id,
				Email = user.Email,
				UserName = user.Email,
				Role = userRoles.FirstOrDefault(),
			};
			ViewData["Role"] = _adminRepository.GetAllRolesExceptAdmin();
			return View(model);
		}

		[Authorize(Roles = "admin")]
		[HttpPost]
		public async Task<IActionResult> EditUser(EditUserViewModel model)
		{
			var user = await _userManager.FindByIdAsync(model.Id.ToString());
			user.Email = model.Email;
			user.UserName = model.Email;
			var roles = await _userManager.GetRolesAsync(user);
			string userRole = roles.FirstOrDefault();
			var result = await _userManager.UpdateAsync(user);
			await _userManager.RemovePasswordAsync(user);
			await _userManager.AddPasswordAsync(user, model.Password);
			await _userManager.RemoveFromRoleAsync(user, userRole);
			await _userManager.AddToRoleAsync(user, model.Role);
			_adminRepository.UpdateAndSaveUser(user);
			if (result.Succeeded)
			{
				if (model.Role == "admin")
				{
					return RedirectToAction("AdminArea");
				}
				else
				{
					return RedirectToAction("Index");
				}
			}
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}
			ViewData["Role"] = _adminRepository.GetAllRolesExceptAdmin();
			return View(model);
		}

		[Authorize(Roles = "admin")]
		public IActionResult DeleteUser(int id)
		{
			int adminId = Convert.ToInt32(_userManager.GetUserId(User));
			return PartialView("UserPar", _adminRepository.DeleteUser(id, adminId));
		}

		[Authorize(Roles = "admin")]
		public async Task<IActionResult> EditAdminPassword()
		{
			var user =  await _userManager.FindByEmailAsync("mgaldobin@mail.ru");
			var userRoles = await _userManager.GetRolesAsync(user);
			var model = new EditUserViewModel
			{
				Id = user.Id,
				Email = user.Email,
				UserName = user.Email,
				Role = userRoles.FirstOrDefault(),
			};
			return View(model);
		}

		[HttpGet]
		[Authorize(Roles = "admin, portfolioManager, mediaManager")]
		public IActionResult AdminArea()
		{
			return View();
		}
	}
}
