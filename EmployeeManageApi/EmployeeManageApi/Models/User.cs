using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static EmployeeManageApi.Enum.CommonEnum;

namespace EmployeeManageApi.Models
{
    public class User
    {
        public IsDeleted IsDeleted { get; set; }

        public string Password { get; set; }
        public IsLocked IsLocked { get; set; }
        public UserStatus Status { get; set; }
        public Guid Guid {get;set;}
        public string DisplayName { get; set; }
        public string Department { get; set; }
        public string LoginName { get; set; }
        public UserType UserType { get; set; }
        public string EmployeeId { get; set; }
        public string lmdate { get; set; }
        public string lmtime { get; set; }
    }
}