using wizz.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Xml.Linq;
using System.Data.Entity.SqlServer;

namespace wizz.Class
{
    public class DBMethod
    {


        public static string stringMethod(string s)
        {
            return Convert.ToString(s.ToLower().Trim());

        }

        #region Mukti
        /// <summary>
        ///Send a verification email
        /// </summary>
        /// <author>Mukti</author>
        /// <Date>june 2, 2015</Date>
        #region SendVerifyEmail
        void SendVerifyEmail(string email, string type, Int64 id)
        {
            string Desctr = Encryption.EncryptURL(id.ToString() + "," + type);
            Desctr = Desctr.Replace("/", "_").Replace("+", "-");
            var sanitized = HttpUtility.UrlEncode(Desctr);
            string body = CommonMethod.ReadEmailformats("signup.html");
            string path = Constants.SiteUrl + "/Account/verifyEmail/" + sanitized;
            body = body.Replace("$$UserEmailId$$", email).Replace("$$url$$", Constants.SiteUrl);
            body = body.Replace("$$SignUpLink$$", "<a href='" + path + "'> Click here to verify your email </a>");
            SendMail objSendMail = new SendMail();
            objSendMail.SendEmail(email, Constants.AdminEmail, Constants.AdminName, "Account Created", body);
        }
        #endregion
        internal static string isReferralCodeExists(string userName)
        {

            string uniqueReferralCode = "WIZZ" + userName.Substring(0, 2) + System.Guid.NewGuid().ToString().Substring(0, 4);
            using (var db = new WizzDataContext())
            {
                if (db.tblUsers.Any(x => x.referralCode == uniqueReferralCode))
                {
                    return isReferralCodeExists(userName);

                }
                else
                {

                    return uniqueReferralCode;
                }

            }
        }

        internal RespLogin Custom(UserModel objRequest)
        {
            RespLogin response = new RespLogin();
            tblUser objUser = new tblUser();
            response.restKey = "0";
            if (objRequest.userEmail == null || objRequest.userEmail == "")
            {
                response.restKey = "6";
                return response;
            }
            try
            {
                using (var db = new WizzDataContext())
                {
                    //check normal login or social login
                    if (string.IsNullOrEmpty(objRequest.socialId))
                    {

                    }

                    else
                    {

                        objUser = db.tblUsers.Where(u => u.facebookId == objRequest.socialId).FirstOrDefault();
                        response.userName = Convert.ToString(objRequest.userName);
                        response.profilePic = string.IsNullOrWhiteSpace(objRequest.profilePic) ? "" : objRequest.profilePic.IndexOf("http") >= 0 ? objRequest.profilePic : Constants.imagepath + objRequest.profilePic;
                        //response.profilePic = objRequest.profilePic == null ? "" : objRequest.profilePic;
                        response.userEmail = objRequest.userEmail == null ? "" : objRequest.userEmail;

                        if (objUser == null)// new social id
                        {
                            objUser = db.tblUsers.Where(u => u.userEmail == objRequest.userEmail.ToLower().Trim()).FirstOrDefault();//get existing record against

                            if (objUser == null)// email if not exists
                            {
                                objUser = new tblUser();

                                objUser.createdDate = DateTime.UtcNow;
                                objUser.userName = objRequest.userName == null ? "" : objRequest.userName;
                                objUser.userEmail = objRequest.userEmail == null ? "" : objRequest.userEmail;
                                objUser.registerType = 2;
                                objUser.guid = Guid.NewGuid();
                                objUser.isActive = true;
                                objUser.isDelete = false;
                                objUser.password = Convert.ToString(Guid.NewGuid());
                                objUser.isNotificationOn = true;
                                objUser.referralCode = isReferralCodeExists(objUser.userName);
                                objUser.profilePic = string.IsNullOrWhiteSpace(objRequest.profilePic) ? "" : objRequest.profilePic.IndexOf("http") >= 0 ? objRequest.profilePic : Constants.imagepath + objRequest.profilePic;
                                //  objUser.profilePic = objRequest.profilePic == null ? "" : objRequest.profilePic;
                                objUser.badge = 0;

                                objUser.isFirstTimeLogin = true;
                                objUser.isTeacher = false;
                                objUser.credits = 0;
                                objUser.facebookId = objRequest.socialId.Trim();

                                db.tblUsers.InsertOnSubmit(objUser);
                                //creating a new jabber Id
                                db.SubmitChanges();
                                try
                                {
                                    user jabClient = new user();
                                    jabClient.created_at = DateTime.UtcNow;
                                    jabClient.password = objUser.pkUserId + "@localhost.com";
                                    jabClient.username = objUser.pkUserId.ToString();
                                    db.users.InsertOnSubmit(jabClient);
                                    db.SubmitChanges();
                                }
                                catch (Exception e)
                                {

                                }


                            }

                            objUser.updatedDate = DateTime.UtcNow;
                            objUser.deviceToken = objRequest.deviceToken == null ? "" : objRequest.deviceToken;
                            objUser.deviceType = Convert.ToInt16(objRequest.deviceType);
                            db.SubmitChanges();
                            response.userId = Convert.ToString(objUser.pkUserId);
                            response.restKey = "1";
                            response.credits = Convert.ToString(objUser.credits);
                            response.referralCode = objUser.referralCode;
                            response.isNotificationOn = objUser.isNotificationOn.ToString();
                            response.isFirstimeLogin = objUser.isFirstTimeLogin == null || objUser.isFirstTimeLogin == true ? "True" : "False";
                            response.isTutor = objUser.isTeacher == null || objUser.isTeacher == false ? "False" : "True";

                            response.badge = Convert.ToString(objUser.badge);

                        }
                        else if (objUser.isDelete == true || objUser.isActive == false)
                            response.restKey = "2";

                        else
                        {
                            response.restKey = "1";
                            response.userId = Convert.ToString(objUser.pkUserId);
                            response.userName = Convert.ToString(objUser.userName);
                            response.profilePic = string.IsNullOrWhiteSpace(objUser.profilePic) ? "" : objUser.profilePic.IndexOf("http") >= 0 ? objUser.profilePic : Constants.imagepath + objUser.profilePic;
                            //   response.profilePic = Constants.imagepath + (objUser.profilePic == null ? "" : objUser.profilePic);
                            response.userEmail = objUser.userEmail == null ? "" : objUser.userEmail;
                            response.isNotificationOn = objUser.isNotificationOn.ToString();
                            response.isFirstimeLogin = objUser.isFirstTimeLogin == null || objUser.isFirstTimeLogin == true ? "True" : "False";
                            response.isTutor = objUser.isTeacher == null || objUser.isTeacher == false ? "False" : "True";
                            response.credits = Convert.ToString(objUser.credits);
                            response.badge = Convert.ToString(objUser.badge);
                            response.referralCode = objUser.referralCode;


                        }
                    }

                    if (objUser != null && response.restKey == "1" && (objUser.isFirstTimeLogin == null || objUser.isFirstTimeLogin == true))
                    {
                        objUser.isFirstTimeLogin = false;
                        db.SubmitChanges();
                    }

                    if (!string.IsNullOrEmpty(objRequest.deviceToken))
                    {
                        var objTokenUser = db.tblUsers.Where(s => s.deviceToken == objRequest.deviceToken).FirstOrDefault();
                        if (objTokenUser != null)
                        {
                            objTokenUser.deviceToken = "";
                        }
                        objUser.deviceToken = objRequest.deviceToken;
                        objUser.deviceType = Convert.ToInt16(objRequest.deviceType);
                    }

                    db.SubmitChanges();
                    return response;
                }
            }
            catch
            {
                return response;
            }

        }
        internal RespLogin Login(UserModel objRequest)
        {
            RespLogin response = new RespLogin();
            tblUser objUser = new tblUser();
            response.restKey = "0";
            if (objRequest.userEmail == null || objRequest.userEmail == "")
            {
                response.restKey = "6";
                return response;
            }
            try
            {
                using (var db = new WizzDataContext())
                {
                    //check normal login or social login
                    if (string.IsNullOrEmpty(objRequest.socialId))
                    {
                        //normal user login
                        #region different checks
                        var loginType = Convert.ToInt16(objRequest.loginType);
                        objUser = db.tblUsers.Where(u => u.userEmail == objRequest.userEmail.ToLower().Trim() && 1 == loginType).FirstOrDefault();
                        if (objUser == null)
                        {
                            response.restKey = "0";
                        }
                        //else if (objUser.isVerified == false)
                        //{
                        //    response.restKey = "4";
                        //}
                        else if (objUser.isDelete == true || objUser.isActive == false)
                        {
                            response.restKey = "2";
                        }

                        else if ((objUser.password) != stringMethod(objRequest.password))
                        {
                            response.restKey = "3";
                        }
                        #endregion
                        else
                        {
                            //logged in success
                            response.restKey = "1";
                            response.userId = Convert.ToString(objUser.pkUserId);
                            response.userName = Convert.ToString(objUser.userName);
                            response.profilePic = string.IsNullOrWhiteSpace(objUser.profilePic) ? "" : objUser.profilePic.IndexOf("http") >= 0 ? objUser.profilePic : Constants.imagepath + objUser.profilePic;
                            response.userEmail = objUser.userEmail == null ? "" : objUser.userEmail;
                            response.isNotificationOn = objUser.isNotificationOn.ToString();
                            response.credits = Convert.ToString(objUser.credits);
                            response.isFirstimeLogin = objUser.isFirstTimeLogin == null || objUser.isFirstTimeLogin == true ? "True" : "False";
                            response.badge = objUser.badge.ToString();
                            response.badge = objUser.badge.ToString();
                            response.isTutor = objUser.isTeacher == null || objUser.isTeacher == false ? "False" : "True";
                            response.paymentType = Convert.ToString(objUser.paymentType);
                            response.isOtpVerified = Convert.ToString(objUser.isOtpVerified);
                        }
                    }

                    else
                    {

                        objUser = db.tblUsers.Where(u => u.facebookId == objRequest.socialId).FirstOrDefault();

                        response.userName = Convert.ToString(objRequest.userName);
                        response.profilePic = string.IsNullOrWhiteSpace(objRequest.profilePic) ? "" : objRequest.profilePic.IndexOf("http") >= 0 ? objRequest.profilePic : Constants.imagepath + objRequest.profilePic;

                        response.userEmail = objRequest.userEmail == null ? "" : objRequest.userEmail;

                        if (objUser == null)// new social id
                        {
                            objUser = db.tblUsers.Where(u => u.userEmail == objRequest.userEmail.ToLower().Trim()).FirstOrDefault();//get existing record against

                            if (objUser == null)// email if not exists
                            {
                                objUser = new tblUser();

                                objUser.createdDate = DateTime.UtcNow;
                                objUser.userName = objRequest.userName == null ? "" : objRequest.userName;
                                objUser.userEmail = objRequest.userEmail == null ? "" : objRequest.userEmail;
                                objUser.registerType = 2;
                                objUser.guid = Guid.NewGuid();
                                objUser.isActive = true;
                                objUser.isDelete = false;
                                objUser.password = Convert.ToString(Guid.NewGuid());
                                objUser.isNotificationOn = true;
                                objUser.referralCode = isReferralCodeExists(objUser.userName);
                                objUser.profilePic = string.IsNullOrWhiteSpace(objRequest.profilePic) ? "" : objRequest.profilePic.IndexOf("http") >= 0 ? objRequest.profilePic : Constants.imagepath + objRequest.profilePic;

                                objUser.badge = 0;
                                objUser.paymentType = 0;
                                objUser.isFirstTimeLogin = true;
                                objUser.isTeacher = false;
                                objUser.credits = 0;
                                objUser.facebookId = objRequest.socialId.Trim();
                                objUser.avgRatingStudent = 0;
                                objUser.avgRatingTutor = 0;
                                db.tblUsers.InsertOnSubmit(objUser);
                                //creating a new jabber Id
                                db.SubmitChanges();
                                #region JabberClient Register
                                user jabClient = new user();
                                jabClient.created_at = DateTime.UtcNow;
                                jabClient.password = objUser.pkUserId + "@localhost.com";
                                jabClient.username = objUser.pkUserId.ToString();
                                db.users.InsertOnSubmit(jabClient);
                                db.SubmitChanges();
                                #endregion
                            }

                            objUser.updatedDate = DateTime.UtcNow;
                            objUser.deviceToken = objRequest.deviceToken == null ? "" : objRequest.deviceToken;
                            objUser.deviceType = Convert.ToInt16(objRequest.deviceType);
                            db.SubmitChanges();
                            response.userId = Convert.ToString(objUser.pkUserId);
                            response.restKey = "1";
                            response.credits = Convert.ToString(objUser.credits);
                            response.referralCode = objUser.referralCode;
                            response.isNotificationOn = objUser.isNotificationOn.ToString();
                            response.isFirstimeLogin = objUser.isFirstTimeLogin == null || objUser.isFirstTimeLogin == true ? "True" : "False";
                            response.isTutor = objUser.isTeacher == null || objUser.isTeacher == false ? "False" : "True";
                            response.badge = Convert.ToString(objUser.badge);
                            response.paymentType = objUser.paymentType.ToString();
                            response.isOtpVerified = Convert.ToString(objUser.isOtpVerified);
                        }
                        else if (objUser.isDelete == true || objUser.isActive == false)
                            response.restKey = "2";
                        else
                        {
                            response.restKey = "1";
                            response.userId = Convert.ToString(objUser.pkUserId);
                            response.userName = Convert.ToString(objUser.userName);
                            response.profilePic = string.IsNullOrWhiteSpace(objUser.profilePic) ? "" : objUser.profilePic.IndexOf("http") >= 0 ? objUser.profilePic : Constants.imagepath + objUser.profilePic;
                            response.userEmail = objUser.userEmail == null ? "" : objUser.userEmail;
                            response.isNotificationOn = objUser.isNotificationOn.ToString();
                            response.isFirstimeLogin = objUser.isFirstTimeLogin == null || objUser.isFirstTimeLogin == true ? "True" : "False";
                            response.isTutor = objUser.isTeacher == null || objUser.isTeacher == false ? "False" : "True";
                            response.credits = Convert.ToString(objUser.credits);
                            response.badge = Convert.ToString(objUser.badge);
                            response.referralCode = objUser.referralCode;
                            response.isOtpVerified = Convert.ToString(objUser.isOtpVerified);
                            response.paymentType = objUser.paymentType.ToString();
                        }
                    }

                    if (objUser != null && response.restKey == "1" && (objUser.isFirstTimeLogin == null || objUser.isFirstTimeLogin == true))
                    {
                        objUser.isFirstTimeLogin = false;
                        db.SubmitChanges();
                    }

                    if (!string.IsNullOrEmpty(objRequest.deviceToken))
                    {
                        var objTokenUser = db.tblUsers.Where(s => s.deviceToken == objRequest.deviceToken).FirstOrDefault();
                        if (objTokenUser != null)
                        {
                            objTokenUser.deviceToken = "";
                        }
                        objUser.deviceToken = objRequest.deviceToken;
                        objUser.deviceType = Convert.ToInt16(objRequest.deviceType);
                    }

                    db.SubmitChanges();
                    // response.isOtpVerified = objUser.isOtpVerified.ToString();
                    if (response.userId != null)
                    {
                        var userObj = db.tblUsers.FirstOrDefault(x => x.pkUserId == Convert.ToInt64(response.userId));

                        response.isOtpVerified = Convert.ToString(userObj.isOtpVerified);

                    }
                    else
                    {

                        var userObj = db.tblUsers.FirstOrDefault(x => x.facebookId == objRequest.socialId);

                        response.isOtpVerified = Convert.ToString(userObj.isOtpVerified);
                    }

                    return response;
                }
            }
            catch
            {
                return response;
            }

        }

        internal string UpdateUser(UserModel objRequest)
        {

            tblUser objUser;
            tblTutorProfile objProfile;
            using (var db = new WizzDataContext())
            {

                objUser = new tblUser();
                long userId = Convert.ToInt64(objRequest.userId);
                objUser = db.tblUsers.FirstOrDefault(x => x.pkUserId == userId);
                if (objUser == null)
                {
                    return "0";
                }
                else if (objUser.isDelete ==true) {

                    return "2";
                }
                else
                {
                    objProfile = new tblTutorProfile();

                    objProfile = db.tblTutorProfiles.Where(x => x.fkUserId == userId).FirstOrDefault();
                    if (objProfile == null)
                    {
                        objProfile = new tblTutorProfile();
                        objProfile.collegeName = objRequest.collageName;
                        objUser.userName = objRequest.userName;

                        //if (objUser.userEmail.ToLower().Trim() == objRequest.userEmail.ToLower().Trim())
                        //{


                        //}
                        //else {
                        //    objUser.userEmail = objRequest.userEmail;
                        //    SendVerifyEmail(objRequest.userEmail, "1", objUser.pkUserId);
                        //}

                        objProfile.description = objRequest.description;
                        objUser.updatedDate = DateTime.UtcNow;
                        objProfile.passingYear = objRequest.passingYear;
                        objUser.profilePic = objRequest.profilePic;
                        objProfile.updatedDate = DateTime.UtcNow;
                        objProfile.createdDate = DateTime.UtcNow;
                        objProfile.fkUserId = userId;
                        db.tblTutorProfiles.InsertOnSubmit(objProfile);

                    }
                    else {

                        objProfile.updatedDate = DateTime.UtcNow;
                        objProfile.createdDate = DateTime.UtcNow;
                        objProfile.collegeName = objRequest.collageName;
                        objUser.userName = objRequest.userName;

                        //if (objUser.userEmail.ToLower().Trim() == objRequest.userEmail.ToLower().Trim())
                        //{


                        //}
                        //else {
                        //    objUser.userEmail = objRequest.userEmail;
                        //    SendVerifyEmail(objRequest.userEmail, "1", objUser.pkUserId);
                        //}

                        objProfile.description = objRequest.description;
                        objUser.updatedDate = DateTime.UtcNow;
                        objProfile.passingYear = objRequest.passingYear;
                        objUser.profilePic = objRequest.profilePic;
                    }
                   
                    db.SubmitChanges();
                    return "1";
                }
            }
        }

        internal bool EmailAvailable(ReqEmail objRequest)
        {
            using (var db = new WizzDataContext())
            {
                return db.tblUsers.Any(u => u.userEmail.Trim().ToLower() == objRequest.userEmail.Trim().ToLower());
            }
        }

        //internal SettingModel GetSetting(ReqSetting objRequest)
        //{
        //    //System.Data.Entity.Core.Objects.ObjectParameter UpdatedLastSyncTime = new System.Data.Entity.Core.Objects.ObjectParameter("UpdatedLastSyncTime", typeof(string));
        //    string UpdatedLastSyncTime = "";
        //    SettingModel response = new SettingModel();

        //    using (var db = new WizzDataContext())
        //    {
        //        db.usp_isUpdatedData(objRequest.lastSyncTime, ref UpdatedLastSyncTime);


        //        if (string.IsNullOrEmpty(UpdatedLastSyncTime))
        //        {
        //            response.lastSyncTime = objRequest.lastSyncTime;
        //            return response;
        //        }
        //        else
        //        {
        //            response.lastSyncTime = UpdatedLastSyncTime;
        //            response.isUpdated = "True";
        //        }

        //        //get admin setting
        //        var setting = db.tblSettings.FirstOrDefault();
        //        if (setting != null)
        //        {
        //           // response.comission = setting.commission == null ? "" : Convert.ToString(setting.commission);
        //          //  response.feePerHour = setting.perHourFees == null ? "" : Convert.ToString(setting.perHourFees);
        //          //  response.feePerStudent = setting.perStudentCharge == null ? "" : Convert.ToString(setting.perStudentCharge);
        //        }


        //        //}

        //    }
        //    return response;
        //}


        
        internal RespLogin GetUser(Entity objRequest)
        {
            RespLogin response = new RespLogin();
            using (var db = new WizzDataContext())
            {
                var userId = Convert.ToInt64(objRequest.userId);
                var objUser = db.tblUsers.Where(u => u.pkUserId == userId).FirstOrDefault();
                var userProfile = db.tblTutorProfiles.Where(x => x.fkUserId == userId).FirstOrDefault();
                if (objUser != null)
                {
                    if (userProfile == null) {

                        response.restKey = "3";
                        return response;
                    }

                    response.restKey = "1";
                    response.userId = Convert.ToString(objUser.pkUserId);
                    response.userName = Convert.ToString(objUser.userName);
                    if (objUser.profilePic != null || objUser.profilePic != "")
                    {
                        if (objUser.profilePic.Contains("http") || objUser.profilePic.Contains("Http") || objUser.profilePic.Contains("https"))
                        {
                            response.profilePic = objUser.profilePic;
                        }
                        else
                        {
                            response.profilePic = Constants.imagepath + objUser.profilePic;

                        }

                    }
                    else
                    {
                        response.profilePic = "";
                    }
                    //  response.profilePic = objUser.profilePic == null ? "" : objUser.profilePic;
                    response.userEmail = objUser.userEmail == null ? "" : objUser.userEmail;
                    //response.isNotificationOn = objUser.isNotificationOn.ToString();
                    //response.credits = Convert.ToString(objUser.credits);
                    //response.isFirstimeLogin = objUser.isFirstTimeLogin == null || objUser.isFirstTimeLogin == true ? "True" : "False";
                    //response.badge = objUser.badge.ToString();
                    //response.badge = objUser.badge.ToString();
                    //response.isTutor = objUser.isTeacher == null || objUser.isTeacher == false ? "False" : "True";
                    response.collageName = userProfile.collegeName;
                    response.passingYear = userProfile.passingYear;
                    response.description = userProfile.description;
                    response.referralCode = objUser.referralCode;

                }
            }
            return response;
        }

        #endregion
        #region Payal

        // To verify email
        internal bool VerifyEmail(Int64 id, string type)
        {
            try
            {
                using (var db = new WizzDataContext())
                {
                    var user = db.tblUsers.Where(u => u.pkUserId == id).FirstOrDefault();
                    if (user != null && (user.isVerified == false))
                    {
                        user.isVerified = true;
                        user.updatedDate = DateTime.UtcNow;
                        db.SubmitChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }


        // To Replace Special Characters

        internal string ReplacedSpecialCharacter(string str, string tag)
        {
            if (tag == "1")
            {
                return str.Replace('+', '@');
            }
            else
            {
                return str.Replace('@', '+');
            }
        }

        // To check isPasswordChanged

        internal string CheckReset(string username)
        {
            try
            {
                using (var db = new WizzDataContext())
                {
                    tblUser userData = new tblUser();
                    userData = db.tblUsers.Where(e => e.guid.ToString() == username && e.isPasswordChanged == true).FirstOrDefault();
                    if (userData != null)
                    {
                        return userData.userName;
                    }
                    else
                    {
                        return "no";
                    }
                }
            }
            catch
            {
                return "no";
            }
        }

        // To reset Password

        internal int ResetPassword(ReqResetPassword objResetPassword)
        {
            try
            {
                using (var db = new WizzDataContext())
                {
                    tblUser userData = new tblUser();
                    userData = db.tblUsers.Where(e => e.guid.ToString() == objResetPassword.userEmail).FirstOrDefault();
                    if (userData != null)
                    {
                        if (userData.isPasswordChanged == true)
                        {
                            userData.isPasswordChanged = false;
                            userData.password = objResetPassword.password.Trim();
                            db.SubmitChanges();
                            return 1;
                        }
                        else
                        {

                            return 0;
                        }

                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        #endregion

        public string UpdateBadgeCounter(ReqBadgeCounter objReq)
        {
            string rst = "";

            try
            {
                using (var db = new WizzDataContext())
                {
                    var user = db.tblUsers.Where(d => d.deviceToken == objReq.deviceToken).FirstOrDefault();
                    if (user != null)
                    {
                        user.badge = 0;
                        db.SubmitChanges();
                    }
                    rst = "1";
                }


            }
            catch
            {
                rst = "";
            }
            return rst;
        }

        internal int UpdateLatLong(ReqLatLong objReq)
        {
            try
            {
                using (var db = new WizzDataContext())
                {
                    tblUser userData = new tblUser();
                    userData = db.tblUsers.Where(e => e.userEmail.ToString().ToLower() == objReq.email.ToLower()).FirstOrDefault();


                    if (userData != null)
                    {
                        // if (objReq.type == "1")
                        //{
                        var tutor = db.tblTutorProfiles.Where(t => t.fkUserId == userData.pkUserId).FirstOrDefault();
                        //tutor.latitude = Convert.ToDecimal(objReq.lat);
                        //tutor.longitude = Convert.ToDecimal(objReq.lon);
                        db.SubmitChanges();
                        return 1;
                        // }
                        // else { }
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        //public string UpdateBankDetails(BankDetail objReq)
        //{
        //    string rst = "";

        //    try
        //    {
        //        using (var db = new WizzDataContext())
        //        {
        //            var user = db.tblTutorBankDetails.Where(d => d.fkUserId == Convert.ToInt64(objReq.userId)).FirstOrDefault();
        //            if (user != null)
        //            {
        //                user.ifscCode = objReq.ifscCode;
        //                user.accountHolderName = objReq.accountHolderName;
        //                user.accountNumber = objReq.accountNumber;
        //                user.bankName = objReq.bankName;
        //                user.branchAddress = objReq.branchAddress;
        //                user.createdDate = DateTime.UtcNow;
        //                user.ifscCode = objReq.ifscCode;
        //                user.micrCode = objReq.micrCode;

        //                db.SubmitChanges();
        //            }
        //            else 
        //            {
        //                user = new tblTutorBankDetail();

        //                user.ifscCode = objReq.ifscCode;
        //                user.accountHolderName = objReq.accountHolderName;
        //                user.accountNumber = objReq.accountNumber;
        //                user.bankName = objReq.bankName;
        //                user.branchAddress = objReq.branchAddress;
        //                user.createdDate = DateTime.UtcNow;
        //                user.ifscCode = objReq.ifscCode;
        //                user.micrCode = objReq.micrCode;
        //                db.tblTutorBankDetails.InsertOnSubmit(user);
        //                db.SubmitChanges();

        //            }
        //            rst = "1";
        //        }


        //    }
        //    catch
        //    {
        //        rst = "";
        //    }
        //    return rst;
        //}
        #region Rishabh
        //db method for forgotpassword
        internal int ForgetPassword(ReqEmail objReq)
        {
            try
            {
                using (var db = new WizzDataContext())
                {
                    tblUser userData = new tblUser();
                    userData = db.tblUsers.Where(u => u.userEmail == objReq.userEmail.Trim().ToLower()).FirstOrDefault();
                    if (userData != null)
                    {
                        string body = CommonMethod.ReadEmailformats("forgetPassword.html");
                        string path = Constants.SiteUrl + "/Account/ResetPassword/" + userData.guid.ToString();
                        body = body.Replace("$$UserName$$", userData.userName == null ? "User" : userData.userName.ToUpper().Trim());
                        body = body.Replace("$$ResetLink$$", "<a href='" + path + "'> Click to reset your password </a>");
                        SendMail objSendMail = new SendMail();
                        objSendMail.SendEmail(userData.userEmail.Trim(), Constants.AdminEmail, Constants.AdminName, "Reset Password", body);

                        //change in db accrodingly
                        //userData.isVarified = false;
                        userData.isPasswordChanged = true;
                        db.SubmitChanges();
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch
            {
                return 0;
            }
        }

        #endregion



        internal int SaveTutorProfile(ModelsV2.TutorProfileModel objReq)
        {
            throw new NotImplementedException();
        }


    }
}