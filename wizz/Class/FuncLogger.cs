using wizz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace wizz.Class
{
    public static class FuncLogger
    {
        //public static Response<T> Onerror<T>(Response<T> response,  value <T>, Exception ex)
        //{
        //    response.Create(false, Messages.FormatMessage(ex.Message), Messages.AppVersion, value);
        //    return response;
        //}
        public static T LogFunc<T>(string methodname, Func<T> func, Func<Exception, T> onerror, params object[] objreq) where T : class , new()
        {
            Response<T> response = new Response<T>();
            T value = new T();
            try
            {
                T returnval = func();
                return returnval;
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objreq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                return onerror(ex);
            }
        }
 
    }

}