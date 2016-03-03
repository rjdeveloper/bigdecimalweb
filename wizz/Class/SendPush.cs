using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Configuration;
using wizz.Models;

namespace wizz.Class
{
    public class SendPush
    {
        //TcpClient apnsClient;
        SslStream sslStream;

        public bool ConnectToAPNS(PushData objPush)
        {
            try
            {
                string p12Certificate = "WizzDevelopement.p12";
#if DEBUG
                //   p12Certificate = "WizzProductioncert.p12";
#else
                 p12Certificate = "WizzProductioncert.p12";
#endif

                string p12fileName = p12Certificate; string password = "123456";


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

                Int32 messa = objPush.message.Length;
                if (messa < 256)
                {
                    Boolean send = PushMessage(objPush);
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
                throw new Exception("Token not in correct formate.");
            }
        }

        public bool PushMessage(PushData objPush)
        {
            try
            {
                String cToken = objPush.registration_ids;
                // String cAlert = objPush.data;
                Int16 iBadge = 0;

               using(var db=new WizzDataContext())
               {

                   Int16 bedge = 0;
                   try
                   {
                       bedge = Convert.ToInt16(db.tblUsers.FirstOrDefault(S => S.deviceToken == cToken).badge);
                   }
                   catch { bedge = 0; }
                   if (bedge == 0)
                   {
                       iBadge = 1;
                   }
                   else
                   {
                       tblUser objuser = new tblUser();
                       objuser = db.tblUsers.First(S => S.deviceToken == cToken);
                       if (objuser != null)
                       {
                           Int16 addbedge = Convert.ToInt16(objuser.badge + 1);
                           iBadge = addbedge;
                       }
                   }

               }

               

                // Ready to create the push notification
                byte[] buf = new byte[256];
                MemoryStream ms = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write(new byte[] { 0, 0, 32 });

                byte[] deviceToken = HexToData(cToken);
                bw.Write(deviceToken);

                bw.Write((byte)0);

                // Create the APNS payload - new.caf is an audio file saved in the application bundle on the device
                //string msg = "{\"aps\":{\"alert\":\"" + cAlert + "\",\"badge\":" + iBadge.ToString() + ",\"sound\":\"default\",\"Tag\":\"" + tag + "\"}}";
              //  string msg = "{\"aps\":{\"alert\":\"" + objPush.message + "\",\"badge\":\"1\",\"sound\":\"default\",\"type\":" + objPush.type + ",\"sessionId\":" + objPush.sessionId + ",\"userId\":" + objPush.userId + ",\"amount\":" + objPush.amount + "}}";

                string msg = "{\"aps\":{\"alert\":\"" + objPush.message + "\",\"badge\":" + iBadge + ",\"sound\":\"default\",\"type\":\"" + objPush.type + "\",\"sessionId\":\"" + objPush.sessionId + "\",\"uniqueRequestId\":\"" + objPush.uniqueRequestid + "\",\"userId\":\"" + objPush.userId + "\",\"tutorId\":\"" + objPush.tutorId + "\",\"amount\":\"" + objPush.amount + "\",\"subject\":\"" + objPush.subject + "\",\"messageType\":\"" + objPush.messageType + "\"}}";

                // Write the data out to the stream
                bw.Write((byte)msg.Length);
                bw.Write(msg.ToCharArray());
                bw.Flush();

                if (sslStream != null)
                {
                    sslStream.Write(ms.ToArray());
                    tblUser objuser = new tblUser();
                    using (var db = new WizzDataContext())
                    {
                        objuser = db.tblUsers.FirstOrDefault(S => S.deviceToken == cToken);
                        if (objuser != null)
                        {
                            objuser.badge = iBadge;
                            db.SubmitChanges();
                        }
                    }
                  
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}