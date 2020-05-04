using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pixel.Models
{
    public class ChatModel
    {
        public string UserFrom { get; set; }

        public string UserTo { get; set; }

        public List<string> Messages { get; set; }
        public string message { get; set; }
    }
}
