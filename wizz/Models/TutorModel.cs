using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wizz.Models
{
    public class TutorModel
    {
        public TutorModel()
        {
            this.tutorId = "0";
            this.tutorName = "";
            this.tutorImage = "";
            this.rating = "";
            this.collageName = "";
            this.desc = "";
        }

        public string tutorId { get; set; }
        public string tutorName { get; set; }
        public string tutorImage { get; set; }
        public string rating { get; set; }
        public string collageName { get; set; }
        public string desc { get; set; }
        public List<TimeSlot> listTime { get; set; }
    }

    public class TimeSlot 
    {
        public TimeSlot()
        {
            this.isBooked = "False";
        }
        public string dayNo { get; set; }
        public string fromTime { get; set; }
        public string toTime { get; set; }
        public string slotId { get; set; }
        public string isBooked { get; set; }
    }

    public class RespIsTutor 
    {
        public RespIsTutor()
        {
            this.isApproved = "True";
            this.isTutor = "True";
            this.privateCost = "";
            this.groupCost = "";
        }
        public string isApproved { get; set; }
        public string isTutor { get; set; }
        public string privateCost { get; set; }
        public string groupCost { get; set; }
    }

    public class ReqTutorSubject :Entity
    {
        public ReqTutorSubject()
        {
            this.listSubject = new List<string>();
        }
        public List<string> listSubject { get; set; }
    }

    
}