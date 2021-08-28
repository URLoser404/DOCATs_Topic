using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Smart_home.DTO
{
    public class DeviceDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public bool status { get; set; }
    }
}