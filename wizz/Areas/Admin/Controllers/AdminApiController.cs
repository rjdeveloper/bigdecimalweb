using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using wizz.Areas.Admin.AdminDal;
using wizz.Areas.Admin.Models;
using System.Web;
using wizz.Models;
using wizz.Class;

namespace wizz.Areas.Admin.Controllers
{
    internal class AdminApiController : ApiController
    {

//        #region Payal
        [HttpPost]
        public string Login(AdminModel objReq)
        {

            AdminMethods obj = new AdminMethods();
            return obj.adminLogin(objReq);
        }

//        [HttpPost]
//        public List<SettingsModel> GetAdminSettings(SettingsModel objReq)
//        {
//            AdminMethods obj = new AdminMethods();
//            return obj.GetAdminSettings(objReq);
//        }

//        [HttpPost]
//        public ResponseForAdmin<string> SaveAdminSettings(SettingsModel objReq)
//        {
//            ResponseForAdmin<string> response = new ResponseForAdmin<string>();
//            AdminMethods obj = new AdminMethods();
//            int result = obj.SaveAdminSettings(objReq);
//            if (result > 0)
//            {
//                response.ResponseForPost(true, Messages.FormatMessage(Messages.Success, "Settings saved"));
//            }
//            else
//            {
//                response.ResponseForPost(false, Messages.ErrorOccure);
//            }
//            return response;
//        }

//        [HttpPost]
//        public ResponseForAdmin<string> ChangePassword(AdminChangePassword objReq)
//        {
//            ResponseForAdmin<string> response = new ResponseForAdmin<string>();
//            AdminMethods obj = new AdminMethods();
//            int result = obj.ChangePassword(objReq);
//            if (result > 0)
//            {
//                response.ResponseForPost(true, Messages.FormatMessage(Messages.Success, "Password changed"));
//            }
//            else
//            {
//                response.ResponseForPost(false, Messages.ErrorOccure);
//            }
//            return response;
//        }

//        [HttpPost]
//        public int SaveClass(ClassModel objReq)
//        {
//            AdminMethods obj = new AdminMethods();
//            return obj.SaveClass(objReq);

//        }
//        [HttpPost]
//        public int SaveCollege(CollegeModel objReq)
//        {
//            AdminMethods obj = new AdminMethods();
//            return obj.SaveCollege(objReq);

//        }

//        [HttpGet]
//        public List<ClassModel> GetClassesData()
//        {
//            AdminMethods obj = new AdminMethods();
//            return obj.GetClassesData();
//        }
//        [HttpGet]
//        public List<CollegeModel> GetCollegesData()
//        {
//            AdminMethods obj = new AdminMethods();
//            return obj.GetCollegesData();
//        }

//        [HttpPost]
//        public ResponseForAdmin<string> ClassActions(ActionModel objReq)
//        {

//            ResponseForAdmin<string> response = new ResponseForAdmin<string>();
//            AdminMethods objDBMethod = new AdminMethods();
//            try
//            {

//                if (objDBMethod.ClassActions(objReq))
//                    response.ResponseForPost(true, Messages.FormatMessage(Messages.Success,  objReq.type == 2 ? "Major Deleted" : " Major status changed"));
//                else
//                    response.ResponseForPost(false, Messages.ErrorOccure);
//                return response;
//            }
//            catch
//            {
//                response.ResponseForPost(false, Messages.ErrorOccure);
//                return response;
//            }
//        }

//        [HttpPost]
//        public ResponseForAdmin<string> CollegeActions(ActionModel objReq)
//        {

//            ResponseForAdmin<string> response = new ResponseForAdmin<string>();
//            AdminMethods objDBMethod = new AdminMethods();
//            try
//            {

//                if (objDBMethod.CollegeActions(objReq))
//                    response.ResponseForPost(true, Messages.FormatMessage(Messages.Success,objReq.type==2? "College Deleted":" College status changed"));
//                else
//                    response.ResponseForPost(false, Messages.ErrorOccure);
//                return response;
//            }
//            catch
//            {
//                response.ResponseForPost(false, Messages.ErrorOccure);
//                return response;
//            }
//        }
//        #endregion
//        #region Rishabh
//        public ResponseForAdmin<string> PostPromo(PromoModel objReq)
//        {
//            ResponseForAdmin<string> response = new ResponseForAdmin<string>();
//            AdminMethods objDBMethod = new AdminMethods();
//            try
//            {

//                if (objDBMethod.SavePromoCode(objReq))
//                    response.ResponseForPost(true, Messages.FormatMessage(Messages.Success, "Promo code saved"));
//                else
//                    response.ResponseForPost(false, "An error occured please note: No duplicate promocode should be added");
//                return response;
//            }
//            catch
//            {
//                return response;
//            }
//        }
//        public ResponseForAdmin<List<PromoModel>> GetPromoCodes()
//        {

//            ResponseForAdmin<List<PromoModel>> response = new ResponseForAdmin<List<PromoModel>>();
//            List<PromoModel> promoList = new List<PromoModel>();
//            AdminMethods objDBMethod = new AdminMethods();
//            try
//            {

//                promoList = objDBMethod.GetPromoCodes();

//                if (promoList != null)
//                    response.ResponseForGet(true, Messages.Success, promoList);
//                else
//                    response.ResponseForGet(false, Messages.NoRecord, promoList);
//                return response;
//            }
//            catch
//            {
//                return response;
//            }
//        }
//        public ResponseForAdmin<string> PostPromoActions(ActionModel objReq)
//        {

//            ResponseForAdmin<string> response = new ResponseForAdmin<string>();
//            AdminMethods objDBMethod = new AdminMethods();
//            try
//            {
//                if (objDBMethod.PromoActions(objReq))
//                    response.ResponseForPost(true, Messages.FormatMessage(Messages.Success, objReq.type == 2 ? "Promo Code Deleted" : " Promo Code status changed"));
//                else
//                    response.ResponseForPost(false, Messages.ErrorOccure);
//                return response;
//            }
//            catch
//            {
//                response.ResponseForPost(false, Messages.ErrorOccure);
//                return response;
//            }
//        }

//        #region Courses
//        public ResponseForAdmin<string> PostCourse(CourseModel objReq)
//        {
//            ResponseForAdmin<string> response = new ResponseForAdmin<string>();
//            AdminMethods objDBMethod = new AdminMethods();
//            try
//            {

//                if (objDBMethod.SaveCourse(objReq))
//                    response.ResponseForPost(true, Messages.FormatMessage(Messages.Success, "Course Saved"));
//                else
//                    response.ResponseForPost(false, Messages.ErrorOccure);
//                return response;
//            }
//            catch
//            {
//                return response;
//            }
//        }
//        public ResponseForAdmin<List<CourseModel>> GetCourses()
//        {

//            ResponseForAdmin<List<CourseModel>> response = new ResponseForAdmin<List<CourseModel>>();
//            List<CourseModel> courseList = new List<CourseModel>();
//            AdminMethods objDBMethod = new AdminMethods();
//            try
//            {

//                courseList = objDBMethod.GetCourses();

//                if (courseList != null)
//                    response.ResponseForGet(true, Messages.Success, courseList);
//                else
//                    response.ResponseForGet(false, Messages.NoRecord, courseList);
//                return response;
//            }
//            catch
//            {
//                return response;
//            }
//        }

//        public ResponseForAdmin<List<MajorModel>> GetMajors()
//        {

//            ResponseForAdmin<List<MajorModel>> response = new ResponseForAdmin<List<MajorModel>>();
//            List<MajorModel> majorsList = new List<MajorModel>();
//            AdminMethods objDBMethod = new AdminMethods();
//            try
//            {

//                majorsList = objDBMethod.GetMajors();

//                if (majorsList != null)
//                    response.ResponseForGet(true, Messages.Success, majorsList);
//                else
//                    response.ResponseForGet(false, Messages.NoRecord, majorsList);
//                return response;
//            }
//            catch
//            {
//                return response;
//            }
//        }
        
//        public ResponseForAdmin<List<ClassModel>> GetClasses()
//        {

//            ResponseForAdmin<List<ClassModel>> response = new ResponseForAdmin<List<ClassModel>>();
//            List<ClassModel> courseList = new List<ClassModel>();
//            AdminMethods objDBMethod = new AdminMethods();
//            try
//            {

//                courseList = objDBMethod.GetClasses();

//                if (courseList != null)
//                    response.ResponseForGet(true, Messages.Success, courseList);
//                else
//                    response.ResponseForGet(false, Messages.NoRecord, courseList);
//                return response;
//            }
//            catch
//            {
//                return response;
//            }
//        }
//        public ResponseForAdmin<string> PostCourseActions(ActionModel objReq)
//        {

//            ResponseForAdmin<string> response = new ResponseForAdmin<string>();
//            AdminMethods objDBMethod = new AdminMethods();
//            try
//            {

//                if (objDBMethod.CourseActions(objReq))
//                    response.ResponseForPost(true, Messages.FormatMessage(Messages.Success, objReq.type == 2 ? "Subject Deleted" : " Subject status changed"));
//                else
//                    response.ResponseForPost(false, "A tutor has already applied for this subject");
//                return response;
//            }
//            catch
//            {
//                response.ResponseForPost(false, Messages.ErrorOccure);
//                return response;
//            }
//        }
//        #endregion
//        #region User Management
//        public ResponseForAdmin<List<UserAdminModel>> GetUsers()
//        {

//            ResponseForAdmin<List<UserAdminModel>> response = new ResponseForAdmin<List<UserAdminModel>>();
//            List<UserAdminModel> userList = new List<UserAdminModel>();
//            AdminMethods objDBMethod = new AdminMethods();
//            try
//            {

//                userList = objDBMethod.GetStudents();

//                if (userList != null)
//                    response.ResponseForGet(true, Messages.Success, userList);
//                else
//                    response.ResponseForGet(false, Messages.NoRecord, userList);
//                return response;
//            }
//            catch
//            {
//                return response;
//            }
//        }
//        public ResponseForAdmin<List<TutorDescriptionModel>> GetTutors()
//        {

//            ResponseForAdmin<List<TutorDescriptionModel>> response = new ResponseForAdmin<List<TutorDescriptionModel>>();
//            List<TutorDescriptionModel> userList = new List<TutorDescriptionModel>();
//            AdminMethods objDBMethod = new AdminMethods();
//            try
//            {

//                userList = objDBMethod.GetTutors();

//                if (userList != null)
//                    response.ResponseForGet(true, Messages.Success, userList);
//                else
//                    response.ResponseForGet(false, Messages.NoRecord, userList);
//                return response;
//            }
//            catch
//            {
//                return response;
//            }
//        }
//        public ResponseForAdmin<string> PostUserActions(ActionModel objReq)
//        {

//            ResponseForAdmin<string> response = new ResponseForAdmin<string>();
//            AdminMethods objDBMethod = new AdminMethods();
//            try
//            {

//                if (objDBMethod.UserActions(objReq))
//                    response.ResponseForPost(true, Messages.FormatMessage(Messages.Success,  objReq.type == 2 ? "User Deleted" : " User status changed"));
//                else
//                    response.ResponseForPost(false, Messages.ErrorOccure);
//                return response;
//            }
//            catch
//            {
//                response.ResponseForPost(false, Messages.ErrorOccure);
//                return response;
//            }
//        }

//        #endregion

//        #region tutorManagement
 
//        public Response<SettingModel> GetSettingsForAdmin()
//        {
//            Response<SettingModel> response = new Response<SettingModel>();
//            List<SettingModel> objResp = new List<SettingModel>();


//            try
//            {
//                AdminMethods objDBMethod = new AdminMethods();
//                objResp.Add(objDBMethod.GetSettingsForAdmin());
//                response.Create(true, "Settings", Messages.AppVersion, objResp);
//            }
//            catch (Exception ex)
//            {

//                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, ex.Message);
//                response.Create(false, Messages.FormatMessage(Messages.ErrorOccure), Messages.AppVersion, objResp);
//            }
//            return response;
//        }

//        [HttpPost]
//        public ResponseForAdmin<List<TutorSubjectModel>> GetTutorSubjects(Entity Model)
//        {

//            ResponseForAdmin<List<TutorSubjectModel>> response = new ResponseForAdmin<List<TutorSubjectModel>>();
         
//            List<TutorSubjectModel> objResp = new List<TutorSubjectModel>();


//            try
//            {
//                AdminMethods objDBMethod = new AdminMethods();
//                objResp = objDBMethod.GetTutorSubjects(Model.userId);
//                response.ResponseForGet(true, "tutor Subjects", objResp);
//            }
//            catch (Exception ex)
//            {

//                LogManager.Error("Error occured while Processing Webservice request :{0}", ex, ex.Message);
//                response.ResponseForGet(true, "tutor Subjects", objResp);
//            }
//            return response;
//        }

//        public ResponseForAdmin<string> PostTutorSubjects(TutorSubjectsModel objReq)
//        {

//            ResponseForAdmin<string> response = new ResponseForAdmin<string>();
//            AdminMethods objDBMethod = new AdminMethods();
//            try
//            {

//                if (objDBMethod.ApproveTutorSubjects(objReq))
//                    response.ResponseForPost(true, Messages.FormatMessage(Messages.Success, "Subjects approved"));
//                else
//                    response.ResponseForPost(false, "An error occured ");
//                return response;
//            }
//            catch
//            {
//                return response;
//            }
      
//        }
//        #endregion


//        #endregion
    }
}