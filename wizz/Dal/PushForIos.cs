using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using wizz.Models;

namespace wizz.Dal
{
    public class PushForIos
    {
        #region IosEndPrivatePush
        public string SendPushForIos(IosPushModel objPush)
        {
            try
            { // msg = "order confirmed";
                WebRequest tRequest;
                tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/x-www-form-urlencoded";
                string resistrationId = objPush.deviceToken;
                string subjectName = objPush.subjectName;
                var key = "";

                key = ConfigurationManager.AppSettings["androidPushKey"];

                tRequest.Headers.Add(string.Format("Authorization: key={0}", key));
                String collaspeKey = Guid.NewGuid().ToString("n");

                //   string messagetosend = // "{ \"aps\" :{ \"alert\" : \""+ objPush.message+"\" }, \"type\" : \""+objPush.type+"\"}";
                // string messagetosend = "{ \"aps\" :{ \"alert\" : \"" + objPush.message + "\" , \"type\" : \"" + objPush.type + "\", \"sessionId\" : \"" + objPush.sessionId + "\"}}";
                string messagetosend = "{\"aps\":{\"alert\":\"" + objPush.senderName + " " + objPush.Message + "\",\"badge\":\"1\",\"sound\":\"default\",\"type\":\"" + objPush.Type + "\",\"subjectName\":\"" + objPush.subjectName + "\",\"senderId\":\"" + objPush.senderId + "\",\"messageId\":\"" + objPush.messageId + "\"}}";

                // "{\"aps\":{\"alert\":\"" + "hi" + "\",\"badge\":" +0.ToString() + ",\"sound\":\"default\",\"Tag\":\"" + "1" + "\"}}";  ;

                String postData = string.Format("registration_id={0}&data.payload={1}&collapse_key={2}&sessionId={3}&amount={3}", resistrationId, "" + messagetosend, collaspeKey, objPush.subjectName, objPush.senderId);
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
                return sResponseFromServer;
            }
            catch (Exception ex)
            { return null; }


        }
        #endregion

        SslStream sslStream;
        public bool NewIosPush(IosPushModel objPush)
        {
            try
            {
                string p12Certificate = "WizzDevelopement.p12";
#if DEBUG
                //   p12Certificate = "WizzProductioncert.p12";
#else
                 p12Certificate = "WizzProductioncert.p12";
#endif

                string p12fileName = p12Certificate;

                string password = "123456";


                X509Certificate2Collection certs = new X509Certificate2Collection();

                // Add the Apple cert to our collection
                certs.Add(getServerCert(p12fileName, password));

                string apsHost;

                if (getServerCert(p12fileName, password).ToString().Contains("Production"))
                    apsHost = "gateway.push.apple.com";
                // Port= Convert.ToInt32("2195")
                else
                    apsHost = "gateway.sandbox.push.apple.com";
                TcpClient tcpClient = new TcpClient(apsHost, 2195);
                //  Connect();
                // Create a TCP socket connection to the Apple server on port 2195

                // Create a new SSL stream over the connection
                sslStream = new SslStream(tcpClient.GetStream());

                // Authenticate using the Apple cert
                sslStream.AuthenticateAsClient(apsHost, certs, SslProtocols.Default, false);

                //6455903fb51593b5fb930ab8184e39c5ec83b57503e6589596d463ddb4b1d835


                if (objPush.Message.Length < 256)
                {
                    Boolean send = PushIosMessage(objPush);
                    return send;
                }
                else
                {
                    throw new Exception("Message not exeed 256 character");

                }


            }
            catch (Exception ex)
            {
                return false;
                // throw new Exception(ex.Message);
            }
        }

        private bool PushIosMessage(IosPushModel objPush)
        {
            try
            {
                ;
                // String cAlert = objPush.data;
                int iBadge = 0;


                // Ready to create the push notification
                byte[] buf = new byte[256];
                MemoryStream ms = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write(new byte[] { 0, 0, 32 });

                byte[] deviceToken = HexToData(objPush.deviceToken);
                bw.Write(deviceToken);

                bw.Write((byte)0);

                // Create the APNS payload - new.caf is an audio file saved in the application bundle on the device
                //string msg = "{\"aps\":{\"alert\":\"" + cAlert + "\",\"badge\":" + iBadge.ToString() + ",\"sound\":\"default\",\"Tag\":\"" + tag + "\"}}";
                //  string msg = "{\"aps\":{\"alert\":\"" + objPush.message + "\",\"badge\":\"1\",\"sound\":\"default\",\"type\":" + objPush.type + ",\"sessionId\":" + objPush.sessionId + ",\"userId\":" + objPush.userId + ",\"amount\":" + objPush.amount + "}}";

                string msg = "{\"aps\":{\"alert\":\"" + objPush.senderName + " " + objPush.Message + "\",\"badge\":\"" + objPush.badge + "\",\"sound\":\"default\",\"type\":\"" + objPush.Type + "\",\"messageId\":\"" + objPush.messageId + "\",\"senderId\":\"" + objPush.senderId + "\",\"subjectName\":\"" + objPush.subjectName + "\"}}";

                // Write the data out to the stream
                bw.Write((byte)msg.Length);
                bw.Write(msg.ToCharArray());
                bw.Flush();

                if (sslStream != null)
                {
                    sslStream.Write(ms.ToArray());
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private X509Certificate getServerCert(string p12File, string password)
        {
            try
            {
                X509Certificate test = new X509Certificate();

                string p12Filename = HttpContext.Current.Server.MapPath("~/" + p12File);
                test = new X509Certificate2(System.IO.File.ReadAllBytes(p12Filename), password, X509KeyStorageFlags.MachineKeySet |
X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);

                return test;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static byte[] HexToData(string hexString)
        {
            try
            {
                if (hexString == null)
                    return null;

                if (hexString.Length % 2 == 1)
                    hexString = '0' + hexString; // Up to you whether to pad the first or last byte

                byte[] data = new byte[hexString.Length / 2];

                for (int i = 0; i < data.Length; i++)
                    data[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);

                return data;
            }
            catch
            {
                throw new Exception("Token not in correct format.");
            }
        }

    }
}