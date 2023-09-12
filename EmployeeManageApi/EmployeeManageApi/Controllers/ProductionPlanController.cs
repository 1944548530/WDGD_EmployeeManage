using EmployeeManageApi.BLL;
using EmployeeManageApi.Common;
using EmployeeManageApi.Models;
using EmployeeManageApi.ResponseModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeManageApi.Controllers
{
    public class ProductionPlanController : Controller
    {
        Tool tool = new Tool();
        ProductionPlan_BLL bll = new ProductionPlan_BLL();
        public JsonResult ProductionImport() {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var response = new Response();
            System.IO.Stream s = System.Web.HttpContext.Current.Request.InputStream;
            BinaryReader reader = new BinaryReader(s);
            string fileClass = "";
            string json = "";
            try
            {
                DataTable dt = SqlHelper<ProductionPlan>.sqlTable($@"select  date, line, gongXu, gongDan, liaoHao, pinMing, guiGe, 
                                                                        riJiHuaTouRu, danPianLiLunShu, jiHuaCPCC,
                                                                        yuGuLiangLv, jiHuaHeGeShu, shiJiChanChu, shiJiCPChanChu, 
                                                                        shiJiHeGeShu, shiJiLiangLv, jiHuaDaChengLv, 
                                                                        gongXuLiangLv, biaoZhunJiShi, jiHuaJiShi, changChuJiShi, shiJiJiShi, 
                                                                        biaoZhunRGS, jiHuaRenShi,changShuRenShi, shiJiRenShi, 
                                                                        wuLiaoDaoDaHour, wuLiaoZhuanYiHour, NGReason
                                                                        from [dbo].[EP_ProductionPlan]");
                for (int i = 0; i < 2; i++)
                {
                    fileClass += reader.ReadByte().ToString();
                }
                if (fileClass == "4545")
                {
                    using (ExcelPackage pck = new ExcelPackage(s))
                    {
                        ExcelWorksheet ws = pck.Workbook.Worksheets[0];
                        int minColumnNum = ws.Dimension.Start.Column;
                        int maxColumnNum = ws.Dimension.End.Column;
                        int minRowNum = ws.Dimension.Start.Row;
                        int maxRowNum = ws.Dimension.End.Row;

                        if (maxColumnNum == dt.Columns.Count + 2)
                        {
                            for (int i = 2; i < maxRowNum; i++)
                            {
                                if ((ws.Cells[i, 6].Value == null ? "" : ws.Cells[i, 6].Value.ToString()).Trim() != "合计")
                                {
                                    DataRow dataRow = dt.NewRow();
                                    string year = ws.Cells[i, 1].Value.ToString();
                                    string month = int.Parse(ws.Cells[i, 2].Value.ToString()) < 10 ? "0" + ws.Cells[i, 2].Value.ToString() : ws.Cells[i, 2].Value.ToString();
                                    string day = int.Parse(ws.Cells[i, 3].Value.ToString()) < 10 ? "0" + ws.Cells[i, 3].Value.ToString() : ws.Cells[i, 3].Value.ToString();
                                    dataRow[0] = year + "-" + month + "-" + day;
                                    for (int j = 4; j < maxColumnNum; j++)
                                    {
                                        if (dt.Columns[j - 3].DataType == typeof(float) || dt.Columns[j - 3].DataType == typeof(int) || dt.Columns[j - 3].DataType == typeof(double))
                                        {
                                            dataRow[j - 3] = ws.Cells[i, j].Value == null ? 0 : float.Parse(ws.Cells[i, j].Value.ToString().Replace("#DIV/0!", "0").Replace("#N/A", "0").Replace("/", "0"));
                                        }
                                        else
                                        {
                                            dataRow[j - 3] = ws.Cells[i, j].Value == null ? "" : ws.Cells[i, j].Value.ToString();
                                        }

                                    }
                                    dt.Rows.Add(dataRow);
                                }
                            }
                            Boolean result = bll.ProductionImport(dt);
                            if (result)
                            {
                                response.SetSuccess("Excel导入成功！");
                            }
                            else
                            {
                                response.SetFailed("Excel导入失败！");
                            }
                        }
                        else {
                            response.SetFailed("Excel导入失败！");
                        }
                    }
                }
                
                return Json(response);
            }
            catch (Exception ex) {
                response.SetFailed("Excel导入失败！");
                LoggerHelper.WriteLog(ex);
                return Json(response);
            }
        }

        public ActionResult ExportInfo(string dateBegin, string dateEnd)
        {
            try
            {
                Byte[] arr = bll.ExportInfo(dateBegin, dateEnd);
                return File(arr, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            catch (Exception ex) {
                LoggerHelper.WriteLog(ex);
                return File(new byte[10], "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
        }

        [HttpGet]
        public JsonResult GetInfoList(string dateBegin, string dateEnd, int pageNum, int pagesize)
        {
            try
            {
                var infoLi = bll.GetInfoList(dateBegin, dateEnd, pageNum, pagesize);
                int count = bll.GetInfoAllList(dateBegin, dateEnd);
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

        [HttpPost]
        public JsonResult SaveRow(ProductionPlan info) {
            string authHeader = this.Request.Headers["Authorization"];//Header中的token
            User user = tool.GetAuthUser(authHeader);
            info.lmUser = user.DisplayName;
            var response = new Response();
            try
            {
                int result = bll.SaveRow(info);
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
    }
}