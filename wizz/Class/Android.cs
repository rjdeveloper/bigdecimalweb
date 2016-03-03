using System.Net;
using System.Text;
using System.IO;
using System.Collections;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;
using wizz.Models;

namespace wizz.Class
{
    //Start: #1
    public class Android
    {

        /// <summary>
        /// Check authentication with supplied credential
        /// </summary>
        /// <param name="SenderID">Google EmailID</param>
        /// <param name="Password">Password of EmailID</param>
        /// <returns></returns>
        public string CheckAuthentication(string SenderID, string Password)
        {
            string Array = "";

            string URL = "https://www.google.com/accounts/ClientLogin?";
            string fullURL = URL + "Email=" + SenderID.Trim() + "&Passwd=" + Password.Trim() + "&accountType=GOOGLE" + "&source=Company-App-Version" + "&service=ac2dm";
            HttpWebRequest Request = (HttpWebRequest)HttpWebRequest.Create(fullURL);

            try
            {

                //-- Post Authentication URL --//
                HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();
                StreamReader Reader;
                int Index = 0;



                //-- Check Response Status --//
                if (Response.StatusCode == HttpStatusCode.OK)
                {
                    Stream Stream = Response.GetResponseStream();
                    Reader = new StreamReader(Stream);
                    string File = Reader.ReadToEnd();

                    Reader.Close();
                    Stream.Close();
                    Index = File.ToString().IndexOf("Auth=") + 5;
                    int len = File.Length - Index;
                    Array = File.ToString().Substring(Index, len);
                }

            }
            catch (Exception ex)
            {
                Array = ex.Message;
                ex = null;
            }

            return Array;
        }

        ///// <summary>
        ///// Send Push Message to Device
        ///// </summary>
        ///// <param name="RegistrationID">RegistrationID or Token</param>
        ///// <param name="Message">Message to be sent on device</param>
        ///// <param name="AuthString">Authentication string</param>
        ///// 
        //public string send(PushMessage objPush,string type)
        //{
        //    // msg = "order confirmed";
        //    WebRequest tRequest;
        //    tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
        //    tRequest.Method = "post";
        //    tRequest.ContentType = "application/x-www-form-urlencoded";
        //    string resistrationId = objPush.registration_ids;
        //    var key="";

        //    key=ConfigurationManager.AppSettings["androidPushKey"];               
           
        //    tRequest.Headers.Add(string.Format("Authorization: key={0}", key));
        //    String collaspeKey = Guid.NewGuid().ToString("n");
        //    string messagetosend = "{\"aps\":{\"alert\":\"" + objPush.message + "\",\"badge\":\"1\",\"sound\":\"default\",\"Tag\":" + objPush.type + ",\"notificationId\":" + objPush.notificationId  + "}}";
        //    String postData = string.Format("registration_id={0}&data.payload={1}&collapse_key={2}", resistrationId, "" + messagetosend, collaspeKey);
        //    Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
        //    tRequest.ContentLength = byteArray.Length;

        //    Stream dataStream = tRequest.GetRequestStream();
        //    dataStream.Write(byteArray, 0, byteArray.Length);
        //    dataStream.Close();

        //    WebResponse tResponse = tRequest.GetResponse();

        //    dataStream = tResponse.GetResponseStream();

        //    StreamReader tReader = new StreamReader(dataStream);

        //    String sResponseFromServer = tReader.ReadToEnd();

        //    tReader.Close();
        //    dataStream.Close();
        //    tResponse.Close();
        //    return sResponseFromServer;

        //}

        /// <summary>
        /// Send Push Message to Device
        /// </summary>
        /// <param name="RegistrationID">RegistrationID or Token</param>
        /// <param name="Message">Message to be sent on device</param>
        /// <param name="AuthString">Authentication string</param>
        /// 
        public string send(PushData objPush)
        {
            tblUser objuser;
            try
                 
            { // msg = "order confirmed";
                WebRequest tRequest;
                tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/x-www-form-urlencoded";
                string resistrationId = objPush.registration_ids;
                string sessionId = objPush.uniqueRequestid;
                var key = "";
                Int16 iBadge = 0;
               
                using (var db = new WizzDataContext())
                {

                    int bedge = 0;
                    try
                    {
                        bedge = Convert.ToInt32(db.tblUsers.FirstOrDefault(S => S.deviceToken == resistrationId).badge);
                    }
                    catch { bedge = 0; }
                    if (bedge == 0)
                    {
                        iBadge = 1;
                    }
                    else
                    {
                      
                        objuser = db.tblUsers.First(S => S.deviceToken == resistrationId);
                        if (objuser != null)
                        {
                            Int16 addbedge = Convert.ToInt16(objuser.badge + 1);
                            iBadge = addbedge;
                        }
                    }

                }
                

                key = ConfigurationManager.AppSettings["androidPushKey"];

                tRequest.Headers.Add(string.Format("Authorization: key={0}", key));
                String collaspeKey = Guid.NewGuid().ToString("n");


                string messagetosend = "{\"aps\":{\"alert\":\"" + objPush.message + "\",\"badge\":\"" + iBadge.ToString() + "\",\"sound\":\"default\",\"type\":\"" + objPush.type + "\",\"sessionId\":\"" + objPush.sessionId + "\",\"tutorId\":\"" + objPush.tutorId + "\",\"uniqueRequestid\":\"" + objPush.uniqueRequestid + "\",\"userId\":\"" + objPush.userId + "\",\"amount\":\"" + objPush.amount + "\"}}";                                 
                String postData = string.Format("registration_id={0}&data.payload={1}&collapse_key={2}&sessionId={3}&amount={3}", resistrationId, "" + messagetosend, collaspeKey, objPush.uniqueRequestid,objPush.amount);
                Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                tRequest.ContentLength = byteArray.Length;

                Stream dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse tResponse = tRequest.GetResponse();

                dataStream = tResponse.GetResponseStream();

                StreamReader tReader = new StreamReader(dataStream);

                String sResponseFromServer = tReader.ReadToEnd();

                tReader.Close();
                dataStream.Close();
                tResponse.Close();
               
                using (var db = new WizzDataContext())
                {
                    objuser = db.tblUsers.FirstOrDefault(S => S.deviceToken == objPush.registration_ids);
                    if (objuser != null)
                    {
                        objuser.badge = iBadge;
                        db.SubmitChanges();
                    }
                }

                return sResponseFromServer;
            }
            catch (Exception ex)
            { return null; }
           

        }
        public string SendMessage(string RegistrationID, PushMessage Message, string AuthString)
        {

            //-- Create C2DM Web Request Object --//
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://android.clients.google.com/c2dm/send");

            Request.Method = "POST";
            Request.KeepAlive = false;

            //-- Create Query String --//
            NameValueCollection postFieldNameValue = new NameValueCollection();
            postFieldNameValue.Add("registration_id", RegistrationID);
            postFieldNameValue.Add("collapse_key", "1");
            postFieldNameValue.Add("delay_while_idle", "0");
            // postFieldNameValue.Add("data.message", Message);
            postFieldNameValue.Add("data.payload", Message.message);
         //   postFieldNameValue.Add("type", Message.type);
            string postData = GetPostStringFrom(postFieldNameValue);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);


            Request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            Request.ContentLength = byteArray.Length;

            Request.Headers.Add(HttpRequestHeader.Authorization, "GoogleLogin auth=" + AuthString);
            //Request.Headers.Add(HttpRequestHeader.Authorization, "AIzaSyB-1uEai2WiUapxCs2Q0GZYzPu7Udno5aA");
          
            /*if multiple project */
            

            Request.Headers.Add(HttpRequestHeader.Authorization, "AIzaSyChZCTQ0seSaeTmgkC4zltqQDhGWCYyIXQ");
            //-- Delegate Modeling to Validate Server Certificate --//
            ServicePointManager.ServerCertificateValidationCallback += delegate(
                        object
                        sender,
                        System.Security.Cryptography.X509Certificates.X509Certificate
                        pCertificate,
                        System.Security.Cryptography.X509Certificates.X509Chain pChain,
                        System.Net.Security.SslPolicyErrors pSSLPolicyErrors)
            {
                return true;
            };

            //-- Create Stream to Write Byte Array --// 
            Stream dataStream = Request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            //-- Post a Message --//
            WebResponse Response = Request.GetResponse();
            HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
            if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
            {
                return "Unauthorized - need new token";

            }
            else if (!ResponseCode.Equals(HttpStatusCode.OK))
            {
                return "Response from web service isn't OK";
                //Console.WriteLine("Response from web service not OK :");
                //Console.WriteLine(((HttpWebResponse)Response).StatusDescription);
            }

            StreamReader Reader = new StreamReader(Response.GetResponseStream());
            string responseLine = Reader.ReadLine();
            Reader.Close();

            return responseLine;
        }

        public string SendMessageDemo(string RegistrationID, PushMessage Message, string AuthString)
        {
            //-- Create GCM Web Request Object --//
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://android.googleapis.com/gcm/send");
            Request.Method = "POST";
            Request.KeepAlive = false;
            Request.ContentType = "application/json";

            PushData msg = new PushData();
            msg.registration_ids = RegistrationID;
            msg.message = "testing";
            //message.data.Add("gcm_message", "12345");
            var jsonMessage = Newtonsoft.Json.JsonConvert.SerializeObject(msg);
            byte[] byteArray = Encoding.UTF8.GetBytes(jsonMessage);
            Request.ContentLength = byteArray.Length;


            Request.Headers.Add(HttpRequestHeader.Authorization, "AIzaSyChZCTQ0seSaeTmgkC4zltqQDhGWCYyIXQ");

            //-- Delegate Modeling to Validate Server Certificate --//
            ServicePointManager.ServerCertificateValidationCallback += delegate(
                        object
                        sender,
                        System.Security.Cryptography.X509Certificates.X509Certificate
                        pCertificate,
                        System.Security.Cryptography.X509Certificates.X509Chain pChain,
                        System.Net.Security.SslPolicyErrors pSSLPolicyErrors)
            {
                return true;
            };

            //-- Create Stream to Write Byte Array --// 
            Stream dataStream = Request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            //-- Post a Message --//
            WebResponse Response = Request.GetResponse();
            HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
            if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
            {
                return "Unauthorized - need new token";

            }
            else if (!ResponseCode.Equals(HttpStatusCode.OK))
            {
                return "Response from web service isn't OK";
            }

            StreamReader Reader = new StreamReader(Response.GetResponseStream());
            string responseLine = Reader.ReadLine();
            Reader.Close();

            return responseLine;
        }

        /// <summary>
        /// Create Query String From Name Value Pair
        /// </summary>
        /// <param name="postFieldNameValue"></param>
        /// <returns></returns>
        private string GetPostStringFrom(NameValueCollection postFieldNameValue)
        {
            //throw new NotImplementedException();
            List<string> items = new List<string>();

            foreach (String name in postFieldNameValue)
                items.Add(String.Concat(name, "=", System.Web.HttpUtility.UrlEncode(postFieldNameValue[name])));

            return String.Join("&", items.ToArray());
        }

        /// <summary>
        /// Validate Server Certificate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="policyErrors"></param>
        /// <returns></returns>
        private static bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }
      
    }
    //End: #1
}