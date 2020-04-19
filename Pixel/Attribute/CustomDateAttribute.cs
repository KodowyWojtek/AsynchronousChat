using System;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attribute
{
    public class CustomDateAttribute : RangeAttribute
    {
        public CustomDateAttribute() : base(typeof(DateTime),
            "1/1/1900",
            DateTime.Now.ToShortDateString())
        {
               
        }
    }
}
