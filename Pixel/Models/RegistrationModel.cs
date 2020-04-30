using Pixel.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Models
{   
    public class RegistrationModel
    {        
        [Required]
        [DisplayName("Email address")]
        [DataType(DataType.EmailAddress)]
        public string UserLogin { get; set; }
        [Required]
        [DisplayName("First name")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last name")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }
        [Required]
        [DisplayName("Date of birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; } 
        [Required]
        [DisplayName("Gender")]      
        public Gender Gender { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("User password")]        
        public string UserPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirm password")]
        [Compare("UserPassword", ErrorMessage="Password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }      
        public DateTime DateOfCreation { get; set; }
    }
}
