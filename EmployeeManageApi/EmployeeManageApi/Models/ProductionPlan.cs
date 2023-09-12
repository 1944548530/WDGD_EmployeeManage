using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManageApi.Models
{
    public class ProductionPlan
    {
        public string date { get; set; }
        public string line { get; set; }
        /// <summary>
        /// 工序
        /// </summary>
        public string gongXu { get; set; }
        /// <summary>
        /// 工单
        /// </summary>
        public string gongDan { get; set; }
        /// <summary>
        /// 料号
        /// </summary>
        public string liaoHao { get; set; }
        /// <summary>
        /// 品名
        /// </summary>
        public string pinMing { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string guiGe { get; set; }
        /// <summary>
        /// 日计划投入WAFER
        /// </summary>
        public int riJiHuaTouRu { get; set; }
        /// <summary>
        /// 单片理论数
        /// </summary>
        public int danPianLiLunShu { get; set; }
        /// <summary>
        /// 计划成品产出
        /// </summary>
        public int jiHuaCPCC { get; set; }
        /// <summary>
        /// 预估良率
        /// </summary>
        public string yuGuLiangLv { get; set; }
        /// <summary>
        /// 计划合格数
        /// </summary>
        public int jiHuaHeGeShu { get; set; }
        /// <summary>
        /// 实际产出WAFER
        /// </summary>
        public int shiJiChanChu { get; set; }
        /// <summary>
        /// 实际成品产出
        /// </summary>
        public int shiJiCPChanChu { get; set; }
        /// <summary>
        /// 实际合格数
        /// </summary>
        public int shiJiHeGeShu { get; set; }
        /// <summary>
        /// 实际良率
        /// </summary>
        public string shiJiLiangLv { get; set; }
        /// <summary>
        /// 计划达成率%
        /// </summary>
        public string jiHuaDaChengLv { get; set; }
        /// <summary>
        /// 工序良率%
        /// </summary>
        public string gongXuLiangLv { get; set; }
        /// <summary>
        /// 标准机时
        /// </summary>
        public float biaoZhunJiShi { get; set; }
        /// <summary>
        /// 计划机时H
        /// </summary>
        public float jiHuaJiShi { get; set; }
        /// <summary>
        /// 产出机时H
        /// </summary>
        public float changChuJiShi { get; set; }
        /// <summary>
        /// 实际机时H
        /// </summary>
        public float shiJiJiShi { get; set; }
        /// <summary>
        /// 标准人工时
        /// </summary>
        public float biaoZhunRGS { get; set; }
        /// <summary>
        /// 计划人时H
        /// </summary>
        public float jiHuaRenShi { get; set; }
        /// <summary>
        /// 产出人时H
        /// </summary>
        public float changShuRenShi { get; set; }
        /// <summary>
        /// 实际人时H
        /// </summary>
        public float shiJiRenShi { get; set; }
        /// <summary>
        /// 工单物料到达本工序时间
        /// </summary>
        public float wuLiaoDaoDaHour { get; set; }

        ///工单物料转移本工序时间
        public float wuLiaoZhuanYiHour { get; set; }
        /// <summary>
        /// 未达成原因
        /// </summary>
        public string NGReason { get; set; }
        public string lmstate { get; set; }
        public string lmUser { get; set; }
    }
}