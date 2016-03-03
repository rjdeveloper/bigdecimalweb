using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace wizz.Class
{
    public class Constants
    {
        public static string imagepath = ConfigurationManager.AppSettings["imageurl"];
        public static string tutorDocs = ConfigurationManager.AppSettings["tutorDocs"];
        
        public static string SiteUrl = ConfigurationManager.AppSettings["SiteUrl"];
        public static string AdminEmail = ConfigurationManager.AppSettings["AdminEmail"];
        public static string isProduction = ConfigurationManager.AppSettings["isProduction"];
        public static string AdminName = ConfigurationManager.AppSettings["AdminName"];
        public static string RequestTime = ConfigurationManager.AppSettings["RequestTime"];
        public const string directoryName = "~/FileUpload/";
        public const string value = "Value";

        public static  string stripeSecretKey = ConfigurationManager.AppSettings["stripeSecretKey"];
        public static  string stripeLiveKey = ConfigurationManager.AppSettings["stripeLiveKey"];
                
    }
}