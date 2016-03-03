using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using wizz.Models;
using wizz.ModelsV2;

namespace wizz.Class
{
    public class Tutor
    {
        internal List<TutorModel> GetTutors(Entity objRequest)
        {
            List<TutorModel> response = new List<TutorModel>();


            using (var db = new WizzDataContext())
            {

                //response = (from t in db.usp_GetTutors()
                //            select new TutorModel
                //             {
                //              tutorId = SqlFunctions.StringConvert((double)t.tutorId),
                //                 rating = SqlFunctions.StringConvert((double)t.avgRatingTutor),
                //                 tutorName=t.userName,
                //                  desc=t.description,
                //              tutorImage = Constants.imagepath + (t.profilePic == null ? "" : t.profilePic.IndexOf("http") >= 0 ? t.profilePic : Constants.imagepath+t.profilePic)
                //              }).ToList();

            }
            return response;
        }

        internal RespIsTutor IsApprovedTutor(Entity objRequest)
        {
            RespIsTutor response = new RespIsTutor();


            using (var db = new WizzDataContext())
            {
                var tutor = db.tblTutorProfiles.Where(u => u.fkUserId == Convert.ToInt64(objRequest.userId)).FirstOrDefault();
                if (tutor != null)
                {
                    if (tutor.isApproved == false)
                    {
                        response.isApproved = "False";
                    }
                    else {
                        response.isApproved = "True";
                    }
                    response.privateCost = tutor.feesPerHour.ToString();
                }
                else
                {
                    response.isApproved = "False";
                    response.isTutor = "False";
                }

            }
            return response;
        }



        internal List<ResponseTutorSubjects> GetSubjectsForTutor(Entity objReq)
        {

            using (var db = new WizzDataContext())
            {
                List<ResponseTutorSubjects> objResponse = new List<ResponseTutorSubjects>();

                objResponse = (from t in db.usp_GetSubjectsForTutor(Convert.ToInt32(objReq.userId))
                               select new ResponseTutorSubjects
                                {
                                    subjectName = t.subjectName,
                                    isApproved = Convert.ToString(t.isApproved),
                                    subjectId = Convert.ToString(t.subjectId),
                                    isApplied = Convert.ToString(t.isApplied)
                                }).ToList();
                return objResponse;
            }

        }

        internal bool SaveSubjectsForTutor(SubjectsListModel objReq)
        {

            using (var db = new WizzDataContext())
            {
                int tutorId = Convert.ToInt32(objReq.userId);
                // List<tblTutorSubject> objSubject = new List<tblTutorSubject>();
                var objSubjectDbList = db.usp_GetSubjectsForTutor(tutorId).ToList();
                tblTutorSubject listObj = new tblTutorSubject();
                if (objReq.subjectsIdList!=null&&objReq.subjectsIdList.Count != 0)
                {

                    foreach (var n in objSubjectDbList)
                    {
                        var data = objReq.subjectsIdList.Where(x => x == Convert.ToString(n.subjectId)).FirstOrDefault();

                        //data is null and n.isadded false h means not in all list wont be added wont be deleted
                        //data is null and n.isadded true h deleted from subject table
                        // if not null then check is added if yes den leave 
                        //if not null and is added false insert
                        //if not null and is added true then do nothing


                        if (data == null && n.isApplied == "True")
                        {
                            var tutorSub = db.tblTutorSubjects.Where(x => x.fkSubjectId == n.subjectId&&x.fkTutorId==Convert.ToInt64(objReq.userId)).FirstOrDefault();
                            db.tblTutorSubjects.DeleteOnSubmit(tutorSub);
                            db.SubmitChanges();

                        }
                        else if (data != null)
                        {
                            if (n.isApplied == "False")
                            {
                                var tutorSub = new tblTutorSubject();
                                tutorSub.fkTutorId = Convert.ToInt64(objReq.userId);
                                tutorSub.fkSubjectId = Convert.ToInt64(data);
                                tutorSub.isApproved = false;
                                db.tblTutorSubjects.InsertOnSubmit(tutorSub);
                                db.SubmitChanges();
                                //insert with is approved false in tutorsubject

                            }
                        }

                    }
                }


                return true;
            }
        }

       
    }
}