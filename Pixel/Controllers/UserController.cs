using System;
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
            var model = await _context.UsersModel.Where(user=>user.UserLogin != User.Identity.Name).Select(user => user.UserLogin).ToListAsync();
            var pagedModel = model.ToPagedList(page ?? 1, 5);
            return View(pagedModel);
        }
        [Authorize]
        [HttpPost]
        public IActionResult UserList(string UserLogin)
        {
            if (UserLogin != null)
            {
                return RedirectToAction("UserChat", "User", new { UserLogin = UserLogin });
            }
            return RedirectToAction("UserList", "User");
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UserChat(string UserLogin)
        {
            var message = await _context.MessageModel.Where(user => user.UserFrom == User.Identity.Name && user.UserTo == UserLogin || (user.UserTo == User.Identity.Name && user.UserFrom == UserLogin)).Select(user => user.MessageStore).FirstOrDefaultAsync() ?? "";
            MessageModel messageModel = new MessageModel
            {
                UserTo = UserLogin,
                UserFrom = User.Identity.Name,
                MessageStore = message,
                MessageSend = "",
            };

            return View(messageModel);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UserChat(MessageModel messageModel)
        {
            var user = await _context.MessageModel.Where(user => user.UserFrom == User.Identity.Name && user.UserTo == messageModel.UserTo || (user.UserTo == User.Identity.Name && user.UserFrom == messageModel.UserTo)).FirstOrDefaultAsync();
            if(user == null)
            {
                messageModel.MessageStore = User.Identity.Name.ToString() + ": " + messageModel.MessageSend + "<br>";
                _context.MessageModel.Add(messageModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("UserChat", "User", new { UserLogin = messageModel.UserTo });
            }
            user.MessageStore += User.Identity.Name.ToString() + ": " + messageModel.MessageSend + "<br>";
            await _context.SaveChangesAsync();

            return RedirectToAction("UserChat", "User", new { UserLogin = messageModel.UserTo });
        }
    }
}