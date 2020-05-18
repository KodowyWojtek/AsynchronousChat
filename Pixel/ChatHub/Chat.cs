using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pixel.Database;
using Pixel.Models;
using System;
using System.Threading.Tasks;

namespace Pixel.ChatHub
{
    public class Chat : Hub
    {
        private  ILogger<Chat> _logger;
        private  AccountContext _context;
        private  MessageModel _user;
        public Chat(AccountContext context, MessageModel user, ILogger<Chat> logger)
        {
            _context = context;
            _user = user;
            _logger = logger;
        }
        public async Task SendMessage(string userFrom, string message, string userTo)
        {
            try
            {
                var chatMessage = new ChatMessage(userFrom, userTo, message, DateTime.Now);
                await Task.Run(() => Save(userFrom, message, userTo));
                await Clients.All.SendAsync("ReceiveMessage", userFrom, chatMessage.ToString(), userTo);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        public async Task Save(string userFrom, string message, string userTo)
        {
            try
            {
                var chatMessage = new ChatMessage(userFrom, userTo, message, DateTime.Now);
                var userDb = await _context.MessageModel.FirstOrDefaultAsync(user => user.UserFrom == userFrom && user.UserTo == userTo || (user.UserTo == userFrom && user.UserFrom == userTo));
                if (userDb == null)
                {
                    if (_user.MessageStore == null)
                        _user.MessageStore = "";
                    _user.MessageStore += chatMessage.ToString() + "<br>";
                    _user.UserFrom = userFrom;
                    _user.UserTo = userTo;
                    _user.UserFromRead = true;
                    _user.UserToRead = false;
                    _context.MessageModel.Add(_user);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    if (userDb.UserFrom == userFrom)
                    {
                        userDb.UserFromRead = true;
                        userDb.UserToRead = false;
                    }
                    else
                    {
                        userDb.UserFromRead = false;
                        userDb.UserToRead = true;
                    }
                    userDb.MessageStore += chatMessage.ToString() + "<br>";
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }          
        }
    }
}


