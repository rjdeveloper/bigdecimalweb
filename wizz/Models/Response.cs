using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace wizz.Models
{
    ///<Summary>
    /// Response
    ///</Summary>

    [Serializable]
    public class Response<T>
    {
        public bool Success;

        public string Message;
        public string AppVersion;
        public List<T> Result;

        public void Create(bool success, string message, string appversion, List<T> result)
        {
            this.Success = success;
            this.Message = message;
            this.AppVersion = appversion;
            this.Result = result;
        }
    }
    [Serializable]
    public class SpecialResponse<T>
    {
        public bool Success;

        public string Message;
        public string uniqueRequestid;

        public string AppVersion;
        public List<T> Result;

        public void Create(bool success, string message, string uniqueRequestid, string appversion, List<T> result)
        {
            this.Success = success;
            this.Message = message;
            this.uniqueRequestid = uniqueRequestid;
            this.AppVersion = appversion;
            this.Result = result;
        }
    }
    [Serializable]
    public class ResponseForAdmin<T>
    {
        public bool Success;

        public string Message;
        public string AppVersion;
        public T Result;

        public void ResponseForPost(bool success, string message)
        {
            this.Success = success;
            this.Message = message;
        }
        public void ResponseForGet(bool success, string message, T result)
        {
            this.Success = success;
            this.Message = message;
            this.Result = result;
        }
    }

    public class Messages
    {
        public const string AppVersion = "1.0";
        public const string InvalidReq = "Invalid Request";
        public const string InvalidParameters = "Invalid parameters for this request";
        public const string InvalidUser = "User does not exist";
        public const string NotAllowedUser = "You are not allowed to login!";
        public const string Success = "{0} successfully.";
        public const string ErrorOccure = "Error Occurred.";
        public const string ForgotPassword = "An email is headed your way! we'll send a message to your email{0} so you can pick your new password.";
        public const string EmailVerify = "Please verify your email address";
        public const string EmailExist = "Email already exists";
        public const string NoEmailExist = "The Email Id is not registered with us.Please enter a valid Email Id.";
        public const string InvalidPassword = "The password you entered is incorrect,please enter a valid password.";
        public const string NoRecord = "No record found.";
        public const string Already = "Already exist.";
        public const string MailSent = "We've send you an email to reset your password.";
        public const string AccountCreated = "Account created Succefully";
        public const string WebError = "Error occured while Processing Webservice request :{0}";
        public const string EmailAvailable = "This email is available";
        public const string InvalidOtp = "The entered code is not valid";
        public static string FormatMessage(string message, params string[] args)
        {
            return String.Format(message, args);
        }

        public const string InvalidFormat = "The file you have uploaded is in invalid format";
        public const string SuccessMessageForFileUpload = "File is successfully uploaded";

    }



    public class Entity
    {
        public string userId { get; set; }
        /// <summary>
        /// required in case of gethistory api
        /// </summary>
      public string isTutor { get; set; }
        // public string uniqueRequestId { get; set; }
    }

    public class SessionHistoryRequestModel
    {
        [Required]
        public string userId { get; set; }
     
        public string sessionId { get; set; }
        //   public string isTutor { get; set; }
        [Required]
        public string uniqueRequestId { get; set; }
    }

    public class RequestEntity
    {
        /// <summary>
        /// The requestid of the requests
        /// </summary>

        public string requestId { get; set; }
        [Required]
        public string userId { get; set; }

        public string isTutor { get; set; }

    }
    public class OtpModel
    {
        [Required]
        public string userId { get; set; }
        [Required]
        public string phoneNum { get; set; }

        /// <summary>
        /// Compulsary in case of verify otp
        /// </summary>
        public string OTPCode { get; set; }

        /// <summary>
        /// compulsary in case of send otp
        /// </summary>
        public string isResend { get; set; }
    }


    public class TutorAvailbilityModel
    {
        [Required]
        public string userId { get; set; }
        [Required]
        public string isAvailable { get; set; }

    }
    public class StudentTeacherMap
    {
        [Required]
        public string tutorId { get; set; }
        [Required]
        public string studentId { get; set; }
        [Required]
        public string isAccept { get; set; }
        [Required]
        public string uniqueRequestId { get; set; }
    }
    public class ReportSpamModel
    {
        [Required]
        public string fromId { get; set; }
        [Required]
        public string toId { get; set; }
        [Required]
        public string forUserType { get; set; }
    }
    enum DeviceType
    {
        ios = 1,
        android = 2,

    }

    public class ReqLatLong
    {
        public string email { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string type { get; set; }

    }

    public class FriendsListModel
    {

        public string friendId { get; set; }
        public string friendName { get; set; }


        public string amount { get; set; }



        public string profilePic { get; set; }
        public string isConfirm { get; set; }
    }

    public class InviteFriendsModel
    {
        public string userId { get; set; }
        public List<FriendsList> Friends { get; set; }

        /// <summary>
        /// It is sessionId now on only send the session id (a unique id which is in integer form)
        /// </summary>
        /// 
        [Required]
        public string sessionId { get; set; }

        /// <summary>
        /// not use full at the time of sending requests
        /// </summary>
        public string isConfirm { get; set; }

    }
    public class PaymentModel
    {
        public string userId { get; set; }
        public string paymentType { get; set; }

    }
    public enum PaymentType
    {
        cash = 1,
        wallet,
        card

    }
    public class FriendsList
    {
        public string userName { get; set; }
        public string phoneNumber { get; set; }
        public string profilePic { get; set; }
    }

}