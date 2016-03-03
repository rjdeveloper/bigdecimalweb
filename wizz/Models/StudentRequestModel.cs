using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace wizz.Models
{
    public class StudentRequestModel
    {
        /// <summary>
        ///Please note required fileds are always required
        /// </summary>        
        [Required]
        public string userId { get; set; }
        [Required]
        public string totalMinutes { get; set; }
        [Required]
        public string assignments { get; set; }
        /// <summary>
        /// Always Required
        /// </summary>
        [Required]
        public string fkSubjectId { get; set; }
        /// <summary>
        ///madndatory in  isNow=false case
        ///it should be day wise slot like if a user is having 3-4,4-5 and 
        ///5-7 slots on monday the time slot in it will be  like
        ///dayNo =1 (for monday)
        ///fromTime(3)
        ///toTime (7)
        /// </summary>
        public List<TimeSlot> listSlot { get; set; }
        [Required]
        public string longitude { get; set; }
        [Required]
        public string latitude { get; set; }
        /// <summary>
        /// Total number of friends or accompanies with the student.(only numeric)
        /// 0 in case of none
        /// </summary>
        [Required]
        public string accompanies { get; set; }
        /// <summary>
        /// IF the request is for now time else it would be null
        /// </summary>
        [Required]
        public string isNow { get; set; }
        /// <summary>
        /// Only in the case of IsNow
        /// </summary>
        public string currentTime { get; set; }
        [Required]
        public string timeStamp { get; set; }
        [Required]
        public string calculatedAmount { get; set; }
        [Required]
        public string location { get; set; }

    }


   
    public class push
    {
        public string deviceToken { get; set; }
        public string deviceType { get; set; }

    }
    public class IosPushModel
    {
        public IosPushModel() {

            this.Type = "1";
        }
        public string senderId { get; set; }
        [Required]
        public string receiverId { get; set; }
        public string requestId { get; set; }
        public string Message { get; set; }
        public string messageId { get; set; }
        public string badge { get; set; }
        public string subjectName { get; set; }
        public string senderName { get; set; }
        public string Type { get; set; }

        /// <summary>
        /// not needed from mobile end
        /// </summary>
        public string deviceToken { get; set; }

    }
}