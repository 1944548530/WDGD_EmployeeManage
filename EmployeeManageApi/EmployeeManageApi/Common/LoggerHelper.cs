using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;

namespace EmployeeManageApi.Common
{
    public class LoggerHelper
    {
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="ex">错误异常</param>
        /// <param name="info">异常信息</param>
        public static void WriteLog(Exception ex)
        {
            object o = ConfigurationManager.GetSection("log4net");
            log4net.Config.XmlConfigurator.Configure(o as System.Xml.XmlElement);
            ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            log.Error(ex.Message, ex);
        }
        /// <summary>
        /// 记录日志
        /// </summary>        
        /// <param name="info">要记录的信息</param>
        public static void WriteLog(string info)
        {
            object o = ConfigurationManager.GetSection("log4net");
            log4net.Config.XmlConfigurator.Configure(o as System.Xml.XmlElement);
            ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            log.Error(info);
        }
    }
}