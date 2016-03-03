using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using wizz.Class;
using wizz.Models;
using wizz.ModelsV2;

namespace wizz.Dal
{
    public class DbMethodsV2
    {
        /// <summary>
        /// New method like get settings 
        /// <developer>Rishabh</developer>
        /// </summary>
        /// <param name="objRequest"></param>
        /// <returns></returns>
        internal GetSettingsModel GetAllActivitesData(ReqSetting objRequest)
        {
            //System.Data.Entity.Core.Objects.ObjectParameter UpdatedLastSyncTime = new System.Data.Entity.Core.Objects.ObjectParameter("UpdatedLastSyncTime", typeof(string));
            //  string UpdatedLastSyncTime = "";

            GetSettingsModel response = new GetSettingsModel();
            using (var db = new WizzDataContext())
            {
                //db.usp_isUpdatedData(objRequest.lastSyncTime, ref UpdatedLastSyncTime);


                //if (string.IsNullOrEmpty(UpdatedLastSyncTime))
                //{
                //    response.lastSyncTime = objRequest.lastSyncTime;
                //    return response;
                //}

                //response.lastSyncTime = UpdatedLastSyncTime;
                //response.isUpdated = "True";
                var courseData = db.tblCourses.Where(a => a.isDelete == false && a.isActive == true).ToList();
                var classes = db.tblClasses.Where(a => a.isActive == true && a.isDelete != true).ToList();
                var subjects = db.tblSubjects.Where(a => a.isActive == true && a.isDelete != true).ToList();

                response.listAcademicCourse = (from c in courseData.Where(a => a.isExtraCurricular == false)
                                               select new AcademicCoursesModel
                                          {
                                              courseId = Convert.ToString(c.pkId),
                                              courseName = c.courseName,
                                              listClasses = (from cl in classes.Where(a => a.fkCourseId == c.pkId)
                                                             select new ClassesModel
                                                             {
                                                                 classId = Convert.ToString(cl.pkId),
                                                                 className = cl.className,
                                                                 listSubejcts = (from sub in subjects.Where(a => a.fkClassId == cl.pkId)
                                                                                 select new SubjectsNewModel
                                                                                 {
                                                                                     subjectId = Convert.ToString(sub.pkId),
                                                                                     subjectName = sub.subjectName,
                                                                                 }).ToList()
                                                             }).ToList()


                                          }).ToList();
                response.listCurricularCourses = (from c in courseData.Where(a => a.isExtraCurricular == true)
                                                  select new CurricularCoursesModel
                                                  {
                                                      courseId = Convert.ToString(c.pkId),
                                                      courseName = c.courseName,
                                                      listClasses = (from cl in classes.Where(a => a.fkCourseId == c.pkId)
                                                                     select new ClassesModel
                                                                     {
                                                                         classId = Convert.ToString(cl.pkId),
                                                                         className = cl.className,
                                                                         listSubejcts = (from sub in subjects.Where(a => a.fkClassId == cl.pkId)
                                                                                         select new SubjectsNewModel
                                                                                         {
                                                                                             subjectId = Convert.ToString(sub.pkId),
                                                                                             subjectName = sub.subjectName,
                                                                                         }).ToList()
                                                                     }).ToList()


                                                  }).ToList();




            }
            return response;
        }
        /// <summary>
        /// saving all the stundet Profile Informations
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        internal List<SearchResponseTutorsModel> SaveStudentRequest(StudentRequestModelV2 objReq)
        {
            List<SearchResponseTutorsModel> tutorList = new List<SearchResponseTutorsModel>();
            try
            {
                using (var db = new WizzDataContext())
                {
                    long userId = Convert.ToInt64(objReq.userId);

                    tblStudentRequest studentData = new tblStudentRequest();

                    Guid uniqueRequestId = Guid.NewGuid();

                    // studentData.fkSubjectId = Convert.ToInt32(objReq.subjectId);
                    List<tblStudentRequest> studentStudentList = new List<tblStudentRequest>();
                    foreach (var n in objReq.dayList)
                    {
                        tblStudentRequest scheduleData = new tblStudentRequest();
                        scheduleData.fkUserId = userId;
                        scheduleData.isDelete = false;
                        scheduleData.timeType = Convert.ToInt16(n.timeType);
                        scheduleData.dayType = Convert.ToInt16(n.dayType);
                        scheduleData.createdDate = DateTime.UtcNow;
                        scheduleData.lat = Convert.ToDouble(objReq.lat);
                        scheduleData.reschedleTimes = 0;
                        scheduleData.longi = Convert.ToDouble(objReq.longi);


                        if (String.IsNullOrEmpty(objReq.subjectId))
                        {
                            if (!String.IsNullOrEmpty(objReq.classId))
                            {
                                scheduleData.fkSubjectId = Convert.ToInt32(objReq.classId);
                                scheduleData.subjectType = 2;
                            }
                            else
                            {
                                scheduleData.fkSubjectId = Convert.ToInt32(objReq.courseId);
                                scheduleData.subjectType = 1;

                            }

                        }
                        else
                        {
                            scheduleData.fkSubjectId = Convert.ToInt32(objReq.subjectId);
                            scheduleData.subjectType = 3;
                        }



                        scheduleData.uniqueStudentRequestId = uniqueRequestId.ToString();
                        scheduleData.location = objReq.location;

                        switch (Convert.ToInt16(n.timeType))
                        {
                            case 1:
                                scheduleData.fromTime = "600";
                                scheduleData.toTime = "1200";
                                break;
                            case 2:

                                scheduleData.fromTime = "1200";
                                scheduleData.toTime = "1600";
                                break;
                            case 3:

                                scheduleData.fromTime = "1600";
                                scheduleData.toTime = "2200";
                                break;
                            default: break;
                        }

                        studentStudentList.Add(scheduleData);
                    }
                    db.tblStudentRequests.InsertAllOnSubmit(studentStudentList);
                    db.SubmitChanges();

                    //  var userInfo = db.tblStudentRequests.Where(x => x.fkUserId == Convert.ToInt64(objReq.userId)).LastOrDefault();

                    tutorList = (from c in db.usp_P2GetTutorsList(objReq.subjectId.ToString(), Convert.ToDecimal(objReq.lat), Convert.ToDecimal(objReq.longi), objReq.location, 0)
                                 select new SearchResponseTutorsModel
                                 {
                                     aboutTutor = c.About,
                                     tutorName = c.userName,
                                     tutorProfilePic = c.profilePic,
                                     tutorLocation = c.location,
                                     tutorId = Convert.ToString(c.fkUserId),
                                     passingYear = Convert.ToString(c.passingYear),
                                     tutorRating = Convert.ToString(c.avgRatingTutor),
                                     tutorSubjects = db.usp_P2GetSubjectsForTutor(c.fkUserId).Select(x => x.subjectName).ToList(),
                                     perHourFees = c.feesPerHour
                                 }).ToList();
                    return tutorList;
                }
            }
            catch (Exception e)
            {

                return tutorList;
            }

        }

        //internal GetSettingsModel ()
        //{
        //    throw new NotImplementedException();
        //}
        internal List<CollegesModel> GetColleges()
        {
            //List<CollegesModel> response = new List<CollegesModel>();
            using (var db = new WizzDataContext())
            {
                List<CollegesModel> response = (from c in db.tblColleges.Where(a => a.isDelete == false)
                                                select new CollegesModel
                                                                   {
                                                                       collegeId = Convert.ToString(c.pkId),
                                                                       collegeName = c.collegeName
                                                                   }).ToList();
                return response;
            }

        }

        internal int SaveTutorProfile(TutorProfileModel objReq, List<string> fileNamesList)
        {

            using (var db = new WizzDataContext())
            {
                try
                {
                    long userId = Convert.ToInt64(objReq.userId);
                    #region Multiple List work
                    List<tblTutorDoc> objTutorDocList = new List<tblTutorDoc>();
                    objTutorDocList = db.tblTutorDocs.Where(x => x.fkTutorId == userId).ToList();
                    if (objTutorDocList.Count > 0)
                    {
                        db.tblTutorDocs.DeleteAllOnSubmit(objTutorDocList);
                        db.SubmitChanges();
                        objTutorDocList = new List<tblTutorDoc>();
                    }
                    foreach (string n in fileNamesList)
                    {
                        tblTutorDoc objDoc = new tblTutorDoc();
                        objDoc.createdDate = DateTime.UtcNow;
                        objDoc.fileName = n;
                        objDoc.fkTutorId = userId;
                        objDoc.isDelete = false;
                        objTutorDocList.Add(objDoc);
                    }
                    db.tblTutorDocs.InsertAllOnSubmit(objTutorDocList);

                    List<tblTutorSubject> objTutorSubList = new List<tblTutorSubject>();
                    objTutorSubList = db.tblTutorSubjects.Where(x => x.fkTutorId == userId).ToList();
                    if (objTutorSubList.Count > 0)
                    {
                        db.tblTutorSubjects.DeleteAllOnSubmit(objTutorSubList);
                        db.SubmitChanges();
                        objTutorSubList = new List<tblTutorSubject>();
                    }

                    if (objReq.subjectIdList.Count > 0)
                        foreach (string n in objReq.subjectIdList)
                        {
                            tblTutorSubject objTutorSubject = new tblTutorSubject();
                            objTutorSubject.isApproved = false;
                            objTutorSubject.fkSubjectId = Convert.ToInt32(n);
                            objTutorSubject.fkTutorId = userId;
                            objTutorSubject.subjectType = 3;
                            objTutorSubList.Add(objTutorSubject);
                        }
                    else if (objReq.classIdList.Count > 0)
                        foreach (string n in objReq.classIdList)
                        {
                            tblTutorSubject objTutorSubject = new tblTutorSubject();
                            objTutorSubject.isApproved = false;
                            objTutorSubject.fkSubjectId = Convert.ToInt32(n);
                            objTutorSubject.fkTutorId = userId;
                            objTutorSubject.subjectType = 2;
                            objTutorSubList.Add(objTutorSubject);
                        }
                    else
                    {

                        foreach (string n in objReq.courseIdList)
                        {
                            tblTutorSubject objTutorSubject = new tblTutorSubject();
                            objTutorSubject.isApproved = false;
                            objTutorSubject.fkSubjectId = Convert.ToInt32(n);
                            objTutorSubject.fkTutorId = userId;
                            objTutorSubject.subjectType = 1;
                            objTutorSubList.Add(objTutorSubject);
                        }
                    }
                    db.tblTutorSubjects.InsertAllOnSubmit(objTutorSubList);
                    #endregion

                    #region tableTutorprofileEntry
                    tblTutorProfile objTutorProfile = new tblTutorProfile();
                    var previousProfileObj = db.tblTutorProfiles.Where(x => x.fkUserId == userId).FirstOrDefault();
                    if (previousProfileObj != null)
                        db.tblTutorProfiles.DeleteOnSubmit(previousProfileObj);
                    objTutorProfile.feesPerHour = objReq.feesPerHour;
                    objTutorProfile.fkCollegeId = objReq.collegeId == null ? 0 : Convert.ToInt32(objReq.collegeId);
                    objTutorProfile.referralCode = objReq.referralCode == null ? "" : objReq.referralCode;
                    objTutorProfile.isApproved = false;

                    //fake data entry for maintaining previous tblschema (not use ful)
                    objTutorProfile.createdDate = DateTime.UtcNow;
                    objTutorProfile.description = "";
                    objTutorProfile.passingYear = "";

                    //creating a unique search code for tutor
                    objTutorProfile.tutorSearchCode = "W" + objReq.userName.Substring(0, 3) + System.Guid.NewGuid().ToString().Substring(0, 4);


                    //getting radius for tutor
                    objTutorProfile.distance = objReq.distanceRadius;
                    objTutorProfile.fkUserId = userId;
                    objTutorProfile.isActive = true;
                    objTutorProfile.updatedDate = DateTime.UtcNow;
                    objTutorProfile.TimeStamp = DateTime.UtcNow.Ticks;
                    objTutorProfile.lat = Convert.ToDouble(objReq.latitude);
                    objTutorProfile.longi = Convert.ToDouble(objReq.longitude);
                    objTutorProfile.location = objReq.location;
                    db.tblTutorProfiles.InsertOnSubmit(objTutorProfile);
                    #endregion
                    db.SubmitChanges();
                    return 1;
                }
                catch (Exception e)
                {

                    return 0;
                }

            }



        }

        internal bool CancelSession(UniqueRequestModel objReq)
        {

            using (var db = new WizzDataContext())
            {
                try
                {
                    db.tblStudentRequests.Where(x => x.uniqueStudentRequestId == objReq.uniqueRequestId).ToList().ForEach(x => x.isDelete = true);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

        }

        internal bool SaveTutorSchedule(TutorScheduleModel objReq)
        {

            using (var db = new WizzDataContext())
            {
                try
                {
                    long userId = Convert.ToInt64(objReq.userId);
                    #region Deleting old data on behalf of user Id
                    var scheduleList = db.tblTutorSchedules.Where(x => x.fkUserId == userId).ToList();
                    List<tblTutorSchedule> objScheduleList = new List<tblTutorSchedule>();
                    if (scheduleList.Count > 0)
                    {

                        db.tblTutorSchedules.DeleteAllOnSubmit(scheduleList);
                        db.SubmitChanges();
                    }
                    #endregion

                    foreach (var n in objReq.scheduleList)
                    {
                        tblTutorSchedule objSchedule = new tblTutorSchedule();
                        objSchedule.fkUserId = userId;
                        objSchedule.fromTime = timeConversionMethod(n.fromTime);
                        objSchedule.toTime = timeConversionMethod(n.toTime);
                        objSchedule.isDelete = false;
                        objSchedule.dayType = Convert.ToInt16(n.dayType);
                        objSchedule.createdDate = DateTime.UtcNow;
                        objScheduleList.Add(objSchedule);
                    }
                    db.tblTutorSchedules.InsertAllOnSubmit(objScheduleList);
                    db.SubmitChanges();
                    return true;

                }
                catch (Exception)
                {

                    return false;
                }

            }
            return false;

        }

        public static string timeConversionMethod(string time)
        {


            int originalTime = Convert.ToInt32(time) ;


            if (originalTime % 2 == 0)
                originalTime = ((originalTime / 2) * 100);
            else
                originalTime = ((originalTime / 2) * 100) + 50;


            return originalTime.ToString();
        }
        public static string ReverseTimeConversionMethod(string time)
        {
            return Convert.ToString(Convert.ToInt32(time) * 2 / 100);
        }

        internal List<SearchResponseTutorsModel> GetTutorsList(TutorSearchModel objReq)
        {
            using (var db = new WizzDataContext())
            {
                List<SearchResponseTutorsModel> tutorList = new List<SearchResponseTutorsModel>();
                var userInfo = db.tblStudentRequests.Where(x => x.fkUserId == Convert.ToInt64(objReq.userId)).FirstOrDefault();

                tutorList = (from c in db.usp_P2GetTutorsList(userInfo.fkSubjectId.ToString(), Convert.ToDecimal(userInfo.lat), Convert.ToDecimal(userInfo.longi), userInfo.location, Convert.ToInt32(objReq.distance))
                             select new SearchResponseTutorsModel
                                                {
                                                    aboutTutor = c.About,
                                                    tutorName = c.userName,
                                                    tutorProfilePic = c.profilePic,
                                                    tutorLocation = c.location,
                                                    tutorId = Convert.ToString(c.fkUserId),
                                                    passingYear = Convert.ToString(c.passingYear),
                                                    tutorRating = Convert.ToString(c.avgRatingTutor),
                                                    tutorSubjects = db.usp_P2GetSubjectsForTutor(c.fkUserId).Select(x => x.subjectName).ToList(),
                                                    perHourFees = c.feesPerHour
                                                }).ToList();
                return tutorList;
            }
        }

        internal UserResponse GetUserRequest(RequestEntity objReq)
        {
            UserResponse objResponse = new UserResponse();
            using (var db = new WizzDataContext())
            {
                if (objReq.isTutor.ToLower() == "true")
                {


                }
                else
                {
                    var tutorData = db.tblTutorSchedules.Where(x => x.fkUserId == Convert.ToInt64(objReq.userId)).ToList();

                    TutorScheduleModel objTutor = new TutorScheduleModel();
                    objTutor.userId = objReq.userId;
                    DayTimeModel dtmObj = new DayTimeModel();
                    foreach (var n in tutorData)
                    {
                        dtmObj = new DayTimeModel();
                        dtmObj.dayType = Convert.ToString(n.dayType);
                        dtmObj.fromTime = ReverseTimeConversionMethod(n.fromTime);
                        dtmObj.toTime = ReverseTimeConversionMethod(n.toTime);
                        objTutor.scheduleList.Add(dtmObj);
                    }

                    objResponse.tutorObj = objTutor;
                }
               
                return objResponse;
                //throw new NotImplementedException();
            }
        }

        internal string SendOtp(OtpModel objReq)
        {

            Random generator = new Random();
            String r = generator.Next(10000, 1000000).ToString();

            using (var db= new WizzDataContext())
            {

                var userObj = db.tblUsers.Where(x => x.pkUserId == Convert.ToInt64(objReq.userId)).FirstOrDefault();
               
                userObj.phoneNum = objReq.phoneNum;
                userObj.isOtpVerified = false;
                twiliorest objTwillio = new twiliorest();
                string msg = "Welcome to Wizz tutors your Otp validation code is " + r;
                if (objTwillio.SendTeilioMessage(objReq.phoneNum, msg))
                {
                    db.SubmitChanges();
                }
                else {

                 
                }

                return r;    
                
                
            }
           
        }

        internal bool VerifyOtp(OtpModel objReq)
        {
            using (var db=new WizzDataContext())
            {
                var userData = db.tblUsers.Where(c => c.pkUserId == Convert.ToInt64(objReq.userId)).FirstOrDefault();
                if (objReq.OTPCode == userData.otpCode)
                {

                    userData.isOtpVerified = true;
                    db.SubmitChanges();
                    return true;
                }
                else {
                    return false;
                
                }
                
            }

         
        }

        internal bool CreateSession(StartSessionModel objReq)
        {
            using (var db=new WizzDataContext())
            {
                try
                {
                    var uniqueRequestData = db.tblStudentRequests.Where(x => x.fkUserId == Convert.ToInt64(objReq.userId) && x.isDelete == false).FirstOrDefault();
                    tblSession sessionObj = new tblSession();
                    sessionObj.createdDate = DateTime.UtcNow;
                    sessionObj.fromTime = timeConversionMethod(objReq.fromTime);
                    sessionObj.toTime = timeConversionMethod(objReq.toTime);
                    sessionObj.uniqueRequestId = uniqueRequestData.uniqueStudentRequestId;
                    sessionObj.isDelete = false;
                    sessionObj.updatedDate = DateTime.UtcNow;
                    sessionObj.studentId = Convert.ToInt64(objReq.userId);
                    sessionObj.tutorId = Convert.ToInt64(objReq.tutorId);
                    db.tblSessions.InsertOnSubmit(sessionObj);
                    db.SubmitChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                    
                }
            }

            
        }
    }
}