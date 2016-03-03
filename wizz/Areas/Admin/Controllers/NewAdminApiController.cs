using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using wizz.Areas.Admin.AdminDal;
using wizz.Areas.Admin.Models;

namespace wizz.Areas.Admin.Controllers
{
    internal class NewAdminApiController : ApiController
    {
        [HttpPost]
        public string Login(AdminModel objReq)
        {

            NewAdminMethods obj = new NewAdminMethods();
            return obj.adminLogin(objReq);
        }
    }
}
