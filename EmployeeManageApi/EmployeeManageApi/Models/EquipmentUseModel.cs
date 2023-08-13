using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManageApi.Models
{
    public class EquipmentUseModel
    {
        public string date { get; set; }
        public string department { get; set; }
        public string equipmentName { get; set; }
        public int equipmentNum { get; set; }
        public int openNum { get; set; }
        public int cancelNum { get; set; }
        public int maintainNum { get; set; }
        public string lmdate { get; set; }
        public string lmtime { get; set; }
        public string lmEmployeeId { get; set; }
        public string lmEmployeeName { get; set; }
        public string lmstate { get; set; }
    }
}