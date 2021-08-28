using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Smart_home.DTO
{
    public class DeviceInfoDTO
    {
        public int area_id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public bool status { get; set; }
        public Dictionary<string,string> properities { get; set; }
    }
}