using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using wizz.Class;
using wizz.Models;


namespace wizz.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult VerifyEmail(string id)
        {
            if (!string.IsNullOrEmpty(id) && id != "Update")
            {
                var encrypted = id.Replace("_", "/").Replace("-", "+");
                string decrypted = Encryption.DecryptURL(encrypted);
                string[] objArray = decrypted.Split(',');
                string uid = objArray[0];
                string type = objArray[1];
                DBMethod objDBMethod = new DBMethod();
                var result = objDBMethod.VerifyEmail(Convert.ToInt64(uid), type);
                ViewBag.Result = result.ToString().ToLower();
            }
            if (id == "Update")
            {
                ViewBag.expire = "Update";
            }
            return View();
        }
        /// <summary>
        /// Link for reset password
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ResetPassword(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {

                DBMethod objDBMethod = new DBMethod();
                //id = objDBMethod.ReplacedSpecialCharacter(id, "");
                ViewBag.id = id;
                ViewBag.userName = objDBMethod.CheckReset(id);
                //ViewBag.isPasswordChanged = objDBMethod.CheckReset(id);
            }
            return View();

        }

    }

}
