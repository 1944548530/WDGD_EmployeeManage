using EmployeeManageApi.Common;
using EmployeeManageApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EmployeeManageApi.DAL
{
    public class Kanban_DAL
    {
        public IEnumerable<KanbanInfo> GetKanbanInfo(string date, string department, string shift)
        {
            string sqlCmd = @"select a.* from
                                (
	                                select emp.employeeId, emp.cname, 
                                    case when isnull(att.borrowDept, '') = '' then emp.department
	                                else att.borrowDept 
	                                end as department,
                                    emp.indate, emp.grade, emp.postSkill, emp.postNow,
	                                emp.[shift], att.attendState, att.attendStartTime, att.attendEndTime, att.borrowDept, att.overEndTime,
	                                att.overStartTime, att.overTimeHour, att.tips  from EP_EmployeeBaseInfo emp
	                                left join 
	                                (
	                                    select * from EP_AttendanceInfo where date = '" + date + @"' and shift = '" + shift + @"'
	                                )att
	                                on emp.employeeId = att.employeeId
	                                where emp.isValid = 'Y'  and emp.shift = '" + shift + @"'
                                )a where a.attendState not in('矿工', '请假', '离职') and department = '" + department + @"'";
            IEnumerable<KanbanInfo> info = SqlHelper<KanbanInfo>.Query(sqlCmd);
            return info;
        }

        public IEnumerable<QualityInfo> GetQualityInfo() {
            string sevenDaysBefore = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            string sqlCmd = $@"select * from EP_Quality where isValid = 'Y' and dealType = 'NG' and eventDate >= '" + sevenDaysBefore + @"'";
            IEnumerable<QualityInfo> info = SqlHelper<QualityInfo>.Query(sqlCmd);
            return info;
        }

        public DataTable GetEmployeeInfo(string name, string post) {
            string postApp = "";
            if (!string.IsNullOrEmpty(post)) {
                postApp = @" and postNow = '" + post + @"'";
            }
            string sqlCmd = @"select * from EP_EmployeeBaseInfo
                              where isValid = 'Y' and cname = '" + name + @"' " + postApp + @"";
            return SqlHelper<EmployeeInfo>.sqlTable(sqlCmd);
        }
    }
}