using EmployeeManageApi.BLL;
using EmployeeManageApi.Common;
using EmployeeManageApi.Models;
using EmployeeManageApi.ResponseModel;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using static EmployeeManageApi.Enum.CommonEnum;

namespace EmployeeManageApi.Controllers
{
    public class AuthController : Controller
    {
        private readonly string secret = "0123456789ABCDEFGHIJKLMNOPRSTUVWXYZ";
        Login_BLL bll = new Login_BLL();
        // GET: Login
        public JsonResult Login(string username, string password)
        {
            var response = new Response();
            try
            {
                User user = new User();
                user = bll.Login(username, password);
                string result = "OK";
                if (string.IsNullOrEmpty(user.Password) || user.IsDeleted == IsDeleted.Yes) {
                    result = "NG";
                    response.SetFailed("用户不存在");
                }else  if(user.Password != password.Trim()) {
                    result = "NG";
                    response.SetFailed("密码不正确");
                }else if (user.IsLocked == IsLocked.Locked) {
                    result = "NG";
                    response.SetFailed("账号已锁定");
                }else if (user.Status == UserStatus.Forbidden)
                {
                    result = "NG";
                    response.SetFailed("账号已被禁用");
                }
                if (result == "NG")
                {
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var claimsIdentity = new ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.Name, username),
                        new Claim("avatar", ""),
                        new Claim("displayName", user.DisplayName),
                        new Claim("loginName", user.LoginName),
                        new Claim("EmployeeId", user.EmployeeId),
                        new Claim("guid", user.Guid.ToString()),
                        new Claim("userType", ((int)user.UserType).ToString())
                    });
                    var token = JwtBearerAuthenticationExtension.GetJwtAccessToken(secret, claimsIdentity);
                    response.SetSuccess("ok");
                    response.SetData(token);
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex) 
            {
                LoggerHelper.WriteLog(ex);
                response.SetFailed("服务器内部错误");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Profile()
        {
            var response = new Response();
            try
            {
                string authHeader = this.Request.Headers["Authorization"];//Header中的token
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
                var json = decoder.Decode(authHeader, secret, verify: true);
                User user = JsonConvert.DeserializeObject<User>(json);
                string userTypeC = "";
                if (user.UserType == UserType.SuperAdministrator)
                {
                    userTypeC = "超级管理员";
                }
                else if (user.UserType == UserType.Admin)
                {
                    userTypeC = "管理员";
                }
                else
                {
                    userTypeC = "用户";
                }
                response.SetData(new
                {
                    access = new string[] { },
                    employeeId = user.EmployeeId,
                    user_guid = user.Guid,
                    user_name = user.DisplayName,
                    user_type = userTypeC

                });
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                LoggerHelper.WriteLog(e);
                response.SetFailed("服务器内部错误");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetAccountList(QueryParam queryParam)
        {
            try
            {
                var infoLi = bll.GetAccountList(queryParam);
                int count = bll.GetAccountAllList(queryParam).Count();
                var obj = new
                {
                    rows = infoLi,
                    total = count
                };
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteLog(ex);
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }
        
        public JsonResult Regist(User user)
        {
            var response = new Response();
            try
            {
                int result = bll.Regist(user);
                if (result == 0)
                {
                    response.SetFailed("保存失败");
                }
                else
                {
                    response.SetSuccess("保存成功");
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteLog(ex);
                response.SetFailed("服务器内部错误");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AccountDel(string loginName) {
            var response = new Response();
            try
            {
                int result = bll.AccountDel(loginName);
                if (result == 0)
                {
                    response.SetFailed("删除失败！");
                }
                else
                {
                    response.SetSuccess("删除成功！");
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteLog(ex);
                response.SetFailed("服务器内部错误");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AccountUp(User user) {
            var response = new Response();
            try
            {
                string authHeader = this.Request.Headers["Authorization"];//Header中的token
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
                var json = decoder.Decode(authHeader, secret, verify: true);
                User adminuser = JsonConvert.DeserializeObject<User>(json);
                string loginName = adminuser.LoginName;
                int result = bll.AccountUp(user);
                if (result == 0)
                {
                    response.SetFailed("更新失败");
                }
                else
                {
                    response.SetSuccess("更新成功");
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteLog(ex);
                response.SetFailed("服务器内部错误");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetMenuList(string employeeId) {
            var response = new Response();
            try
            {
                List<MenuList> menuList = bll.GetMenuList(employeeId);
                response.SetSuccess("成功获取菜单!");
                response.SetData(menuList);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                LoggerHelper.WriteLog(ex);
                response.SetFailed("服务器内部错误");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
    }
}