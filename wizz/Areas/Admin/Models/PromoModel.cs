using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wizz.Areas.Admin.Models
{
    public class PromoModel
    {


        public string promoName { get; set; }
        public int counts { get; set; }
        public int discount { get; set; }
        public string validFrom { get; set; }
        public string validTo { get; set; }
        public string description { get; set; }
        public int pkPromoId { get; set; }
        public int usedTimes { get; set; }
        public bool isActive { get; set; }
        public int usageTimes { get; set; }
        public string sno { get; set; }
    }
    public class TutorSubjectModel {
        public int pkTutorSubjectId { get; set; }
        public int subjectId { get; set; }
        public bool isApproved { get; set; }
    }
    public class TutorSubjectsModel {
        public TutorSubjectsModel()
        {
            this.tutorSubjectList = new List<TutorSubjectModel>();
        }
        public int userId { get; set; }
        public List<TutorSubjectModel> tutorSubjectList { get; set; }
    
    }
}