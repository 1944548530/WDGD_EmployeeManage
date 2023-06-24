using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManageApi.Models
{
    public class Attendance
    {
        public string date { get; set; }
        public string shift { get; set; }
        public string department { get; set; }
        public string employeeId { get; set; }
        public string cname { get; set; }
        public string attendState { get; set; }
        public string attendStartTime { get; set; }
        public string attendEndTime { get; set; }
        public string borrowDept { get; set; }
        public string overStartTime { get; set; }
        public string overEndTime { get; set; }
        public string overTime { get; set; }
        public string overTimeHour { get; set; }
        public string lmName { get; set; }
        public string lmEmployeeId { get; set; }
        public string overLmEmployeeId { get; set; }
        public string overLmName { get; set; }
        public string lmstate { get; set; }
        public string tips { get; set; }
        public string overTips { get; set; }
    }
}