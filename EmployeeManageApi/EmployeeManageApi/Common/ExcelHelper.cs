using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EmployeeManageApi.Common
{
    public class ExcelHelper
    {
        public static byte[] GetExcel(DataTable dt, string sheetName, string region)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(sheetName);
                ws.Cells["A1"].LoadFromDataTable(dt, true);
                #region 设置样式，可以不要
                ws.Columns.Width = 25;
                using (ExcelRange rng = ws.Cells[region])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    rng.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    rng.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    rng.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    rng.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(79, 129, 189));
                    rng.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    rng.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    rng.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    rng.Style.WrapText = true;
                }
                using (ExcelRange col = ws.Cells[2, 1, 2 + dt.Rows.Count - 1, dt.Columns.Count])
                {
                    col.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    col.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    col.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    col.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    col.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    col.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    col.Style.WrapText = true;

                }
                #endregion
                return pck.GetAsByteArray();
            }
        }
    }
}