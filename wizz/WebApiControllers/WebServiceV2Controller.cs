using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using wizz.Class;
using wizz.Dal;
using wizz.Models;
using wizz.ModelsV2;

namespace wizz.WebApiControllers
{
    /// <summary>
    /// Apis for Wizz for V2 changes
    /// </summary>
    public class WebServiceV2Controller : ApiController
    {
        DBMethod objDBMethod;
        wizz.Dal.DbMethodsV2 objDbMethodV2;
        #region Rishabh
        #region commonApi
        /// <summary>
        ///[Common] Login User by Email and Password/ Login with facebook or Google
        /// parameters are:- userId,userEmail,userName,password,deviceType,deviceToken,loginType,socialId 
        /// 1- userEmail,password,deviceType,deviceToken,loginType=1,socialId=""  in normal login
        /// 2- userEmail,userName,deviceType,deviceToken,loginType=2,profilePic,socialId=faceboolid in facebook Login
        /// 2- userEmail,userName,deviceType,deviceToken,loginType=3,profilePic,socialId=google in google Login
        /// profilePic will be send in update image case       
        /// </summary>
        ///  <developer>Mukti</developer>
        /// <Date>26/05/15</Date>
        [HttpPost]
        public Response<RespLogin> Login(UserModel objUserModel)
        {
            List<RespLogin> objResp = new List<RespLogin>();
            Response<RespLogin> response = new Response<RespLogin>();
            try
            {
                objDBMethod = new DBMethod();

                if (CheckRequestIsvalidornot(this.Request))
                {
                    objResp.Add(objDBMethod.Login(objUserModel));
                    switch (objResp[0].restKey)
                    {

                        case "0":
                            response.Create(false, Messages.WebError, Messages.AppVersion, objResp);
                            break;
                        case "1":
                            response.Create(true, Messages.FormatMessage(Messages.Success, "Logged in "), Messages.AppVersion, objResp);
                            break;
                        case "2":
                            response.Create(false, Messages.NotAllowedUser, Messages.AppVersion, objResp);
                            break;
                        case "3":
                            response.Create(false, Messages.InvalidPassword, Messages.AppVersion, objResp);
                            break;
                        case "4":
                            response.Create(false, Messages.EmailVerify, Messages.AppVersion, objResp);
                            break;
                        case "5":
                            response.Create(true, Messages.AccountCreated, Messages.AppVersion, objResp);
                            break;
                        case "6":
                            response.Create(false, "Sorry!!! we were unable to fetch your email please change your Facebook settings for email.", Messages.AppVersion, objResp);
                            break;
                        default:
                            response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
                            break;
                    }
                }
                else
                {
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);

                }

            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objUserModel);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            finally
            {
            }
            return response;
        }
        /// <summary>
        /// Check request is valid or not   
        /// <developer>Mukti</developer>
        /// <Date>26/05/15</Date>
        /// <Purpose>This methode provide facility to check request is valid or not</Purpose>
        /// </summary>
        #region CheckRequestIsvalidornot
        private bool CheckRequestIsvalidornot(HttpRequestMessage request)
        {
            bool rst = true;
            if (Request.Headers.GetValues("Authentication").First() == "wizz60134")
            {
                return true;
            }
            string str = Request.Headers.GetValues("Authentication").First();
            if (!string.IsNullOrEmpty(str))
            {
                try
                {
                    str = Encryption.Decrypt(str, "Wizz");
                    string[] words = str.Split('_');
                    string token = words[1].ToString();
                    double reqTimeStap = Convert.ToDouble(token);
                    //double currentTimeStamp = DateTime.UtcNow.Ticks;
                    double currentTimeStamp = (double)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    var diffInSeconds = (currentTimeStamp - reqTimeStap);
                    int validDiff = Convert.ToInt32(Constants.RequestTime);
                    if (validDiff == 0)
                    {
                        rst = true;
                    }
                    else if (diffInSeconds > validDiff)
                    {
                        rst = false;
                    }
                }
                catch
                {
                    rst = false;
                }


            }

            return rst;
        }

        #region GetHttpContext()
        private HttpContextWrapper GetHttpContext(HttpRequestMessage request = null)
        {
            request = request ?? Request;

            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]);
            }
            else if (HttpContext.Current != null)
            {
                return new HttpContextWrapper(HttpContext.Current);
            }
            else
            {
                return null;
            }
        }
        #endregion

        #endregion
        [HttpPost]
        public Response<string> VerifyOtp(OtpModel objReq)
        {
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();
            try
            {
                objDbMethodV2 = new wizz.Dal.DbMethodsV2();

                if (CheckRequestIsvalidornot(this.Request))
                {
                    if (ModelState.IsValid)
                    {
                        //
                        if (objDbMethodV2.VerifyOtp(objReq))
                            response.Create(true, Messages.FormatMessage(Messages.Success, "user verified"), Messages.AppVersion, objResp);
                        else
                            response.Create(false, Messages.InvalidOtp, Messages.AppVersion, objResp);
                    }
                    else
                        response.Create(false, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage, Messages.AppVersion, objResp);
                }
                else
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);

            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }

            return response;
        }
        /// <summary>
        /// History for student
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<FilteredHistoryResponsetoStudent> GetHistoryForStudent(Entity objReq)
        {
            Response<FilteredHistoryResponsetoStudent> response = new Response<FilteredHistoryResponsetoStudent>();
            List<FilteredHistoryResponsetoStudent> objResp = new List<FilteredHistoryResponsetoStudent>();


            try
            {
                var obj = new Session();
                if (CheckRequestIsvalidornot(this.Request))
                {
                    objResp = obj.GetHistoryForStudent(objReq);
                    response.Create(true, "History List", Messages.AppVersion, objResp);
                }
                else
                {
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
                }
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }

        /// <summary>
        /// History for Tutor
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<FilteredHistoryResponsetoTutor> GetHistoryForTutor(Entity objReq)
        {
            Response<FilteredHistoryResponsetoTutor> response = new Response<FilteredHistoryResponsetoTutor>();
            List<FilteredHistoryResponsetoTutor> objResp = new List<FilteredHistoryResponsetoTutor>();


            try
            {
                var obj = new Session();
                if (CheckRequestIsvalidornot(this.Request))
                {
                    objResp = obj.GetHistoryForTutor(objReq);
                    response.Create(true, "History List", Messages.AppVersion, objResp);
                }
                else
                {
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
                }
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }

        /// <summary>
        /// detailed history on the basis of sessionId
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<HistoryDetailsModel> GetHistoryDetailsForStudent(SessionHistoryRequestModel objReq)
        {
            Response<HistoryDetailsModel> response = new Response<HistoryDetailsModel>();
            List<HistoryDetailsModel> objResp = new List<HistoryDetailsModel>();


            try
            {
                var obj = new Session();
                if (CheckRequestIsvalidornot(this.Request))
                {
                    objResp = obj.GetHistoryDetailsForStudent(objReq);
                    response.Create(true, "History Details", Messages.AppVersion, objResp);
                }
                else
                {
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
                }
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }





        /// <summary>
        /// detailed history on the basis of sessionId
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<HistoryDetailsModel> GetHistoryDetailsForTutor(SessionHistoryRequestModel objReq)
        {
            Response<HistoryDetailsModel> response = new Response<HistoryDetailsModel>();
            List<HistoryDetailsModel> objResp = new List<HistoryDetailsModel>();


            try
            {
                var obj = new Session();
                if (CheckRequestIsvalidornot(this.Request))
                {
                    objResp = obj.GetHistoryDetailsForTutor(objReq);
                    response.Create(true, "History Details", Messages.AppVersion, objResp);
                }
                else
                {
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
                }
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }

        /// <summary>
        /// Need phone number and userId
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> SendOtp(OtpModel objReq)
        {
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();
            try
            {
                objDbMethodV2 = new wizz.Dal.DbMethodsV2();

                if (CheckRequestIsvalidornot(this.Request))
                {
                    if (ModelState.IsValid)
                    {
                        string res = objDbMethodV2.SendOtp(objReq);
                        if (res == "4")
                        {
                            res = "Phone number already exists for a different user";
                        }

                        objResp.Add(res);
                        response.Create(false, Messages.FormatMessage(Messages.Success, "Otp"), Messages.AppVersion, objResp);

                    }
                    else
                        response.Create(false, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage, Messages.AppVersion, objResp);
                }
                else
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);

            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }

            return response;
        }
        #endregion




        /// <summary>
        ///[Common] Get Lists for courses=>classes=>subjects
        /// </summary>
        /// <developer>Rishabh</developer>
        /// <Date>14/1/16</Date>
        [HttpPost]
        public Response<GetSettingsModel> GetAllActivitesData(ReqSetting objReq)
        {
            Response<GetSettingsModel> response = new Response<GetSettingsModel>();
            List<GetSettingsModel> objResp = new List<GetSettingsModel>();


            try
            {
                if (ModelState.IsValid)
                {
                    objDbMethodV2 = new wizz.Dal.DbMethodsV2();
                    if (CheckRequestIsvalidornot(this.Request))
                    {
                        objResp.Add(objDbMethodV2.GetAllActivitesData(objReq));
                        response.Create(true, " ActivitesData", Messages.AppVersion, objResp);
                    }
                    else
                    {
                        response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
                    }
                }
                else
                    response.Create(false, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage, Messages.AppVersion, objResp);
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }



        ///[Common] Get Lists for courses=>classes=>subjects
        /// </summary>
        /// <developer>Rishabh</developer>
        /// <Date>14/1/16</Date>
        [HttpPost]
        public Response<GetSettingsModel> GetEligibleCourses(UserIdModel objReq)
        {
            Response<GetSettingsModel> response = new Response<GetSettingsModel>();
            List<GetSettingsModel> objResp = new List<GetSettingsModel>();


            try
            {
                if (ModelState.IsValid)
                {
                    objDbMethodV2 = new wizz.Dal.DbMethodsV2();
                    if (CheckRequestIsvalidornot(this.Request))
                    {
                        objResp.Add(objDbMethodV2.GetEligibleCourses(objReq));
                        response.Create(true, " Eligiblie courses", Messages.AppVersion, objResp);
                    }
                    else
                    {
                        response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
                    }
                }
                else
                    response.Create(false, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage, Messages.AppVersion, objResp);
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }
        #region Student apis
        /// <summary>
        ///[Common]Save StudentProfileInfo
        /// </summary>
        /// <developer>Rishabh</developer>
        /// <Date>14/1/16</Date>
        [HttpPost]
        public Response<SearchResponseTutorsModel> SaveStudentRequest(StudentRequestModelV2 objReq)
        {
            Response<SearchResponseTutorsModel> response = new Response<SearchResponseTutorsModel>();
            List<SearchResponseTutorsModel> objResp = new List<SearchResponseTutorsModel>();
            try
            {
                objDbMethodV2 = new wizz.Dal.DbMethodsV2();

                if (CheckRequestIsvalidornot(this.Request))
                {
                    if (ModelState.IsValid)
                    {
                        objResp.Add(objDbMethodV2.SaveStudentRequest(objReq));
                        if (objResp.Count > 0)
                            response.Create(true, Messages.FormatMessage(Messages.Success, "Student request saved"), Messages.AppVersion, objResp);
                        else
                            response.Create(false, "No tutor Found", Messages.AppVersion, objResp);
                    }
                    else
                        response.Create(false, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage, Messages.AppVersion, objResp);
                }
                else
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);

            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }

            return response;
        }

        [HttpPost]
        public Response<string> CreateSession(StartSessionModel objReq)
        {
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();
            try
            {
                Session obj = new Session();
                if (CheckRequestIsvalidornot(this.Request))
                {
                    objDbMethodV2 = new Dal.DbMethodsV2();
                    if (objDbMethodV2.CreateSession(objReq))
                        response.Create(true, Messages.FormatMessage(Messages.Success, "Your session has been created"), Messages.AppVersion, objResp);
                    else
                        response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
                }
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }

        /// <summary>
        /// get upcoming sessions
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<SessionResponseStudentModel> GetSessionsForStudents(RequestEntity objReq)
        {
            Response<SessionResponseStudentModel> response = new Response<SessionResponseStudentModel>();
            List<SessionResponseStudentModel> objResp = new List<SessionResponseStudentModel>();
            try
            {
                Session obj = new Session();
                if (CheckRequestIsvalidornot(this.Request))
                {
                    objDbMethodV2 = new Dal.DbMethodsV2();
                    objResp = objDbMethodV2.GetSessionsForStudent(objReq);
                    if (objResp.Count > 0)
                        response.Create(true, Messages.FormatMessage(Messages.Success, "Session List"), Messages.AppVersion, objResp);
                    else
                        response.Create(true, Messages.FormatMessage(Messages.NoRecord), Messages.AppVersion, objResp);
                }
                else
                {

                    response.Create(false, Messages.FormatMessage(Messages.NotAllowedUser), Messages.AppVersion, objResp);
                }
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }
        /// <summary>
        /// A single session Information
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<SessionInfo> GetSessionInfo(SessionEntity objReq)
        {
            Response<SessionInfo> response = new Response<SessionInfo>();
            List<SessionInfo> objResp = new List<SessionInfo>();
            try
            {
                Session obj = new Session();
                if (CheckRequestIsvalidornot(this.Request))
                {

                    objResp = obj.GetSessionInfo(objReq);

                    if (objResp != null)
                        response.Create(true, "Session Info", Messages.AppVersion, objResp);
                    else
                        response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);

                }
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }

        [HttpPost]
        public Response<string> ReportSpam(ReportSpamModel objReq)
        {
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();
            try
            {
                RequestMethods objDBMethod = new RequestMethods();
                if (CheckRequestIsvalidornot(this.Request))
                {
                    if (ModelState.IsValid)
                    {
                        if (objDBMethod.ReportSpam(objReq))
                            response.Create(true, Messages.FormatMessage(Messages.Success, "You have spammed"), Messages.AppVersion, objResp);
                        else
                            response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
                    }
                    else
                        response.Create(false, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage, Messages.AppVersion, objResp);
                }
                else
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);

            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<SessionResponseStudentModel> GetSessionInfoForStudent(UniqueRequestModel objReq)
        {
            Response<SessionResponseStudentModel> response = new Response<SessionResponseStudentModel>();
            List<SessionResponseStudentModel> objResp = new List<SessionResponseStudentModel>();
            try
            {
                Session obj = new Session();
                if (CheckRequestIsvalidornot(this.Request))
                {
                    objDbMethodV2 = new Dal.DbMethodsV2();
                    objResp.Add(objDbMethodV2.GetSessionInfoForStudent(objReq));
                    if (objResp.Count > 0)
                        response.Create(true, Messages.FormatMessage(Messages.Success, "Session List"), Messages.AppVersion, objResp);
                    else
                        response.Create(false, Messages.FormatMessage(Messages.NoRecord), Messages.AppVersion, objResp);
                }
                else
                {

                    response.Create(false, Messages.FormatMessage(Messages.NotAllowedUser), Messages.AppVersion, objResp);
                }
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }

        /// <summary>
        /// session list for tutor
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<SessionResponseTutorModel> GetSessionsForTutor(RequestEntity objReq)
        {
            Response<SessionResponseTutorModel> response = new Response<SessionResponseTutorModel>();
            List<SessionResponseTutorModel> objResp = new List<SessionResponseTutorModel>();
            try
            {
                Session obj = new Session();
                if (CheckRequestIsvalidornot(this.Request))
                {
                    objDbMethodV2 = new Dal.DbMethodsV2();
                    objResp = objDbMethodV2.GetSessionsForTutor(objReq);
                    if (objResp.Count > 0)
                        response.Create(true, Messages.FormatMessage(Messages.Success, "Session List"), Messages.AppVersion, objResp);
                    else
                        response.Create(true, Messages.FormatMessage(Messages.NoRecord), Messages.AppVersion, objResp);
                }
                else
                {

                    response.Create(false, Messages.FormatMessage(Messages.NotAllowedUser), Messages.AppVersion, objResp);
                }
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }



        /// <summary>
        /// start session with session Id which you get in the session list
        /// 
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> StartSession(CreateSesion objReq)
        {
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();
            try
            {
                Session obj = new Session();
                if (CheckRequestIsvalidornot(this.Request))
                {
                    if (obj.StartSession(objReq))
                        response.Create(true, Messages.FormatMessage(Messages.Success, "Your session has been started"), Messages.AppVersion, objResp);
                    else
                        response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
                }
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }
        /// <summary>
        /// session will be ended on behalf
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<SessionEndResponseModel> EndSession(CreateSesion objReq)
        {
            Response<SessionEndResponseModel> response = new Response<SessionEndResponseModel>();
            List<SessionEndResponseModel> objResp = new List<SessionEndResponseModel>();
            try
            {
                Session obj = new Session();
                if (CheckRequestIsvalidornot(this.Request))
                {
                    if (ModelState.IsValid)
                    {
                        objResp.Add(obj.EndSession(objReq));
                        if (objResp.Count > 0)
                            response.Create(true, Messages.FormatMessage(Messages.Success, "You have ended session"), Messages.AppVersion, objResp);
                        else
                            response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
                    }
                    else
                        response.Create(false, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage, Messages.AppVersion, objResp);
                }
                else
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);

            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }




        /// <summary>
        /// Review by tutor send is
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> SessionReviewByTutor(SessionReviewModel objReq)
        {
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();
            try
            {
                Session obj = new Session();
                if (CheckRequestIsvalidornot(this.Request))
                {
                    if (obj.SessionReviewByTutor(objReq))
                        response.Create(true, Messages.FormatMessage(Messages.Success, "You have submitted"), Messages.AppVersion, objResp);
                    else
                        response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
                }
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }


        /// <summary>
        /// Review by tutor send is
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> SessionReviewByStudent(StudentReviewModel objReq)
        {
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();
            try
            {
                Session obj = new Session();
                if (CheckRequestIsvalidornot(this.Request))
                {
                    if (ModelState.IsValid)
                    {
                        if (obj.SessionReviewByStudent(objReq))
                            response.Create(true, Messages.FormatMessage(Messages.Success, "You have reviewed"), Messages.AppVersion, objResp);
                        else
                            response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);

                    }
                    else
                        response.Create(false, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage, Messages.AppVersion, objResp);
                }
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }


        /// <summary>
        /// Review by tutor send is
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> SessionRatingByStudent(StudentRatingwModel objReq)
        {
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();
            try
            {
                Session obj = new Session();
                if (CheckRequestIsvalidornot(this.Request))
                {
                    if (ModelState.IsValid)
                    {
                        if (obj.SessionRatingByStudent(objReq))
                            response.Create(true, Messages.FormatMessage(Messages.Success, "You have rated"), Messages.AppVersion, objResp);
                        else
                            response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);

                    }
                    else
                        response.Create(false, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage, Messages.AppVersion, objResp);
                }
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }

        ///<summary>
        /// <purpose>[Common] Saving Card Information</purpose>
        /// </summary>
        /// <returns></returns>
        /// <developer>Rishabh</developer>
        /// <Date>10/6/2015</Date>
        public Response<string> PostCardInfo(PaymentInfo objReq)
        {
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();
            try
            {
                PaymentMethods objDBMethod = new PaymentMethods();

                if (CheckRequestIsvalidornot(this.Request))
                {
                    if (ModelState.IsValid)
                    {
                        if (objDBMethod.SaveCardInfo(objReq))
                            response.Create(true, Messages.FormatMessage(Messages.Success, "Card Info saved"), Messages.AppVersion, objResp);
                        else
                            response.Create(false, Messages.ErrorOccure, Messages.AppVersion, objResp);
                    }
                    else
                        response.Create(false, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage, Messages.AppVersion, objResp);
                }
                else
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);

            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }

            return response;
        }
        /// <summary>
        /// Cancel a session
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> CancelSession(UniqueRequestModel objReq)
        {
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();
            try
            {
                objDbMethodV2 = new wizz.Dal.DbMethodsV2();

                if (CheckRequestIsvalidornot(this.Request))
                {
                    if (ModelState.IsValid)
                    {
                        if (objDbMethodV2.CancelSession(objReq))
                            response.Create(true, Messages.FormatMessage(Messages.Success, "Session cancelled"), Messages.AppVersion, objResp);
                        else
                            response.Create(false, Messages.ErrorOccure, Messages.AppVersion, objResp);
                    }
                    else
                        response.Create(false, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage, Messages.AppVersion, objResp);
                }
                else
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);

            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }

            return response;
        }
        #endregion

        /// <summary>
        ///[Common] Forgot password service     
        /// <summary>
        ///To avail the forgot password service and send mail to the user for reset link        
        /// </summary>
        ///  <developer>Rishabh</developer>
        /// <Date>26/05/15</Date>
        /// </summary>
        [HttpPost]
        public Response<string> ForgetPassword(ReqEmail objReq)
        {
            List<string> objResp = new List<string>();
            Response<string> response = new Response<string>();
            try
            {
                objDBMethod = new DBMethod();
                if (CheckRequestIsvalidornot(this.Request))
                {
                    switch (objDBMethod.ForgetPassword(objReq))
                    {
                        case 1:
                            response.Create(true, Messages.MailSent, Messages.AppVersion, objResp);
                            break;
                        case 0:
                            response.Create(false, Messages.NoEmailExist, Messages.AppVersion, objResp);
                            break;
                        default:
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);

            }

            return response;
        }
        /// <summary>
        /// Get colleges list
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<CollegesModel> GetColleges()
        {
            Response<CollegesModel> response = new Response<CollegesModel>();
            List<CollegesModel> objResp = new List<CollegesModel>();
            try
            {
                objDbMethodV2 = new wizz.Dal.DbMethodsV2();
                if (CheckRequestIsvalidornot(this.Request))
                {
                    objResp = objDbMethodV2.GetColleges();
                    response.Create(true, "Colleges Data", Messages.AppVersion, objResp);
                }
                else
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize("");
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }




        //[HttpPost]
        //public Response<UserResponse> GetTutorCalendar(RequestEntity objReq) {
        //    Response<UserResponse> response = new Response<UserResponse>();
        //    List<UserResponse> objResp = new List<UserResponse>();
        //    try
        //    {
        //        objDbMethodV2 = new wizz.Dal.DbMethodsV2();

        //        if (CheckRequestIsvalidornot(this.Request))
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                objResp.Add(objDbMethodV2.GetUserRequest(objReq));
        //                if (objResp.Count > 0)
        //                    response.Create(true, Messages.FormatMessage(Messages.Success, "GetUserRequest"), Messages.AppVersion, objResp);
        //                else
        //                    response.Create(false, Messages.ErrorOccure, Messages.AppVersion, objResp);
        //            }
        //            else
        //                response.Create(false, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage, Messages.AppVersion, objResp);
        //        }
        //        else
        //            response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);

        //    }
        //    catch (Exception ex)
        //    {
        //        object session = new JavaScriptSerializer().Serialize(objReq);
        //        LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
        //        response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
        //    }

        //    return response;

        //}
        #region Phase 6 apis
        /// <summary>
        ///  filled and unfilled slots for student calendar
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<StudentOccupiedCalendarModel> GetStudentOccupiedCalendar(Entity objReq)
        {
            Response<StudentOccupiedCalendarModel> response = new Response<StudentOccupiedCalendarModel>();
            List<StudentOccupiedCalendarModel> objResp = new List<StudentOccupiedCalendarModel>();


            try
            {

                DbMethodsV2 obj = new DbMethodsV2();
                if (CheckRequestIsvalidornot(this.Request))
                {

                    objResp.Add(obj.GetStudentOccupiedCalendar(objReq));
                    response.Create(true, "Student Occupied list", Messages.AppVersion, objResp);
                }
                else
                {
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
                }
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<TutorOccupiedCalendarModel> GetTutorOccupiedCalendar(Entity objReq)
        {
            Response<TutorOccupiedCalendarModel> response = new Response<TutorOccupiedCalendarModel>();
            List<TutorOccupiedCalendarModel> objResp = new List<TutorOccupiedCalendarModel>();


            try
            {

                DbMethodsV2 obj = new DbMethodsV2();
                if (CheckRequestIsvalidornot(this.Request))
                {

                    objResp.Add(obj.GetTutorOccupiedCalendar(objReq));
                    response.Create(true, "Student Occupied list", Messages.AppVersion, objResp);
                }
                else
                {
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
                }
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }

        /// <summary>
        /// Save payment type
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> SetPaymentType(PaymentModel objReq)
        {
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();
            try
            {
                objDbMethodV2 = new wizz.Dal.DbMethodsV2();
                if (objDbMethodV2.SetPaymentType(objReq))
                    response.Create(true, "done", Messages.AppVersion, objResp);
                else
                {
                    response.Create(false, "you are not allowed", Messages.AppVersion, objResp);
                }
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            finally
            {
            }
            return response;
        }




        #region PostBankDetails

        /// <summary>
        /// Post bank details
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> SaveTutorBankDetails(RespTutorAccountDetails objReq)
        {

            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();
            try
            {
                objDbMethodV2 = new wizz.Dal.DbMethodsV2();
                if (objDbMethodV2.PostBankAccountDetails(objReq))
                    response.Create(true, "done", Messages.AppVersion, objResp);
                else
                {
                    response.Create(false, "you are not allowed", Messages.AppVersion, objResp);
                }
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            finally
            {
            }
            return response;



        }

        /// <summary>
        /// get bank details
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<RespTutorAccountDetails> GetTutorBankDetails(ReqTutorAccountDetails objReq)
        {
            Response<RespTutorAccountDetails> response = new Response<RespTutorAccountDetails>();
            List<RespTutorAccountDetails> objResp = new List<RespTutorAccountDetails>();
            try
            {
                objDbMethodV2 = new wizz.Dal.DbMethodsV2();
                RespTutorAccountDetails tryObj = new RespTutorAccountDetails();
                tryObj = objDbMethodV2.GetTutorBankDetail(objReq);
                if (tryObj.accountNumber == null)
                {

                }
                else
                {
                    objResp.Add(tryObj);
                }

                if (objResp.Count > 0)
                {
                    response.Create(true, "done", Messages.AppVersion, objResp);

                }
                else
                {
                    //  objResp = new List<RespTutorAccountDetails>();
                    response.Create(false, "No data found", Messages.AppVersion, objResp);
                }
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            finally
            {
            }
            return response;

            return response;

        }
        #endregion PostBankDetails

        #endregion
        #region TutorApis

        /// <summary>
        ///[tutor]save tutor Profile info
        ///[Multipart api]
        ///<description>
        ///
        /// <parameters>
        /// fileNameList-String list for filenames(optional),
        /// userId-required,
        /// subjectIdList-String list for subjectIds
        /// courseIdList-String list for courseIds
        ///classIdList-String list for classIds
        /// lat- required,
        /// longi-required,
        /// location,
        /// collegeName-optional,
        /// distanceRadius-required,
        /// referralCode-optional,
        /// feesPerHour-required,
        /// userName-required(for generating a search code)
        /// 
        /// </parameters>
        /// 
        /// 
        /// </description>
        /// </summary>
        /// <developer>Rishabh</developer>
        /// <Date>15/1/16</Date>
        [HttpPost]
        public Response<string> SaveTutorProfile()
        {
            TutorProfileModel objReq = new TutorProfileModel();
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();
            int resp = 0;
            try
            {
                objDbMethodV2 = new wizz.Dal.DbMethodsV2();
                if (CheckRequestIsvalidornot(this.Request))
                {
                    HttpContextWrapper objwrapper = GetHttpContext(this.Request);
                    HttpFileCollectionBase collection = objwrapper.Request.Files;
                    string jsonvalue = objwrapper.Request.Form["json"];
                    if (!string.IsNullOrEmpty(jsonvalue))
                    {
                        objReq = JsonConvert.DeserializeObject<TutorProfileModel>(jsonvalue);

                        List<string> newFileNames = new List<string>();

                        newFileNames = SaveMultipleFiles(collection, "");
                        //    objReq.fileNameList = newFileNames;
                        if (newFileNames.Count == 0)
                            response.Create(false, "No Transcripts is uploaded please try again !", Messages.AppVersion, objResp);
                        else
                            resp = objDbMethodV2.SaveTutorProfile(objReq, newFileNames);
                        if (resp == 1)
                        {
                            response.Create(true, "Tutor profile info saved successfully !", Messages.AppVersion, objResp);

                        }
                        else
                        {
                            response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);

                        }

                    }

                }
                return response;
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }

        /// <summary>
        /// Tutor preferences of schedule saving api
        /// </summary>
        /// <developer>Rishabh</developer>
        /// <Date>15/1/2016</Date>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> SaveTutorSchedule(TutorScheduleModel objReq)
        {
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();


            try
            {
                if (ModelState.IsValid)
                {
                    objDbMethodV2 = new wizz.Dal.DbMethodsV2();
                    if (CheckRequestIsvalidornot(this.Request))
                    {
                        if (objDbMethodV2.SaveTutorSchedule(objReq))
                            response.Create(true, "Tutor Schedule saved successfully", Messages.AppVersion, objResp);
                    }
                    else
                    {
                        response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
                    }
                }
                else
                    response.Create(false, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage, Messages.AppVersion, objResp);
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }



        /// <summary>
        /// Tutor preferences of schedule saving api
        /// </summary>
        /// <developer>Rishabh</developer>
        /// <Date>15/1/2016</Date>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> SetTutorSubjects(TutorSubjectModel objReq)
        {
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();


            try
            {
                if (ModelState.IsValid)
                {
                    objDbMethodV2 = new wizz.Dal.DbMethodsV2();
                    if (CheckRequestIsvalidornot(this.Request))
                    {
                        if (objDbMethodV2.SetTutorSubjects(objReq))
                            response.Create(true, "Tutor subjects saved successfully", Messages.AppVersion, objResp);
                    }
                    else
                    {
                        response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
                    }
                }
                else
                    response.Create(false, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage, Messages.AppVersion, objResp);
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }

        #region Save multiple Files
        /// <summary>
        /// Not in use for mobile end i.e. this is not a service for android or ios
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>

        List<string> SaveMultipleFiles(HttpFileCollectionBase collection, string fileName)
        {
            List<string> UniqueFileNames = new List<string>();
            string ext = "";
            string path = HttpContext.Current.Request.PhysicalApplicationPath + "tutorDocs\\";
            if (collection.Count > 0)
            {
                #region LoopFor multipleFiles upload and save and get their names
                for (int i = 0; i < collection.Count; i++)
                {
                    HttpPostedFileBase postedFile = collection.Get(i);
                    ext = Path.GetExtension(postedFile.FileName).ToLower();
                    if (string.IsNullOrEmpty(fileName))
                    {
                        UniqueFileNames.Add(Guid.NewGuid().ToString("n") + ext);
                    }
                    else
                    {
                        if (File.Exists(@path + fileName))
                        {
                            File.Delete(@path + fileName);
                        }
                        UniqueFileNames[i] = fileName;
                    }

                    postedFile.SaveAs(path + UniqueFileNames[i]);

                }
                #endregion


            }

            return UniqueFileNames;
        }

        #endregion
        /// <summary>
        /// Api to filter the tutors according to different parameters
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<SearchResponseTutorsModel> GetTutorsList(TutorSearchModel objReq)
        {
            Response<SearchResponseTutorsModel> response = new Response<SearchResponseTutorsModel>();
            List<SearchResponseTutorsModel> objResp = new List<SearchResponseTutorsModel>();


            try
            {
                if (ModelState.IsValid)
                {
                    objDbMethodV2 = new wizz.Dal.DbMethodsV2();
                    if (CheckRequestIsvalidornot(this.Request))
                    {
                        objResp.Add(objDbMethodV2.GetTutorsList(objReq));
                        if (objResp.Count > 0)
                            response.Create(true, "Tutor List", Messages.AppVersion, objResp);
                    }
                    else
                    {
                        response.Create(false, Messages.FormatMessage("Sorry no tutor found !!"), Messages.AppVersion, objResp);
                    }
                }
                else
                    response.Create(false, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage, Messages.AppVersion, objResp);
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }



        /// <summary>
        /// Api to filter the tutors according to different parameters
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<SearchResponseTutorsModel> GetTutorBySearchCode(SearchTutorByCodeModel objReq)
        {
            Response<SearchResponseTutorsModel> response = new Response<SearchResponseTutorsModel>();
            List<SearchResponseTutorsModel> objResp = new List<SearchResponseTutorsModel>();


            try
            {
                if (ModelState.IsValid)
                {
                    objDbMethodV2 = new wizz.Dal.DbMethodsV2();
                    if (CheckRequestIsvalidornot(this.Request))
                    {
                        objResp.Add(objDbMethodV2.GetTutorBySearchCode(objReq));
                        if (objResp.Count > 0)
                            response.Create(true, "Tutor Data ", Messages.AppVersion, objResp);
                    }
                    else
                    {
                        response.Create(false, Messages.FormatMessage("Sorry no tutor found !!"), Messages.AppVersion, objResp);
                    }
                }
                else
                    response.Create(false, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage, Messages.AppVersion, objResp);
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }

        #endregion

        #endregion
        #region mukti
        /// <summary>
        ///[Tutor]Aplied for Tutor or not if yes then approved or not
        /// </summary>
        /// <developer>Mukti</developer>
        /// <Date>20/11/15</Date>
        [HttpPost]
        public Response<RespIsTutor> IsApprovedTutor(Entity objReq)
        {
            Response<RespIsTutor> response = new Response<RespIsTutor>();
            List<RespIsTutor> objResp = new List<RespIsTutor>();
            try
            {
                var obj = new Tutor();
                if (CheckRequestIsvalidornot(this.Request))
                {
                    //  var resp = obj.IsApprovedTutor(objReq);
                    objResp.Add(obj.IsApprovedTutor(objReq));
                    response.Create(true, Messages.FormatMessage(Messages.Success, "Tutor "), Messages.AppVersion, objResp);
                }
                else
                {
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
                }
            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }

        /// <summary>
        ///[Common] Add new user and update existing user  public string       
        /// parameters are:- userId,userEmail,userName,password,deviceType,deviceToken
        /// key image- for images
        /// key json- for data
        /// userId will be 0 for signup and will be > 0 in update case (for later use)       
        /// profilePic will be send in update image case        
        /// </summary>
        /// <developer>Mukti</developer>
        /// <Date>5/10/15</Date>
        [HttpPost]
        public Response<string> UpdateUser()
        {
            UserModel objReq = new UserModel();
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();

            string resp = "";

            try
            {
                objDBMethod = new DBMethod();

                if (CheckRequestIsvalidornot(this.Request))
                {
                    // //HttpContextWrapper objwrapper = GetHttpContext(this.Request);
                    //   HttpFileCollectionBase collection = objwrapper.Request.Files;

                    // string jsonvalue = objwrapper.Request.Form["json"];
                    HttpContextWrapper objwrapper = GetHttpContext(this.Request);
                    HttpFileCollectionBase collection = objwrapper.Request.Files;

                    //var s = HttpContext.Current.Request;
                    //string jsonVlaue = s.Form["json"];
                    //HttpFileCollectionBase o = s.Files;


                    string jsonvalue = objwrapper.Request.Form["json"];
                    if (!string.IsNullOrEmpty(jsonvalue))
                    {
                        objReq = JsonConvert.DeserializeObject<UserModel>(jsonvalue);
                        objReq.profilePic = SaveImage(collection, objReq.profilePic);

                        resp = objDBMethod.UpdateUser(objReq);
                        switch (Convert.ToInt32(resp))
                        {
                            case 0:
                                response.Create(false, "No user found", Messages.AppVersion, objResp);
                                break;
                            case 1:
                                response.Create(true, "Profile updated successfully", Messages.AppVersion, objResp);
                                break;
                            case 2:
                                response.Create(true, "You are blocked from admin", Messages.AppVersion, objResp);
                                break;

                            default:
                                break;
                        }
                    }
                    else
                    {
                        response.Create(false, Messages.InvalidReq, Messages.AppVersion, objResp);
                    }
                }
                else
                {
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);

                }

            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error(ex.Message, ex, session, ex.Message);
                response.Create(false, "Error occured while Processing Webservice request", Messages.AppVersion, objResp);
            }
            finally
            {
            }
            return response;
        }

        string SaveImage(HttpFileCollectionBase collection, string imageName)
        {

            string UniqueFileName = imageName;
            string path = HttpContext.Current.Request.PhysicalApplicationPath + "WebImages\\";

            if (collection.Count > 0)
            {
                if (string.IsNullOrEmpty(imageName))
                {
                    UniqueFileName = Guid.NewGuid().ToString("n") + ".jpg";

                }
                else
                {
                    if (File.Exists(@path + imageName))
                    {
                        File.Delete(@path + imageName);
                    }
                    UniqueFileName = imageName;
                }

                HttpPostedFileBase imgFile = collection.Get(0);
                if (imgFile.ContentType == "image/jpeg" || imgFile.ContentType == "image/png" || imgFile.ContentType == "image/*")
                {
                    Stream requestStream = imgFile.InputStream;
                    Image img = System.Drawing.Image.FromStream(requestStream);

                    img.Save(path + UniqueFileName, System.Drawing.Imaging.ImageFormat.Jpeg);

                    requestStream.Close();

                }
                else
                {
                    if (Constants.isProduction.ToLower() == "true")
                    {
                        Stream requestStream = imgFile.InputStream;
                        Image img = System.Drawing.Image.FromStream(requestStream);
                        img.Save(path + UniqueFileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        requestStream.Close();
                    }


                }
            }
            return UniqueFileName;
        }
        #endregion

        #region olderapis
        /// <summary>
        ///[Common] Change password
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> ChangePassword(ChangePasswordModel objReq)
        {
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();
            try
            {
                WebMethods objDBMethod = new WebMethods();

                if (CheckRequestIsvalidornot(this.Request))
                {
                    if (ModelState.IsValid)
                    {
                        switch (objDBMethod.ChangePassword(objReq))
                        {
                            case 1:
                                response.Create(true, Messages.FormatMessage(Messages.Success, "Password changed"), Messages.AppVersion, objResp);
                                break;
                            case 0:
                                response.Create(false, "User do not exists", Messages.AppVersion, objResp);
                                break;
                            case 2:
                                response.Create(false, "You are not allowed to change", Messages.AppVersion, objResp);
                                break;
                            case 3:
                                response.Create(false, "Your old password is wrong", Messages.AppVersion, objResp);
                                break;
                            default:
                                break;
                        }



                    }
                    else
                        response.Create(false, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage, Messages.AppVersion, objResp);
                }
                else
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);

            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }

            return response;
        }
        /// <summary>
        ///[Common] Change status for tutor
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<RespIsTeacherRequest> ChangeTutorStatus(IsTeacherRequest objReq)
        {
            Response<RespIsTeacherRequest> response = new Response<RespIsTeacherRequest>();
            List<RespIsTeacherRequest> objResp = new List<RespIsTeacherRequest>();
            try
            {
                WebMethods objDBMethod = new WebMethods();

                if (CheckRequestIsvalidornot(this.Request))
                {
                    if (ModelState.IsValid)
                    {
                        objResp.Add(objDBMethod.isTeacher(objReq));
                        if (objResp != null)
                        {

                            response.Create(true, Messages.FormatMessage(Messages.Success, "Status Changed"), Messages.AppVersion, objResp);

                        }
                        else
                        {

                            response.Create(false, Messages.ErrorOccure, Messages.AppVersion, objResp);
                        }



                    }
                    else
                        response.Create(false, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage, Messages.AppVersion, objResp);
                }
                else
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);

            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            finally
            {
            }
            return response;
        }
        /// <summary>
        /// [Common]Log out service
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<string> LogOutService(ReqLogOut objReq)
        {
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();
            try
            {
                WebMethods objDBMethod = new WebMethods();
                if (CheckRequestIsvalidornot(this.Request))
                {
                    if (ModelState.IsValid)
                    {
                        if (objDBMethod.UserLogout(objReq))
                            response.Create(true, Messages.FormatMessage(Messages.Success, "Logged out"), Messages.AppVersion, objResp);
                        else
                            response.Create(false, Messages.ErrorOccure, Messages.AppVersion, objResp);
                    }
                    else
                        response.Create(false, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage, Messages.AppVersion, objResp);
                }
                else
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);

            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }
        /// <summary>
        ///[Common] Get user Profile
        /// </summary>
        /// <developer>Mukti</developer>
        /// <Date>3/11/15</Date>
        [HttpPost]
        public Response<RespLogin> GetProfile(Entity objReq)
        {
            List<RespLogin> objResp = new List<RespLogin>();
            Response<RespLogin> response = new Response<RespLogin>();
            try
            {
                objDBMethod = new DBMethod();

                if (CheckRequestIsvalidornot(this.Request))
                {
                    objResp.Add(objDBMethod.GetUser(objReq));
                    response.Create(true, "User Data", Messages.AppVersion, objResp);

                }
                else
                {
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);

                }

            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }

        /// <summary>
        /// Badge update
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> UpdateBadgeCounter(ReqBadgeCounter objReq)
        {

            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();
            if (objReq != null)
            {
                try
                {
                    bool var = CheckRequestIsvalidornot(this.Request);
                    if (var == false)
                    {
                        response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
                    }
                    else
                    {
                        objDBMethod = new DBMethod();
                        string rst = objDBMethod.UpdateBadgeCounter(objReq);
                        if (rst == "1")
                        {
                            response.Create(true, Messages.FormatMessage(Messages.Success, ""), Messages.AppVersion, objResp);
                        }
                        else
                        {
                            response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
                        }
                    }


                }
                catch (Exception ex)
                {

                    response.Create(false, Messages.FormatMessage(Messages.ErrorOccure, ""), Messages.AppVersion, objResp);
                }

            }
            else
            {
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure, ""), Messages.AppVersion, objResp);
            }


            return response;
        }
        #endregion


        #region group activites

        /// <summary>
        /// Multi part api,  please send the following parameters
        /// 
        /// groupName,groupPic(optional),userId(admin id) required,groupId,subjectName,timeSlot
        /// List of string of groupMembers (in ids)
        /// 
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<string> CreateGroup()
        {
            GroupModel objReq = new GroupModel();
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();

            try
            {
                DbMethodsV2 objDBMethodV2 = new DbMethodsV2();

                if (CheckRequestIsvalidornot(this.Request))
                {
                    HttpContextWrapper objwrapper = GetHttpContext(this.Request);
                    HttpFileCollectionBase collection = objwrapper.Request.Files;

                    //var s = HttpContext.Current.Request;
                    //string jsonVlaue = s.Form["json"];
                    //HttpFileCollectionBase o = s.Files;


                    string jsonvalue = objwrapper.Request.Form["json"];
                    //     jsonvalue=HttpUtility.HtmlDecode(jsonvalue);
                    if (!string.IsNullOrEmpty(jsonvalue))
                    {
                        objReq = JsonConvert.DeserializeObject<GroupModel>(jsonvalue);
                        objReq.groupPic = SaveImage(collection, "");


                        if (objDBMethodV2.CreateGroup(objReq))
                        {
                            response.Create(true, Messages.FormatMessage(Messages.Success, "Created"), Messages.AppVersion, objResp);

                        }
                        else
                        {
                            response.Create(false, Messages.InvalidReq, Messages.AppVersion, objResp);
                        }
                    }
                    else
                    {
                        response.Create(false, Messages.InvalidReq, Messages.AppVersion, objResp);
                    }
                }
                else
                {
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);

                }

            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error(ex.Message, ex, session, ex.Message);
                response.Create(false, "Error occured while Processing Webservice request", Messages.AppVersion, objResp);
            }
            finally
            {
            }
            return response;
        }

        /// <summary>
        /// Get GroupList
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<GetGroupModel> GetGroups(GroupEntity objReq)
        {
            // GroupModel objReq = new GroupModel();
            Response<GetGroupModel> response = new Response<GetGroupModel>();
            List<GetGroupModel> objResp = new List<GetGroupModel>();

            try
            {
                DbMethodsV2 objDBMethodV2 = new DbMethodsV2();

                if (CheckRequestIsvalidornot(this.Request))
                {


                    objResp = objDBMethodV2.GetGroup(objReq);

                    if (objResp.Count > 0)
                    {
                        response.Create(true, Messages.FormatMessage(Messages.Success, "Group List"), Messages.AppVersion, objResp);

                    }
                    else
                    {
                        response.Create(false, Messages.InvalidReq, Messages.AppVersion, objResp);
                    }

                }
                else
                {
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);

                }

            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error(ex.Message, ex, session, ex.Message);
                response.Create(false, "Error occured while Processing Webservice request", Messages.AppVersion, objResp);
            }
            finally
            {
            }
            return response;
        }
        /// <summary>
        /// Get GroupList
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<GetGroupModel> GetGroupDetails(GetGroupRequest objReq)
        {
            // GroupModel objReq = new GroupModel();
            Response<GetGroupModel> response = new Response<GetGroupModel>();
            List<GetGroupModel> objResp = new List<GetGroupModel>();

            try
            {
                DbMethodsV2 objDBMethodV2 = new DbMethodsV2();

                if (CheckRequestIsvalidornot(this.Request))
                {


                    objResp.Add(objDBMethodV2.GetGroupDetails(objReq));

                    if (objResp.Count > 0)
                    {
                        response.Create(true, Messages.FormatMessage(Messages.Success, "Group Details"), Messages.AppVersion, objResp);

                    }
                    else
                    {
                        response.Create(false, Messages.InvalidReq, Messages.AppVersion, objResp);
                    }

                }
                else
                {
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);

                }

            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error(ex.Message, ex, session, ex.Message);
                response.Create(false, "Error occured while Processing Webservice request", Messages.AppVersion, objResp);
            }
            finally
            {
            }
            return response;
        }


        /// <summary>
        /// Get GroupList
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<GetGroupModel> DeleteGroup(GetGroupRequest objReq)
        {
            // GroupModel objReq = new GroupModel();
            Response<GetGroupModel> response = new Response<GetGroupModel>();
            List<GetGroupModel> objResp = new List<GetGroupModel>();

            try
            {
                DbMethodsV2 objDBMethodV2 = new DbMethodsV2();

                if (CheckRequestIsvalidornot(this.Request))
                {




                    if (objDBMethodV2.DeleteGroup(objReq))
                    {
                        response.Create(true, Messages.FormatMessage(Messages.Success, "Group Deleted"), Messages.AppVersion, objResp);

                    }
                    else
                    {
                        response.Create(false, Messages.InvalidReq, Messages.AppVersion, objResp);
                    }

                }
                else
                {
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);

                }

            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error(ex.Message, ex, session, ex.Message);
                response.Create(false, "Error occured while Processing Webservice request", Messages.AppVersion, objResp);
            }
            finally
            {
            }
            return response;
        }
        [HttpPost]
        public Response<StripeModel> GetStripeInfo(Entity objReq)
        {
            Response<StripeModel> response = new Response<StripeModel>();
            List<StripeModel> objResp = new List<StripeModel>();
            try
            {
                StripeMethods obj = new StripeMethods();
                if (CheckRequestIsvalidornot(this.Request))
                {
                    if (ModelState.IsValid)
                    {
                        objResp.Add(obj.GetStripeInfo(objReq));
                        if (objResp != null)
                            response.Create(true, "Stripe details", Messages.AppVersion, objResp);
                        else
                            response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
                    }
                    else
                        response.Create(false, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage, Messages.AppVersion, objResp);
                }
                else
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);

            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }
        [HttpPost]
        public Response<string> SaveStripeDetails(StripeModel objReq)
        {
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();
            try
            {
                StripeMethods obj = new StripeMethods();
                if (CheckRequestIsvalidornot(this.Request))
                {
                    if (ModelState.IsValid)
                    {
                        if (obj.SaveStripeDetails(objReq))
                            response.Create(true, Messages.FormatMessage(Messages.Success, "Stripe credentials saved"), Messages.AppVersion, objResp);
                        else
                            response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
                    }
                    else
                        response.Create(false, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage, Messages.AppVersion, objResp);
                }
                else
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);

            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }

        /// <summary>
        /// All payment realted Data
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> SavePaymentDetails(StripePaymentModel objReq)
        {
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();
            try
            {
                StripeMethods obj = new StripeMethods();
                if (CheckRequestIsvalidornot(this.Request))
                {
                    if (ModelState.IsValid)
                    {
                        if (obj.SavePaymentDetails(objReq) == 1)
                            response.Create(true, "Payment details saved successfully", Messages.AppVersion, objResp);
                        else if (obj.SavePaymentDetails(objReq) == 3)
                            response.Create(true, "Sorry Request time is expired", Messages.AppVersion, objResp);
                        else if (obj.SavePaymentDetails(objReq) == 2)
                            response.Create(true, "Already Paid", Messages.AppVersion, objResp);
                        else
                            response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
                    }
                    else
                        response.Create(false, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage, Messages.AppVersion, objResp);
                }
                else
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);

            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            return response;
        }




        /// <summary>
        /// [Common]<purpose>Applying PromoCode</purpose>
        /// <reqParameters> Parameters : promocode,userId
        ///  </reqParameters>
        /// <param name="objReq"></param>
        /// </summary>
        /// <returns></returns>
        /// <developer>Rishabh</developer>
        /// <Date>10/6/2015</Date>
        [HttpPost]
        public Response<string> ApplyPromoCode(PromoCodeReq objReq)
        {
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();
            try
            {
                PromoCodeMethods objDBMethod = new PromoCodeMethods();

                if (CheckRequestIsvalidornot(this.Request))
                {
                    if (ModelState.IsValid)
                    {
                        switch (objDBMethod.ApplyPromoCode(objReq))
                        {
                            case 0:
                                response.Create(false, "Invalid Promo code", Messages.AppVersion, objResp);
                                break;
                            case 1:
                                response.Create(true, "Promo code applied successfully", Messages.AppVersion, objResp);
                                break;
                            case 2:
                                response.Create(false, "Sorry you have already used this promo code !", Messages.AppVersion, objResp);
                                break;
                            case 3:
                                response.Create(false, "Sorry this promo code is expired!", Messages.AppVersion, objResp);
                                break;
                            default:
                                break;
                        }

                    }
                    else
                        response.Create(false, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage, Messages.AppVersion, objResp);
                }
                else
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);

            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            finally
            {
            }
            return response;
        }
        #endregion


        #region isAvailable
        [HttpPost]
        public Response<string> SetTutorAvailability(TutorAvailbilityModel objReq)
        {
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();
            try
            {
                objDbMethodV2 = new wizz.Dal.DbMethodsV2();

                if (CheckRequestIsvalidornot(this.Request))
                {
                    if (ModelState.IsValid)
                    {

                        //
                        if (objDbMethodV2.SetTutorAvailability(objReq))
                            response.Create(true, Messages.FormatMessage(Messages.Success, "user verified"), Messages.AppVersion, objResp);
                        else
                            response.Create(false, Messages.ErrorOccure, Messages.AppVersion, objResp);
                    }
                    else
                        response.Create(false, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage, Messages.AppVersion, objResp);
                }
                else
                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);

            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }

            return response;
        }
        #endregion
        #region Akanksha
        /// <summary>
        /// add friends
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> InviteFriends(InviteFriendsModel objReq)
        {
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();
            try
            {
                if (ModelState.IsValid)
                {

                    objDbMethodV2 = new wizz.Dal.DbMethodsV2();
                    int retValue = objDbMethodV2.InviteFriends(objReq);
                    if (retValue > 0)
                        response.Create(true, "done", Messages.AppVersion, objResp);
                    else
                    {
                        response.Create(false, "you are not allowed", Messages.AppVersion, objResp);
                    }
                }
                else
                    response.Create(false, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage, Messages.AppVersion, objResp);

            }
            catch (Exception ex)
            {
                object session = new JavaScriptSerializer().Serialize(objReq);
                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            finally
            {
            }
            return response;
        }

        #endregion
    }
}
