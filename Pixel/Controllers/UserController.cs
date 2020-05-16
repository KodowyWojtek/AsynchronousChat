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
        public async Task<IActionResult> Notifications(int? page)
        {
            List<string> allNotReadedMessages = new List<string>();
            var userMessageOne = await _context.MessageModel.Where(user => user.UserFrom == User.Identity.Name && user.UserFromRead == false).Select(user => user.UserTo).ToListAsync();
            var userMessageSecond = await _context.MessageModel.Where(user => user.UserTo == User.Identity.Name && user.UserToRead == false).Select(user => user.UserFrom).ToListAsync();
            allNotReadedMessages = userMessageOne.Concat(userMessageSecond).ToList();
            var pagedModel = allNotReadedMessages.ToPagedList(page ?? 1, 5);
            return View(pagedModel);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UserList(int? page)
        {
            var model = await _context.UsersModel.Where(user => user.UserLogin != User.Identity.Name).Select(user => user.UserLogin).ToListAsync();
            var pagedModel = model.ToPagedList(page ?? 1, 5);
            return View(pagedModel);
        }
        [Authorize]
        [HttpPost]
        public IActionResult UserList(string UserLogin)
        {
            if (UserLogin != null)
                return RedirectToAction("UserChat", "User", new { UserLogin });
            return RedirectToAction("UserList", "User");
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UserChat(string UserLogin)
        {
            var userValues = await _context.MessageModel.Where(user => user.UserFrom == User.Identity.Name && user.UserTo == UserLogin || (user.UserTo == User.Identity.Name && user.UserFrom == UserLogin)).FirstOrDefaultAsync();
            MessageModel messageModel = new MessageModel();
            if (userValues == null)
            {
                messageModel.UserTo = UserLogin;
                messageModel.UserFrom = User.Identity.Name;
                messageModel. MessageStore = "";
                messageModel.MessageSend = "";
            }
            else
            {
                if(userValues.UserFrom == User.Identity.Name)
                {
                    messageModel.UserFrom = User.Identity.Name;
                    messageModel.UserTo = UserLogin;
                    messageModel.UserFromRead = true;
                    messageModel.UserToRead = false;
                    userValues.UserFromRead = true;
                    userValues.UserToRead = false;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    messageModel.UserFrom = UserLogin;
                    messageModel.UserTo = User.Identity.Name;
                    messageModel.UserFromRead = false;
                    messageModel.UserToRead = true;
                    userValues.UserFromRead = false;
                    userValues.UserToRead = true;
                    await _context.SaveChangesAsync();
                }
                messageModel.MessageStore = userValues.MessageStore;
                messageModel.MessageSend = "";               
            }            
            return View(messageModel);
        }
    }
}