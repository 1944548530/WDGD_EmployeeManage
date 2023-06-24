using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManageApi.Models
{
    public class Menu
    {
        public string MenuId { get; set; }
        public string MenuPath { get; set; }
        public string redirect { get; set; }
        public string MenuName { get; set; }
        public string MenuIcon { get; set; }
        public string parentId { get; set; }
        public string component { get; set; }
    }
}