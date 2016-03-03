using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using wizz.ModelsV2;

namespace wizz.Models
{


    public class SessionModel
    {
        public SessionModel()
        {
            this.listSlot = new List<TimeSlot>();
          
        }
        public string tutorId { get; set; }
        public string tutorName { get; set; }
        public string tutorProfilePic { get; set; }
        public string course { get; set; }
        public string assignments { get; set; }
        public string hours { get; set; }
      
        public List<TimeSlot> listSlot { get; set; }
        public string accompanies { get; set; }

    }
    public class TutorSessionModel
    {
        public TutorSessionModel()
        {
            this.listSlot=new List<TimeSlot>();
        }
        public string studentId { get; set; }
        public string studentName { get; set; }
        public string studentProfilePic { get; set; }
        public string subjectId { get; set; }
        public string subjectName { get; set; }
        public string assignments { get; set; }
        public string hours { get; set; }

        public List<TimeSlot> listSlot { get; set; }
        public string accompanies { get; set; }

    }
    public class HistoryModel
    {
        public HistoryModel()
        {
            this.listTutor = new List<HistoryTutor>();
            this.availableAmount = "10";
            this.learnHours = "0";
        }
        public string learnHours { get; set; }
        public string availableAmount { get; set; }
        public List<HistoryTutor> listTutor { get; set; }

    }
    public class TutorHistoryModel
    {
        public TutorHistoryModel()
        {
            this.listStudent = new List<HistoryStudent>();
        }
        public string learnHours { get; set; }
        public string availableAmount { get; set; }
        public List<HistoryStudent> listStudent { get; set; }

    }
    public class HistoryTutor
    {
        public string paidAmount { get; set; }
        public string sessionDate { get; set; }
        public string tutorId { get; set; }
        public string tutorName { get; set; }
        public string tutorImage { get; set; }
        public string subject { get; set; }
    }
    public class HistoryStudent
    {
        public string paidAmount { get; set; }
        public string sessionDate { get; set; }
        public string studentId { get; set; }
        public string studentName { get; set; }
        public string studentImage { get; set; }
        public string subject { get; set; }
    }
    public class TimeSlotEntity {
        [Required]
        public string uniqueRequestId { get; set; }
        public string tutorId { get; set; }
        public string studentId { get; set; }
        public List<TimeSlot> listSlot { get; set; }
    
    }
    public class SessionReviewModel :GeoLocationModel
    {

   //   public string userId { get; set; }
        public string sessionNotes { get; set; }
        public string homeWork { get; set; }
        public string sessionCost { get; set; }
        public string isreschedule { get; set; }
     
        public string rating { get; set; }
     //   public string isReport { get; set; }
        public string sessionId { get; set; }
      //  public string uniqueRequestId { get; set; }


        public string userId { get; set; }
        public string studentId { get; set; }
        public string fromTime { get; set; }
        public string toTime { get; set; }
        public string dayType { get; set; }
        public string uniqueRequestId { get; set; }
    
    }
    public class StudentReviewModel {
     


        /// <summary>
        /// not required in request as of now
        /// </summary>
        public string tutorId { get; set; }
        [Required]
        public string userId { get; set; }
        [Required]
        public string userName { get; set; }
        [Required]
        public string reviewText { get; set; }
        [Required]
        public string sessionId { get; set; }
    
    }

    public class StudentRatingwModel
    {
        [Required]
        public string tutorId { get; set; }
        [Required]
        public string userId { get; set; }
        [Required]
        public string punctual { get; set; }
        [Required]
        public string knowledge { get; set; }
        [Required]
        public string sessionId { get; set; }
        [Required]
        public string helpFul { get; set; }
        [Required]
        public string userName { get; set; }

    }


    public class CreateSesion
    {
        public CreateSesion() {

            this.earnedAmt = "";
        }
        //public string tutorId { get; set; }
        //public string slotId { get; set; }
        //public string userId { get; set; }

        /// <summary>
        /// Required in all three cases
        /// </summary>
        [Required]
        public string sessionId { get; set; }
        //Will be treated as fromtime in session start and toTime in session end


        [Required]
        public string uniqueRequestId { get; set; }

        /// <summary>
        /// Required in only start session case
        /// </summary>
        public string startTime { get; set; }

        //Will be treated as from date in session start and todate in session end
        /// <summary>
        /// Required in start and end session case
        /// </summary>
        public string sessionDate { get; set; }

        /// <summary>
        /// required in end session case
        /// </summary>
        public string endTime { get; set; }

        //will be required in end session

        /// <summary>
        /// required in end session case
        /// </summary>
      public string earnedAmt { get; set; }
        /// <summary>
        /// The amount will be commission deducted as of admin settings
        /// </summary>
     //   public string tutorEarnedAmt { get; set; }
    }
    public class SessionEntity {
        [Required]
        public string sessionId { get; set; }

       // public string isTutor { get; set; }
    }
    public class SessionInfo {
 public SessionInfo(){
     this.sessionTime = "";
 }
        public string tutorName { get; set; }
        public string profilePic { get; set; }
        public string sessionTime { get; set; }
        public string className { get; set; }
        public string sessionStartTime { get; set; }
        public string sessionEndTime { get; set; }
    
    }


    public class StartSessionModel {

        public string userId { get; set; }
        public string tutorId { get; set; }
        public string fromTime { get; set; }
        public string toTime { get; set; }
        public string dayType { get; set; }
        public string uniqueRequestId{get;set;}

    }

    public class SessionResponseStudentModel {
        public SessionResponseStudentModel()
        {
            this.homeWork = "";
            this.homeWorkDueDate = "";
            this.friendsList = new List<FriendsListModel>();
        }
        public string tutorId { get; set; }
        public string tutorName { get; set; }
        public string profilePic { get; set; }
        public string subjectName { get; set; }
        public string sessionStartTime { get; set; }
        public string sessionEndTime { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string location { get; set; }
        public string homeWork { get; set; }
        public string homeWorkDueDate { get; set; }
        public string fromTime { get; set; }
        public string sessionId { get; set; }
        public string uniqueRequestId { get; set; }
        public string toTime { get; set; }
        public string dayType { get; set; }
        public List<FriendsListModel> friendsList { get; set; }
    }
    public class SessionResponseTutorModel : GeoLocationModel
    {

        public SessionResponseTutorModel() {
            this.friendsList = new List<FriendsListModel>();
            this.homeWork = "";
            this.homeWorkDueDate = "";
        
        }
        public string studentId { get; set; }
        public string studentName { get; set; }
        public string profilePic { get; set; }
        public string subjectName { get; set; }
        public string sessionStartTime { get; set; }
        public string sessionEndTime { get; set; }
   
        public string homeWork { get; set; }
        public string homeWorkDueDate { get; set; }
        public string fromTime { get; set; }
        public string sessionId { get; set; }
        public string uniqueRequestId { get; set; }
        public string toTime { get; set; }
        public string dayType { get; set; }
        public string sessionCost { get; set; }
        public List<FriendsListModel> friendsList { get; set; }
    }

    public class SessionEndResponseModel : GeoLocationModel
    {

        public string uniqueRequestId { get; set; }
        public string sessionCost { get; set; }
        public string scheduleCounts { get; set; }
        public string sessionId { get; set; }
    }
    public class GeoLocationModel {
        public string latitude { get; set; }
        public string location { get; set; }
        public string longitude { get; set; }
    
    }
}