using EmployeeManageApi.Common;
using EmployeeManageApi.DAL;
using EmployeeManageApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManageApi.BLL
{
    
    public class Equipment_BLL
    {
        Tool tool = new Tool();
        Equipment_DAL dal = new Equipment_DAL();
        EmployeeInfoMain_DAL employeeDal = new EmployeeInfoMain_DAL();
        public int SaveEquipmentInfo(EquipmentModel equipmentInfo) {
            equipmentInfo.Create();
            return dal.SaveEquipmentInfo(equipmentInfo);
        }

        public IEnumerable<EquipmentModel> GetEquipmentList(string department, string equipmentName, int pageNum, int pagesize) {
            return dal.GetEquipmentList(department, equipmentName, pageNum, pagesize);
        }

        public int GetEquipmentCount(string department, string date) {
            return dal.GetEquipmentCount(department, date);
        }

        public int EquipmentInfoDel(string department, string equipmentName) {
            return dal.EquipmentInfoDel(department, equipmentName);
        }

        public IEnumerable<EquipmentUseModel> GetEquipmentUseInfo(string department, string date, int pageNum, int pagesize)
        {
            return dal.GetEquipmentUseInfo(department, date, pageNum, pagesize);
        }

        public int GetEquipmentUseInfoCount(string department, string date) {
            return dal.GetEquipmentUseInfoCount(department, date);
        }

        public int SaveOverRow(EquipmentUseModel info)
        {
            return dal.SaveOverRow(info);
        }

        public int UpdateOverRow(EquipmentUseModel info) {
            return dal.UpdateOverRow(info);
        }

        public int IsMaintainOver(EquipmentUseModel info) {
            return dal.IsMaintainOver(info);
        }

        public Object GetEquipUseInfo() {
            var infoLi = dal.GetEquipUseInfo();
            var deptLi = employeeDal.GetDeptOption();
            List<List<object>> result = new List<List<object>>();
            foreach (var deptModel in deptLi) {
                string dept = deptModel.value;
                var infoByDept = infoLi.Where(v => v.department == dept);
                List<object> infoByDeptLi = new List<object>();
                if (infoByDept.Count() > 0) {
                    foreach (var info in infoByDept) {
                        string equipment = info.equipmentName;
                        int openNum = info.openNum;
                        int equipNum = info.equipmentNum;
                        float openPer = tool.perCal(openNum, equipNum);
                        string openPerStr = openPer + "%";
                        var obj = new { 
                            department = dept,
                            equipment = equipment,
                            openPerStr = openPerStr,
                            openNum = openNum,
                            equipNum = equipNum
                        };
                        infoByDeptLi.Add(obj);
                    }
                    result.Add(infoByDeptLi);
                }

            }
            return result;
        }
    }
}