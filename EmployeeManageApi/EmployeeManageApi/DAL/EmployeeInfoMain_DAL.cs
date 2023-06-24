using EmployeeManageApi.Common;
using EmployeeManageApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using static EmployeeManageApi.Enum.CommonEnum;

namespace EmployeeManageApi.DAL
{
    public class EmployeeInfoMain_DAL
    {
        Tool tool = new Tool();
        public int EmployeeInfoSave(EmployeeInfo info) {
            string sqlStr = @"insert into EP_EmployeeBaseInfo
                              (employeeId, cname, department, indate, grade, postSkill, postNow, shift, isvalid, lmdate, lmtime)
                              values
                              ('" + info.employeeId + @"','" + info.cname + @"','" + info.department + @"','" + info.indate + @"',
                              '" + info.grade + @"','" + info.postSkill + @"','" + info.postNow + @"', '" + info.shift + @"', 'Y','" + info.lmdate + @"','" + info.lmtime + @"')";
            int result = SqlHelper<EmployeeInfo>.Execute(sqlStr);
            return result;
        }

        public IEnumerable<EmployeeInfo> GetInfoList(QueryParam queryParam) {
            int indexBegin = (queryParam.pageNum - 1) * queryParam.pagesize + 1;
            int indexEnd = queryParam.pageNum * queryParam.pagesize;
            string emploeeIDApp = tool.sqlAppend2("employeeId", queryParam.employeeId);
            string deptApp = tool.sqlAppend2("department", queryParam.department);
            string sqlCmd = @"select * from
                              (
                                select ROW_NUMBER() over(order by lmdate desc, lmtime desc) num, * from
                                (select * from EP_EmployeeBaseInfo where isValid = 'Y' " + emploeeIDApp + @" " + deptApp + @")a
                              )b where b.num between '" + indexBegin + @"' and '" + indexEnd + @"'";
            return SqlHelper<EmployeeInfo>.Query(sqlCmd);
        }

        public IEnumerable<EmployeeInfo> GetInfoAllList(QueryParam queryParam) {
            string emploeeIDApp = tool.sqlAppend2("employeeId", queryParam.employeeId);
            string deptApp = tool.sqlAppend2("department", queryParam.department);
            string sqlCmd = @"select * from EP_EmployeeBaseInfo where isValid = 'Y' " + emploeeIDApp + @" " + deptApp + @"";
            return SqlHelper<EmployeeInfo>.Query(sqlCmd);
        }

        public IEnumerable<deptModel> GetDeptList(int deptPageNum, int deptPagesize) {
            int indexBegin = (deptPageNum - 1) * deptPagesize + 1;
            int indexEnd = deptPageNum * deptPagesize;
            string sqlCmd = @"select b.param1 dept from 
                              (
                                select ROW_NUMBER() over(order by lmdate desc, lmtime desc)num, * from
                                (
                                    select *  from systemBase where isValid = 'Y' and module = '车间管理' and functionName = '车间维护'
                                )a
                              )b where b.num between  '" + indexBegin + @"' and '" + indexEnd + @"'";
            return SqlHelper<deptModel>.Query(sqlCmd);
        }

        public IEnumerable<deptModel> GetPostList(int deptPageNum, int deptPagesize)
        {
            int indexBegin = (deptPageNum - 1) * deptPagesize + 1;
            int indexEnd = deptPageNum * deptPagesize;
            string sqlCmd = @"select b.param1 post from 
                              (
                                select ROW_NUMBER() over(order by lmdate desc, lmtime desc)num, * from
                                (
                                    select *  from systemBase where isValid = 'Y' and module = '岗位管理' and functionName = '岗位维护'
                                )a
                              )b where b.num between  '" + indexBegin + @"' and '" + indexEnd + @"'";
            return SqlHelper<deptModel>.Query(sqlCmd);
        }

        public IEnumerable<deptModel> GetDeptAllList() {
            string sqlCmd = @"select *  from systemBase where isValid = 'Y' and module = '车间管理' and functionName = '车间维护'";
            return SqlHelper<deptModel>.Query(sqlCmd);
        }

        public IEnumerable<deptModel> GetPostAllList()
        {
            string sqlCmd = @"select *  from systemBase where isValid = 'Y' and module = '岗位管理' and functionName = '岗位维护'";
            return SqlHelper<deptModel>.Query(sqlCmd);
        }

        public int InfoListDel(string employyeeId) {
            string sqlCmd = @"update EP_EmployeeBaseInfo
                              set isValid = 'N'
                              where employeeId = '" + employyeeId + @"'";
            return SqlHelper<EmployeeInfo>.Execute(sqlCmd);
        }

        public int InfoListUp(EmployeeInfo info) {
            string sqlCmd = @"update EP_EmployeeBaseInfo
                              set department = '" + info.department + @"', indate = '" + info.indate + @"', grade = '" + info.grade + @"', 
                              postSkill = '" + info.postSkill + @"', postNow = '" + info.postNow + @"', shift = '" + info.shift + @"'
                              where employeeid =  '" + info.employeeId + @"'";
            return SqlHelper<EmployeeInfo>.Execute(sqlCmd);
        }

        public DataTable ExportExcel(ExcelParam param) {
            string emploeeIDApp = tool.sqlAppend2("employeeId", param.employeeId);
            string sqlCmd = @"select employeeId 工号, cname 姓名, shift 班别, department 车间, indate 入职日期, 
                              grade 等级, postSkill 上岗技能, postNow 当前岗位
                              from EP_EmployeeBaseInfo where isValid = 'Y' " + emploeeIDApp + @" order by employeeId";
            return SqlHelper<EmployeeInfo>.sqlTable(sqlCmd);
        }

        public int deptInfoSave(string dept) {
            string lmdate = DateTime.Now.ToString("yyyy-MM-dd");
            string lmtime = DateTime.Now.ToString("HH:mm:ss");
            string sqlCmd = @"insert into systemBase(module, functionName, param1, isValid, lmuser, lmdate, lmtime)
                              values
                              ('车间管理', '车间维护', '" + dept + @"', 'Y', 'system', '" + lmdate + @"', '" + lmtime + @"')";
            return SqlHelper<deptModel>.Execute(sqlCmd);
        }

        public int PostInfoSave(string post) {
            string lmdate = DateTime.Now.ToString("yyyy-MM-dd");
            string lmtime = DateTime.Now.ToString("HH:mm:ss");
            string sqlCmd = @"insert into systemBase(module, functionName, param1, isValid, lmuser, lmdate, lmtime)
                              values
                              ('岗位管理', '岗位维护', '" + post + @"', 'Y', 'system', '" + lmdate + @"', '" + lmtime + @"')";
            return SqlHelper<deptModel>.Execute(sqlCmd);
        }

        public int DeptListDel(string dept) {
            string sqlCmd = @"update systemBase set isValid = 'N'
                              where isValid = 'Y' and module = '车间管理' and functionName = '车间维护'
                              and param1 = '" + dept + @"'";
            return SqlHelper<deptModel>.Execute(sqlCmd);
        }

        public int PostListDel(string post)
        {
            string sqlCmd = @"update systemBase set isValid = 'N'
                              where isValid = 'Y' and module = '岗位管理' and functionName = '岗位维护'
                              and param1 = '" + post + @"'";
            return SqlHelper<deptModel>.Execute(sqlCmd);
        }

        public IEnumerable<Option> GetDeptOption() {
            string sqlCmd = @"select param1 value, param1 label from [dbo].[systemBase] where functionName = '车间维护' and isValid = 'Y'";
            return SqlHelper<Option>.Query(sqlCmd);
        }

        public IEnumerable<Option> GetPostOption() {
            string sqlCmd = @"select param1 value, param1 label from [dbo].[systemBase] where functionName = '岗位维护' and isValid = 'Y'";
            return SqlHelper<Option>.Query(sqlCmd);
        }

        public Boolean ReverseShift(QueryParam queryParam) {
            string emploeeIDApp = tool.sqlAppend2("employeeId", queryParam.employeeId);
            string deptApp = tool.sqlAppend2("department", queryParam.department);
            string sqlCmd1 = @"update EP_EmployeeBaseInfo set shift = 'A' where shift = 'D' and isValid = 'Y' " + emploeeIDApp + @" " + deptApp + @"";
            string sqlCmd2 = @"update EP_EmployeeBaseInfo set shift = 'D' where shift = 'N' and isValid = 'Y' " + emploeeIDApp + @" " + deptApp + @"";
            string sqlCmd3 = @"update EP_EmployeeBaseInfo set shift = 'N' where shift = 'A' and isValid = 'Y' " + emploeeIDApp + @" " + deptApp + @"";
            List<string> sqlCmdLi = new List<string>();
            sqlCmdLi.Add(sqlCmd1);
            sqlCmdLi.Add(sqlCmd2);
            sqlCmdLi.Add(sqlCmd3);
            Boolean result = SqlHelper<Option>.Transaction(sqlCmdLi);
            return result;
        }

        public IEnumerable<EmployeeInfo> GetInfoByID(string employeeId) {
            string sqlCmd = @"select employeeId, cname, department, indate, grade
                              postSkill, postNow, shift
                              from [dbo].[EP_EmployeeBaseInfo] where isValid = 'Y' and employeeId = '" + employeeId + @"'";
            return SqlHelper<EmployeeInfo>.Query(sqlCmd);
        }
    }
}