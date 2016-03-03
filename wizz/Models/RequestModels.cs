using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace wizz.Models
{
    public class RequestModels
    {
    }
    public class ChangePasswordModel {

        /// <summary>
        /// Required= old password
        /// </summary>
          [Required(ErrorMessage = "oldPassword is required")]
        public string oldPassword { get; set; }
      
          [Required(ErrorMessage = "userId is required")]
          public string userId { get; set; }
        /// <summary>
        /// Required new password
        /// </summary>
         [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
    public class IsTeacherRequest
    {     
        [Required(ErrorMessage = "userId is required")]
        public string userId { get; set; }
        public string isTutor { get; set; }
    }
    public class RespIsTeacherRequest
    {
        public string isTutor { get; set; }
        public string isAlreadySignup { get; set; }
    }
    public class ReqLogOut
    {     
        [Required(ErrorMessage = "userId is required")]
        public string userId { get; set; }     
    }

    public class RespNewRequest 
    {
        public RespNewRequest()
        { this.listSlot = new List<TimeSlot>();
        this.listTutorSlot = new List<TimeSlot>();
        this.location = "";
        }
       

        public string id{ get; set; }

        //public string tutorId{ get; set; }

        public string name{ get; set; }

        public string profilepic{ get; set; }

       // public string tutorName{ get; set; }

        public string className{ get; set; }
        public string location { get; set; }
        public string requestId { get; set; }
        public string calculatedAmount{ get; set; }

        public string uniqueRequestId{ get; set; }

        public string assignment{ get; set; }
        public string sessionDate { get; set; }
        public string accompanyNum{ get; set; }

        public string forMinutes{ get; set; }

        public string latitude{ get; set; }

        public string longitude{ get; set; }
        public string isNow{ get; set; }
        public List<TimeSlot> listSlot { get; set; }
        public List<TimeSlot> listTutorSlot { get; set; }

        public string sessionStartTime { get; set; }
        public string sessionEndTime { get; set; }
    }


    public class FilteredHistoryResponsetoStudent {
        public FilteredHistoryResponsetoStudent()
        {
            this.DetailsList = new List<HistoryResponsetoStudent>();
        }
        public string subjectName { get; set; }
        public string subjectId { get; set; }
        public string subjectType { get; set; }
        public List<HistoryResponsetoStudent> DetailsList { get; set; }
    }
    public class HistoryResponsetoStudent
    {
       


        public string sessionId { get; set; }

        public string tutorName { get; set; }
        public string tutorRating { get; set; }
        public string totalHours { get; set; }
        public string profilepic { get; set; }
        public string tutorId { get; set; }
        public string searchCode { get; set; }
     //   public string paidAmount { get; set; }
        public string uniqueRequestId { get; set; }
     //   public string paymentType { get; set; }
      //  public List<HistoryDetailsModel> sessionWiseList { get; set; }

    }

    public class FilteredHistoryResponsetoTutor
    {
        public FilteredHistoryResponsetoTutor()
        {
            this.DetailsList = new List<HistoryResponsetoTutor>();
        }
        public string subjectName { get; set; }
        public string subjectId { get; set; }
        public string subjectType { get; set; }
        public List<HistoryResponsetoTutor> DetailsList { get; set; }
    }

    public class HistoryResponsetoTutor
    {



        public string sessionId { get; set; }

        public string studentName { get; set; }
        public string tutorRating { get; set; }
        public string totalHours { get; set; }
        public string profilepic { get; set; }
        public string studentId { get; set; }
        public string subjectName { get; set; }
        //   public string paidAmount { get; set; }
        public string uniqueRequestId { get; set; }
      //  public string paymentType { get; set; }
        //  public List<HistoryDetailsModel> sessionWiseList { get; set; }

    }
    public class HistoryDetailsModel {

      //  public string forMinutes { get; set; }
        public string sessionDate { get; set; }
        public string sessionCost { get; set; }
        public string latitude { get; set; }
        public string sno { get; set; }
        public string longitude { get; set; }
        public string sessionHours { get; set; }
        public string sessionNotes { get; set; }
        public string homework { get; set; }

        public string paymentType { get; set; }
    }
    
}