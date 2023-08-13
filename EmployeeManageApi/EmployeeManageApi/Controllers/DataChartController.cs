using EmployeeManageApi.BLL;
using EmployeeManageApi.Common;
using EmployeeManageApi.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeManageApi.Controllers
{
    
    public class DataChartController : Controller
    {
        DataChart_BLL bll = new DataChart_BLL();
        
        /// <summary>
        /// 获取各车间的出勤率
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetDeptAttendance()
        {
            var response = new Response();
            try
            {
                var info = bll.GetDeptAttendance();
                response.SetData(info);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                response.SetFailed("服务器内部错误!");
                LoggerHelper.WriteLog(e);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
    }
}