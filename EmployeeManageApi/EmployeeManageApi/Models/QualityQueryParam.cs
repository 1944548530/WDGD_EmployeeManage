using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManageApi.Models
{
    public class QualityQueryParam
    {
        public int pageNum { get; set; }
        public int pagesize { get; set; }
        public string department { get; set; }
    }
}