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
    public class Login_DAL
    {
        Tool tool = new Tool();
        public User Login(string username, string password)
        {
            string sqlCmd = @"select Password, IsLocked, IsDeleted, UserType, Status, DisplayName, LoginName, Guid, EmployeeId
                              from EP_User where LoginName = '" + username + @"'";
            DataTable dt = SqlHelper<User>.sqlTable(sqlCmd);
            User user = new User();
            if (dt.Rows.Count > 0)
            {
                user.Password = dt.Rows[0]["Password"].ToString();
                user.IsLocked = (IsLocked)int.Parse(dt.Rows[0]["IsLocked"].ToString());
                user.IsDeleted = (IsDeleted)int.Parse(dt.Rows[0]["IsDeleted"].ToString());
                user.Status = (UserStatus)int.Parse(dt.Rows[0]["Status"].ToString());
                user.DisplayName = dt.Rows[0]["DisplayName"].ToString();
                user.LoginName = dt.Rows[0]["LoginName"].ToString();
                user.Guid = new Guid(dt.Rows[0]["Guid"].ToString());
                user.EmployeeId = dt.Rows[0]["EmployeeId"].ToString();
                user.UserType = (UserType)int.Parse(dt.Rows[0]["UserType"].ToString());
            }
            return user;
        }

        public IEnumerable<User> GetAccountList(QueryParam queryParam) {
            int indexBegin = (queryParam.pageNum - 1) * queryParam.pagesize + 1;
            int indexEnd = queryParam.pageNum * queryParam.pagesize;
            string emploeeIDApp = tool.sqlAppend2("employeeId", queryParam.employeeId);
            string deptApp = tool.sqlAppend2("department", queryParam.department);
            string sqlCmd = @"select * from
                              (
	                              select ROW_NUMBER() over(order by lmdate desc, lmtime desc) num , * from
	                              (
		                              select * from [dbo].[EP_User] where IsDeleted = '0' " + emploeeIDApp + @" " + deptApp + @"
	                              )a
                              )b where b.num between '" + indexBegin + @"' and '" + indexEnd + @"'";
            IEnumerable<User> userLi = SqlHelper<User>.Query(sqlCmd);
            return userLi;
        }

        public IEnumerable<User> GetAccountAllList(QueryParam queryParam) {
            string emploeeIDApp = tool.sqlAppend2("employeeId", queryParam.employeeId);
            string deptApp = tool.sqlAppend2("department", queryParam.department);
            string sqlCmd = "select* from[dbo].[EP_User] where IsDeleted = '0' " + emploeeIDApp + @" " + deptApp + @"";
            return SqlHelper<User>.Query(sqlCmd);
        }

        public int Regist(User user) {
            System.Guid guid = new Guid();
            guid = Guid.NewGuid();
            user.Guid = guid;
            string sqlCmd = @"insert into EP_User
                            (Guid, LoginName, Password, DisplayName, Department, UserType, EmployeeId, IsLocked, Status, IsDeleted, Lmdate, Lmtime)
                            values
                            ('" + user.Guid + @"', '" + user.LoginName + @"', '" + user.Password + @"', '" + user.DisplayName + @"',
                            '" + user.Department + @"', " + (int)user.UserType + @", '" + user.EmployeeId + @"', 0, " + (int)user.Status + @",
                            0, '" + user.lmdate + @"', '" + user.lmtime + @"' )";
            int result = SqlHelper<User>.Execute(sqlCmd);
            return result;
        }

        public int AccountDel(string loginName) {
            string sqlCmd = @"update EP_User set IsDeleted = 1 where LoginName = '" + loginName + @"' and IsDeleted = 0";
            return SqlHelper<User>.Execute(sqlCmd);
        }

        public int AccountUp(User user) {
            string sqlCmd = @"update EP_User
                                set LoginName = '" + user.LoginName + @"', [Password] = '" + user.Password + @"', DisplayName = '" + user.DisplayName + @"', Department = '" + user.Department + @"', 
                                UserType = '" + (int)user.UserType + @"', EmployeeId = '" + user.EmployeeId + @"', [Status] = '" + (int)user.Status + @"'
                                where IsDeleted = '0' and Guid = '" + (Guid)user.Guid + @"'";
            return SqlHelper<User>.Execute(sqlCmd);
        }

        public List<Menu> GetMenuList(string employeeId) {
            string sqlCmd = @"select MenuId, MenuPath, MenuName, MenuIcon, parentId, component, redirect,sort
                              from [dbo].[EP_Menu] where MenuId in
                              (
	                              select menuId from
	                              [dbo].[EP_Role] where UserType in
	                              (
		                              select UserType from [dbo].[EP_User] where EmployeeId = '" + employeeId + @"'
		                              and IsLocked = '0' and IsDeleted = '0'
	                              ) and IsValid = 'Y'
                              ) and IsValid = 'Y'
                              union
                              select MenuId, MenuPath, MenuName, MenuIcon, parentId, component, redirect,sort
                              from [dbo].[EP_Menu] where isnull(ParentId, '') = '' and IsValid = 'Y' order by sort";
            return SqlHelper<Menu>.Query(sqlCmd).ToList();
        }
    }
}