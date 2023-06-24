using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManageApi.Models
{
    public class QualityInfo
    {
        public string employeeId { get; set; }
        public string employeeIdB { get; set; }
        public string cname { get; set; }
        public string eventDate { get; set; }
        public string department { get; set; }
        public string postNow { get; set; }
        public string eventDesc { get; set; }
        public string checkDate { get; set; }
        public string checkDateB { get; set; }
        public string checkName { get; set; }
        public string checkEmployeeId { get; set; }
        public string eventType { get; set; }
        public string eventTypeB { get; set; }
        public string OKNG { get; set; }
        public string dealType { get; set; }
        public string dealResult { get; set; }
        public string lmdate { get; set; }
        public string lmtime { get; set; }
        public string monthid { get; set; }
        public string weekid { get; set; }
    }
}