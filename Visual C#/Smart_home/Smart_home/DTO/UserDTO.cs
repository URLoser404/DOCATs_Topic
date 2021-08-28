using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Smart_home.DTO
{
    public class UserDTO
    {
        public string name { get; set; }
        public string account { get; set; }
        public string password { get; set; }
        public bool status { get; set; }
        public string country { get; set; }
        public string language { get; set; }
    }
}