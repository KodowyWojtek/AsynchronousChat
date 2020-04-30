using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pixel.Models
{
    public class UsersChat
    {
        [Key]
        public int MessageId { get; set; } 

        [Required]
        [Column(TypeName = "varchar(450)")]
        public string UserTo { get; set; }

        [Required]
        [Column(TypeName = "varchar(450)")]
        public string UserTwo { get; set; }

        [Required]
        public DateTime MessageDate { get; set; }

        [ForeignKey("UserTo")]
        public virtual UsersModel UserModelTo { get; set; }

        [ForeignKey("UserFrom")]
        public virtual UsersModel UserModelFrom { get; set; }

    }
}
