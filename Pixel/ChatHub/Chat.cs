using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pixel.Database;
using Pixel.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pixel.ChatHub
{
    public class Chat : Hub
    {
        private ILogger<Chat> _logger;
        private AccountContext _context;
        private MessageModel _user;
        private static Dictionary<string, string> connections = new Dictionary<string, string>();

        public Chat(AccountContext context, MessageModel user, ILogger<Chat> logger)
        {
            _context = context;
            _user = user;
            _logger = logger;
        }

        public override Task OnConnectedAsync()
        {
            var name = Context.User.Identity.Name;
            if(connections.ContainsKey(name))
            {
                connections[name] = Context.ConnectionId;
            }
            else
            {
                connections.Add(name, Context.ConnectionId);
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var name = Context.User.Identity.Name;
            //connections.Remove(name);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string userFrom, string message, string userTo)
        {
            try
            {
                var chatMessage = new ChatMessage(userFrom, userTo, message, DateTime.Now);
                await Task.Run(() => Save(userFrom, message, userTo));

                if (connections.ContainsKey(userTo))
                {
                    await Clients.Clients(connections[userFrom], connections[userTo]).SendAsync("ReceiveMessage", userFrom, chatMessage.ToString(), userTo);
                }
                else
                {
                    await Clients.Clients(connections[userFrom]).SendAsync("ReceiveMessage", userFrom, chatMessage.ToString(), userTo);
                }
                
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task ClearNotification(string userFrom, string userTo)
        {
            var messageModel = await _context.MessageModel.FirstOrDefaultAsync(user => user.UserFrom == userFrom && user.UserTo == userTo || (user.UserTo == userFrom && user.UserFrom == userTo));
            if(messageModel != null)
            {
                messageModel.UserToRead = true;
                messageModel.UserFromRead = true;
                await _context.SaveChangesAsync();
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


