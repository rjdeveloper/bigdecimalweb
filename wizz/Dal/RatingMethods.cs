using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using wizz.Models;

namespace wizz.Dal
{
    public class RatingMethods
    {
        //internal int RateTutor(RatingModel objReq)
        //{

        //    using (var db = new WizzDataContext()) {
        //        int tutorId=Convert.ToInt32(objReq.fkTutorId);
        //        tblUser userData = new tblUser();
        //        userData = db.tblUsers.Where(x => x.pkUserId == tutorId && x.isTeacher==true).FirstOrDefault();
        //        if (userData == null) {
        //            return 0;
        //        }
        //        int totalRating = Convert.ToInt32(objReq.helpfulRating) + Convert.ToInt32(objReq.knowledgeRating) + Convert.ToInt32(objReq.punctualRating);
        //        if (totalRating > 15)
        //            return 2;
        //        int avgRating = totalRating / 3;
        //        if (userData.avgRatingTutor == 0)
        //        {
        //            userData.avgRatingTutor = Convert.ToInt16(avgRating);
        //        }
        //        else {

        //            int avgUser = userData.avgRatingTutor + Convert.ToInt16(avgRating) ;
        //            userData.avgRatingTutor = Convert.ToInt16(avgUser/ 2);
        //        }
        //        //logic for tutor table
        //        tblTutorRating tutorData = new tblTutorRating();
        //        tutorData = db.tblTutorRatings.Where(x => x.fkTutorId == tutorId).FirstOrDefault();
        //        if (tutorData == null)
        //        {
        //            tutorData = new tblTutorRating();
        //            tutorData.fkTutorId = tutorId;
        //            tutorData.fkStudentId = Convert.ToInt32(objReq.fkStudentId);
        //            tutorData.helpful = Convert.ToInt16(objReq.helpfulRating);
        //            tutorData.knowledgable = Convert.ToInt16(objReq.knowledgeRating);
        //            tutorData.punctual = Convert.ToInt16(objReq.punctualRating);
        //            tutorData.createdDate = DateTime.UtcNow;
        //            tutorData.updatedDate = DateTime.UtcNow;
        //            db.tblTutorRatings.InsertOnSubmit(tutorData);
                 
        //        }
        //        else {
        //            tutorData.fkTutorId = tutorId;
        //            tutorData.fkStudentId = Convert.ToInt32(objReq.fkStudentId);
        //            tutorData.helpful = Convert.ToInt16(objReq.helpfulRating);
        //            tutorData.knowledgable = Convert.ToInt16(objReq.knowledgeRating);
        //            tutorData.punctual = Convert.ToInt16(objReq.punctualRating);
        //            tutorData.updatedDate = DateTime.UtcNow;                   
                
        //        }
        //        db.SubmitChanges();
        //        return 1;
        //    }
           
        //}
        //internal bool RateStudent(StudentRatingModel objReq)
        //{

        //    using (var db = new WizzDataContext())
        //    {
        //        int studentId = Convert.ToInt32(objReq.studentId);
        //        tblUser userData = new tblUser();
        //        userData = db.tblUsers.Where(x => x.pkUserId == studentId ).FirstOrDefault();
               

        //        short avgRating = Convert.ToInt16(objReq.studentRating);
        //        if (userData.avgRatingStudent == 0)
        //        {
        //            userData.avgRatingStudent = avgRating;
        //        }
        //        else
        //        {

        //            int avgUser = userData.avgRatingStudent + avgRating;
        //            userData.avgRatingStudent = Convert.ToInt16(avgUser / 2);
        //        }
        //        //logic for tutor table
        //        tblStudentRating tutorData = new tblStudentRating();
        //        tutorData = db.tblStudentRatings.Where(x => x.fkStudentId == studentId).FirstOrDefault();
        //        if (tutorData == null)
        //        {
        //            tutorData = new tblStudentRating();
        //            tutorData.fkTutorId = Convert.ToInt32(objReq.tutorId);
        //            tutorData.fkStudentId = studentId;
        //            tutorData.studentRating = avgRating;
                 
        //            tutorData.createdDate = DateTime.UtcNow;
        //            tutorData.updatedDate = DateTime.UtcNow;
        //            db.tblStudentRatings.InsertOnSubmit(tutorData);

        //        }
        //        else
        //        {
        //            tutorData.fkTutorId = Convert.ToInt32(objReq.tutorId);
        //            tutorData.fkStudentId = studentId;
        //            tutorData.studentRating = avgRating;
        //            tutorData.updatedDate = DateTime.UtcNow;
                


        //        }
        //        db.SubmitChanges();
        //        return true;
        //    }

        //}
    }
}