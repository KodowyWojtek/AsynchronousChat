using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Pixel.Database;
using Pixel.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pixel.ChatHub
{

    public class Chat : Hub
    {
        private readonly AccountContext _context;
        private readonly MessageModel _user;
        public Chat(AccountContext context, MessageModel user)
        {
            _context = context;
            _user = user;
        }
        public async Task SendMessage(string user, string message, string userTo)
        {
            await Task.Run(() =>  Save(user, message, userTo));
            await Clients.All.SendAsync("ReceiveMessage", user, message, userTo);
        }

        public async Task Save(string user, string message, string userTo)
        {
            try
            {
                _user.UserFrom = user;
                _user.UserTo = userTo;

                var userDb = await _context.MessageModel.Where(user => user.UserFrom == _user.UserFrom && user.UserTo == _user.UserTo || (user.UserTo == _user.UserFrom && user.UserFrom == _user.UserTo)).FirstOrDefaultAsync();
                if (userDb == null)
                {
                    if (_user.MessageStore == null)
                        _user.MessageStore = "";
                    _user.MessageStore = user.ToString() + ": " + message + "<br>";
                    _context.MessageModel.Add(_user);
                    await _context.SaveChangesAsync();                    
                }   
                else
                {
                    userDb.MessageStore += user.ToString() + ": " + message + "<br>";
                    await _context.SaveChangesAsync();
                }              

            }
            catch (Exception ex)
            {

            }

        }
    }
}

