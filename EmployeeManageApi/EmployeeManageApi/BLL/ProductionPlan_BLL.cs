using EmployeeManageApi.Common;
using EmployeeManageApi.DAL;
using EmployeeManageApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EmployeeManageApi.BLL
{
    
    public class ProductionPlan_BLL
    {
        ProductionPlan_DAL dal = new ProductionPlan_DAL();
        public Boolean ProductionImport(DataTable dt) {
            return dal.ProductionImport(dt);
        }

        public IEnumerable<ProductionPlan> GetInfoList(string dateBegin, string dateEnd, int pageNum, int pagesize) {
            return dal.GetInfoList(dateBegin, dateEnd, pageNum, pagesize);
        }

        public int GetInfoAllList(string dateBegin, string dateEnd) {
            return dal.GetInfoAllList(dateBegin, dateEnd);
        }

        public int SaveRow(ProductionPlan info) {
            return dal.SaveRow(info);
        }

        public Byte[] ExportInfo(string dateBegin, string dateEnd) {
            DataTable dt = dal.ExportInfo(dateBegin, dateEnd);
            Byte[] arr = ExcelHelper.GetExcel(dt, "品质信息", "A1:AC1");
            return arr;
        }
    }
}