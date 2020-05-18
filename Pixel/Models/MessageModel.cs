using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pixel.Models
{
    public class MessageModel
    {
        [Key]
        public int MessageId { get; set; } 
        [Required]
        [Column(TypeName = "varchar(450)")]
        public string UserTo { get; set; }
        [Required]
        [Column(TypeName = "varchar(450)")]
        public string UserFrom { get; set; }   
        public string MessageStore { get; set; }
        [Required]
        public bool UserToRead { get; set; }
        [Required]
        public bool UserFromRead { get; set; }
        [NotMapped]
        public string MessageSend { get; set; }

        [NotMapped]
        public List<ChatMessage> Messages { get; set; }

        public void GetMessages()
        {
            var messages = (MessageStore+MessageSend).Split("<br>");
            Messages = new List<ChatMessage>();

            foreach(var m in messages)
            {
                Messages.Add(ChatMessage.FromString(this, m));
            }
            Messages.RemoveAll(m => m == null);
        }
    }
}
