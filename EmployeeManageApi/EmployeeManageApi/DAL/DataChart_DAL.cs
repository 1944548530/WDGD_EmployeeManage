using EmployeeManageApi.Common;
using EmployeeManageApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManageApi.DAL
{
    public class DataChart_DAL
    {
        public List<Attendance> GetDeptAttendance() {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string strSql = $@"select dept department, employeeId, cname, attendState from [dbo].[EP_AttendanceInfo]
                              where [date] = '{date}' and attendState = '正常'";
            List<Attendance> info = SqlHelper<Attendance>.Query(strSql).ToList();
            return info;
        }
    }
}