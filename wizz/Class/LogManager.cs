using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using wizz.Models;

namespace wizz.Class
{
    public static class LogManager
    {

        enum enLogType
        {
            Info = 1,
            Error,
        }
        private static ILog _logger;
        static LogManager()
        {
            log4net.Config.XmlConfigurator.Configure();
            _logger = log4net.LogManager.GetLogger("Cacophony Log");
        }
        public static void Info(string messageFormat, params object[] args)
        {
            Info(messageFormat, (Exception)null, args);
        }
        public static void Info(string messageFormat, Exception ex, params object[] args)
        {
            _logger.Info(string.Format(messageFormat, args), ex);
            SaveLogInfo(enLogType.Info, messageFormat, ex, args);
        }

        //public static void Error(string messageFormat, Exception ex, params object[] args)
        //{
        //    Error(messageFormat, ex, args);
        //}
        public static void Error(string messageFormat, Exception ex, params object[] args)
        {
          //  _logger.Error(string.Format(messageFormat, args), ex);
            SaveLogInfo(enLogType.Error, messageFormat, ex, args);
        }

        private static void SaveLogInfo(enLogType logType, string messageFormat, Exception ex, params object[] args)
        {
            object stackTrace = DBNull.Value;
            object LoggedInUserId = DBNull.Value;
            object url = DBNull.Value;
            object refUrl = DBNull.Value;
            object session = DBNull.Value;
            object methodName = DBNull.Value;
            bool hasError = false;
            StringBuilder stackInfo = new StringBuilder();
            var current = HttpContext.Current;

            if (current != null && current.Request != null)
            {
                url = current.Request.Url.ToString();
                if (current.Request.UrlReferrer != null)
                    refUrl = current.Request.UrlReferrer.ToString();
            }

            string userid = HttpContext.Current.User.Identity.Name;
            if (string.IsNullOrEmpty(userid))
            {
              

                try
                {
                    session = new JavaScriptSerializer().Serialize(userid);
                }
                catch (Exception exSer)
                {
                    stackInfo.AppendLine("Session Values Serialization Error");
                    stackInfo.AppendLine(exSer.Message);
                    if (exSer.InnerException != null)
                    {
                        stackInfo.AppendLine();
                        stackInfo.AppendLine(exSer.InnerException.Message);
                        stackInfo.AppendLine();
                        stackInfo.AppendLine();
                    }
                }
            }
            if (ex != null)
            {
                hasError = true;
                stackInfo.AppendLine("Error Message:");
                stackInfo.AppendLine(ex.Message);
                stackInfo.AppendLine();

                stackInfo.AppendLine("Source:");
                stackInfo.AppendLine(ex.Source);
                stackInfo.AppendLine();

                stackInfo.AppendLine("Stack Trace:");
                stackInfo.AppendLine(ex.StackTrace);
                if (ex.InnerException != null)
                {
                    stackInfo.AppendLine();
                    stackInfo.AppendLine("Inner Exception");
                    stackInfo.AppendLine();

                    stackInfo.AppendLine("Error Message:");
                    stackInfo.AppendLine(ex.InnerException.Message);
                    stackInfo.AppendLine();

                    stackInfo.AppendLine("Source:");
                    stackInfo.AppendLine(ex.InnerException.Source);
                    stackInfo.AppendLine();

                    stackInfo.AppendLine("Stack Trace:");
                    stackInfo.AppendLine(ex.InnerException.StackTrace);
                }



            }
            if (hasError) stackTrace = stackInfo.ToString();

            try
            {
                var frames = new System.Diagnostics.StackTrace().GetFrames();
                System.Diagnostics.StackFrame frame = null;
                for (var i = 1; i < frames.Length; i++)
                {
                    if (frames[i].GetMethod().Name != "Info" && frames[i].GetMethod().Name != "Error")
                    {
                        frame = frames[i];
                        break;
                    }
                }
                if (frame != null)
                {
                    var stackMethod = frame.GetMethod();
                    methodName = stackMethod.ReflectedType.FullName + "." + stackMethod.Name;
                }
                using (var objDBContext = new WizzDataContext())
                {
                    objDBContext.usp_SaveLog((int)logType, Convert.ToString(messageFormat), Convert.ToString(stackTrace), Convert.ToString(methodName), 0, Convert.ToString(url), Convert.ToString(refUrl), Convert.ToString(session));
                    objDBContext.SubmitChanges();
                }
            }
            catch (Exception logEx)
            {
                _logger.Error("Error occurd while saving error log in database", logEx);
            }
           
        }
    }
}