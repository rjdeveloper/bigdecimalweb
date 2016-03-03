using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace wizz.Class
{
    public class SendMail
    {
        public string SendEmail(string emailto, string fromEmail, string fromName, string subject, string body)
        {
            string retVal = "";
            try
            {
                MailMessage message;
                message = new MailMessage();
                message.To.Add(emailto);
                //message.Bcc.Add("mukti@techaheadcorp.com");
                message.From = new MailAddress(fromEmail, fromName);
                message.Subject = subject;
                //message.Body = "Hi, <br/> This mail is generated from ShowingPad. <br/><br/> Thanks";
                message.Body = body;
                message.Priority = MailPriority.Normal;
                SmtpClient client = new SmtpClient();
                message.IsBodyHtml = true;
                client.Send(message);
                retVal = "Email sent successfully";
            }
            catch (Exception ex)
            {
                retVal = ex.Message;// +"\n\n" + ex.StackTrace;
            }
            return retVal;
        }


    }
}