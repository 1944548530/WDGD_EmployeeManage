using EmployeeManageApi.BLL;
using EmployeeManageApi.Common;
using EmployeeManageApi.Models;
using EmployeeManageApi.ResponseModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeManageApi.Controllers
{
    public class EmployeeInfoMainController : Controller
    {
        EmployeeInfoMain_BLL bll = new EmployeeInfoMain_BLL();
        public JsonResult EmployeeInfoSave(EmployeeInfo info)
        {
            var response = new Response();
            try
            {
                int result = bll.EmployeeInfoSave(info);
                if (result == 0)
                {
                    response.SetFailed("保存失败");
                }
                else {
                    response.SetSuccess("保存成功");
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
        public JsonResult GetInfoList(QueryParam queryParam)
        {
            try
            {
                var infoLi = bll.GetInfoList(queryParam);
                int count = bll.GetInfoAllList(queryParam).Count();
                var obj = new
                {
                    rows = infoLi,
                    total = count
                };
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                LoggerHelper.WriteLog(ex);
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult InfoListDel(string employeeId)
        {
            var response = new Response();
            try
            {
                int result = bll.InfoListDel(employeeId);
                if (result >= 1)
                {
                    response.SetSuccess("删除成功");
                }
                else {
                    response.SetFailed("删除失败");
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }catch (Exception ex) {
                LoggerHelper.WriteLog(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult InfoListUp(EmployeeInfo info)
        {
            var response = new Response();
            try
            {
                int result = bll.InfoListUp(info);
                if (result > 0)
                {
                    response.SetSuccess("修改成功");
                }
                else {
                    response.SetFailed("修改失败");
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                LoggerHelper.WriteLog(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ExportExcel(ExcelParam param)
        {
            try
            {
                byte[] excelByte = bll.ExportExcel(param);
                return File(excelByte, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            catch (Exception ex) {
                LoggerHelper.WriteLog(ex);
                return File(new byte[10], "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
        }

        [HttpGet]
        public JsonResult deptInfoSave(string dept, string startTime, string endTime) {
            var response = new Response();
            try
            {
                int result = bll.deptInfoSave(dept, startTime, endTime);
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

        [HttpGet]
        public JsonResult PostInfoSave(string post) {
            var response = new Response();
            try
            {
                int result = bll.PostInfoSave(post);
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

        [HttpGet]
        public JsonResult GetDeptList(int deptPageNum, int deptPagesize)
        {
            try
            {
                var infoLi = bll.GetDeptList(deptPageNum, deptPagesize);
                int count = bll.GetDeptAllList().Count();
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

        [HttpGet]
        public JsonResult GetPostList(int postPageNum, int postPagesize)
        {
            try
            {
                var infoLi = bll.GetPostList(postPageNum, postPagesize);
                int count = bll.GetPostAllList().Count();
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

        [HttpGet]
        public JsonResult DeptListDel(string dept) {
            var response = new Response();
            try
            {
                int result = bll.DeptListDel(dept);
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
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult PostListDel(string post)
        {
            var response = new Response();
            try
            {
                int result = bll.PostListDel(post);
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
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetDeptOption() {
            var response = new Response();
            try
            {
                IEnumerable<Option> dept = bll.GetDeptOption();
                response.SetData(dept);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteLog(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetPostOption() {
            var response = new Response();
            try
            {
                IEnumerable<Option> dept = bll.GetPostOption();
                response.SetData(dept);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteLog(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ReverseShift(QueryParam param) {
            var response = new Response();
            try
            {
                Boolean result = bll.ReverseShift(param);
                if (result)
                {
                    response.SetSuccess("翻班成功！");
                }
                else {
                    response.SetFailed("翻班失败！");
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteLog(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetInfoByID(string employeeId) {
            var response = new Response();
            try
            {
                EmployeeInfo employeeInfo = new EmployeeInfo();
                IEnumerable<EmployeeInfo> info = bll.GetInfoByID(employeeId);
                if (info.Count() > 0)
                {
                    employeeInfo = info.ElementAt(0);
                }
                response.SetData(employeeInfo);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteLog(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
    }
}