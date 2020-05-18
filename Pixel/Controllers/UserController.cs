using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pixel.Database;
using Pixel.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Pixel.Controllers
{
    public class UserController : Controller
    {
        private readonly AccountContext _context;
        private MessageModel _messageModel; 
        public UserController(AccountContext contetx, MessageModel messageModel)
        {
            _messageModel = messageModel;
            _context = contetx;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Notifications(int? page)
        {
            List<string> unreadUsersMessages = new List<string>();
            var modelOne = await _context.MessageModel.Where(user => user.UserFrom == User.Identity.Name && user.UserFromRead == false).Select(user => user.UserTo).ToListAsync();
            var modelSecond = await _context.MessageModel.Where(user => user.UserTo == User.Identity.Name && user.UserToRead == false).Select(user => user.UserFrom).ToListAsync();
            unreadUsersMessages = modelOne.Concat(modelSecond).ToList();
            var pagedModel = unreadUsersMessages.ToPagedList(page ?? 1, 5);
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
            var userValues = await _context.MessageModel.FirstOrDefaultAsync(user => user.UserFrom == User.Identity.Name && user.UserTo == UserLogin || (user.UserTo == User.Identity.Name && user.UserFrom == UserLogin));
            if (userValues == null)
            {
                _messageModel.UserTo = UserLogin;
                _messageModel.UserFrom = User.Identity.Name;
                _messageModel. MessageStore = string.Empty;
                _messageModel.MessageSend = string.Empty;
            }
            else
            {
                if(userValues.UserFrom == User.Identity.Name)
                {
                    _messageModel.UserFrom = User.Identity.Name;
                    _messageModel.UserTo = UserLogin;
                    _messageModel.UserFromRead = true;
                    userValues.UserFromRead = true;
                    userValues.UserToRead = false;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _messageModel.UserFrom = UserLogin;
                    _messageModel.UserTo = User.Identity.Name;
                    _messageModel.UserToRead = true;
                    userValues.UserFromRead = false;
                    userValues.UserToRead = true;
                    await _context.SaveChangesAsync();
                }
                _messageModel.MessageStore = userValues.MessageStore;
                _messageModel.MessageSend = string.Empty;               
            }            
            return View(_messageModel);
        }
    }
}  