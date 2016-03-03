using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wizz.Models
{
    public class UserModel
    {
        public string userId { get; set; }
        public string loginType { get; set; }
        public string socialId { get; set; }
        public string userName { get; set; }
        public string userEmail { get; set; }       
        public string password { get; set; }
        public string profilePic { get; set; }
        public string collageName { get; set; }
        public string description { get; set; }
       
        public string passingYear { get; set; }
        public string deviceType { get; set; }
        public string deviceToken { get; set; }  
         
      
    }
    public class ReqBadgeCounter
    {
        public string deviceToken { get; set; }
    }
    public class RespLogin
    {
       
        public string userId { get; set; }       
        public string userName { get; set; }
        public string userEmail { get; set; }
        public string profilePic { get; set; }
        public string badge { get; set; }
        public string isNotificationOn { get; set; }
        public string restKey { get; set; }
        public string isFirstimeLogin { get; set; }
        public string credits { get; set; }
        public string isTutor { get; set; }
        public string collageName { get; set; }
        public string description { get; set; }
        public string referralCode { get; set; }
        public string passingYear { get; set; }
        public string isOtpVerified { get; set; }
        public string paymentType { get; set; }
            
    }
    public class RespStripe {
        public string userId { get; set; }
        public string stripeKey { get; set; }
        public string stripeUserKey { get; set; }       
    }
    public class ReqEmail
    {
        public string userEmail { get; set; }    
    }

    public class ReqResetPassword : ReqEmail
    {
        public string password { get; set; }

    }

    public class ReqPassword
    {
        public string userEmail { get; set; }
        public string userId { get; set; }
        public string oldPassword { get; set; }
    }
}