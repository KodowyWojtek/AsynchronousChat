using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Models
{
    public class LoginModel
    {
        [Required]
        [DisplayName("Email address")]
        [DataType(DataType.EmailAddress)]
        public string UserLogin { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("User password")]
        public string UserPassword { get; set; }
        [DisplayName("Remember me")]
        public bool RememberMe { get; set; }
    }
}
