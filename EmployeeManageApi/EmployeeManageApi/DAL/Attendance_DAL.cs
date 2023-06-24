using EmployeeManageApi.Common;
using EmployeeManageApi.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace EmployeeManageApi.DAL
{
    public class Attendance_DAL
    {
        Tool tool = new Tool();

        public IEnumerable<Attendance> GetOvertTimeInfo(string date, string department, string shift, int pageNum, int pageSize) {
            int indexBegin = (pageNum - 1) * pageSize + 1;
            int indexEnd = pageNum * pageSize;
            StringBuilder sb1 = new StringBuilder();
            sb1.Append("select * from [dbo].[EP_AttendanceInfo] att where 1=1 ");
            if (!string.IsNullOrWhiteSpace(date))
            {
                sb1.Append($@" and [date] = '{date}' and shift = '{shift}'");
            }

            StringBuilder sb2 = new StringBuilder();
            sb2.Append($@"select ebi.employeeId, ebi.cname, ebi.department, 
                           att.lmstate,att.date,att.lmName, att.lmEmployeeId,att.overLmEmployeeId, att.overLmName,
		                   att.overStartTime, att.overEndTime overTime, att.overTimeHour, att.overTips from (select * from [dbo].[EP_EmployeeBaseInfo] where isValid = 'Y' and shift = '{shift}') ebi
                              left join 
                              (
	                             {sb1.ToString()}   
                              ) att on ebi.employeeId = att.employeeId and ebi.isValid = 'Y'
                              where  1=1");
            if (!string.IsNullOrWhiteSpace(department))
            {
                sb2.Append($@" and ebi.department = '{department}'");
            }
            StringBuilder sb3 = new StringBuilder();
            //sb3.Append($@"select * from
            //              (
            //                select ROW_NUMBER() over (order by lmstate desc )num, * from
            //                (
            //                    " + sb2.ToString() + @"
            //                )a
            //              )b where b.num between '{indexBegin}' and '{indexEnd}' ");
            sb3.Append($@"select * from
                          (
                            select ROW_NUMBER() over (order by lmstate desc )num, * from
                            (
                                {sb2.ToString()}
                            )a
                          )b where b.num between '{indexBegin}' and '{indexEnd}' ");
            return SqlHelper<Attendance>.Query(sb3.ToString());
        }
        public IEnumerable<Attendance> GetAttendanceInfo(string date, string department, string shift, int pageNum, int pagesize)
        {
            int indexBegin = (pageNum - 1) * pagesize + 1;
            int indexEnd = pageNum * pagesize;
            StringBuilder sb1 = new StringBuilder();
            sb1.Append("select * from [dbo].[EP_AttendanceInfo] att where 1=1 ");
            if (!string.IsNullOrWhiteSpace(date)) {
                sb1.Append($@" and [date] = '{date}' and shift = '{shift}'");
            }
            string attendStartTime = "08:00:00";
            string attendEndTime = "16:30:00";
            if (shift == "N") {
                attendStartTime = "20:00:00";
                attendEndTime = "05:00:00";
            }
            StringBuilder sb2 = new StringBuilder();
            sb2.Append($@"select ebi.employeeId, ebi.cname, ebi.department, 
                           case when isnull(att.attendState, '') = ''
						   then '正常'
						   else att.attendState
						   end as attendState,
                           case when att.attendStartTime is null
						   then '{attendStartTime}' 
						   else att.attendStartTime
						   end as attendStartTime,
						   case when att.attendEndTime is null
						   then '{attendEndTime}' 
						   else att.attendEndTime
						   end as attendEndTime,
                           att.borrowDept,att.lmstate,att.date,
                           att.lmName, att.lmEmployeeId,
		                   att.overStartTime, att.overEndTime overTime, att.overTimeHour, att.tips from (select * from [dbo].[EP_EmployeeBaseInfo] where isValid = 'Y' and shift = '{shift}') ebi
                              left join 
                              (
	                             {sb1.ToString()}   
                              ) att on ebi.employeeId = att.employeeId and ebi.isValid = 'Y'
                              where  1=1");
            if (!string.IsNullOrWhiteSpace(department))
            {
                sb2.Append($@" and ebi.department = '{department}'");
            }
            StringBuilder sb3 = new StringBuilder();
            //sb3.Append($@"select * from
            //              (
            //                select ROW_NUMBER() over (order by lmstate desc )num, * from
            //                (
            //                    " + sb2.ToString() + @"
            //                )a
            //              )b where b.num between '{indexBegin}' and '{indexEnd}' ");
            sb3.Append($@"select * from
                          (
                            select ROW_NUMBER() over (order by lmstate desc )num, * from
                            (
                                {sb2.ToString()}
                            )a
                          )b where b.num between '{indexBegin}' and '{indexEnd}' ");
            return SqlHelper<Attendance>.Query(sb3.ToString());
        }

        public int GetOvertTimeInfoTotal(string date, string department, string shift) {
            StringBuilder sb1 = new StringBuilder();
            sb1.Append("select * from [dbo].[EP_AttendanceInfo] where 1=1 ");
            if (!string.IsNullOrWhiteSpace(date))
            {
                sb1.Append($@" and [date] = '{date}' and shift = '{shift}'");
            }
            StringBuilder sb2 = new StringBuilder();
            sb2.Append($@"select * from [dbo].[EP_EmployeeBaseInfo] ebi
                              left join 
                              (
	                             {sb1.ToString()}   
                              ) att on ebi.employeeId = att.employeeId
                              where  1=1 and ebi.isValid = 'Y' and ebi.shift = '{shift}'   ");
            if (!string.IsNullOrWhiteSpace(department))
            {
                sb2.Append($@" and ebi.department = '{department}'");
            }
            return SqlHelper<Attendance>.sqlTable(sb2.ToString()).Rows.Count;
        }

        public int GetAttendanceInfoTotal(string date, string department, string shift) {
            StringBuilder sb1 = new StringBuilder();
            sb1.Append("select * from [dbo].[EP_AttendanceInfo] where 1=1 ");
            if (!string.IsNullOrWhiteSpace(date))
            {
                sb1.Append($@" and [date] = '{date}' and shift = '{shift}'");
            }
            StringBuilder sb2 = new StringBuilder();
            sb2.Append($@"select * from [dbo].[EP_EmployeeBaseInfo] ebi
                              left join 
                              (
	                             {sb1.ToString()}   
                              ) att on ebi.employeeId = att.employeeId
                              where  1=1 and ebi.isValid = 'Y' and ebi.shift = '{shift}'   ");
            if (!string.IsNullOrWhiteSpace(department))
            {
                sb2.Append($@" and ebi.department = '{department}'");
            }
            return SqlHelper<Attendance>.sqlTable(sb2.ToString()).Rows.Count;
        }

        public int SaveOverTimeRow(AttendanceModel attendModel) {
            string sqlCmd = $@"insert into EP_AttendanceInfo
                                (date, shift, dept, employeeId, cname, 
                                overStartTime, overEndTime, overTimeHour, overTips, overLmEmployeeId, overLmName, lmstate, lmdate, lmtime, isValid)
                                values
                                ('" + attendModel.date + @"', '" + attendModel.shift + @"', '" + attendModel.department + @"', '" + attendModel.employeeId + @"',
                                '" + attendModel.cname + @"', '" + attendModel.overStartTime + @"', '" + attendModel.overEndTime + @"', '" + attendModel.overTimeHour + @"',
                                '" + attendModel.overTips + @"', '" + attendModel.overLmEmployeeId + @"', '" + attendModel.overLmName + @"', 'Y', '" + attendModel.lmdate + @"', '" + attendModel.lmtime + @"', 'Y')";
            return SqlHelper<Attendance>.Execute(sqlCmd);
        }
        public int SaveAttendanceRow(AttendanceModel attendModel) {
            string sqlCmd = $@"insert into EP_AttendanceInfo
                                (date, shift, dept, employeeId, cname, attendState, attendStartTime, attendEndTime, borrowDept, 
                                overStartTime, overEndTime, overTimeHour, tips, lmEmployeeId, lmName, lmstate, lmdate, lmtime, isValid)
                                values
                                ('" + attendModel.date + @"', '" + attendModel.shift + @"', '" + attendModel.department + @"', '" + attendModel.employeeId + @"',
                                '" + attendModel.cname + @"', '" + attendModel.attendState + @"', '" + attendModel.attendStartTime + @"', '" + attendModel.attendEndTime + @"',
                                '" + attendModel.borrowDept + @"', '" + attendModel.overStartTime + @"', '" + attendModel.overEndTime + @"', '" + attendModel.overTimeHour + @"',
                                '" + attendModel.tips + @"', '" + attendModel.lmEmployeeId + @"', '" + attendModel.lmName + @"', 'Y', '" + attendModel.lmdate + @"', '" + attendModel.lmtime + @"', 'Y')";
            return SqlHelper<Attendance>.Execute(sqlCmd);
        }

        public DataTable AttendIfSaved(AttendanceModel attendModel) {
            string sqlCmd = $@"select * from EP_AttendanceInfo where [date] = '" + attendModel.date + @"' and employeeId = '" + attendModel.employeeId + @"' and lmstate = 'Y'";
            DataTable dt = SqlHelper<Attendance>.sqlTable(sqlCmd);
            return dt;
        }

        public int UpOverTimeRow(AttendanceModel attendModel) {
            string sqlCmd = $@"update EP_AttendanceInfo
                               set  overStartTime = '" + attendModel.overStartTime + @"', overEndTime = '" + attendModel.overEndTime + @"', overTimeHour = '" + attendModel.overTimeHour + @"', overTips = '" + attendModel.overTips + @"', 
                               overLmEmployeeId = '" + attendModel.overLmEmployeeId + @"', overLmName = '" + attendModel.overLmName + @"'
                               where [date] = '" + attendModel.date + @"' and employeeId = '" + attendModel.employeeId + @"' and lmstate = 'Y'";
            return SqlHelper<Attendance>.Execute(sqlCmd);
        }

        public int UpAttendanceRow(AttendanceModel attendModel) {
            string sqlCmd = $@"update EP_AttendanceInfo
                               set attendState = '" + attendModel.attendState + @"', attendStartTime = '" + attendModel.attendStartTime + @"', attendEndTime = '" + attendModel.attendEndTime + @"', borrowDept = '" + attendModel.borrowDept + @"', 
                               overStartTime = '" + attendModel.overStartTime + @"', overEndTime = '" + attendModel.overEndTime + @"', overTimeHour = '" + attendModel.overTimeHour + @"', tips = '" + attendModel.overTips + @"', 
                               lmEmployeeId = '" + attendModel.lmEmployeeId + @"', lmName = '" + attendModel.lmName + @"'
                               where [date] = '" + attendModel.date + @"' and employeeId = '" + attendModel.employeeId + @"' and lmstate = 'Y'";
            return SqlHelper<Attendance>.Execute(sqlCmd);
        }

        public DataTable ExportAttendanceInfo(string date, string department, string shift) {
            StringBuilder sb1 = new StringBuilder();
            sb1.Append("select * from [dbo].[EP_AttendanceInfo] att where 1=1 ");
            if (!string.IsNullOrWhiteSpace(date))
            {
                sb1.Append($@" and [date] = '{date}' and shift = '{shift}'");
            }
            string attendStartTime = "08:00:00";
            string attendEndTime = "17:00:00";
            if (shift == "N")
            {
                attendStartTime = "20:00:00";
                attendEndTime = "05:00:00";
            }
            string sqlCmd = $@"select att.date 日期, ebi.employeeId 工号, ebi.cname 姓名, ebi.department 部门, 
						      case when att.lmstate = 'Y'
						      then '已维护'
						      else '未维护'
						      end as 是否维护,
                              case when isnull(att.attendState, '') = ''
						      then '正常'
						      else att.attendState 
						      end as 出勤状况,
                              case when att.attendStartTime is null
						      then '" + attendStartTime + @"' 
						      else att.attendStartTime
						      end as 出勤开始时间,
						      case when att.attendEndTime is null
						      then '" + attendEndTime + @"' 
						      else att.attendEndTime
						      end as 出勤结束时间,
                              att.borrowDept 借出车间,
		                      att.overStartTime 加班开始时间, att.overEndTime 加班结束时间, att.overTimeHour 加班时长, att.tips 备注 from [dbo].[EP_EmployeeBaseInfo] ebi
                                 left join 
                                 (
	                                " + sb1.ToString() + @"
                                 ) att on ebi.employeeId = att.employeeId
                                  where  1=1 order by lmstate desc ";
            return SqlHelper<Attendance>.sqlTable(sqlCmd);
        }
    }
}