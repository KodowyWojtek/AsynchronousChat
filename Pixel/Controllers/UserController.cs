﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pixel.Database;
using Pixel.Models;
using X.PagedList;

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
        public async Task<IActionResult> UserList(int? page)
        {
            var model = await _context.UsersModel.Select(user => user.UserLogin).ToListAsync();
            var pagedModel = model.ToPagedList(page ?? 1, 5);
            return View(pagedModel);
        }
        [Authorize]
        [HttpPost] 
        public IActionResult UserList(string UserLogin)
        {          
            if(UserLogin != null)
            {
                return RedirectToAction("UserChat", "User", new { UserLogin = UserLogin });
            }
            return RedirectToAction("UserList", "User");
        }
        [Authorize]
        [HttpGet]
        public IActionResult UserChat(string UserLogin)
        {            
            return View(UserLogin);
        }
    }
}