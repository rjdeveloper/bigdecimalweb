using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wizz.Areas.Admin.Models
{
    public class CourseModel
    {
        public int fkClassId { get; set; }
        public bool isActive { get; set; }
        public bool isDelete { get; set; }
        public int pkCourseId { get; set; }
        public string courseName { get; set; }
        public string sno { get; set; }
    }
    public class MajorModel
    {
        public int fkCollegeId { get; set; }
        public bool isActive { get; set; }
        public bool isDelete { get; set; }
        public int pkMajorId { get; set; }
        public string majorName { get; set; }
        public string sno { get; set; }
    }
}