using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManageApi.Models
{
    public class MenuList
    {
        public string path { get; set; }
        public string component { get; set; }
        public string redirect { get; set; }
        public string name { get; set; }
        public MenuConfig meta { get; set; }
        public List<MenuChildren> children { get; set; }
    }
}