using EmployeeManageApi.Common;
using EmployeeManageApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EmployeeManageApi.DAL
{
    public class ProductionPlan_DAL
    {
        public Boolean ProductionImport(DataTable dt) {
            List<string> sqlLi = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string sqlCmd = $@"insert into EP_ProductionPlan 
                                    (date, line, gongXu, gongDan, liaoHao, pinMing, guiGe, riJiHuaTouRu, danPianLiLunShu, jiHuaCPCC,
                                    yuGuLiangLv, jiHuaHeGeShu, shiJiChanChu, shiJiCPChanChu, shiJiHeGeShu, shiJiLiangLv, jiHuaDaChengLv, 
                                    gongXuLiangLv, biaoZhunJiShi, jiHuaJiShi, changChuJiShi, shiJiJiShi, biaoZhunRGS, jiHuaRenShi,
                                    changShuRenShi, shiJiRenShi, wuLiaoDaoDaHour, wuLiaoZhuanYiHour, NGReason) values
                                    ('{dt.Rows[i]["date"]}', '{dt.Rows[i]["line"]}', '{dt.Rows[i]["gongXu"]}', '{dt.Rows[i]["gongDan"]}', 
                                    '{dt.Rows[i]["liaoHao"]}', '{dt.Rows[i]["pinMing"]}', '{dt.Rows[i]["guiGe"]}', '{dt.Rows[i]["riJiHuaTouRu"]}', 
                                    '{dt.Rows[i]["danPianLiLunShu"]}', '{dt.Rows[i]["jiHuaCPCC"]}', '{dt.Rows[i]["yuGuLiangLv"]}', '{dt.Rows[i]["jiHuaHeGeShu"]}', 
                                    '{dt.Rows[i]["shiJiChanChu"]}', '{dt.Rows[i]["shiJiCPChanChu"]}', '{dt.Rows[i]["shiJiHeGeShu"]}', '{dt.Rows[i]["shiJiLiangLv"]}',
                                    '{dt.Rows[i]["jiHuaDaChengLv"]}', '{dt.Rows[i]["gongXuLiangLv"]}', '{dt.Rows[i]["biaoZhunJiShi"]}', '{dt.Rows[i]["jiHuaJiShi"]}', 
                                    '{dt.Rows[i]["changChuJiShi"]}', '{dt.Rows[i]["shiJiJiShi"]}', '{dt.Rows[i]["biaoZhunRGS"]}', '{dt.Rows[i]["jiHuaRenShi"]}',
                                    '{dt.Rows[i]["changShuRenShi"]}', '{dt.Rows[i]["shiJiRenShi"]}', '{dt.Rows[i]["wuLiaoDaoDaHour"]}', '{dt.Rows[i]["wuLiaoZhuanYiHour"]}',
                                    '{dt.Rows[i]["NGReason"]}')";
                sqlLi.Add(sqlCmd);
            }
            Boolean result = SqlHelper<ProductionPlan>.Transaction(sqlLi);
            return result;
        }

        public IEnumerable<ProductionPlan> GetInfoList(string dateBegin, string dateEnd, int pageNum, int pagesize) {
            int indexBegin = (pageNum - 1) * pagesize + 1;
            int indexEnd = pagesize * pageNum;
            string sqlCmd = $@"select * from
                                (
	                                select ROW_NUMBER() OVER(order by lmdate desc, lmtime desc)num, *  from 
	                                (select * from EP_ProductionPlan where date between '{dateBegin}' and '{dateEnd}')a
                                )b where b.num between '{indexBegin}' and '{indexEnd}'";
            return SqlHelper<ProductionPlan>.Query(sqlCmd);
        }

        public int GetInfoAllList(string dateBegin, string dateEnd) {
            string sqlCmd = $@"select * from EP_ProductionPlan where date between '{dateBegin}' and '{dateEnd}'";
            DataTable dt = SqlHelper<ProductionPlan>.sqlTable(sqlCmd);
            return dt.Rows.Count;
        }

        public int SaveRow(ProductionPlan info) {
            string sqlCmd = $@"update EP_ProductionPlan set shiJiChanChu = '{info.shiJiChanChu}', shiJiCPChanChu = '{info.shiJiCPChanChu}', shiJiHeGeShu = '{info.shiJiHeGeShu}', lmUser = '{info.lmUser}',
                                shiJiLiangLv = '{info.shiJiLiangLv}', NGReason = '{info.NGReason}', lmstate = 'Y' where [date] = '{info.date}' and line = '{info.line}' and gongXu = '{info.gongXu}' 
                                and gongDan = '{info.gongDan}' and liaoHao = '{info.liaoHao}' ";
            int result = SqlHelper<ProductionPlan>.Execute(sqlCmd);
            return result;
        }

        public DataTable ExportInfo(string dateBegin, string dateEnd) {
            string sqlCmd = $@"select  date '日期', line '线别', gongXu '工序', gongDan '工单', liaoHao '料号', pinMing '品名', guiGe '规格', 
                                riJiHuaTouRu '日计划投入WAFER', danPianLiLunShu '单片理论数', jiHuaCPCC '计划成品产出',
                                yuGuLiangLv '预估良率', jiHuaHeGeShu '计划合格数', shiJiChanChu '实际产出WAFER', shiJiCPChanChu '实际成品产出', 
                                shiJiHeGeShu '实际合格数', shiJiLiangLv '实际良率', jiHuaDaChengLv '计划达成率%', 
                                gongXuLiangLv '工序良率%', biaoZhunJiShi '标准机时', jiHuaJiShi '计划机时H', changChuJiShi '产出机时H', shiJiJiShi '实际机时H', 
                                biaoZhunRGS '标准人工时', jiHuaRenShi '计划人时H',changShuRenShi '产出人时H', shiJiRenShi '实际人时H', 
                                wuLiaoDaoDaHour '工单物料到达本工序时间', wuLiaoZhuanYiHour '工单物料转移本工序时间', NGReason '未达成原因'
                                from [dbo].[EP_ProductionPlan] where [date] between '{dateBegin}' and '{dateEnd}'";
            DataTable dt = SqlHelper<ProductionPlan>.sqlTable(sqlCmd);
            return dt;
        }
    }
}