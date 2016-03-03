using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using wizz.Class;
using wizz.Models;

namespace wizz.Dal
{
    /// <summary>
    /// Db methods for Request for students and tutors
    /// <Developer>Rishabh Jain</Developer>
    /// <date>11/4/2015</date>
    /// </summary>
    public class RequestMethods
    {
        //internal int StudentRequest(StudentRequestModel objReq)
        //{
           
        //    using (var db = new WizzDataContext())
        //    {
        //        var studentId = Convert.ToInt64(objReq.userId);

        //        tblRequestStudent objStudent;
        //       var  requestUniqueId = Guid.NewGuid().ToString();
        //        if (objReq.isNow.ToLower()=="false"||String.IsNullOrEmpty(objReq.isNow))
        //        {
        //            foreach (var n in objReq.listSlot)
        //            {
        //                objStudent = new tblRequestStudent();
        //                objStudent.latitude = Convert.ToDecimal(objReq.latitude);
        //                objStudent.longitude = Convert.ToDecimal(objReq.longitude);
        //                objStudent.forMinutes = Convert.ToInt32(objReq.totalMinutes);
        //                objStudent.accompanyNum = Convert.ToInt16(objReq.accompanies);
        //                objStudent.updatedDate = DateTime.UtcNow;
        //                objStudent.createdDate = DateTime.UtcNow;
        //                objStudent.day = Convert.ToInt16(n.dayNo);
        //                objStudent.subjects = objReq.assignments;
        //                objStudent.fkClassId = Convert.ToInt32(objReq.fkSubjectId);
        //                objStudent.isActive = true;
        //                objStudent.fromTime = n.fromTime;
        //                objStudent.toTime = n.toTime;
        //                objStudent.fkUserId = studentId;
        //                objStudent.isDelete = false;
        //                objStudent.isNow = false;
        //                objStudent.TimeStamp = Convert.ToInt64(objReq.timeStamp);
        //                objStudent.uniqueRequestId = requestUniqueId;
        //                objStudent.calculatedAmount=Convert.ToInt32(objReq.calculatedAmount);
        //                objStudent.location = objReq.location;
                        
        //                db.tblRequestStudents.InsertOnSubmit(objStudent);
        //                db.SubmitChanges();
                      
        //            }
        //        }
        //        else if (objReq.isNow.ToLower() == "true")
        //        {
        //            objStudent = new tblRequestStudent();
        //            objStudent.isNow = true;
        //            objStudent.latitude = Convert.ToDecimal(objReq.latitude);
        //            objStudent.longitude = Convert.ToDecimal(objReq.longitude);
        //            objStudent.forMinutes = Convert.ToInt32(objReq.totalMinutes);
        //            objStudent.accompanyNum = Convert.ToInt16(objReq.accompanies);
        //            objStudent.fromTime = objReq.currentTime;
        //            DateTime currentTime = Convert.ToDateTime(objReq.currentTime);
        //            double minuts = Convert.ToDouble(objReq.totalMinutes);
        //            currentTime = currentTime.AddMinutes(minuts);
        //            objStudent.toTime = Convert.ToString(currentTime);
        //            objStudent.updatedDate = DateTime.UtcNow;
        //            objStudent.createdDate = DateTime.UtcNow;
        //            objStudent.subjects = objReq.assignments;
        //            objStudent.fkClassId = Convert.ToInt32(objReq.fkSubjectId);
        //            objStudent.isActive = true;
        //            objStudent.isDelete = false;
        //            objStudent.fkUserId = studentId;
        //            objStudent.TimeStamp =Convert.ToInt64(objReq.timeStamp);
        //            objStudent.uniqueRequestId = requestUniqueId;
        //            objStudent.calculatedAmount = Convert.ToInt32(objReq.calculatedAmount);
        //                 objStudent.location = objReq.location;
        //            db.tblRequestStudents.InsertOnSubmit(objStudent);
        //            db.SubmitChanges();

        //        }
        //        else
        //        {
        //            return 0;
        //        }
              
            
        //        //get matched tutors
        //        var turors = db.usp_GetTutors(objReq.fkSubjectId, Convert.ToDecimal(objReq.latitude), Convert.ToDecimal(objReq.longitude),Convert.ToInt64(objReq.userId)).ToList();
        //        //save request to db and send push to tutor
        //        SendRequest(objReq, turors, requestUniqueId);
        //        return 1;

        //    }

        //}

        

        internal int SaveTutorProfile(TutorProfile objReq)
        {

            tblTutorProfile profileData = new tblTutorProfile();
            using(var db=new WizzDataContext()){
                profileData = db.tblTutorProfiles.Where(x => x.fkUserId == Convert.ToInt64(objReq.userId) && x.isDelete!=true).FirstOrDefault();
                if (profileData == null)
                {
                   //objReq.fileName
                    profileData = new tblTutorProfile();
                    profileData.isApproved = false;
                    profileData.isActive = true;
                    profileData.isDelete = false;
                    profileData.fkUserId = Convert.ToInt64(objReq.userId);
                    //profileData.latitude = Convert.ToDecimal(objReq.latitude);
                    //profileData.longitude = Convert.ToDecimal(objReq.longitude);
                    //profileData.subjects = objReq.majors;
                    
                    profileData.TimeStamp = Convert.ToInt64(objReq.timeStamp);
                    profileData.updatedDate = DateTime.UtcNow;
                    profileData.createdDate = DateTime.UtcNow;
                    profileData.passingYear = objReq.passingYear;
                //    profileData.referralcode = objReq.referalCode==null?"": objReq.referalCode;
                //    profileData.fkClassId = String.Join(",", objReq.classes);
                //    profileData.docName = objReq.fileName;
                    
                    //profileData. = Convert.ToInt32(objReq.collegeId);
                    db.tblTutorProfiles.InsertOnSubmit(profileData);
                   
                }
                else {
                    profileData.isApproved = false;
                    profileData.isActive = true;
                    //profileData.fkUserId = Convert.ToInt64(objReq.userId);
                    //profileData.latitude = Convert.ToDecimal(objReq.latitude);
                    //profileData.longitude = Convert.ToDecimal(objReq.longitude);
                    //profileData.subjects = objReq.majors;
                    //profileData.passingYear = objReq.passingYear;
                    //profileData.docName = objReq.fileName;
                    profileData.TimeStamp = Convert.ToInt64(objReq.timeStamp);
                    profileData.updatedDate = DateTime.UtcNow;
                    profileData.createdDate = DateTime.UtcNow;
                  //  profileData.fkClassId = String.Join(",", objReq.classes);
                    //profileData.fkCollegeId = Convert.ToInt32(objReq.collegeId);
                }
                db.SubmitChanges();
                return 1;
            }
          
        }
        //internal int SaveTutorSubjects(TutorSubjects objReq)
        //{

        //    tblTutorSubject objSubjectData = new tblTutorSubject();
        //    using (var db = new WizzDataContext())
        //    {
        //        objSubjectData = db.tblTutorSubjects.Where(x => x.fkTutorId == Convert.ToInt64(objReq.tutorId)).FirstOrDefault();
        //        if (objSubjectData == null)
        //        {
        //            //objSubjectData = new TutorSubjects();
        //            //objSubjectData.isApproved = true;
        //            //objSubjectData.isActive = true;
        //            //objSubjectData.fkUserId = Convert.ToInt64(objReq.userId);
        //            //objSubjectData.latitude = Convert.ToDecimal(objReq.latitude);
        //            //objSubjectData.longitude = Convert.ToDecimal(objReq.longitude);
        //            //objSubjectData.subjects = objReq.majors;
        //            //objSubjectData.TimeStamp = Convert.ToDateTime(objReq.timeStamp).Ticks;
        //            //objSubjectData.updatedDate = DateTime.UtcNow;
        //            //objSubjectData.createdDate = DateTime.UtcNow;
        //            //objSubjectData.passingYear = objReq.passingYear;
        //            //objSubjectData.referralcode = objReq.referalCode == null ? "" : objReq.referalCode;
        //            ////    profileData.fkClassId = String.Join(",", objReq.classes);
        //            //objSubjectData.fkCollegeId = Convert.ToInt32(objReq.collegeId);
        //            //db.tblTutorProfiles.InsertOnSubmit(objSubjectData);

        //        }
        //        else
        //        {
        //            //objSubjectData.isApproved = true;
        //            //objSubjectData.isActive = true;
        //            //objSubjectData.fkUserId = Convert.ToInt64(objReq.userId);
        //            //objSubjectData.latitude = Convert.ToDecimal(objReq.latitude);
        //            //objSubjectData.longitude = Convert.ToDecimal(objReq.longitude);
        //            //objSubjectData.subjects = objReq.majors;
        //            //objSubjectData.TimeStamp = Convert.ToDateTime(objReq.timeStamp).Ticks;
        //            //objSubjectData.updatedDate = DateTime.UtcNow;
        //            //objSubjectData.createdDate = DateTime.UtcNow;
        //            ////  profileData.fkClassId = String.Join(",", objReq.classes);
        //            //objSubjectData.fkCollegeId = Convert.ToInt32(objReq.collegeId);
        //        }
        //        db.SubmitChanges();
        //        return 1;
        //    }

        //}


        //save matched tutos in db and send notifications 
        void SendRequest(StudentRequestModel objReq,List<usp_GetTutorsResult> tutors,string requestId)
        {
            var studentId =Convert.ToInt64(objReq.userId);
            using(var db=new WizzDataContext())
            {
                foreach (var tutor in tutors.ToList())
                {
                    var request = new tblRequest();
                    request.createdDate = DateTime.UtcNow;
                    request.updatedDate = DateTime.UtcNow;
                    request.isAccepted = false;
                    request.isDeleted = false;
                    request.fkStudentId = studentId;
                    request.fkTutorId = tutor.fkUserId;
                    request.uniqueRequestId = requestId;
                    request.timeStamp = DateTime.UtcNow.Ticks;
                    db.tblRequests.InsertOnSubmit(request);
                    db.SubmitChanges();
                    //sendpush

                    if(!string.IsNullOrEmpty(tutor.deviceToken))
                    {
                        PushData push=new PushData()
                        {
                        
                        registration_ids = tutor.deviceToken,
                        message = PushMessages.newRequest,
                        type = Convert.ToInt16(PushType.newRequest).ToString(),
                        badge = Convert.ToString(tutor.badge)
                    };
                       // push.data = CommonMethod.ObjectToJson(objPush);

                        if (tutor.deviceType == Convert.ToInt16(DeviceType.ios))//ios
                        { 
                            SendPush objIOS=new SendPush();
                            objIOS.ConnectToAPNS(push);
                        }
                        else if (tutor.deviceType == Convert.ToInt16(DeviceType.android))//android
                        {
                            Android objAndroid = new Android();
                            objAndroid.send(push);
                            
                        }
                    }
                }
            }
        }


        internal bool ReportSpam(ReportSpamModel objReq)
        {
            using (var db = new WizzDataContext()) {

                tblReportSpam objSpamData = new tblReportSpam();
                objSpamData.fkFromId = Convert.ToInt64(objReq.fromId);
                objSpamData.fkToId = Convert.ToInt64(objReq.toId);
                objSpamData.userType = Convert.ToInt16(objReq.forUserType);
                objSpamData.isDelete = false;
                objSpamData.createdDate = DateTime.UtcNow;
                objSpamData.updatedDate = DateTime.UtcNow;
                db.tblReportSpams.InsertOnSubmit(objSpamData);
                db.SubmitChanges();
                return true;
            
            }
        }



        //internal bool CancelRequest(RequestEntity objReq)
        //{
        //    using (var db = new WizzDataContext())
        //    {

        //        tblRequestStudent objReqStudent = new tblRequestStudent();
        //      //  objReqStudent=db.tblRequests.Where(x=>x.)

        //    }
        //    throw new NotImplementedException();
        //}

        //internal List<RespNewRequest> GetNewRequest(Entity objReq)
        //{
        //    List<RespNewRequest> objResp = new List<RespNewRequest>();
        //    using (var db = new WizzDataContext())
        //    {
        //        objResp = (from i in db.usp_GetNewRequest(Convert.ToInt64(objReq.userId),0,false)
        //                   select new RespNewRequest
        //                   {
        //                       accompanyNum = Convert.ToString(i.accompanyNum),
        //                       assignment = i.assignment,
        //                       calculatedAmount = Convert.ToString(i.calculatedAmount),
        //                       className = Convert.ToString(i.className),
        //                       forMinutes = Convert.ToString(i.forMinutes),
        //                       latitude = Convert.ToString(i.latitude),
        //                       longitude = Convert.ToString(i.longitude),
        //                       profilepic = string.IsNullOrWhiteSpace(i.studentPic) ? "" : i.studentPic.IndexOf("http") >= 0 ? i.studentPic : Constants.imagepath + i.studentPic,
                             
        //                       requestId = Convert.ToString(i.requestId),
        //                       uniqueRequestId = Convert.ToString(i.uniqueRequestId),
        //                       name = i.studenName,
        //                       id = Convert.ToString(i.fkStudentId),
        //                    //   tutorId = Convert.ToString(i.fkTutorId),
        //                      // tutorName = i.tutorName,
        //                       isNow=i.isNow,
        //                      location=i.location,
        //                      listSlot= (from l in db.tblRequestStudents   where l.uniqueRequestId==i.uniqueRequestId
        //                                 select new TimeSlot
        //                      {
        //                          dayNo = l.day == null ? "0" : l.day.ToString(),
        //                          slotId=l.pkRequirementId.ToString(),
        //                          fromTime=l.fromTime,
        //                          toTime=l.toTime
        //                      }).ToList() // im i in objDBContext.usp_GetItemListForApp(0, 0, Convert.ToInt64(objReq.merchantId), 0, 0, 0, "", 0)
                                    
        //                   }).ToList();
        //        return objResp;
        //    }

        //}
    }
}