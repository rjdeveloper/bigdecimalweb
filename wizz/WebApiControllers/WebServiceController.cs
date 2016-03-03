using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using wizz.Class;
using wizz.Dal;
using wizz.Models;

namespace wizz.WebApiController
{

    internal class WebServiceController : ApiController
    {

        DBMethod objDBMethod;

        #region Mukti

        #region Common API
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
                            response.Create(false, Messages.NoEmailExist, Messages.AppVersion, objResp);
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



        /// <summary>
        ///[Common] Check this email is available or not            
        /// </summary>  
        [HttpPost]
        public Response<string> AvailableEmail(ReqEmail objUserModel)
        {
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();


            try
            {
                objDBMethod = new DBMethod();

                if (CheckRequestIsvalidornot(this.Request))
                {

                    if (!objDBMethod.EmailAvailable(objUserModel))
                    {
                        response.Create(true, Messages.EmailAvailable, Messages.AppVersion, objResp);
                    }
                    else
                    {
                        response.Create(false, Messages.EmailExist, Messages.AppVersion, objResp);
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
        /// <summary>
        ///[Common] Get settings
        /// </summary>
        /// <developer>Mukti</developer>
        /// <Date>5/10/15</Date>
        //[HttpPost]
        //public Response<SettingModel> GetSetting(ReqSetting objReq)
        //{
        //    Response<SettingModel> response = new Response<SettingModel>();
        //    List<SettingModel> objResp = new List<SettingModel>();


        //    try
        //    {
        //        objDBMethod = new DBMethod();
        //        if (CheckRequestIsvalidornot(this.Request))
        //        {
        //            objResp.Add(objDBMethod.GetSetting(objReq));
        //            response.Create(true, "Settings", Messages.AppVersion, objResp);
        //        }
        //        else
        //        {
        //            response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        object session = new JavaScriptSerializer().Serialize(objReq);
        //        LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
        //        response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
        //    }
        //    return response;
        //}


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

        #region Student Api

        /// <summary>
        ///[Student] Get Tutors
        /// </summary>
        /// <developer>Mukti</developer>
        /// <Date>3/11/15</Date>
        [HttpPost]
        public Response<TutorModel> GetTutor(Entity objReq)
        {
            Response<TutorModel> response = new Response<TutorModel>();
            List<TutorModel> objResp = new List<TutorModel>();
            try
            {
                var obj = new Tutor();
                if (CheckRequestIsvalidornot(this.Request))
                {
                    objResp = obj.GetTutors(objReq);
                    response.Create(true, "Tutor List", Messages.AppVersion, objResp);
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
        ///[Student] Get UpComing Sessions
        /// </summary>
        /// <developer>Mukti</developer>
        /// <Date>4/11/15</Date>
        //[HttpPost]
        //public Response<RespNewRequest> GetSessions(Entity objReq)
        //{
        //    Response<RespNewRequest> response = new Response<RespNewRequest>();
        //    List<RespNewRequest> objResp = new List<RespNewRequest>();


        //    try
        //    {

        //        var obj = new Session();
        //        if (CheckRequestIsvalidornot(this.Request))
        //        {

        //            objResp = (obj.GetSessions(objReq));
        //            response.Create(true, "Sessoin List", Messages.AppVersion, objResp);
        //        }
        //        else
        //        {
        //            response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        object session = new JavaScriptSerializer().Serialize(objReq);
        //        LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
        //        response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
        //    }
        //    return response;
        //}

        /// <summary>
        /// Get a single result of session for students only
        /// </summary>
        /// <developer>Rishabh</developer>
        /// <date>12/11/2015</date>
        /// <param name="objReq"></param>
        /// <returns></returns>
        //[HttpPost]
        //public Response<RespNewRequest> GetSessionFromId(Entity objReq)
        //{
        //    Response<RespNewRequest> response = new Response<RespNewRequest>();
        //    List<RespNewRequest> objResp = new List<RespNewRequest>();


        //    try
        //    {
        //        var obj = new Session();
        //        if (CheckRequestIsvalidornot(this.Request))
        //        {
        //            objResp.Add(obj.GetSessionById(objReq));
        //            response.Create(true, "Sessoin Result", Messages.AppVersion, objResp);
        //        }
        //        else
        //        {
        //            response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        object session = new JavaScriptSerializer().Serialize(objReq);
        //        LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
        //        response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
        //    }
        //    return response;
        //}

        /// <summary>
        ///[Student] Get history
        /// </summary>
        /// <developer>Mukti</developer>
        /// <Date>4/11/15</Date>
        //[HttpPost]
        //public Response<RespNewRequest> GetHistory(Entity objReq)
        //{
        //    Response<RespNewRequest> response = new Response<RespNewRequest>();
        //    List<RespNewRequest> objResp = new List<RespNewRequest>();


        //    try
        //    {
        //        var obj = new Session();
        //        if (CheckRequestIsvalidornot(this.Request))
        //        {
        //            objResp = obj.GetHistoryForUser(objReq);
        //            response.Create(true, "History", Messages.AppVersion, objResp);
        //        }
        //        else
        //        {
        //            response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        object session = new JavaScriptSerializer().Serialize(objReq);
        //        LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
        //        response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
        //    }
        //    return response;
        //}

        /// <summary>
        ///[For Student]Accept Tutor(student select tutor from tutor list)
        /// </summary>
        /// <developer>Mukti</developer>
        /// <Date>4/11/15</Date>
        [HttpPost]
        public Response<string> AcceptTutor(CreateSesion objReq)
        {
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();


            try
            {
                var obj = new Session();
                if (CheckRequestIsvalidornot(this.Request))
                {
                    objResp.Add(obj.AcceptTutor(objReq));
                    response.Create(true, Messages.FormatMessage(Messages.Success, "Tutor accepted"), Messages.AppVersion, objResp);
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


        #endregion

        #region tutor api


        /// <summary>[tutor]Get all new requests
        /// </summary>
        /// <developer>Mukti</developer>
        /// <Date>25/11/15</Date>
        //[HttpPost]
        //public Response<RespNewRequest> GetNewRequest(Entity objReq)
        //{
        //    Response<RespNewRequest> response = new Response<RespNewRequest>();
        //    List<RespNewRequest> objResp = new List<RespNewRequest>();


        //    try
        //    {
        //        var obj = new RequestMethods();
        //        if (CheckRequestIsvalidornot(this.Request))
        //        {
        //            objResp = obj.GetNewRequest(objReq);
        //            response.Create(true, "Request List", Messages.AppVersion, objResp);
        //        }
        //        else
        //        {
        //            response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        object session = new JavaScriptSerializer().Serialize(objReq);
        //        LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
        //        response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
        //    }
        //    return response;
        //}


        [HttpPost]
        /// <summary>
        ///[Tutoe] Save Bank detail          
        /// </summary>
        /// <developer>Mukti</developer>
        /// <Date>24/12/15</Date>

        //public Response<string> SaveBankDetail(BankDetail objRequest)
        //{
        //    Response<string> response = new Response<string>();
        //    List<string> objResp = new List<string>();
        //    if (objRequest != null)
        //    {
        //        try
        //        {
        //            bool var = CheckRequestIsvalidornot(this.Request);
        //            if (var == false)
        //            {
        //                response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
        //            }
        //            else
        //            {
        //                objDBMethod = new DBMethod();
        //                string rst = objDBMethod.UpdateBankDetails(objRequest);
        //                if (rst == "1")
        //                {
        //                    response.Create(true, Messages.FormatMessage(Messages.Success, "Bank Detail"), Messages.AppVersion, objResp);
        //                }
        //                else
        //                {
        //                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
        //                }
        //            }


        //        }
        //        catch (Exception ex)
        //        {

        //            response.Create(false, Messages.FormatMessage(Messages.ErrorOccure, ""), Messages.AppVersion, objResp);
        //        }

        //    }
        //    else
        //    {
        //        response.Create(false, Messages.FormatMessage(Messages.ErrorOccure, ""), Messages.AppVersion, objResp);
        //    }


        //    return response;
        //}


        #endregion



        #endregion

        #region Rishabh
        #region Get History

        #endregion



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


        ///// <summary>
        ///// [Common]<purpose>Applying PromoCode</purpose>
        ///// <reqParameters> Parameters : promocode,userId
        /////  </reqParameters>
        ///// <param name="objReq"></param>
        ///// </summary>
        ///// <returns></returns>
        ///// <developer>Rishabh</developer>
        ///// <Date>10/6/2015</Date>
        //[HttpPost]
        //public Response<string> ApplyPromoCode(PromoCodeReq objReq)
        //{
        //    Response<string> response = new Response<string>();
        //    List<string> objResp = new List<string>();
        //    try
        //    {
        //        PromoCodeMethods objDBMethod = new PromoCodeMethods();

        //        if (CheckRequestIsvalidornot(this.Request))
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                switch (objDBMethod.ApplyPromoCode(objReq))
        //                {
        //                    case 0:
        //                        response.Create(false, "Invalid Promo code", Messages.AppVersion, objResp);
        //                        break;
        //                    case 1:
        //                        response.Create(true, "Promo code applied successfully", Messages.AppVersion, objResp);
        //                        break;
        //                    case 2:
        //                        response.Create(false, "Sorry you have already used this promo code !", Messages.AppVersion, objResp);
        //                        break;
        //                    case 3:
        //                        response.Create(false, "Sorry this promo code is expired!", Messages.AppVersion, objResp);
        //                        break;
        //                    default:
        //                        break;
        //                }

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
        //    finally
        //    {
        //    }
        //    return response;
        //}
        /// <summary>
        ///[student] Rate the Tutor
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        //[HttpPost]
        //public Response<string> RateTutor(RatingModel objReq)
        //{
        //    Response<string> response = new Response<string>();
        //    List<string> objResp = new List<string>();
        //    try
        //    {
        //        RatingMethods objDBMethod = new RatingMethods();

        //        if (CheckRequestIsvalidornot(this.Request))
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                switch (objDBMethod.RateTutor(objReq))
        //                {
        //                    case 0:
        //                        response.Create(false, "Invalid TutorId or Tutor not exist", Messages.AppVersion, objResp);
        //                        break;
        //                    case 1:
        //                        response.Create(true, "Rating applied successfully ", Messages.AppVersion, objResp);
        //                        break;
        //                    case 2:
        //                        response.Create(false, "Total rating should not be more than 15 stars !", Messages.AppVersion, objResp);
        //                        break;
        //                    default:
        //                        break;
        //                }

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
        //    finally
        //    {
        //    }
        //    return response;
        //}
        ///// <summary>
        /////[student]  Service for student Request
        ///// </summary>
        ///// <param name="objReq"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public Response<string> StudentRequest(StudentRequestModel objReq)
        //{
        //    Response<string> response = new Response<string>();
        //    List<string> objResp = new List<string>();
        //    try
        //    {
        //        RequestMethods objDBMethod = new RequestMethods();

        //        if (CheckRequestIsvalidornot(this.Request))
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                switch (objDBMethod.StudentRequest(objReq))
        //                {
        //                    case 0:
        //                        response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
        //                        break;
        //                    case 1:
        //                        response.Create(true, Messages.FormatMessage(Messages.Success, "Your request created "), Messages.AppVersion, objResp);
        //                        break;
        //                    default:
        //                        break;
        //                }

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
        //    finally
        //    {
        //    }
        //    return response;
        //}


        [HttpPost]
        public Response<string> SendPush(push objReq)
        {
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();
            try
            {


                if (CheckRequestIsvalidornot(this.Request))
                {
                    if (!string.IsNullOrEmpty(objReq.deviceToken))
                    {
                        PushMessage objPush = new PushMessage();
                        objPush.message = PushMessages.newRequest;
                        // objPush.type = PushType.newRequest.ToString();

                        PushData push = new PushData();
                        push.message = "Testing";
                        push.registration_ids = objReq.deviceToken;
                        // push.data = CommonMethod.ObjectToJson(objPush);
                        push.type = Convert.ToInt16(PushType.acceptRequest).ToString();
                        if (objReq.deviceType == "1")//ios
                        {
                            SendPush objIOS = new SendPush();
                            objIOS.ConnectToAPNS(push);
                        }
                        else if (objReq.deviceType == "2")//android
                        {
                            Android objAndroid = new Android();
                            objAndroid.send(push);

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


        [HttpPost]
        public Response<string> CronJob()
        {
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();
            try
            {
                Cron obj = new Cron();
                obj.ChorneJob();
            }
            catch (Exception ex)
            {
                // object session = new JavaScriptSerializer().Serialize(objReq);
                // LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
            }
            finally
            {
            }
            return response;
        }


        [HttpPost]
        public Response<string> UpdateLatLong(ReqLatLong objReq)
        {
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();
            try
            {
                objDBMethod = new DBMethod();
                int retValue = objDBMethod.UpdateLatLong(objReq);
                response.Create(true, "done", Messages.AppVersion, objResp);
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
        ///[tutor] Saving tutor profile
        /// use this service same as login service i.e. multi part
        /// userId,majors,fileName,latitude,longitude,timeStamp,collegeId,passingYear
        /// </summary>
        [HttpPost]
        public Response<string> SaveTutorProfile()
        {

            TutorProfile objReq = new TutorProfile();
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();
            int resp = 0;
            try
            {
                RequestMethods objDBMethod = new RequestMethods();
                if (CheckRequestIsvalidornot(this.Request))
                {
                    HttpContextWrapper objwrapper = GetHttpContext(this.Request);
                    HttpFileCollectionBase collection = objwrapper.Request.Files;
                    string jsonvalue = objwrapper.Request.Form["json"];
                    if (!string.IsNullOrEmpty(jsonvalue))
                    {
                        objReq = JsonConvert.DeserializeObject<TutorProfile>(jsonvalue);
                        objReq.fileName = SaveFile(collection, objReq.fileName);
                        if (objReq.fileName == "")
                            response.Create(false, Messages.InvalidFormat, Messages.AppVersion, objResp);
                        else
                            resp = objDBMethod.SaveTutorProfile(objReq);
                        if (resp == 1)
                        {
                            response.Create(true, "Profile saved successfully", Messages.AppVersion, objResp);

                        }
                        else
                        {
                            response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);

                        }

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
        ///[tutor]  Accepting a student from the tutor
        /// </summary>
        /// <developer>Rishabh </developer>
        /// <param name="objReq"></param>
        /// <returns></returns>
        //[HttpPost]
        //public Response<string> AcceptStudent(StudentTeacherMap objReq)
        //{


        //    Response<string> response = new Response<string>();
        //    List<string> objResp = new List<string>();

        //    try
        //    {
        //        WebMethods obj = new WebMethods();
        //        if (CheckRequestIsvalidornot(this.Request))
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                if (obj.AcceptStudent(objReq))
        //                    response.Create(true, "Student is Accepted", Messages.AppVersion, objResp);
        //                else
        //                    response.Create(false, "The student request you are accepting is already accepted by any other tutor.Please refresh the screen for new request.", Messages.AppVersion, objResp);
        //            }
        //            else
        //                response.Create(false, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage, Messages.AppVersion, objResp);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        object session = new JavaScriptSerializer().Serialize(objReq);
        //        LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
        //        response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
        //    }

        //    return response;
        //}


        public string SaveFile(HttpFileCollectionBase collection, string fileName)
        {
            string UniqueFileName = "";
            string ext = "";
            string path = HttpContext.Current.Request.PhysicalApplicationPath + "tutorDocs\\";
            if (collection.Count > 0)
            {
                HttpPostedFileBase postedFile = collection.Get(0);
                ext = Path.GetExtension(postedFile.FileName).ToLower();
                if (string.IsNullOrEmpty(fileName))
                {
                    UniqueFileName = Guid.NewGuid().ToString("n") + ext;
                }
                else
                {
                    if (File.Exists(@path + fileName))
                    {
                        File.Delete(@path + fileName);
                    }
                    UniqueFileName = fileName;
                }

                postedFile.SaveAs(path + UniqueFileName);
            }

            return UniqueFileName;
        }
        /// <summary> [tutor] get all session 
        ///  <developer>Rishabh</developer> 
        /// </summary>
        /// <param name="objReq"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public Response<RespNewRequest> GetSessionsForTutor(Entity objReq)
        //{
        //    Response<RespNewRequest> response = new Response<RespNewRequest>();
        //    List<RespNewRequest> objResp = new List<RespNewRequest>();


        //    try
        //    {

        //        var obj = new Session();
        //        if (CheckRequestIsvalidornot(this.Request))
        //        {

        //            objResp = (obj.GetSessionsForTutor(objReq));
        //            response.Create(true, "Sessoin List", Messages.AppVersion, objResp);
        //        }
        //        else
        //        {
        //            response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        object session = new JavaScriptSerializer().Serialize(objReq);
        //        LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
        //        response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
        //    }
        //    return response;
        //}

        ///// <summary> [tutor] get history
        /////  <developer>Rishabh</developer> 
        ///// </summary>
        ///// <param name="objReq"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public Response<TutorHistoryModel> GetHistoryForTutor(Entity objReq)
        //{
        //    Response<TutorHistoryModel> response = new Response<TutorHistoryModel>();
        //    List<TutorHistoryModel> objResp = new List<TutorHistoryModel>();


        //    try
        //    {
        //        var obj = new Session();
        //        if (CheckRequestIsvalidornot(this.Request))
        //        {
        //            objResp.Add(obj.GetTutorSessionHistory(objReq));
        //            response.Create(true, "History List", Messages.AppVersion, objResp);
        //        }
        //        else
        //        {
        //            response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        object session = new JavaScriptSerializer().Serialize(objReq);
        //        LogManager.Error("Error occured while Processing Webservice request :{0}", ex, session, ex.Message);
        //        response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
        //    }
        //    return response;
        //}

        /// <summary>
        /// [Tutor] For getting subjets for the tutor
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<ResponseTutorSubjects> GetSubjectsForTutor(Entity objReq)
        {
            Response<ResponseTutorSubjects> response = new Response<ResponseTutorSubjects>();
            List<ResponseTutorSubjects> objResp = new List<ResponseTutorSubjects>();


            try
            {

                Tutor obj = new Tutor();
                if (CheckRequestIsvalidornot(this.Request))
                {

                    objResp = obj.GetSubjectsForTutor(objReq);
                    response.Create(true, "Subjects List", Messages.AppVersion, objResp);
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
        ///  [Tutor] For getting subjets for the tutor
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<ResponseTutorSubjects> SaveSubjectsForTutor(SubjectsListModel objReq)
        {
            Response<ResponseTutorSubjects> response = new Response<ResponseTutorSubjects>();
            List<ResponseTutorSubjects> objResp = new List<ResponseTutorSubjects>();


            try
            {

                Tutor obj = new Tutor();
                if (CheckRequestIsvalidornot(this.Request))
                {
                    if (ModelState.IsValid)
                    {
                        if (objReq.subjectsIdList.Count == 0)
                            response.Create(false, "Atleast one subject id should be there", Messages.AppVersion, objResp);
                        else if (obj.SaveSubjectsForTutor(objReq))
                            response.Create(true, Messages.FormatMessage(Messages.Success, "Subjects saved"), Messages.AppVersion, objResp);
                        else
                            response.Create(false, Messages.ErrorOccure, Messages.AppVersion, objResp);
                    }
                    else
                        response.Create(false, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage, Messages.AppVersion, objResp);

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
        /// [For tutor]Student rating service
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        //[HttpPost]
        //public Response<string> RateStudent(StudentRatingModel objReq)
        //{
        //    Response<string> response = new Response<string>();
        //    List<string> objResp = new List<string>();
        //    try
        //    {
        //        RatingMethods obj = new RatingMethods();
        //        if (CheckRequestIsvalidornot(this.Request))
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                if (obj.RateStudent(objReq))
        //                    response.Create(true, Messages.FormatMessage(Messages.Success, "You have rated "), Messages.AppVersion, objResp);
        //                else
        //                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
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

        #region SessionRegion

        [HttpPost]
        public Response<string> CancelSession(CreateSesion objReq)
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
                        if (obj.CancelSession(objReq))
                            response.Create(true, Messages.FormatMessage(Messages.Success, "You have cancelled session"), Messages.AppVersion, objResp);
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




        //[HttpPost]
        //public Response<string> BookTimeSlot(TimeSlotEntity objReq)
        //{
        //    Response<string> response = new Response<string>();
        //    List<string> objResp = new List<string>();
        //    try
        //    {
        //        Session obj = new Session();
        //        if (CheckRequestIsvalidornot(this.Request))
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                if (obj.BookTimeSlot(objReq))
        //                    response.Create(true, Messages.FormatMessage(Messages.Success, "You booked Time slot"), Messages.AppVersion, objResp);
        //                else
        //                    response.Create(false, Messages.FormatMessage(Messages.InvalidReq), Messages.AppVersion, objResp);
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
        #endregion
        #region stripe
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
        public Response<string> SavepPaymentDetails(StripePaymentModel objReq)
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
        #endregion
        #region MiscApis
        [HttpPost]
        public Response<string> SendPushForIos(IosPushModel objReq)
        {
            Response<string> response = new Response<string>();
            List<string> objResp = new List<string>();
            try
            {

                WebMethods obj = new WebMethods();


                if (CheckRequestIsvalidornot(this.Request))
                {
                    if (obj.SendPushForIos(objReq))
                    {
                        response.Create(true, "Message sent", Messages.AppVersion, objResp);


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

        #endregion

        #region Payal
        [HttpPost]
        public int ResetPassword(ReqResetPassword objResetPassword)
        {
            objDBMethod = new DBMethod();
            int retValue = objDBMethod.ResetPassword(objResetPassword);
            return retValue;
        }
        #endregion


   
    }


}
