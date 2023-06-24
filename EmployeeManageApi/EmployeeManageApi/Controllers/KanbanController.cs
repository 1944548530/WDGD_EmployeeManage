using EmployeeManageApi.BLL;
using EmployeeManageApi.Models;
using EmployeeManageApi.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeManageApi.Controllers
{
    public class KanbanController : Controller
    {
        Kanban_BLL bll = new Kanban_BLL(); 
        /// <summary>
        /// 获取当前部门的员工看板信息
        /// </summary>
        /// <returns></returns>
        public JsonResult GetKanbanInfo(string date, string department, string shift)
        {
            var response = new Response();
            try
            {
                List<KanbanPerson> kanbanPersonLi = bll.GetKanbanInfo(date, department, shift);
                response.SetData(kanbanPersonLi);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                response.SetFailed("服务器内部错误！");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetEmployeeInfo(string name, string post) 
        {
            var response = new Response();
            try
            {
                EmployeeInfo info = bll.GetEmployeeInfo(name, post);
                response.SetData(info);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                response.SetFailed("查找失败");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
    }
}