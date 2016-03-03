using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wizz.Areas.Admin.Models
{
    public class ClassModel
    {
        public int pkClassId { get; set; }
        public int fkCollegeId { get; set; }
        public string className { get; set; }
        public bool isActive { get; set; }
        public bool isDelete { get; set; }
        public string createdDate { get; set; }
        public string updatedDate { get; set; }
    }
    public class CollegeModel
    {
        public int pkCollegeId { get; set; }
        public string collegeName { get; set; }
        public bool isActive { get; set; }
        public bool isDelete { get; set; }
        public string createdDate { get; set; }
        public string updatedDate { get; set; }
    }
}