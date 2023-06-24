using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManageApi.Models
{
    public class KanbanPerson
    {
        public string post { get; set; }
        public List<KanbanPerByPost> kanbanPerByPost { get; set; }
        
    }
}