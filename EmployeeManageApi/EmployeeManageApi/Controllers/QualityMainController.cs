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
    public class QualityMainController : Controller
    {
        QualityMain_BLL bll = new QualityMain_BLL();
        public JsonResult GetInfoById(string employeeId)
        {
            var response = new Response();
            try
            {
                EmployeeInfo employeeInfo = bll.GetInfoById(employeeId);
                if (employeeInfo != null)
                {
                    response.SetSuccess("查询成功");
                    response.SetData(employeeInfo);
                }
                else {
                    response.SetFailed("查询失败,此工号不存在");
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                LoggerHelper.WriteLog(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult QualityInfoSave(QualityInfo qualityInfo)
        {
            var response = new Response();
            try
            {
                int result = bll.QualityInfoSave(qualityInfo);
                if (result > 0)
                {
                    response.SetSuccess("保存成功");
                }
                else {
                    response.SetFailed("保存失败");
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
        public JsonResult GetInfoList(QualityQueryParam param) {
            try
            {
                var infoLi = bll.GetInfoList(param);
                int count = bll.GetInfoAllList(param).Count();
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

        public JsonResult InfoDel(string employeeId, string eventType, string checkDate) {
            var response = new Response();
            try
            {
                int result = bll.InfoDel(employeeId, eventType, checkDate);
                if (result > 0)
                {
                    response.SetSuccess("删除成功");
                }
                else
                {
                    response.SetFailed("删除失败");
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

        public ActionResult ExportExcel(QualityQueryParam param)
        {
            try
            {
                byte[] arr = bll.ExportExcel(param);
                return File(arr, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            catch (Exception ex) {
                LoggerHelper.WriteLog(ex);
                return File(new byte[10], "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
        }

        [HttpPost]
        public JsonResult InfoUp(QualityInfo qualityInfo) {
            var response = new Response();
            try
            {
                int result = bll.InfoUp(qualityInfo);
                if (result > 0)
                {
                    response.SetSuccess("修改成功");
                }
                else
                {
                    response.SetFailed("修改失败");
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
    }
}