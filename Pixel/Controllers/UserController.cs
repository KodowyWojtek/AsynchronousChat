using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pixel.Database;
using Pixel.Models;

namespace Pixel.Controllers
{
    public class UserController : Controller
    {
        private readonly AccountContext _context;
        public UserController(AccountContext contetx)
        {
            _context = contetx;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> User()
        {       
            var model = await _context.UsersModel.Select(user=>user.UserLogin).ToListAsync();
            return View(model);
        }
        [Authorize]
        [HttpPost] 
        public async Task<IActionResult> User(string UserLogin)
        {          
            if(UserLogin != null)
            {
                return RedirectToAction("UserChat", "User", new { UserLogin = UserLogin });
            }
            return RedirectToAction("User", "User");
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UserChat(string UserLogin)
        {
            
            return View();
        }
    }
}