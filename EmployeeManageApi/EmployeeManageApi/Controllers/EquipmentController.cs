using EmployeeManageApi.BLL;
using EmployeeManageApi.Common;
using EmployeeManageApi.Models;
using EmployeeManageApi.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeManageApi.Controllers
{
    public class EquipmentController : Controller
    {
        Equipment_BLL bll = new Equipment_BLL();
        Tool tool = new Tool();

        [HttpPost]
        public JsonResult SaveEquipmentInfo(EquipmentModel equimentInfo) {
            string authHeader = this.Request.Headers["Authorization"];
            var response = new Response();
            try
            {
                User user = tool.GetAuthUser(authHeader);
                equimentInfo.lmEmployeeId = user.EmployeeId;
                equimentInfo.lmName = user.DisplayName;
                int result = bll.SaveEquipmentInfo(equimentInfo);
                if (result > 0)
                {
                    response.SetSuccess("保存成功！");
                }
                else {
                    response.SetFailed("保存失败！");
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e) {
                LoggerHelper.WriteLog(e);
                response.SetFailed("服务器内部错误");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetEquipmentList(string department, string equipmentName, int pageNum, int pagesize)
        {
            var response = new Response();
            try
            {
                IEnumerable<EquipmentModel> infoLi = bll.GetEquipmentList(department, equipmentName, pageNum, pagesize);
                int count = bll.GetEquipmentCount(department, equipmentName);
                var obj = new
                {
                    rows = infoLi,
                    total = count
                };
                response.SetData(obj);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                LoggerHelper.WriteLog(ex);
                response.SetFailed("服务器内部错误");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult EquipmentInfoDel(string department, string equipmentName) {
            var response = new Response();
            try
            {
                int result = bll.EquipmentInfoDel(department, equipmentName);
                if (result > 0) {
                    response.SetSuccess("删除成功！");
                }
                else {
                    response.SetFailed("删除失败！");
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                LoggerHelper.WriteLog(ex);
                response.SetFailed("服务器内部错误");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetEquipmentUseInfo(string department, string date, int pageNum, int pagesize)
        {
            var response = new Response();
            try
            {
                IEnumerable<EquipmentUseModel> infoLi = bll.GetEquipmentUseInfo(department, date, pageNum, pagesize);
                int count = bll.GetEquipmentUseInfoCount(department, date);
                var obj = new
                {
                    infoLi = infoLi,
                    total = count
                };
                response.SetData(obj);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteLog(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SaveOverRow(EquipmentUseModel info)
        {
            string authHeader = this.Request.Headers["Authorization"];
            var response = new Response();
            try
            {
                User user = tool.GetAuthUser(authHeader);
                string lmdate = DateTime.Now.ToString("yyyy-MM-dd");
                string lmtime = DateTime.Now.ToString("HH:mm:ss");
                info.lmEmployeeId = user.EmployeeId;
                info.lmEmployeeName = user.LoginName;
                info.lmdate = lmdate;
                info.lmtime = lmtime;
                int maintainOver = bll.IsMaintainOver(info);
                int result = 0;
                if (maintainOver > 0)
                {
                    result = bll.UpdateOverRow(info);
                }
                else {
                    result = bll.SaveOverRow(info);
                }
                
                if (result > 0)
                {
                    response.SetSuccess("保存成功！");
                }
                else {
                    response.SetFailed("保存失败！");
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteLog(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetEquipUseInfo()
        {
            var response = new Response();
            try
            {
                var result = bll.GetEquipUseInfo();
                response.SetData(result);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                LoggerHelper.WriteLog(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
    }
}