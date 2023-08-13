using EmployeeManageApi.Common;
using EmployeeManageApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManageApi.DAL
{
    public class Equipment_DAL
    {
        Tool tool = new Tool();
        public int SaveEquipmentInfo(EquipmentModel equipmentInfo) {
            string sqlCmd = $@"insert into EP_Equipment(department, equipmentName, equipmentNum, lmEmployeeId, lmName,isValid, lmdate, lmtime) 
                                values
                                ('{equipmentInfo.department}', '{equipmentInfo.equipmentName}', '{equipmentInfo.equipmentNum}', 
                                '{equipmentInfo.lmEmployeeId}', '{equipmentInfo.lmName}', 'Y', '{equipmentInfo.lmdate}', '{equipmentInfo.lmtime}')";
            return SqlHelper<EquipmentModel>.Execute(sqlCmd);
        }

        public IEnumerable<EquipmentModel> GetEquipmentList(string department, string equipmentName, int pageNum, int pagesize) {
            int indexBegin = (pageNum - 1) * pagesize + 1;
            int indexEnd = pageNum * pagesize;
            string departmentApp = tool.sqlAppend2("department", department);
            string equipmentNameApp = tool.sqlAppend2("equipmentName", equipmentName);
            string sqlCmd = $@"select * from
                                (
	                                select ROW_NUMBER() over(order by lmdate desc, lmtime desc)num,  *
	                                from (select * from EP_Equipment where isValid = 'Y' {departmentApp} {equipmentNameApp})a
                                )b where b.num between '{indexBegin}' and '{indexEnd}'";
            return SqlHelper<EquipmentModel>.Query(sqlCmd);

        }

        public int GetEquipmentCount(string department, string equipmentName) {
            string departmentApp = tool.sqlAppend2("department", department);
            string equipmentNameApp = tool.sqlAppend2("equipmentName", equipmentName);
            string sqlCmd = $@"select * from EP_Equipment where isValid = 'Y' {departmentApp} {equipmentNameApp}";
            return SqlHelper<EquipmentModel>.Query(sqlCmd).Count();
        }

        public int EquipmentInfoDel(string deparment, string equipmentName) {
            string sqlCmd = $@"update EP_Equipment set isValid = 'N'
                                where department = '{deparment}' and equipmentName = '{equipmentName}'";
            return SqlHelper<EquipmentModel>.Execute(sqlCmd);
        }

        public IEnumerable<EquipmentUseModel> GetEquipmentUseInfo(string department, string date, int pageNum, int pagesize) {
            string departmentApp = tool.sqlAppend2("a.department", department);
            int indexBegin = (pageNum - 1) * pagesize + 1;
            int indexEnd = pageNum * pagesize;
            string sqlCmd = $@"select * from 
                                (
	                                select ROW_NUMBER() over(order by lmdate desc, lmtime desc) num, * from
	                                (
		                                select '{date}' as date, b.cancelNum, b.maintainNum, b.openNum, a.equipmentNum, a.equipmentName, a.department, b.lmdate, b.lmtime, b.lmEmployeeName, b.lmstate 
		                                from [EP_Equipment] a
		                                left join (select * from [EP_EquipmentUseInfo] where date = '{date}') b on a.equipmentName = b.equipmentName and a.department = b.department
		                                where a.isValid = 'Y' {departmentApp}
	                                )a
                                )c where c.num between '{indexBegin}' and '{indexEnd}'";

            return SqlHelper<EquipmentUseModel>.Query(sqlCmd);
        }

        public int GetEquipmentUseInfoCount(string department, string date) {
            string departmentApp = tool.sqlAppend2("a.department", department);
            string sqlCmd = $@"select b.date, b.cancelNum, b.maintainNum, b.openNum, a.equipmentNum, a.equipmentName, a.department, b.lmdate, b.lmtime, b.lmstate 
		                        from [EP_Equipment] a
		                        left join (select * from [EP_EquipmentUseInfo] where date = '{date}') b on a.equipmentName = b.equipmentName and a.department = b.department
		                        where a.isValid = 'Y' {departmentApp}";
            return SqlHelper<EquipmentUseModel>.Query(sqlCmd).Count();
        }

        public int SaveOverRow(EquipmentUseModel info) {
            string sqlCmd = $@"insert into EP_EquipmentUseInfo 
                                (date, department, equipmentName, openNum, cancelNum, maintainNum, lmEmployeeId, lmEmployeeName, lmdate, lmtime, lmstate)
                                values
                                ('{info.date}', '{info.department}', '{info.equipmentName}', '{info.openNum}', '{info.cancelNum}',
                                '{info.maintainNum}', '{info.lmEmployeeId}', '{info.lmEmployeeName}', '{info.lmdate}', '{info.lmtime}', 'Y')";
            return SqlHelper<EquipmentModel>.Execute(sqlCmd);
        }

        public int IsMaintainOver(EquipmentUseModel info) {
            string sqlCmd = $@"select * from EP_EquipmentUseInfo where date = '{info.date}' and department = '{info.department}' and equipmentName = '{info.equipmentName}'
                                and lmstate = 'Y'";
            return SqlHelper<EquipmentUseModel>.Query(sqlCmd).Count();
        }

        public int UpdateOverRow(EquipmentUseModel info) {
            string sqlCmd = $@"update EP_EquipmentUseInfo set
                                openNum = {info.openNum}, cancelNum = {info.cancelNum}, maintainNum = {info.maintainNum} 
                                where [date] = '{info.date}' and department = '{info.department}' and equipmentName = '{info.equipmentName}'";
            return SqlHelper<EquipmentUseModel>.Execute(sqlCmd);
        }

        public IEnumerable<EquipmentUseModel> GetEquipUseInfo(){
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string sqlCmd = $@"select '{date}' as [date], ep.department, ep.equipmentName, 
                                case when epu.openNum is null
                                then 0
                                else epu.openNum 
                                end as openNum, 
                                ep.equipmentNum
                                from EP_Equipment ep
                                left join (select * from [EP_EquipmentUseInfo] where date = '{date}') epu
                                on epu.department = ep.department and epu.equipmentName = ep.equipmentName
                                where ep.isValid = 'Y'";
            return SqlHelper<EquipmentUseModel>.Query(sqlCmd);
        }
    }
}