using EmployeeManageApi.Common;
using EmployeeManageApi.DAL;
using EmployeeManageApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EmployeeManageApi.BLL
{
    public class Attendance_BLL
    {
        Tool tool = new Tool();
        Attendance_DAL dal = new Attendance_DAL();
        public IEnumerable<Attendance> GetAttendanceInfo(string date, string department, string shift, int pageNum, int pagesize) {
            return dal.GetAttendanceInfo(date, department, shift, pageNum, pagesize);
        }

        public IEnumerable<Attendance> GetOvertTimeInfo(string date, string department, string shift, int pageNum, int pageSize) {
            return dal.GetOvertTimeInfo(date, department, shift, pageNum, pageSize);
        }

        public int GetOvertTimeInfoTotal(string date, string department, string shift) {
            return dal.GetOvertTimeInfoTotal(date, department, shift);
        }

        public int GetAttendanceInfoTotal(string date, string department, string shift) {
            return dal.GetAttendanceInfoTotal(date, department, shift);
        }

        public int SaveAttendanceRow(AttendanceModel attendModel) {
            string lmdate = DateTime.Now.ToString("yyyy-MM-ddd");
            string lmtime = DateTime.Now.ToString("HH-mm-ss");
            attendModel.lmdate = lmdate;
            attendModel.lmtime = lmtime;
            DataTable dt = dal.AttendIfSaved(attendModel);
            if (dt.Rows.Count > 0)
            {
                return dal.UpAttendanceRow(attendModel);
            }
            else {
                return dal.SaveAttendanceRow(attendModel);
            }
            
        }

        public int SaveOverTimeRow(AttendanceModel attendModel) {
            string lmdate = DateTime.Now.ToString("yyyy-MM-ddd");
            string lmtime = DateTime.Now.ToString("HH-mm-ss");
            attendModel.lmdate = lmdate;
            attendModel.lmtime = lmtime;
            DataTable dt = dal.AttendIfSaved(attendModel);
            if (dt.Rows.Count > 0)
            {
                return dal.UpOverTimeRow(attendModel);
            }
            else
            {
                return dal.SaveOverTimeRow(attendModel);
            }
        }

        public byte[] ExportAttendanceInfo(string date, string department, string shift) {
            DataTable dt = dal.ExportAttendanceInfo(date, department, shift);
            byte[] arr = ExcelHelper.GetExcel(dt, "品质信息", "A1:M1");
            return arr;
        }

        public byte[] ExportOvertimeInfo(string date, string dateEnd, string department, string shift)
        {
            DataTable dt = dal.ExportOvertimeInfo(date, dateEnd, department, shift);
            byte[] arr = ExcelHelper.GetExcel(dt, "加班信息", "A1:L1");
            return arr;
        }

        public IEnumerable<Attendance> GetWorkTimeInfo() {
            return dal.GetWorkTimeInfo();
        }
    }
}