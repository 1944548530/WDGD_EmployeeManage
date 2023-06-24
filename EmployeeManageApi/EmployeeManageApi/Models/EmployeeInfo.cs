using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManageApi.Models
{
    public class EmployeeInfo
    {
        public string employeeId { get; set; }
        public string cname { get; set; }
        public string department { get; set; }
        public string indate { get; set; }
        public string grade { get; set; }
        public string postSkill { get; set; }
        public string postNow { get; set; }
        public string isValid { get; set; }
        public string lmdate { get; set; }
        public string lmtime { get; set; }
        public string shift { get; set; }
    }
}