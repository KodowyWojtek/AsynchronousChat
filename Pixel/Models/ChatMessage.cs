using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pixel.Models
{
    public class ChatMessage
    {
        public const string UrlFilter = "(https?:\\/\\/(?:www\\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\\.[^\\s]{2,}|www\\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\\.[^\\s]{2,}|https?:\\/\\/(?:www\\.|(?!www))[a-zA-Z0-9]+\\.[^\\s]{2,}|www\\.[a-zA-Z0-9]+\\.[^\\s]{2,})";
        public const string MessageRegex = "\\[.*\\] .*:.*";
        public string UserFrom { get; }
        public string UserTo { get; }
        public DateTime SendTime { get; }
        public string Content { get; }
        public MessageType Type { get; }
        public string Url { get; }

        public ChatMessage(string userFrom, string userTo, string content, DateTime time, MessageType type = 0)
        {
            UserFrom = userFrom;
            UserTo = userTo;
            Content = content;
            SendTime = time;
            if (type == 0)
                type = GetType(content);
            
            Type = type;
            if(type == MessageType.Link)
            {
                Url = Regex.Match(content, UrlFilter).Value;
            }
        }

        public static ChatMessage FromString(MessageModel users, string message)
        {
            if (Regex.IsMatch(message, MessageRegex))
            {
                var parts = message.Split(']');
                string date = parts[0].TrimStart('[');
                DateTime time = DateTime.Parse(date);
                parts = parts[1].Split(':');
                string from = parts[0];
                string content = parts[1];
                string to = (from.Equals(users.UserFrom)) ? users.UserTo : users.UserFrom;
                var type = GetType(content);
                return new ChatMessage(from, to, content, time, type);
            }
            else
                return null;
        }

        public static MessageType GetType(string content)
        {
            MessageType type = MessageType.Text;

            if(Regex.IsMatch(content, UrlFilter))
            {
                type = MessageType.Link;
            }

            return type;
        }

        public override string ToString()
        {
            string str = "[";

            str += SendTime.ToString();
            str += "] ";
            str += UserFrom + ": ";
            str += Content;

            return str;
        }
    }

    public enum MessageType
    {
        Text = 0,
        Link = 1
    }
}
