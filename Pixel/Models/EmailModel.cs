using System;

namespace Pixel.Models
{
    public class EmailModel
    {
        public int Id { get; set; }
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime DateOfPost { get; set; }
        public UsersModel UsersModel { get; set; }

    }
}
