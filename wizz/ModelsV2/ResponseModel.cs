using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wizz.ModelsV2
{
    public class ResponseModel
    {
    }
    public class GetSettingsModel
    {
        public GetSettingsModel()
        {
            this.listAcademicCourse = new List<AcademicCoursesModel>();
            this.listCurricularCourses = new List<CurricularCoursesModel>();
        }


        public List<AcademicCoursesModel> listAcademicCourse { get; set; }
        public List<CurricularCoursesModel> listCurricularCourses { get; set; }



    }
    public class CollegesModel
    {
        public string collegeId { get; set; }
        public string collegeName { get; set; }

    }

    public class CurricularCoursesModel
    {
        public CurricularCoursesModel() {
           // this.isApplied = "false";
            this.isApproved = "false";
        
        }
        public string courseId { get; set; }
        public string courseName { get; set; }
        public List<ClassesModel> listClasses { get; set; }
        public string isApproved { get; set; }
       // public string isApplied { get; set; }

    }
    public class AcademicCoursesModel
    {
        public AcademicCoursesModel()
        {
           // this.isApplied = "false";
            this.isApproved = "false";
        
        }
        public string courseId { get; set; }
        public string courseName { get; set; }
        public string isApproved { get; set; }
       // public string isApplied { get; set; }
        public List<ClassesModel> listClasses { get; set; }

    }
    public class ClassesModel
    {
        public ClassesModel()
        {
           // this.isApplied = "false";
            this.isApproved = "false";
        
        }
        public string classId { get; set; }
        public string className { get; set; }
        public string isApproved { get; set; }
       // public string isApplied { get; set; }
        public List<SubjectsNewModel> listSubejcts { get; set; }
    }
    public class SubjectsNewModel
    {
        public SubjectsNewModel()
        {
           // this.isApplied = "false";
            this.isApproved = "false";
        
        }
        public string subjectId { get; set; }
        public string subjectName { get; set; }
        public string isApproved { get; set; }
       // public string isApplied { get; set; }

    }
}