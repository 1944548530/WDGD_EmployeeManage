using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManageApi.ResponseModel
{
    public class Response
    {

        public Response()
        {
            code = 200;
            message = "调用成功";
        }

        //响应代码
        public int code { get; set; }
        //响应消息内容
        public string message { get; set; }
        //响应的数据
        public object Data { get; set; }

        /// <summary>
        /// 设置响应状态为成功
        /// </summary>
        /// <param name="mes"></param>
        public void SetSuccess(string mes)
        {
            code = 200;
            message = mes;
        }

        /// <summary>
        /// 设置响应状态为失败
        /// </summary>
        /// <param name="mes"></param>
        public void SetFailed(string mes)
        {
            code = 400;
            message = mes;
        }

        /// <summary>
        /// 设置响应数据
        /// </summary>
        /// <param name="data"></param>
        public void SetData(object data)
        {
            Data = data;
        }
    }
}