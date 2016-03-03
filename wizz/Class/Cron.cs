using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using wizz.Class;
using wizz.Models;

namespace wizz.Dal
{
    public class Cron
    {
        ///this method will called once in a day
        internal void ChorneJob()
        {
         
            using (var db = new WizzDataContext())
            {
                var tutorList = db.usp_GetPendingRequest().ToList();

                if(tutorList!=null&&tutorList.Count>0)
                {
                    foreach(var item in tutorList )
                    {
                        if(!string.IsNullOrEmpty(item.devicetoken))
                        {
                            PushData push = new PushData();
                            push.registration_ids = item.devicetoken;
                            push.badge = Convert.ToString(item.badge);
                            push.message = PushMessages.cron;
                            if(item.devicetype==Convert.ToInt16(DeviceType.ios))
                            {
                               SendPush objIOS = new SendPush();
                                objIOS.ConnectToAPNS(push);
                              
                            }
                            else if (item.devicetype == Convert.ToInt16(DeviceType.android))//android
                            {
                                Android objAndroid = new Android();
                                objAndroid.send(push);

                            }


                        }
                    }
                }

                
            }
           
        }
    }
}