using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pixel.Models
{
    public class UsersModel
    {
        [Key]
        [Column(TypeName = "varchar(450)")]
        public string UserId { get; set; }
        public string UserLogin { get; set; }
        [DisplayName("First name")]
        public string FirstName { get; set; }
        [DisplayName("Last name")]
        public string LastName { get; set; }
        [DisplayName("Date of birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [DisplayName("Gender")]
        public string Gender { get; set; }
        public DateTime DateOfCreation { get; set; }     
    }
}
