using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace wizz.ModelsV2
{
    public class RequestModel
    {
    }

    #region Models for studentApis
    public class StudentRequestModelV2
    {
        public StudentRequestModelV2()
        {
            this.dayList = new List<DayModel>();

        }
        [Required]
        public string userId { get; set; }
  

        /// <summary>
        /// the id which have the value should be containing the value and others will be null or or empty
        /// </summary>
        public string subjectId { get; set; }
        public string courseId { get; set; }
        public string classId { get; set; }

        [Required]
        public string lat { get; set; }
        [Required]
        public string longi { get; set; }
      
        public string location { get; set; }

        public List<DayModel> dayList { get; set; }


        /// <summary>
        /// not useful at the time of request
        /// </summary>
        public string uniqueRequestId { get; set; }
        //public <DayModel> dayList {}

    }
    public class DayModel
    {
        /// <summary>
        /// 1 for monday,2 for tuesday so on.. up to 7 days
        /// </summary>
        public string dayType { get; set; }
        /// <summary>
        /// 1 For morning,2 for afternoon and 3 for evening
        /// </summary>
        public string timeType { get; set; }
    }
    public class StudentOccupiedCalendarModel
    {
        public StudentOccupiedCalendarModel() {

           
            this.StudentBookedSlot = new List<DayTimeModel>();
        }
        
        public List<DayTimeModel> StudentBookedSlot { get; set; }
    }
    public class TutorOccupiedCalendarModel
    {
        public TutorOccupiedCalendarModel()
        {

            this.TutorEmptySlot = new List<DayTimeModel>();
            this.TutorBookedSlot = new List<DayTimeModel>();
        }
        public List<DayTimeModel> TutorEmptySlot { get; set; }
        public List<DayTimeModel> TutorBookedSlot { get; set; }

        public string isAvailable { get; set; }
    }



    public class GroupModel {
        public GroupModel() {
            this.groupMembers = new List<string>();
        }
        public string groupName { get; set; }
        public string groupPic { get; set; }
        public string userId { get; set; }
        public string timeSlot { get;set; }
        public string groupId { get; set; }
        public string subjectName { get; set; }
        public List<string> groupMembers { get; set; }
    }
    public class GetGroupModel
    {
        public GetGroupModel()
        {
            this.groupMembers = new List<BasicUserModel>();
        }
        public string groupName { get; set; }
        public string groupPic { get; set; }
        public string userId { get; set; }
        public string timeSlot { get; set; }
        public string groupId { get; set; }
        public string subjectName { get; set; }
        public List<BasicUserModel> groupMembers { get; set; }
    }

    public class GroupEntity {

        public string userId { get; set; }
    }
    public class GetGroupRequest {
        public string groupId { get; set; }
    
    }

    public class BasicUserModel
    {
        public string userId { get; set; }
        public string profilePic { get; set; }
        public string userName { get; set; }
    
    }
    #endregion
    #region Model for tutorApis
    public class DayTimeModel
    {
        /// <summary>
        /// 1 for monday,2 for tuesday so on.. up to 7 days
        /// </summary>
        public string dayType { get; set; }

        /// <summary>
        /// It should be in 48 equal format(as discussed) like for morning 12:30 am it should be 1
        /// and for the night 12:00 pm it should be 48
        /// </summary>
        public string fromTime { get; set; }
        /// <summary>
        /// It should be in 48 equal format(as discussed) like for morning 12:30 am it should be 1
        /// and for the night 12:00 pm it should be 48
        /// </summary>
        public string toTime { get; set; }
    }




    public class TutorSubjectModel {

        public string userId { get; set; }
        public List<AppliedModel> subjectIdList { get; set; }

        public List<AppliedModel> courseIdList { get; set; }

        public List<AppliedModel> classIdList { get; set; }
    
    }
    public class AppliedModel {

        public string isApproved{ get; set; }
        public string id { get; set; }
        public string name { get; set; }
    
    }
    /// <summary>
    /// Tutor Description
    /// </summary>
    public class TutorProfileModel
    {
        public TutorProfileModel()
        {
     this.fileNameList = new List<string>();
        }

        [Required]
        public string userId { get; set; }

        public List<string> subjectIdList { get; set; }

        public List<string> courseIdList { get; set; }

        public List<string> classIdList { get; set; }

       public List<string> fileNameList { get; set; }


        [Required]
       public string latitude { get; set; }
        [Required]
        public string longitude { get; set; }

        public string location { get; set; }


        /// <summary>
        /// optional
        /// </summary>
        public string collegeName { get; set; }


        [Required]
        /// <summary>
        /// should be a whole num
        /// </summary>
        public string distanceRadius { get; set; }



        public string referralCode { get; set; }

        [Required]
        public string feesPerHour { get; set; }
        [Required]
        public string userName { get; set; }
        //public <DayModel> dayList {}

    }

    public class TutorScheduleModel
    {

        public TutorScheduleModel()
        {

            this.scheduleList = new List<DayTimeModel>();
        }
        [Required]
        public string userId { get; set; }

        public List<DayTimeModel> scheduleList { get; set; }
    }

    public class TutorSearchModel
    {
        public TutorSearchModel() {
            this.userId = "";
        }

        public string uniqueRequestId { get; set; }
        public string userId { get; set; }
       
        public List<filter> filterList { get; set; }
        public string distance { get; set; }
        public string gender { get; set; }

    }

    public class SearchTutorByCodeModel
    {
        
    [Required]
        public string tutorCode { get; set; }

    }
    public class filter
    {
        public string filterName { get; set; }
        public string orderBy { get; set; }
    }
    public enum filterType
    {
        highToLow = 1,
        lowToHigh
    }

    public enum gender
    {
        male = 1,
        female,
        both
    }
    public class UserResponse
    {
        public UserResponse() {
            this.tutorObj = new TutorScheduleModel();
        }
        public TutorScheduleModel tutorObj { get; set; }
        

    }

    public class TutorSearchResponseModel {
        public TutorSearchResponseModel()
        {
            this.tutorSubjects = new List<string>();
            this.reviews = new List<TutorReviewModel>();
        }
        public string tutorId { get; set; }
        public string tutorName { get; set; }
        public string tutorProfilePic { get; set; }
        public List<string> tutorSubjects { get; set; }

        public List<TutorReviewModel> reviews { get; set; }
        public string aboutTutor { get; set; }
        public string tutorRating { get; set; }
        public string tutorLocation { get; set; }
        public string passingYear { get; set; }
        public string perHourFees { get; set; }
    }
    public class TutorReviewModel {

        public string studentName { get; set; }
        public string rating { get; set; }
        public string reviewText { get; set; }
    
    }
    public class SearchResponseTutorsModel
    {
        public SearchResponseTutorsModel() {
            this.tutorList = new List<TutorSearchResponseModel>();
            this.uniqueRequestId = "";
        }
       public  List<TutorSearchResponseModel> tutorList { get; set; }
        public string uniqueRequestId { get; set; }
    }
    #endregion

    #region Models for Entity request apis
    public class UniqueRequestModel:UserIdModel
    {
        public string uniqueRequestId { get; set; }
        public string sessionId { get; set; }
    }
    public class UserIdModel
    {
        public string userId { get; set; }
        public string isTutor { get; set; }
    }
    #endregion
}