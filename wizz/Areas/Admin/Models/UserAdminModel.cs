using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wizz.Areas.Admin.Models
{
    public class UserAdminModel
    {
        public int pkUserId { get; set; }
        public string userName { get; set; }
        public string userEmail { get; set; }
        public string profilePic { get; set; }
        public int credits { get; set; }
        public bool isActive { get; set; }
        public bool isDelete { get; set; }
        public string sno { get; set; }
        public bool isTutor { get; set; }
        public bool isVarified { get; set; }


    }
    public class TutorDescriptionModel
    {
        public int pkUserId { get; set; }
        public string docUrl { get; set; }
        public string userEmail { get; set; }
        public string passingYear { get; set; }
        public int fkClassId { get; set; }
        public int fkCollegeId { get; set; }
        public string subjects { get; set; }
        public string sno { get; set; }
        public string userName { get; set; }
        public bool isApproved { get; set; }
        public string profilePic { get; set; }
    }
}