using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace wizz.Models
{
    public class SettingModel
    {
        public SettingModel()
        {
            this.listAssignment = new List<Assignment>();
        //    this.listClass = new List<SubjectsModel>();
            this.listColleges = new List<Colleges>();
            this.listMajor = new List<Majors>();
            this.isUpdated = "False";
            this.lastSyncTime = "";
            this.comission = "0";
            this.feePerHour = "0";
            this.feePerStudent = "0";
        }
        public string isUpdated { get; set; }
        public string lastSyncTime { get; set; }
        public string comission { get; set; }
        public string feePerHour { get; set; }
        public string feePerStudent { get; set; }
      //  public List<SubjectsModel> listClass { get; set; }
        public List<Majors> listMajor { get; set; }
        public List<Colleges> listColleges { get; set; }
        public List<Assignment> listAssignment { get; set; }
        
    }

    public class ReqSetting
    {
        [Required(ErrorMessage = "UserId is required")]
        public string userId { get; set; }

        [Required(ErrorMessage = "lastSyncTime is required")]
        public string lastSyncTime { get; set; }
    }

    public class ResponseTutorSubjects {
        public ResponseTutorSubjects()
        { this.isApplied = "False"; }
        public string subjectId { get; set; }
        public string subjectName { get; set; }
        public string isApproved { get; set; }

        public string isApplied { get; set; }

       


    }
    //public class SubjectsModel
    //{
    //    public string subjectId { get; set; }
    //    public string subjectName { get; set; }
    //}
    public class SubjectsListModel {
        public SubjectsListModel() {

            this.subjectsIdList = new List<string>();
        }
        public List<string> subjectsIdList = new List<string>();
        [Required]
        public string userId { get; set; }
    }
    public class Majors
    {
        public string majorId { get; set; }
        public string majorName { get; set; }
        public string fkClassId { get; set; }
    }
    public class Colleges
    {
        public string collegeId { get; set; }
        public string collegeName { get; set; }
    }

    public class Assignment
    {
        public string assignmentId { get; set; }
        public string assignmentName { get; set; }
        public string classId { get; set; }
    }
}