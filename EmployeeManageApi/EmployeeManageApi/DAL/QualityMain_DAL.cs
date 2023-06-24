using EmployeeManageApi.Common;
using EmployeeManageApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace EmployeeManageApi.DAL
{
    public class QualityMain_DAL
    {
        public DataTable GetInfoById(string employeeId) {
            string sqlCmd = @"select employeeId, cname, department, indate, grade, postSkill, postNow 
                              from EP_EmployeeBaseInfo where isValid = 'Y' and employeeId = '" + employeeId + @"'";
            return SqlHelper<EmployeeInfo>.sqlTable(sqlCmd);
        }

        public int QualityInfoSave(QualityInfo qualityInfo) {
            string sqlCmd = @"insert into EP_Quality
                              (employeeId, eventDate, eventDesc, eventType, dealType, dealResult, checkDate, checkName, 
                              checkId, monthid, weekid, isValid, lmdate, lmtime) values
                              ('" + qualityInfo.employeeId + @"','" + qualityInfo.eventDate + @"','" + qualityInfo.eventDesc + @"','" + qualityInfo.eventType + @"',
                              '" + qualityInfo.OKNG + @"','" + qualityInfo.dealResult + @"','" + qualityInfo.checkDate + @"','" + qualityInfo.checkName + @"',
                              '" + qualityInfo.checkEmployeeId + @"','" + qualityInfo.monthid + @"','" + qualityInfo.weekid + @"','Y','" + qualityInfo.lmdate + @"','" + qualityInfo.lmtime + @"')";
            return SqlHelper<QualityInfo>.Execute(sqlCmd);
        }

        public IEnumerable<QualityInfo> GetInfoList(QualityQueryParam param) {
            int indexBegin = (param.pageNum - 1) * param.pagesize + 1;
            int indexEnd = param.pageNum * param.pagesize;
            StringBuilder sql1 = new StringBuilder();
            sql1.Append(@"select eq.employeeId, eq.eventDate, eq.eventDesc, eq.eventType, 
		                  case when eq.dealType = 'NG' then '批评'
		                  else '表扬'
		                  end as dealType, eq.dealResult, eq.checkDate, eq.checkName, eq.checkId checkEmployeeId,eq.lmdate, eq.lmtime,
		                  ee.cname, ee.department, ee.postNow from EP_Quality eq inner join EP_EmployeeBaseInfo ee
		                  on eq.employeeId = ee.employeeId and eq.isValid = 'Y'");
            if (!string.IsNullOrWhiteSpace(param.department)) {
                sql1.Append(@" and ee.department = '" + param.department + @"'");
            }
            string sqlCmd = @"select * from
                              (
                                select ROW_NUMBER() over(order by lmdate desc, lmtime desc) num, * from 
                                (" + sql1.ToString() + @")a
                              )b where b.num between '" + indexBegin + @"' and '" + indexEnd + @"'";
            return SqlHelper<QualityInfo>.Query(sqlCmd);
        }

        public IEnumerable<QualityInfo> GetInfoAllList(QualityQueryParam param) {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"select eq.*, ee.cname, ee.department, ee.postNow from EP_Quality eq inner join EP_EmployeeBaseInfo ee
		                      on eq.employeeId = ee.employeeId and eq.isValid = 'Y' ");
            if (!string.IsNullOrWhiteSpace(param.department))
            {
                sqlCmd.Append(@" and ee.department = '" + param.department + @"'");
            }
            return SqlHelper<QualityInfo>.Query(sqlCmd.ToString());
        }

        public int InfoDel(string employeeId, string eventType, string checkDate) {
            string sqlCmd = @"update EP_Quality set isValid = 'N'
                              where employeeId = '" + employeeId + @"' and eventType = '" + eventType + @"' and eventDate = '" + checkDate + @"' and isValid = 'Y'";
            return SqlHelper<QualityInfo>.Execute(sqlCmd);
        }

        public DataTable ExportExcel(QualityQueryParam param) {
            StringBuilder sql1 = new StringBuilder();
            sql1.Append(@"select eq.employeeId, eq.eventDate, eq.eventDesc, eq.eventType, 
		                  case when eq.dealType = 'NG' then '批评'
		                  else '表扬'
		                  end as dealType, eq.dealResult, eq.checkDate, eq.checkName, eq.checkId checkEmployeeId,eq.lmdate, eq.lmtime,
		                  ee.cname, ee.department, ee.postNow from EP_Quality eq inner join EP_EmployeeBaseInfo ee
		                  on eq.employeeId = ee.employeeId and eq.isValid = 'Y'");
            if (!string.IsNullOrWhiteSpace(param.department))
            {
                sql1.Append(@" and ee.department = '" + param.department + @"'");
            }
            DataTable dt = SqlHelper<QualityInfo>.sqlTable(sql1.ToString());
            return dt;
        }

        public int InfoUp(QualityInfo qualityInfo) {
            string sqlCmd = @"update EP_Quality
                              set employeeId = '" + qualityInfo.employeeId + @"', eventDate = '" + qualityInfo.eventDate + @"', eventDesc = '" + qualityInfo.eventDesc + @"', 
                              eventType = '" + qualityInfo.eventType + @"', dealType = '" + qualityInfo.OKNG + @"',dealResult = '" + qualityInfo.dealResult + @"', checkDate = '" + qualityInfo.checkDate + @"', 
                              checkName = '" + qualityInfo.checkName + @"', checkId = '" + qualityInfo.checkEmployeeId + @"'
                              where employeeId = '" + qualityInfo.employeeIdB + @"' and eventType = '" + qualityInfo.eventTypeB + @"' and checkDate = '" + qualityInfo.checkDateB + @"'";
            return SqlHelper<QualityInfo>.Execute(sqlCmd);
        }
    }
}