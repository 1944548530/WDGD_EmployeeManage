using EmployeeManageApi.Common;
using EmployeeManageApi.DAL;
using EmployeeManageApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManageApi.BLL
{
    public class DataChart_BLL
    {
        DataChart_DAL dal = new DataChart_DAL();
        Tool tool = new Tool();
        public List<object> GetDeptAttendance()
        {
            var attendanceInfo = dal.GetDeptAttendance();
            List<deptModel> deptInfo = SqlHelper<deptModel>.Query($@"select param1 dept, param2 startTime, param3 endTime  from systemBase where isValid = 'Y' and module = '车间管理' and functionName = '车间维护'").ToList();
            List<string> deptList = deptInfo.Select(v => v.dept).ToList();
            var employeeInfo = SqlHelper<EmployeeInfo>.Query($@"select * from [dbo].[EP_EmployeeBaseInfo] where isValid = 'Y'");
            List<object> info = new List<object>();
            foreach (var item in deptList) {
                int attendCount = attendanceInfo.Where(v => v.department == item).Count();
                int deptEmployeeAll = employeeInfo.Where(v => v.department == item).Count();
                float attendPer = tool.perCal(attendCount, deptEmployeeAll);
                string attendPerStr = attendPer + "%";
                if (attendPer == 0) {
                    attendPerStr = "0";
                }
                var obj = new {
                    dept = item,
                    percent = attendPerStr
                };
                info.Add(obj);
            }
            return info;
        }
    }
}