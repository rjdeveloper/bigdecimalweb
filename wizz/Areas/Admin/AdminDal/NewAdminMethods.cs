using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using wizz.Areas.Admin.Models;
using wizz.Class;
using wizz.Models;

namespace wizz.Areas.Admin.AdminDal
{

    public class NewAdminMethods
    {
       
        internal string adminLogin(AdminModel objReq)
        {
            string i = "0";
            try
            {
                using (var db = new WizzDataContext())
                {
                    tblUser admin = new tblUser();
                    if (objReq != null)
                    {
                        admin = db.tblUsers.Where(e => e.userEmail == objReq.email && e.registerType == 1).FirstOrDefault();
                        if (admin != null)
                        {
                            if (objReq.password.ToLower() == admin.password)
                            {

                                i = "loggedIn";
                                CurrentUser user = new CurrentUser()
                                {
                                    PkUserId = Convert.ToString(admin.pkUserId),
                                    Email = admin.userEmail,
                                    isActive = Convert.ToString(admin.isActive),
                                    usertype = Convert.ToString(admin.registerType),
                                };
                                string json = JsonConvert.SerializeObject(user);
                                FormsAuthentication.SetAuthCookie(json, true);

                            }
                            else
                            {
                                i = "Invalid password !";
                            }
                        }
                        else
                        {
                            i = "The Email Id is not registered with us !";
                        }
                    }
                    return i;
                }
            }
            catch (Exception ex)
            {
                i = ex.ToString();
                return i;
            }
        }
        internal int ChangePassword(AdminChangePassword objReq)
        {
            using (var db = new WizzDataContext())
            {

                tblUser tUser = new tblUser();
                CurrentUser cu = new CurrentUser();
                tUser = db.tblUsers.Where(x => x.pkUserId == Convert.ToInt32(cu.PkUserId)).FirstOrDefault();
                if (tUser.password != objReq.oldPassword)
                    return 0;
                else
                {
                    tUser.password = objReq.newPassword.Trim();
                    db.SubmitChanges();
                    return 1;
                }
            }

        }
    }
}