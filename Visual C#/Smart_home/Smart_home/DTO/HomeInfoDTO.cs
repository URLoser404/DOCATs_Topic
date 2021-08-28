using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Smart_home.DTO
{
    public class HomeInfoDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool status { get; set; }
        public IList<AreaSimpleDTO> Areas{ get; set; }
    }
}