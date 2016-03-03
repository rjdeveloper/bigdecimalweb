using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace wizz.Models
{
    public class TutorProfile
    {
       
        public string userId { get; set; }
        public string majors { get; set;}

        public List<string> fileNames { get; set; }

        public string fileName { get; set; }
        public string latitude{get;set;}
      
        public string longitude{get;set;}
      
        public string timeStamp { get; set; }
        public string collegeId { get; set; }

        public string passingYear { get; set; }
        public string referalCode { get; set; }
    }
//    public class TutorSubjects {
//        public TutorSubjects() {

//            this.subjects = new List<SubjectsModel>();
//        }
//        public string tutorId { get; set; }
//        public List<SubjectsModel> subjects { get; set; }
//    }
}