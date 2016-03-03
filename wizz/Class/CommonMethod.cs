using wizz.Models;
//using GoogleMaps.LocationServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web;
using System.Configuration;
using System.Web.Script.Serialization;
using Twilio;

namespace wizz.Class
{
    public class CommonMethod
    {
        public static string GetBaseUrl(bool trailingSlash = true)
        {
            string sBaseUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath;
            sBaseUrl = sBaseUrl.TrimEnd(new char[] { '/' });
            if (trailingSlash)
                sBaseUrl = sBaseUrl + "/";
            return sBaseUrl;
        }

        public static string ObjectToJson(Object obj)
        { 
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(obj);

        }

        public static string ToLongString(double input)
        {
            string str = input.ToString().ToUpper();

            // if string representation was collapsed from scientific notation, just return it:
            if (!str.Contains("E")) return str;

            bool negativeNumber = false;

            if (str[0] == '-')
            {
                str = str.Remove(0, 1);
                negativeNumber = true;
            }

            string sep = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            char decSeparator = sep.ToCharArray()[0];

            string[] exponentParts = str.Split('E');
            string[] decimalParts = exponentParts[0].Split(decSeparator);

            // fix missing decimal point:
            if (decimalParts.Length == 1) decimalParts = new string[] { exponentParts[0], "0" };

            int exponentValue = int.Parse(exponentParts[1]);

            string newNumber = decimalParts[0] + decimalParts[1];

            string result;

            if (exponentValue > 0)
            {
                result =
                    newNumber +
                    GetZeros(exponentValue - decimalParts[1].Length);
            }
            else // negative exponent
            {
                result =
                    "0" +
                    decSeparator +
                    GetZeros(exponentValue + decimalParts[0].Length) +
                    newNumber;

                result = result.TrimEnd('0');
            }

            if (negativeNumber)
                result = "-" + result;

            return result;
        }

        private static string GetZeros(int zeroCount)
        {
            if (zeroCount < 0)
                zeroCount = Math.Abs(zeroCount);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < zeroCount; i++) sb.Append("0");

            return sb.ToString();
        }

        public static string ReadEmailformats(string Filename)
        {
            //ConfigurationManager.AppSettings["ImagePath"] + UniqueFileName
            StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/HtmlTemplates/" + Filename + ""));
            string readFile = reader.ReadToEnd();
            string strEmailBody = "";
            strEmailBody = readFile;
            // strEmailBody = strEmailBody.Replace("$$user$$", username);
            return strEmailBody.ToString();
        }

      

    }
    public class Common
    {
        //<Coder>Mukti</Coder>
        //<Date>12/23/2014</Date>
        //<Purpose>To Send mail through SMTP server</Purpose>


        public Bitmap CropImage(string Imagename, System.Drawing.Image originalImage, Rectangle sourceRectangle,
   Rectangle? destinationRectangle = null)
        {
            if (destinationRectangle == null)
            {
                destinationRectangle = new Rectangle(Point.Empty, sourceRectangle.Size);
            }

            var croppedImage = new Bitmap(destinationRectangle.Value.Width,
                destinationRectangle.Value.Height);
            using (var graphics = Graphics.FromImage(croppedImage))
            {
                graphics.DrawImage(originalImage, destinationRectangle.Value,
                    sourceRectangle, GraphicsUnit.Pixel);
            }
            croppedImage.Save(System.Web.HttpContext.Current.Server.MapPath("~/NewImages/" + Imagename + ""));
            return croppedImage;
        }

        public static string Sendmail(string emailto, string subject, string body)
        {
            string response = "";
            try
            {
                MailMessage message;
                message = new MailMessage();//emailfrom, emailto);
                message.From = new MailAddress(ConfigurationManager.AppSettings["AdminEmail"], ConfigurationManager.AppSettings["AdminName"]);
                message.To.Add(emailto);
                message.Body = body;
                message.Subject = subject;
                message.Priority = MailPriority.Normal;
                SmtpClient client = new SmtpClient();
                message.IsBodyHtml = true;
                client.Send(message);
                response = "Email sent successfully.";
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return response;

        }
    }


    public class twiliorest
    {
        public bool SendTeilioMessage(string MsgTo, string Msg)
        {
            string AccountSid = ConfigurationManager.AppSettings["MsgAccountSid"];
            string AuthToken = ConfigurationManager.AppSettings["MsgAuthToken"];
            string MsgFrom = ConfigurationManager.AppSettings["MsgFrom"];

            string CountryCode = ConfigurationManager.AppSettings["CountryCode"];
            var twilio = new TwilioRestClient(AccountSid, AuthToken);
            var message = twilio.SendMessage(MsgFrom, MsgTo.Trim(), Msg, "");
            //var message = twilio.SendMessage("+12015618404", "+919540707348", "testing", "");
            return true;
        }

        //public void makeACall()
        //{
        //    string AccountSid = ConfigurationManager.AppSettings["MsgAccountSid"];
        //    string AuthToken = ConfigurationManager.AppSettings["MsgAuthToken"];
        //    var twilio = new TwilioRestClient(AccountSid, AuthToken);

        //    // Build the parameters 
        //    var options = new CallOptions();
        //    options.To = "+918527655596";
        //    options.From = "+919958360711";
        //    options.Url = "http://www.justdelivr.com/";
        //    options.Method = "GET";
        //    options.FallbackMethod = "GET";
        //    options.StatusCallbackMethod = "GET";
        //    options.Record = false;

        //    var call = twilio.InitiateOutboundCall(options);
        //    Console.WriteLine(call.Sid);
        //}
    }
}