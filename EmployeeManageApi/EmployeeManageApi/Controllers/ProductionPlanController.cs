using EmployeeManageApi.Common;
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
        
        public JsonResult ProductionImport() {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var response = new Response();
            System.IO.Stream s = System.Web.HttpContext.Current.Request.InputStream;
            BinaryReader reader = new BinaryReader(s);
            string fileClass = "";
            string json = "";
            try
            {
                for (int i = 0; i < 2; i++)
                {
                    fileClass += reader.ReadByte().ToString();
                }
                if (fileClass == "4545") {
                    DataTable dt = new DataTable();
                    using (ExcelPackage pck = new ExcelPackage(s)) {
                        ExcelWorksheet ws = pck.Workbook.Worksheets[0];
                        int minColumnNum = ws.Dimension.Start.Column;
                        int maxColumnNum = ws.Dimension.End.Column;
                        int minRowNum = ws.Dimension.Start.Row;
                        int maxRowNum = ws.Dimension.End.Row;
                        DataColumn dataCol;
                        string[] columnStr = { "col1", "col2", "col3", "col4", "col5", "col6", "col7", "col8", "col9",
                                                "col10", "col11", "col12", "col13", "col14", "col15", "col16", "col17", "col18"};
                        if (maxColumnNum == columnStr.Length) {
                            foreach (string i in columnStr) {
                                dataCol = new DataColumn(i, typeof(string));
                                dt.Columns.Add(dataCol);
                            }
                            for (int i = 3; i < maxRowNum; i++) {
                                DataRow dataRow = dt.NewRow();
                                for (int j = 1; j < maxColumnNum; j++) {
                                    dataRow[j - 1] = ws.Cells[i, j].Value;
                                }
                                dt.Rows.Add(dataRow);
                            }
                        }
                    }
                }
                return Json(response);
            }
            catch (Exception ex) {
                LoggerHelper.WriteLog(ex);
                return Json(response);
            }
        }
    }
}