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
    public class QualityMain_BLL
    {
        Tool tool = new Tool();
        QualityMain_DAL dal = new QualityMain_DAL();
        public EmployeeInfo GetInfoById(string employeeId) {
            EmployeeInfo employeeInfo = null;
            DataTable dt = dal.GetInfoById(employeeId);
            if (dt.Rows.Count > 0) {
                employeeInfo = new EmployeeInfo();
                employeeInfo.employeeId = employeeId;
                employeeInfo.cname = dt.Rows[0]["cname"].ToString();
                employeeInfo.department = dt.Rows[0]["department"].ToString();
                employeeInfo.indate = dt.Rows[0]["indate"].ToString();
                employeeInfo.grade = dt.Rows[0]["grade"].ToString();
                employeeInfo.postSkill = dt.Rows[0]["postSkill"].ToString();
                employeeInfo.postNow = dt.Rows[0]["postNow"].ToString();
            }
            return employeeInfo;
        }
        public int QualityInfoSave(QualityInfo qualityInfo)
        {
            string lmdate = DateTime.Now.ToString("yyyy-MM-dd");
            string lmtime = DateTime.Now.ToString("HH:mm:ss");
            string monthid = tool.getMonthid(qualityInfo.eventDate);
            string weekid = tool.getWeekid(qualityInfo.eventDate);
            qualityInfo.lmdate = lmdate;
            qualityInfo.lmtime = lmtime;
            qualityInfo.monthid = monthid;
            qualityInfo.weekid = weekid;
            return dal.QualityInfoSave(qualityInfo);
        }

        public IEnumerable<QualityInfo> GetInfoList(QualityQueryParam param) {
            return dal.GetInfoList(param);
        }

        public IEnumerable<QualityInfo> GetInfoAllList(QualityQueryParam param) {
            return dal.GetInfoAllList(param);
        }

        public int InfoDel(string employeeId, string eventType, string checkDate) {
            return dal.InfoDel(employeeId, eventType, checkDate);
        }

        public byte[] ExportExcel(QualityQueryParam param) {
            DataTable dt = dal.ExportExcel(param);
            byte[] arr = ExcelHelper.GetExcel(dt, "出勤信息", "A1:N1");
            return arr;
        }

        public int InfoUp(QualityInfo qualityInfo) {
            return dal.InfoUp(qualityInfo);
        }
    }
    
}