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
using System.Web;
using System.Web.Mvc;

namespace EmployeeManageApi.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly string secret = "0123456789ABCDEFGHIJKLMNOPRSTUVWXYZ";
        Attendance_BLL bll = new Attendance_BLL();
        Tool tool = new Tool();
        public JsonResult GetOvertTimeInfo(string date, string department, string shift, int overTimePage, int overTimePageNum) {
            var response = new Response();
            try
            {
                var info = bll.GetOvertTimeInfo(date, department, shift, overTimePage, overTimePageNum);
                int count = bll.GetOvertTimeInfoTotal(date, department, shift);
                var obj = new
                {
                    rows = info,
                    total = count
                };
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                LoggerHelper.WriteLog(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetAttendanceInfo(string date, string department, string shift, int pageNum, int pagesize) {
            var response = new Response();
            try
            {
                var info = bll.GetAttendanceInfo(date, department, shift, pageNum, pagesize);
                int count = bll.GetAttendanceInfoTotal(date, department, shift);
                var obj = new
                {
                    rows = info,
                    total = count
                };
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteLog(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SaveOverTimeRow(AttendanceModel attendModel) {
            string authHeader = this.Request.Headers["Authorization"];//Header中的token
            User user = tool.GetAuthUser(authHeader);
            attendModel.overLmEmployeeId = user.EmployeeId;
            attendModel.overLmName = user.DisplayName;
            var response = new Response();
            try
            {
                int result = bll.SaveOverTimeRow(attendModel);
                if (result > 0)
                {
                    response.SetSuccess("保存成功!");
                }
                else
                {
                    response.SetFailed("保存失败!");
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                response.SetFailed("服务器内部错误!");
                LoggerHelper.WriteLog(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SaveAttendanceRow(AttendanceModel attendModel) {
            string authHeader = this.Request.Headers["Authorization"];//Header中的token
            //IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            //IJsonSerializer serializer = new JsonNetSerializer();
            //IDateTimeProvider provider = new UtcDateTimeProvider();
            //IJwtValidator validator = new JwtValidator(serializer, provider);
            //IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            //IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
            //var json = decoder.Decode(authHeader, secret, verify: true);
            //User user = JsonConvert.DeserializeObject<User>(json);
            User user = tool.GetAuthUser(authHeader);
            attendModel.lmEmployeeId = user.EmployeeId;
            attendModel.lmName = user.DisplayName;
            var response = new Response();
            try
            {
                int result = bll.SaveAttendanceRow(attendModel);
                if (result > 0) {
                    response.SetSuccess("保存成功!");
                }
                else {
                    response.SetFailed("保存失败!");
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                response.SetFailed("服务器内部错误!");
                LoggerHelper.WriteLog(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ExportAttendanceInfo(string date, string department, string shift) {
            try
            {
                byte[] arr = bll.ExportAttendanceInfo(date, department, shift);
                return File(arr, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteLog(ex);
                return File(new byte[10], "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
        }

        
    }
}