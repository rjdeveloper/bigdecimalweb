using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using wizz.Class;

namespace wizz.Areas.Admin.Controllers
{
    
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
         [AllowAnonymous]
        public ActionResult Login()
        {
            // SendMail objSendMail = new SendMail();

            //objSendMail.SendEmail("mukti@techaheadcorp.com", Constants.AdminEmail, "wizz", "wizz", "test");
            return View();
        }
        public ActionResult PromoCodeManagement()
        {
            @ViewBag.BreadCrumb = "Manage Promo Codes";
            return View();
        }

        public ActionResult Dashboard()
        {

            @ViewBag.BreadCrumb = "Dashboard";
            return View();


        }

        public ActionResult ActivityManagement() {
            @ViewBag.BreadCrumb = "Activity management";
            return View();
        }

           [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
            //return View("Login");
        }
        public ActionResult ChangePassword() {

            @ViewBag.BreadCrumb = "Change Password";
            return View();
        }
        public ActionResult Courses()
        {
            @ViewBag.BreadCrumb = "Subjects Management";
            return View();

        }
        public ActionResult Settings()
        {
            @ViewBag.BreadCrumb = "Settings";
            return View();
        }

        public ActionResult Class()
        {
            @ViewBag.BreadCrumb = "Majors Management";
            return View();
        }
        public ActionResult UserManagement()
        {
            @ViewBag.BreadCrumb = "Student Management";
            return View();
        }
        public ActionResult TutorManagement()
        {
            @ViewBag.BreadCrumb = "Tutor Management";
            return View();
        }
        public ActionResult CollegeManagement()
        {
            @ViewBag.BreadCrumb = "College Management";
            return View();
        }
        public ActionResult MajorsManagement() {
            @ViewBag.BreadCrumb = "Majors Management";
            return View();
        
        }
        public ActionResult UpdateLatLong() {

            @ViewBag.BreadCrumb = "UpdateLatLong"; 
            
            return View();
        }
    }
}