using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManageApi.Models
{
    public class EquipmentModel
    {
        public string department { get; set; }
        public string equipmentName { get; set; }
        public int equipmentNum { get; set; }
        public string lmdate { get; set; }
        public string lmtime { get; set; }
        public string lmEmployeeId { get; set; }
        public string lmName { get; set; }

        public void Create() {
            lmdate = DateTime.Now.ToString("yyyy-MM-dd");
            lmtime = DateTime.Now.ToString("HH:mm:ss");
        }
    }
}