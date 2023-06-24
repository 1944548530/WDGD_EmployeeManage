using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManageApi.Models
{
    public class MenuChildren
    {
        public string path { get; set; }
        public string name { get; set; }
        public string component { get; set; }
        public MenuConfig meta { get; set; }

    }
}