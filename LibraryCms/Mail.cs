using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace LibraryCms
{
    public static class Mail
    {
        private const string Sender = "admin@opom.cn";
        private const string Password = "aBcD1234";
        private const string Host = "smtp.opom.cn";

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailTo">收信人</param>
        /// <param name="subject">主题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="isHtmlBody">是否是Html格式的内容</param>
        /// <returns></returns>
        public static bool SnedMsgTo(string mailTo, string subject, string body, bool isHtmlBody = false)
        {
            MailMessage msg = new MailMessage();
            msg.To.Add(mailTo);
            msg.From = new MailAddress(Sender);
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            msg.Subject = subject;
            msg.IsBodyHtml = isHtmlBody;
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.Body = body;
            msg.Priority = MailPriority.Normal;
            SmtpClient client = new SmtpClient(Host) { Credentials = new System.Net.NetworkCredential(Sender, Password) };
            try
            {
                client.Send(msg);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

    }
}