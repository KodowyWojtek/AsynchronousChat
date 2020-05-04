using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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
        [NotMapped]
        public string MessageSend { get; set; }

    }
}
