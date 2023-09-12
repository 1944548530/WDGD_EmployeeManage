using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Email
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // 创建一个 SmtpClient 实例
                SmtpClient smtpClient = new SmtpClient("smtp.qq.com"); // 你的 SMTP 服务器地址

                // 设置 SMTP 服务器的端口号和凭据
                smtpClient.Port = 587; // 通常为 587 或 465
                smtpClient.Credentials = new NetworkCredential("1944548530@qq.com", "bhwpldcmhkeebjed");

                // 创建一个 MailMessage 实例
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("1944548530@qq.com"); // 发件人邮箱
                mailMessage.To.Add("1944548530@qq.com"); // 收件人邮箱
                mailMessage.Subject = "邮件主题";
                mailMessage.Body = "测试邮件";

                // 发送邮件
                smtpClient.Send(mailMessage);

                Console.WriteLine("邮件发送成功！");
            }
            catch (SmtpException ex) {
                Console.WriteLine("SMTP错误：" + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("邮件发送失败: " + ex.Message);
            }
        }
    }
}
