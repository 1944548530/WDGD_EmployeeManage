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
    public class EmployeeInfoMain_BLL
    {
        Tool tool = new Tool();
        EmployeeInfoMain_DAL dal = new EmployeeInfoMain_DAL();
        public int EmployeeInfoSave(EmployeeInfo info) {
            string lmdate = DateTime.Now.ToString("yyyy-MM-dd");
            string lmtime = DateTime.Now.ToString("HH:mm:ss");
            info.lmdate = lmdate;
            info.lmtime = lmtime;
            return dal.EmployeeInfoSave(info);
        }

        public IEnumerable<EmployeeInfo> GetInfoList(QueryParam queryParam) {
            return dal.GetInfoList(queryParam);
        }

        public IEnumerable<EmployeeInfo> GetInfoAllList(QueryParam queryParam) {
            return dal.GetInfoAllList(queryParam);
        }

        public IEnumerable<deptModel> GetDeptList(int deptPageNum, int deptPagesize) {
            return dal.GetDeptList(deptPageNum, deptPagesize);
        }

        public IEnumerable<deptModel> GetPostList(int deptPageNum, int deptPagesize)
        {
            return dal.GetPostList(deptPageNum, deptPagesize);
        }

        public IEnumerable<deptModel> GetDeptAllList() {
            return dal.GetDeptAllList();
        }

        public IEnumerable<deptModel> GetPostAllList()
        {
            return dal.GetPostAllList();
        }

        public int InfoListDel(string employeeId) {
            return dal.InfoListDel(employeeId);
        }

        public int InfoListUp(EmployeeInfo info) {
            return dal.InfoListUp(info);
        }

        public byte[] ExportExcel(ExcelParam param) {
            DataTable dt = dal.ExportExcel(param);
            return ExcelHelper.GetExcel(dt, "员工信息", "A1:H1");
        }

        public int deptInfoSave(string dept, string startTime, string endTime) {
            return dal.deptInfoSave(dept, startTime, endTime);
        }

        public int PostInfoSave(string post) {
            return dal.PostInfoSave(post);
        }

        public int DeptListDel(string dept) {
            return dal.DeptListDel(dept);
        }

        public int PostListDel(string dept)
        {
            return dal.PostListDel(dept);
        }

        public IEnumerable<Option> GetDeptOption() {
            return dal.GetDeptOption();
        }

        public IEnumerable<Option> GetPostOption() {
            return dal.GetPostOption();
        }

        public Boolean ReverseShift(QueryParam param) {
            return dal.ReverseShift(param);
        }

        public IEnumerable<EmployeeInfo> GetInfoByID(string employeeId) {
            EmployeeInfo employeeInfo = new EmployeeInfo();
            return dal.GetInfoByID(employeeId);
        }
    }
}