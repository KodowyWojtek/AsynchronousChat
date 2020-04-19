using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pixel.Database;
using Pixel.Models;
using System;
using System.Threading.Tasks;

namespace Pixel.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AccountContext _context;
        public AccountController(AccountContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, AccountContext cotnext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.UserLogin, Email = model.UserLogin };
                var result = await _userManager.CreateAsync(user, model.UserPassword);
                var userModel = new UsersModel { UserId = user.Id, UserLogin = model.UserLogin, FirstName = model.FirstName, LastName = model.LastName, DateOfBirth = model.DateOfBirth, Gender = model.Gender.ToString(), DateOfCreation = DateTime.Now };
                await _context.UsersModel.AddAsync(userModel);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("index", "home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserLogin, model.UserPassword, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }

                ModelState.AddModelError("", "Invalid login attempt");
            }
            return View(model);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Account()
        {
            var model = await _context.UsersModel.FirstOrDefaultAsync(user => user.UserId == _userManager.GetUserId(HttpContext.User));
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Account(UsersModel model)
        {
            var update = await _context.UsersModel.FirstOrDefaultAsync(user => user.UserId == _userManager.GetUserId(HttpContext.User));
            update.FirstName = model.FirstName;
            update.LastName = model.LastName;
            update.DateOfBirth = model.DateOfBirth;
            update.Gender = Enum.GetName(typeof(Gender), int.Parse(model.Gender));
            _context.Update(update);
            await _context.SaveChangesAsync();


            return View(model);
        }
    }
}