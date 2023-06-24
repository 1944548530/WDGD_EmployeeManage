using EmployeeManageApi.DAL;
using EmployeeManageApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EmployeeManageApi.BLL
{
    public class Login_BLL
    {
        Login_DAL dal = new Login_DAL();
        public User Login(string username, string password)
        {
            return dal.Login(username, password);
        }

        public IEnumerable<User> GetAccountList(QueryParam queryParam) {
            return dal.GetAccountList(queryParam);
        }

        public IEnumerable<User> GetAccountAllList(QueryParam queryParam) {
            return dal.GetAccountAllList(queryParam);
        }

        public int Regist(User user) {
            string lmdate = DateTime.Now.ToString("yyyy-MM-dd");
            string lmtime = DateTime.Now.ToString("HH:mm:ss");
            user.lmdate = lmdate;
            user.lmtime = lmtime;
            return dal.Regist(user);
        }

        public int AccountDel(string loginName) {
            return dal.AccountDel(loginName);
        }

        public int AccountUp(User user) {
            return dal.AccountUp(user);
        }

        public List<MenuList> GetMenuList(string employeeId) {
            List<Menu> menuInfo = dal.GetMenuList(employeeId);
            List<MenuList> menuList = new List<MenuList>();
            List<Menu> firLi = menuInfo.Where(v => string.IsNullOrEmpty(v.parentId)).ToList();
            if (firLi.Count > 0) {
                for (int i = 0; i < firLi.Count; i++) {
                    MenuList menu = new MenuList();
                    menu.path = firLi[i].MenuPath;
                    menu.component = firLi[i].component;
                    menu.redirect = firLi[i].redirect;
                    menu.name = firLi[i].MenuId;
                    MenuConfig meta = new MenuConfig();
                    meta.icon = firLi[i].MenuIcon;
                    meta.title = firLi[i].MenuName;
                    meta.keepAlive = true;
                    menu.meta = meta;
                    List<MenuChildren> menuChildrenLi = new List<MenuChildren>();
                    List<Menu> childrenLi = menuInfo.Where(v => v.parentId == firLi[i].MenuId).ToList();
                    if (childrenLi.Count > 0) {
                        for (int j = 0; j < childrenLi.Count; j++) {
                            MenuChildren menuChildren = new MenuChildren();
                            menuChildren.path = childrenLi[j].MenuPath;
                            menuChildren.name = childrenLi[j].MenuId;
                            menuChildren.component = childrenLi[j].component;
                            MenuConfig metaChild = new MenuConfig();
                            metaChild.icon = childrenLi[j].MenuIcon;
                            metaChild.title = childrenLi[j].MenuName;
                            metaChild.keepAlive = true;
                            menuChildren.meta = metaChild;
                            menuChildrenLi.Add(menuChildren);
                        }
                    }
                    menu.children = menuChildrenLi;
                    menuList.Add(menu);
                }
            }
            return menuList;
        }
    }
}