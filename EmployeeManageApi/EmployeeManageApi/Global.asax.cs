using EmployeeManageApi.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace EmployeeManageApi
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected void Application_BeginRequest()
        {
            /*Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;  filename=Excel.xlsx");*/
            //OPTIONS���󷽷�����Ҫ���ã�Ԥ�����ж��Ƿ��ܹ�����ɹ�����
            //�����������������ܡ��磺AJAX���п�������ʱ��Ԥ�죬��Ҫ������һ����������Դ����һ��HTTP OPTIONS����ͷ�������ж�ʵ�ʷ��͵������Ƿ�ȫ��
            if (Request.Headers.AllKeys.Contains("Origin") && Request.HttpMethod == "OPTIONS")
            {
                //��ʾ����������ݽ��л��壬ִ��page.Response.Flush()ʱ������������ݻ�����ϣ������ݷ��͵��ͻ��ˡ�
                //�����Ͳ���������ҳ�濨��״̬�����û������Ƶĵ���ȥ
                Response.Flush();
            }

        }
    }
}
