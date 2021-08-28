using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Smart_home.DTO
{
    public class AreaSimpleDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool status { get; set; }
        public DataTable devices { get; set; }
    }
}