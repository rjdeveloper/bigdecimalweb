using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wizz.Models
{
    public class PushModel
    {
    }
    public class PushMessage
    {
        public string message { get; set; }
        public string type { get; set; }
        //   public string registration_ids { get; set; }
        //    public string notificationId { get; set; }

    }
    public class PushData
    {
        public PushData() {
            this.uniqueRequestid = "";
            this.sessionId = "";
            this.amount = "";
            this.userId = "";
            this.subject = "";
            this.messageType = "";
            this.tutorId = "";
        }
        public string tutorId { get; set; }
        public string registration_ids { get; set; }
        public string uniqueRequestid { get; set; }
        public string sessionId { get; set; }
        public string userId { get; set; }
        public string amount { get; set; }
        public string type { get; set; }
        public string message { get; set; }
        public string messageType { get; set; }
        public string subject { get; set; }
        public string badge { get; set; }
     //   public PushResponseModel data { get; set; }
    }
    public class PushResponseModel {
        public string sessionId { get; set; }
    
    }
    enum PushType
    {
        newRequest = 1,//tutor end-when any new request wud be assigned to a tutor
        acceptRequest = 2,//student end- whenever tutor accepts the request 

        rejectRequest=3,
        sessionStart=4,// student end- when tutor starts the session
        cancelRequest=5,
        sessionEnd=6,// student end - when tutor ends the session
        approveTutor=7,// tutor end- when tutor will be approved by admin
        chron=8,// tutor end- if any request (not of today) is neither accepted nor rejected by tutor...
        chatPush = 9,

    }

    public class PushMessages
    {
        public const string approveSubject = "Your subjects have been approved by admin";
        public const string acceptRequest = "{0} has accepted your request";
        public const string rejectRequest = "Tutor has rejected your request";
        public const string newRequest = "A new student has sent request.";
        public const string cancelRequest = "Student has canceled the request";
        public const string sessionStart = "Tutor has started session";
        public const string sessionEnd = "Tutor has finished session";
        public const string cron = "A new student has sent request.";
        public static string FormatMessage(string message, params string[] args)
        {
            return String.Format(message, args);
        }
    }

}