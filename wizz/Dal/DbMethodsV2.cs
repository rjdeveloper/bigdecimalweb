using System;
using System.Collections.Generic;
using System.IO;
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
        internal SearchResponseTutorsModel SaveStudentRequest(StudentRequestModelV2 objReq)
        {
            SearchResponseTutorsModel searchResponse = new SearchResponseTutorsModel();
            List<TutorSearchResponseModel> tutorList = new List<TutorSearchResponseModel>();
            try
            {
                using (var db = new WizzDataContext())
                {
                    long userId = Convert.ToInt64(objReq.userId);

                    tblStudentRequest studentData = new tblStudentRequest();

                    Guid uniqueRequestId = Guid.NewGuid();

                    db.tblStudentRequests.Where(x => x.fkUserId == Convert.ToInt64(objReq.userId)).ToList().ForEach(c => c.isDelete = true);
                    db.SubmitChanges();
                    //if (previousStudentRequests.Count > 0)
                    //{
                    //    db.tblStudentRequests.DeleteAllOnSubmit(previousStudentRequests);
                    //    db.SubmitChanges();
                    //}
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

                    tutorList = (from c in db.usp_P2GetTutorsList(Convert.ToInt32(objReq.userId),objReq.subjectId.ToString(), Convert.ToDecimal(objReq.lat), Convert.ToDecimal(objReq.longi), objReq.location, 0)
                                 select new TutorSearchResponseModel
                                 {
                                     aboutTutor = c.About,
                                     tutorName = c.userName,
                                     tutorProfilePic = c.profilePic,
                                     tutorLocation = c.location,
                                     tutorId = Convert.ToString(c.fkUserId),
                                     passingYear = Convert.ToString(c.passingYear),
                                     tutorRating = Convert.ToString(c.avgRatingTutor),
                                     tutorSubjects = db.usp_P2GetSubjectsForTutor(c.fkUserId).Select(x => x.subjectName).ToList(),
                                     perHourFees = c.feesPerHour,
                                     reviews =
                                                  (from r in db.tblTutorRatings.Where(x => x.fkTutorId == c.fkUserId)
                                                   select new TutorReviewModel
                                                   {
                                                       rating = Convert.ToString((r.helpful + r.knowledgable + r.punctual) / 3),
                                                       studentName = r.userName,
                                                       reviewText = r.review,

                                                   }).ToList()
                                 }).ToList();
                    searchResponse.tutorList = tutorList;
                    searchResponse.uniqueRequestId = uniqueRequestId.ToString();
                    return searchResponse;
                }
            }
            catch (Exception e)
            {

                return searchResponse;
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

        public static void deleteTutorDocsFromDisk(List<string> fileNames)
        {

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
                        foreach (var item in objTutorDocList)
                        {
                            try
                            {
                                string filePath = (System.Web.HttpContext.Current.Server.MapPath("~" + Constants.tutorDocs + item.fileName));
                                //  string thumbPath = (System.Web.HttpContext.Current.Server.MapPath("~" + Constants.thumbnailpath + item.imageName));
                                File.Delete(filePath);
                                //  File.Delete(thumbPath);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
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
                            objTutorSubject.isApproved = true;

                            objTutorSubject.fkSubjectId = Convert.ToInt32(n);
                            objTutorSubject.fkTutorId = userId;
                            objTutorSubject.subjectType = 3;
                            objTutorSubList.Add(objTutorSubject);
                        }
                  if (objReq.classIdList.Count > 0)
                        foreach (string n in objReq.classIdList)
                        {
                            tblTutorSubject objTutorSubject = new tblTutorSubject();
                            objTutorSubject.isApproved = false;
                            objTutorSubject.fkSubjectId = Convert.ToInt32(n);
                            objTutorSubject.fkTutorId = userId;
                            objTutorSubject.subjectType = 2;
                            objTutorSubList.Add(objTutorSubject);
                        }
                  if (objReq.courseIdList.Count > 0)
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
                    objTutorProfile.collegeName = objReq.collegeName == null ? "" : objReq.collegeName;
                    objTutorProfile.referralCode = objReq.referralCode == null ? "" : objReq.referralCode;
                    objTutorProfile.isApproved = true;

                    //fake data entry for maintaining previous tblschema (not use ful)
                    objTutorProfile.createdDate = DateTime.UtcNow;
                    objTutorProfile.description = "";
                    objTutorProfile.passingYear = "";
                    objTutorProfile.isAvailable = true;
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
                    Int32 userId = Convert.ToInt32(objReq.userId);

                    if (Convert.ToBoolean(objReq.isTutor))
                    {

                        var sessionData = db.tblSessions.Where(x => x.pkSessionId == Convert.ToInt64(objReq.sessionId)).FirstOrDefault();
                        sessionData.isDelete = true;
                        sessionData.isCancelled = true;

                    }
                    else
                    {

                        db.tblStudentRequests.Where(x => x.uniqueStudentRequestId == objReq.uniqueRequestId && x.fkUserId == userId).ToList().ForEach(x => x.isDelete = true);
                        var friendData = db.tblInviteFriends.Where(x => x.uniqueRequestId == objReq.uniqueRequestId && x.isDelete == false).ToList();


                        if (friendData.Count == 0)
                        {

                            var sessionData = db.tblSessions.Where(x => x.pkSessionId == Convert.ToInt64(objReq.sessionId)).FirstOrDefault();
                            sessionData.isDelete = true;
                            sessionData.isCancelled = true;
                        }
                        else
                        {
                            var userData = friendData.Where(x => x.fkFriendId == userId).FirstOrDefault();
                            userData.isDelete = true;

                        }
                    }
                    db.SubmitChanges();
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


            int originalTime = Convert.ToInt32(time);


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

        internal SearchResponseTutorsModel GetTutorsList(TutorSearchModel objReq)
        {
            using (var db = new WizzDataContext())
            {
                SearchResponseTutorsModel objResponse = new SearchResponseTutorsModel();
                List<TutorSearchResponseModel> tutorList = new List<TutorSearchResponseModel>();
                var userInfo = db.tblStudentRequests.Where(x => x.uniqueStudentRequestId == objReq.uniqueRequestId).FirstOrDefault();

                tutorList = (from c in db.usp_P2GetTutorsList(Convert.ToInt32(objReq.userId),userInfo.fkSubjectId.ToString(), Convert.ToDecimal(userInfo.lat), Convert.ToDecimal(userInfo.longi), userInfo.location, Convert.ToInt32(objReq.distance))
                             select new TutorSearchResponseModel
                                                {
                                                    aboutTutor = c.About,
                                                    tutorName = c.userName,
                                                    tutorProfilePic = c.profilePic,
                                                    tutorLocation = c.location,
                                                    tutorId = Convert.ToString(c.fkUserId),
                                                    passingYear = Convert.ToString(c.passingYear),
                                                    tutorRating = Convert.ToString(c.avgRatingTutor),
                                                    tutorSubjects = db.usp_P2GetSubjectsForTutor(c.fkUserId).Select(x => x.subjectName).ToList(),
                                                    perHourFees = c.feesPerHour,
                                                    reviews =
                                                    (from r in db.tblTutorRatings.Where(x => x.fkTutorId == c.fkUserId)
                                                     select new TutorReviewModel
                                                                        {
                                                                            rating = Convert.ToString((r.helpful + r.knowledgable + r.punctual) / 3),
                                                                            studentName = r.userName,
                                                                            reviewText = r.review,

                                                                        }).ToList()


                                                }).ToList();

                //foreach (var n in objReq.filterList) {

                //    if (n.orderBy == "1")
                //    {
                //        //tutorList = (from t in tutorList

                //        //             select t).OrderByDescending(c => c.perHourFees);

                //            tutorList.Sort(x=>x.perHourFees);

                //    }
                //    else if(n.orderBy=="2") {


                //    }
                //}
                objResponse.tutorList = tutorList;
                return objResponse;
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
        internal StudentOccupiedCalendarModel GetStudentOccupiedCalendar(Entity objReq)
        {

            using (var db = new WizzDataContext())
            {
                StudentOccupiedCalendarModel responseList = new StudentOccupiedCalendarModel();

                List<DayTimeModel> bookedStudentCalendarObj = new List<DayTimeModel>();
                var studentData = db.tblSessions.Where(x => x.studentId == Convert.ToInt64(objReq.userId) && x.isDelete == false && x.isComplete == false).ToList();
                // var sessionData = db.tblSessions.Where(x => x.studentId == Convert.ToInt64(objReq.userId)).ToList();
                foreach (var n in studentData)
                {
                    if (n.isDelete == true)
                        continue;
                    DayTimeModel bookedDayOBj = new DayTimeModel();

                    bookedDayOBj.dayType = Convert.ToString(n.dayType);
                    bookedDayOBj.fromTime = ReverseTimeConversionMethod(n.fromTime);
                    bookedDayOBj.toTime = ReverseTimeConversionMethod(n.toTime);
                    bookedStudentCalendarObj.Add(bookedDayOBj);


                }




                //  responseList.StudentSlot = studentCalendarObj;

                // responseList.StudentBookedSlot = bookedStudentCalendarObj;

                //DayTimeModel tryObj = new DayTimeModel();
                //tryObj.dayType = Convert.ToString(1);
                //tryObj.fromTime = Convert.ToString(28);
                //tryObj.toTime = Convert.ToString(34);
                //bookedStudentCalendarObj.Add(tryObj);
                //DayTimeModel tryObj2 = new DayTimeModel();
                //tryObj2.dayType = Convert.ToString(4);
                //tryObj2.fromTime = Convert.ToString(20);
                //tryObj2.toTime = Convert.ToString(24);
                //bookedStudentCalendarObj.Add(tryObj);
                //bookedStudentCalendarObj.Add(tryObj2);
                //  responseList.StudentSlot = studentCalendarObj;

                responseList.StudentBookedSlot = bookedStudentCalendarObj;

                return responseList;
            }

        }


        internal TutorOccupiedCalendarModel GetTutorOccupiedCalendar(Entity objReq)
        {

            using (var db = new WizzDataContext())
            {
                TutorOccupiedCalendarModel responseList = new TutorOccupiedCalendarModel();
                List<DayTimeModel> tutorCalendarObj = new List<DayTimeModel>();
                List<DayTimeModel> bookedTutorCalendarObj = new List<DayTimeModel>();
                var tutorData = db.tblTutorSchedules.Where(x => x.fkUserId == Convert.ToInt64(objReq.userId)).ToList();
                var sessionData = db.tblSessions.Where(x => x.tutorId == Convert.ToInt64(objReq.userId) && x.isDelete == false && x.isComplete == false && x.isCancelled == false).ToList();
                foreach (var n in tutorData)
                {
                    DayTimeModel dayOBj = new DayTimeModel();
                    DayTimeModel bookedDayOBj = new DayTimeModel();
                    dayOBj.dayType = Convert.ToString(n.dayType);
                    dayOBj.fromTime = ReverseTimeConversionMethod(n.fromTime);
                    dayOBj.toTime = ReverseTimeConversionMethod(n.toTime);
                    tutorCalendarObj.Add(dayOBj);
                    var bookedData = sessionData.Where(c => c.tutorId == n.fkUserId && Convert.ToInt32(c.fromTime) >= Convert.ToInt32(n.fromTime) && Convert.ToInt32(c.toTime) <= Convert.ToInt32(n.toTime) && c.isDelete == false).FirstOrDefault();
                    if (bookedData != null)
                    {
                        bookedDayOBj.dayType = Convert.ToString(bookedData.dayType);
                        bookedDayOBj.fromTime = ReverseTimeConversionMethod(bookedData.fromTime);
                        bookedDayOBj.toTime = ReverseTimeConversionMethod(bookedData.toTime);
                        bookedTutorCalendarObj.Add(bookedDayOBj);
                    }
                }
                responseList.TutorEmptySlot = tutorCalendarObj;

                responseList.TutorBookedSlot = bookedTutorCalendarObj;

                var isTutorAvail = db.tblTutorProfiles.FirstOrDefault(x => x.fkUserId == Convert.ToInt64(objReq.userId)).isAvailable;
                responseList.isAvailable = Convert.ToString(isTutorAvail);
                return responseList;
            }

        }
        internal string SendOtp(OtpModel objReq)
        {

            Random generator = new Random();
            String r = generator.Next(10000, 1000000).ToString();
            r = r.Substring(0, 5);
            using (var db = new WizzDataContext())
            {
                if (db.tblUsers.Any(x => x.phoneNum.ToLower().Trim() == objReq.phoneNum.ToLower().Trim() && x.isOtpVerified == true && x.pkUserId != Convert.ToInt64(objReq.userId)))
                {
                    return "4";
                }

                var userObj = db.tblUsers.Where(x => x.pkUserId == Convert.ToInt64(objReq.userId)).FirstOrDefault();

                userObj.phoneNum = objReq.phoneNum;
                userObj.isOtpVerified = false;
                userObj.otpCode = r;
                twiliorest objTwillio = new twiliorest();
                string msg = "Welcome to Wizz Tutors your Otp validation code is " + r;
                db.SubmitChanges();
                if (objTwillio.SendTeilioMessage(objReq.phoneNum, msg))
                {

                }
                else
                {


                }

                return r;


            }

        }

        internal bool VerifyOtp(OtpModel objReq)
        {
            using (var db = new WizzDataContext())
            {
                var userData = db.tblUsers.Where(c => c.pkUserId == Convert.ToInt64(objReq.userId)).FirstOrDefault();
                if (objReq.OTPCode.ToLower() == userData.otpCode.ToLower())
                {

                    userData.isOtpVerified = true;
                    db.SubmitChanges();
                    return true;
                }
                else
                {
                    return false;

                }

            }


        }

        internal bool CreateSession(StartSessionModel objReq)
        {
            using (var db = new WizzDataContext())
            {
                try
                {
                    // var uniqueRequestData = db.tblStudentRequests.Where(x => x.fkUserId == Convert.ToInt64(objReq.userId) && x.isDelete == false).FirstOrDefault();
                    tblSession sessionObj = new tblSession();
                    sessionObj.createdDate = DateTime.UtcNow;
                    sessionObj.fromTime = timeConversionMethod(objReq.fromTime);
                    sessionObj.toTime = timeConversionMethod(objReq.toTime);
                    sessionObj.uniqueRequestId = objReq.uniqueRequestId;
                    sessionObj.dayType = Convert.ToInt16(objReq.dayType);
                    sessionObj.isDelete = false;
                    sessionObj.updatedDate = DateTime.UtcNow;
                    sessionObj.studentId = Convert.ToInt64(objReq.userId);
                    sessionObj.tutorId = Convert.ToInt64(objReq.tutorId);
                    sessionObj.isCancelled = false;
                    sessionObj.isComplete = false;
                    sessionObj.homeWork = "";
                    sessionObj.homeWorkDueDate = "";
                    sessionObj.isActive = true;
                    sessionObj.isStarted = false;
                    sessionObj.sessionAmount = "0";
                    sessionObj.sessionNotes = "";


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

        internal List<SessionResponseStudentModel> GetSessionsForStudent(RequestEntity objReq)
        {
            using (var db = new WizzDataContext())
            {

                List<SessionResponseStudentModel> tutorList = new List<SessionResponseStudentModel>();
                int userId = Convert.ToInt32(objReq.userId);
                tblInviteFriend friendsData = new tblInviteFriend();
                friendsData = db.tblInviteFriends.Where(x => x.fkFriendId == userId).FirstOrDefault();
                if (friendsData != null)
                {

                    objReq.userId = Convert.ToString(friendsData.fkUserId);
                }
                var sessionList = db.usp_GetSessions(Convert.ToInt32(objReq.userId)).ToList();


                foreach (var c in sessionList)
                {
                    SessionResponseStudentModel model = new SessionResponseStudentModel();


                    model.homeWork = c.homeWork;
                    model.homeWorkDueDate = c.homeWorkDueDate;
                    model.latitude = Convert.ToString(c.lat);
                    model.location = c.location;
                    model.longitude = Convert.ToString(c.longi);
                    model.profilePic = c.profilePic;
                    model.sessionEndTime = c.toTime;
                    model.sessionStartTime = c.fromTime;
                    model.fromTime = ReverseTimeConversionMethod(c.fromTime);
                    model.toTime = ReverseTimeConversionMethod(c.toTime);
                    model.dayType = Convert.ToString(c.dayType);
                    model.subjectName = c.subjectName.ToString();
                    model.tutorId = c.tutorId.ToString();
                    model.tutorName = c.tutorName;
                    model.sessionId = c.sessionId.ToString();
                    model.uniqueRequestId = c.uniqueRequestId;
                    var friendData = db.usp_GetFriendsOfUser(c.sessionId.ToString()).ToList();
                    if (friendData.Count > 0)
                    {
                        foreach (var d in friendData)
                        {

                            FriendsListModel mod = new FriendsListModel();
                            if (d.fkFriendId == Convert.ToInt32(objReq.userId) || d.isDelete == true)
                            {

                                continue;
                            }


                            mod.friendId = Convert.ToString(d.fkFriendId);
                            mod.friendName = d.frienName == "" ? d.userName : d.frienName;
                            mod.amount = "";

                            mod.isConfirm = Convert.ToString(d.isConfirm);
                            mod.profilePic = string.IsNullOrWhiteSpace(d.profilePic) ? "" : d.profilePic.IndexOf("http") >= 0 ? d.profilePic : Constants.imagepath + d.profilePic;
                            if (mod.friendId == null)
                            {


                            }
                            else
                            {
                                model.friendsList.Add(mod);

                            }


                        }
                    }



                    tutorList.Add(model);
                }

                return tutorList;
            }
        }

        internal SessionResponseStudentModel GetSessionInfoForStudent(UniqueRequestModel objReq)
        {
            using (var db = new WizzDataContext())
            {
                SessionResponseStudentModel sessObj = new SessionResponseStudentModel();
                List<SessionResponseStudentModel> tutorList = new List<SessionResponseStudentModel>();
                int userId = Convert.ToInt32(objReq.userId);
                tblInviteFriend friendsData = new tblInviteFriend();
                friendsData = db.tblInviteFriends.Where(x => x.fkFriendId == userId).FirstOrDefault();
                if (friendsData != null)
                {

                    objReq.userId = Convert.ToString(friendsData.fkUserId);
                }
                var sessionList = db.usp_GetSessions(Convert.ToInt32(objReq.userId)).ToList();


                foreach (var c in sessionList)
                {
                    SessionResponseStudentModel model = new SessionResponseStudentModel();

                    if (db.tblInviteFriends.Any(x => x.fkUserId == Convert.ToInt32(objReq.userId) && x.isDelete == true))
                        continue;
                    model.homeWork = c.homeWork;
                    model.homeWorkDueDate = c.homeWorkDueDate;
                    model.latitude = Convert.ToString(c.lat);
                    model.location = c.location;
                    model.longitude = Convert.ToString(c.longi);
                    model.profilePic = c.profilePic;
                    //model.sessionEndTime = c.;
                    model.sessionStartTime = c.sessionStartTime;
                    model.fromTime = ReverseTimeConversionMethod(c.fromTime);
                    model.toTime = ReverseTimeConversionMethod(c.toTime);
                    model.dayType = Convert.ToString(c.dayType);
                    model.subjectName = c.subjectName.ToString();
                    model.tutorId = c.tutorId.ToString();
                    model.tutorName = c.tutorName;
                    model.sessionId = c.uniqueRequestId;
                    var friendData = db.usp_GetFriendsOfUser(c.uniqueRequestId).ToList();
                    foreach (var d in friendData)
                    {

                        FriendsListModel mod = new FriendsListModel();
                        if (d.fkFriendId == Convert.ToInt32(objReq.userId) || d.isDelete == false)
                            continue;
                        //  pkId=Convert.ToString(d.fId),
                        mod.friendId = Convert.ToString(d.fkFriendId);
                        mod.friendName = d.frienName == "" ? d.userName : d.frienName;
                        mod.amount = "";
                        //  friendphoneNumber = d.friendPhone == "" ? d.phoneNum : d.friendPhone,
                        mod.isConfirm = Convert.ToString(d.isConfirm);
                        mod.profilePic = string.IsNullOrWhiteSpace(d.profilePic) ? "" : d.profilePic.IndexOf("http") >= 0 ? d.profilePic : Constants.imagepath + d.profilePic;
                        model.friendsList.Add(mod);
                    }

                    //model.friendsList = (from d in db.usp_GetFriendsOfUser(c.uniqueRequestId)
                    //                 select new 
                    //                 {

                    //                 }).ToList();

                    tutorList.Add(model);
                }
                sessObj = tutorList.Where(x => x.sessionId == objReq.uniqueRequestId).FirstOrDefault();
                return sessObj;
            }
        }
        internal bool CreateGroup(GroupModel objReq)
        {
            try
            {
                using (var db = new WizzDataContext())
                {

                    tblGroup groupObj = new tblGroup();
                    groupObj = db.tblGroups.FirstOrDefault(x => x.groupId == objReq.groupId);
                    //if (groupData != null)
                    //{
                    //    db.tblGroups.DeleteOnSubmit(groupData);
                    //}
                    if (groupObj == null)
                    {
                        groupObj = new tblGroup();
                        groupObj.groupName = objReq.groupName;
                        groupObj.fkAdminId = objReq.userId;
                        groupObj.createdDate = DateTime.UtcNow;
                        groupObj.updatedDate = DateTime.UtcNow;
                        groupObj.isActive = true;
                        groupObj.timeSlot = objReq.timeSlot;
                        groupObj.subjectName = objReq.subjectName;
                        groupObj.groupId = objReq.groupId;
                        groupObj.groupPic = objReq.groupPic;
                        db.tblGroups.InsertOnSubmit(groupObj);
                        db.SubmitChanges();
                    }
                    else
                    {
                        groupObj.groupName = objReq.groupName;
                        groupObj.fkAdminId = objReq.userId;
                        groupObj.createdDate = DateTime.UtcNow;
                        groupObj.updatedDate = DateTime.UtcNow;
                        groupObj.isActive = true;
                        groupObj.timeSlot = objReq.timeSlot;
                        groupObj.subjectName = objReq.subjectName;
                        groupObj.groupId = objReq.groupId;
                        groupObj.groupPic = objReq.groupPic;



                    }







                    List<string> groupMems = new List<string>();
                    List<tblGroupMember> memObjList = new List<tblGroupMember>();


                    int groupId = Convert.ToInt32(groupObj.pkId);
                    foreach (string n in objReq.groupMembers)
                    {
                        tblGroupMember memObj = new tblGroupMember();
                        memObj.fkGroupId = groupId;
                        memObj.fkUserId = Convert.ToInt32(n);
                        memObj.createdDate = DateTime.UtcNow;
                        memObj.updatedDate = DateTime.UtcNow;
                        memObj.isDelete = false;
                        memObjList.Add(memObj);
                    }
                    db.tblGroupMembers.InsertAllOnSubmit(memObjList);
                    db.SubmitChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;

            }


        }
        internal List<GetGroupModel> GetGroup(GroupEntity objReq)
        {
            using (var db = new WizzDataContext())
            {

                //List<GetGroupModel> groupList = new List<GetGroupModel>();

                List<GetGroupModel> groupList = new List<GetGroupModel>();
                //  GetGroupModel OBjGroup = new GetGroupModel();
                var groupData = db.tblGroups.Where(x => x.fkAdminId == objReq.userId).ToList();

                // var groupData = db.tblGroups.Where(x => x.fkAdminId == objReq.userId).FirstOrDefault();
                //OBjGroup.groupId = groupData.groupId;
                //OBjGroup.groupPic = Constants.imagepath + groupData.groupPic;
                //OBjGroup.subjectName = groupData.subjectName;
                //OBjGroup.timeSlot = groupData.timeSlot;

                foreach (var i in groupData)
                {
                    GetGroupModel OBjGroup = new GetGroupModel();
                    OBjGroup.groupId = i.groupId;
                    OBjGroup.groupPic = Constants.imagepath + i.groupPic;
                    OBjGroup.subjectName = i.subjectName;
                    OBjGroup.timeSlot = i.timeSlot;
                    OBjGroup.groupName = i.groupName;
                    OBjGroup.groupMembers = (from c in db.usp_P2GetGroupMembers(Convert.ToInt32(i.pkId))
                                             select new BasicUserModel
                                              {
                                                  userName = c.userName,
                                                  profilePic = c.profilePic,
                                                  userId = Convert.ToString(c.memberId)
                                              }).ToList();
                    // return OBjGroup

                    groupList.Add(OBjGroup);
                }
                return groupList;
            }
        }

        #region Akanksha
        internal int InviteFriends(InviteFriendsModel objReq)
        {
            try
            {
                var response = 0;
                using (var db = new WizzDataContext())
                {
                    List<tblInviteFriend> userList = new List<tblInviteFriend>();
                    tblInviteFriend myObj = new tblInviteFriend();
                    List<tblInviteFriend> friendList = new List<tblInviteFriend>();
                    friendList = db.tblInviteFriends.Where(x => x.uniqueRequestId == objReq.sessionId).ToList();
                    if (friendList.Count > 0)
                    {
                        db.tblInviteFriends.DeleteAllOnSubmit(friendList);
                        db.SubmitChanges();
                    }
                    myObj.fkUserId = Convert.ToInt32(objReq.userId);
                    myObj.isConfirm = true;
                    myObj.userName = "";
                    myObj.phoneNum = "";
                    myObj.isDelete = false;
                    myObj.uniqueRequestId = objReq.sessionId;
                    myObj.fkFriendId = Convert.ToInt32(objReq.userId);
                    userList.Add(myObj);
                    foreach (var i in objReq.Friends)
                    {

                        tblInviteFriend userObj = new tblInviteFriend();
                        var isExist = db.tblUsers.FirstOrDefault(e => e.phoneNum.Contains(i.phoneNumber));
                        if (isExist != null)
                        {
                            userObj.userName = "";
                            userObj.phoneNum = "";
                            userObj.uniqueRequestId = objReq.sessionId;
                            userObj.isConfirm = true;
                            userObj.fkUserId = Convert.ToInt32(objReq.userId);
                            userObj.fkFriendId = Convert.ToInt32(isExist.pkUserId);
                            userObj.createdDate = DateTime.UtcNow;
                            userObj.isDelete = false;
                            userObj.updatedDate = DateTime.UtcNow;
                            response = 1;
                        }
                        else
                        {
                            userObj.userName = i.userName;
                            userObj.phoneNum = i.phoneNumber;
                            userObj.uniqueRequestId = objReq.sessionId;
                            userObj.isConfirm = false;
                            userObj.fkUserId = Convert.ToInt32(objReq.userId);
                            userObj.fkFriendId = 0;
                            userObj.createdDate = DateTime.UtcNow;
                            userObj.isDelete = false;
                            userObj.updatedDate = DateTime.UtcNow;
                            response = 2;
                        }
                        userList.Add(userObj);
                        // db.tblInviteFriends.InsertOnSubmit(obj);
                    }
                    db.tblInviteFriends.InsertAllOnSubmit(userList);
                    db.SubmitChanges();
                }
                return response;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }



        #endregion


        internal bool SetPaymentType(PaymentModel objReq)
        {

            using (var db = new WizzDataContext())
            {
                var userDetails = db.tblUsers.FirstOrDefault(x => x.pkUserId == Convert.ToInt64(objReq.userId) && x.isDelete == false);
                if (userDetails != null)
                {

                    userDetails.paymentType = Convert.ToInt16(objReq.paymentType);
                    db.SubmitChanges();
                    return true;
                }
                else
                {

                    return false;
                }

            }

        }

        internal bool PostBankAccountDetails(RespTutorAccountDetails objReq)
        {

            try
            {
                using (var db = new WizzDataContext())
                {
                    tblTutorBankDetail obj;
                    obj = db.tblTutorBankDetails.Where(c => c.fkUserId == Convert.ToInt64(objReq.userId)).FirstOrDefault();
                    if (obj == null)
                    {
                        obj = new tblTutorBankDetail();
                        obj.fkUserId = Convert.ToInt32(objReq.userId);
                        obj.pkId = Convert.ToInt32(objReq.accountId);
                        obj.bankName = objReq.bankName;
                        obj.accountHolderName = objReq.accountHolderName;
                        obj.accountNumber = objReq.accountNumber;
                        obj.ifscCode = objReq.ifscCode;
                        obj.micrCode = objReq.micrCode;
                        obj.branchAddress = objReq.branchAddress;
                        obj.createdDate = DateTime.UtcNow;
                        db.tblTutorBankDetails.InsertOnSubmit(obj);
                        db.SubmitChanges();
                        return true;
                    }
                    else
                    {
                        obj.fkUserId = Convert.ToInt32(objReq.userId);
                        //obj.pkId = Convert.ToInt32(model.accountId);
                        obj.bankName = objReq.bankName;
                        obj.accountHolderName = objReq.accountHolderName;
                        obj.accountNumber = objReq.accountNumber;
                        obj.ifscCode = objReq.ifscCode;
                        obj.micrCode = objReq.micrCode;
                        obj.branchAddress = objReq.branchAddress;
                        obj.createdDate = DateTime.UtcNow;
                        db.SubmitChanges();

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {

                return false;
            }

        }

        internal List<SessionResponseTutorModel> GetSessionsForTutor(RequestEntity objReq)
        {
            using (var db = new WizzDataContext())
            {

                List<SessionResponseTutorModel> studentList = new List<SessionResponseTutorModel>();
                var ListData = db.usp_GetSessionsForTutor(Convert.ToInt32(objReq.userId)).ToList();
                Int32 tutorId = Convert.ToInt32(objReq.userId);
                foreach (var c in ListData)
                {
                    SessionResponseTutorModel obj = new SessionResponseTutorModel();


                    obj.homeWork = c.homeWork;
                    obj.homeWorkDueDate = c.homeWorkDueDate;
                    obj.latitude = Convert.ToString(c.lat);
                    obj.location = c.location;
                    obj.longitude = Convert.ToString(c.longi);
                    obj.profilePic = c.profilePic;
                    obj.sessionEndTime = c.toTime;
                    obj.sessionStartTime = c.fromTime;
                    obj.fromTime = ReverseTimeConversionMethod(c.fromTime);
                    obj.toTime = ReverseTimeConversionMethod(c.toTime);
                    obj.dayType = Convert.ToString(c.dayType);
                    obj.subjectName = c.subjectName.ToString();
                    obj.studentId = c.studentId.ToString();
                    obj.studentName = c.studentName;
                    obj.uniqueRequestId = c.uniqueRequestId;
                    obj.sessionId = c.sessionId.ToString();
                    //var frien
                    // obj.friendsList = (from d in db.usp_GetFriendsOfUser(c.uniqueRequestId)
                    //                    select new FriendsListModel
                    //                    {
                    //                        friendId = Convert.ToString(d.fkFriendId),
                    //                        friendName = d.frienName == "" ? d.userName : d.frienName,
                    //                        amount = "",
                    //                        isConfirm = Convert.ToString(d.isConfirm),
                    //                        profilePic = string.IsNullOrWhiteSpace(d.profilePic) ? "" : d.profilePic.IndexOf("http") >= 0 ? d.profilePic : Constants.imagepath + d.profilePic
                    //                    }).ToList();
                    var friendData = db.usp_GetFriendsOfUser(c.sessionId.ToString()).ToList();
                    foreach (var d in friendData)
                    {

                      
                        if (d.fkFriendId == Convert.ToInt32(objReq.userId) || d.isDelete == true)
                        {

                            continue;
                        }
                        FriendsListModel mod = new FriendsListModel();
                        mod.friendId = Convert.ToString(d.fkFriendId);
                        mod.friendName = d.frienName == "" ? d.userName : d.frienName;
                        mod.amount = "";

                        mod.isConfirm = Convert.ToString(d.isConfirm);
                        mod.profilePic = string.IsNullOrWhiteSpace(d.profilePic) ? "" : d.profilePic.IndexOf("http") >= 0 ? d.profilePic : Constants.imagepath + d.profilePic;
                        if (mod.friendId == null)
                        {


                        }
                        else
                        {
                            obj.friendsList.Add(mod);

                        }


                    }


                    obj.sessionCost = CalculateSessionCost(fromTime: obj.fromTime, friendsCount: obj.friendsList.Count, db: db, toTime: obj.toTime, tutorId: tutorId);
                    studentList.Add(obj);

                }

                return studentList;
            }
        }
        internal string CalculateSessionCost(string fromTime, string toTime, int friendsCount, WizzDataContext db, Int64 tutorId)
        {
            int totalHours = Convert.ToInt32(toTime) / 2 - Convert.ToInt32(fromTime) / 2;
            var tutorData = db.tblTutorProfiles.FirstOrDefault(x => x.fkUserId == tutorId);
            Decimal totalCost = 0;
            totalCost = totalHours * Convert.ToInt32(tutorData.feesPerHour) + (friendsCount * 25 / 100);
            return totalCost.ToString();
        }
        internal GetGroupModel GetGroupDetails(GetGroupRequest objReq)
        {

            using (var db = new WizzDataContext())
            {
                var groupData = db.tblGroups.Where(x => x.groupId == objReq.groupId).FirstOrDefault();
                GetGroupModel OBjGroup = new GetGroupModel();
                OBjGroup.groupId = groupData.groupId;
                OBjGroup.groupPic = Constants.imagepath + groupData.groupPic;
                OBjGroup.subjectName = groupData.subjectName;
                OBjGroup.timeSlot = groupData.timeSlot;
                OBjGroup.groupName = groupData.groupName;
                OBjGroup.groupMembers = (from c in db.usp_P2GetGroupMembers(Convert.ToInt32(groupData.pkId))

                                         select new BasicUserModel
                                         {
                                             userName = c.userName,
                                             profilePic = c.profilePic,
                                             userId = c.pkUserId.ToString()
                                         }).ToList();
                return OBjGroup;


            }



        }

        internal bool DeleteGroup(GetGroupRequest objReq)
        {
            using (var db = new WizzDataContext())
            {
                var groupData = db.tblGroups.Where(x => x.groupId == objReq.groupId).FirstOrDefault();



                var groupMembers = db.tblGroupMembers.Where(x => x.fkGroupId == groupData.pkId).ToList();

                db.tblGroupMembers.DeleteAllOnSubmit(groupMembers);
                db.tblGroups.DeleteOnSubmit(groupData);
                db.SubmitChanges();



                return true;


            }

        }


        internal RespTutorAccountDetails GetTutorBankDetail(ReqTutorAccountDetails objReq)
        {
            RespTutorAccountDetails objResp = new RespTutorAccountDetails();

            if (string.IsNullOrEmpty(objReq.userId))
            {
                objReq.userId = "0";
            }

            using (var db = new WizzDataContext())
            {

                objResp = (from d in db.tblTutorBankDetails
                           where d.fkUserId == Convert.ToInt64(objReq.userId)
                           select new RespTutorAccountDetails
                               {
                                   micrCode = d.micrCode == null ? "" : d.micrCode,
                                   accountHolderName = d.accountHolderName.ToString(),
                                   accountId = d.accountNumber.ToString(),
                                   accountNumber = d.accountNumber.ToString(),
                                   branchAddress = d.branchAddress.ToString(),
                                   bankName = d.bankName.ToString(),
                                   ifscCode = d.ifscCode.ToString()

                               }).FirstOrDefault();
                if (objResp == null)
                {

                    objResp = new RespTutorAccountDetails();
                }
                return objResp;
            }
        }



        internal bool SetTutorAvailability(TutorAvailbilityModel objReq)
        {
            using (var db = new WizzDataContext())
            {
                var tutorObj = db.tblTutorProfiles.FirstOrDefault(x => x.fkUserId == Convert.ToInt64(objReq.userId));
                tutorObj.isAvailable = Convert.ToBoolean(objReq.isAvailable);
                db.SubmitChanges();
                return true;
                //throw new NotImplementedException(); 
            }
        }

        internal SearchResponseTutorsModel GetTutorBySearchCode(SearchTutorByCodeModel objReq)
        {
            using (var db = new WizzDataContext())
            {
                SearchResponseTutorsModel objResponse = new SearchResponseTutorsModel();
                List<TutorSearchResponseModel> tutorList = new List<TutorSearchResponseModel>();
                //   var userInfo = db.tblStudentRequests.Where(x => x.uniqueStudentRequestId == objReq.uniqueRequestId).FirstOrDefault();

                tutorList = (from c in db.usp_P2GetTutorBySearchCode(objReq.tutorCode)
                             select new TutorSearchResponseModel
                                                {
                                                    aboutTutor = c.About,
                                                    tutorName = c.userName,
                                                    tutorProfilePic = c.profilePic,
                                                    tutorLocation = c.location,
                                                    tutorId = Convert.ToString(c.fkUserId),
                                                    passingYear = Convert.ToString(c.passingYear),
                                                    tutorRating = Convert.ToString(c.avgRatingTutor),
                                                    tutorSubjects = db.usp_P2GetSubjectsForTutor(c.fkUserId).Select(x => x.subjectName).ToList(),
                                                    perHourFees = c.feesPerHour,
                                                    reviews =
                                                    (from r in db.tblTutorRatings.Where(x => x.fkTutorId == c.fkUserId)
                                                     select new TutorReviewModel
                                                                        {
                                                                            rating = Convert.ToString((r.helpful + r.knowledgable + r.punctual) / 3),
                                                                            studentName = r.userName,
                                                                            reviewText = r.review,

                                                                        }).ToList()


                                                }).ToList();


                objResponse.tutorList = tutorList;
                return objResponse;
            }
        }

        internal GetSettingsModel GetEligibleCourses(UserIdModel objReq)
        {
            using (var db = new WizzDataContext())
            {

                GetSettingsModel objResponse = new GetSettingsModel();
                //List<AcademicCoursesModel academic = new AcademicCoursesModel();
                //   List<tblTutorSubject> tutorSubList = new List<tblTutorSubject>();
                //   var userInfo = db.tblStudentRequests.Where(x => x.uniqueStudentRequestId == objReq.uniqueRequestId).FirstOrDefault();

                var tutorSubList = db.usp_GetEligibleSubjects(Convert.ToInt32(objReq.userId)).ToList();


                //foreach (var n in tutorSubList)
                //{
                //    switch (n.subjectType)
                //    {
                //        case 1:

                //            //   objResponse.listAcademicCourse.Add(n.fkSubjectId)
                //            break;
                //        case 2:
                //            break;
                //        case 3:

                //            break;
                //        default:


                //            break;

                //    }


                //}


                var courseData = db.tblCourses.Where(a => a.isDelete == false && a.isActive == true).ToList();
                var classes = db.tblClasses.Where(a => a.isActive == true && a.isDelete != true).ToList();
                var subjects = db.tblSubjects.Where(a => a.isActive == true && a.isDelete != true).ToList();

                //foreach (var n in courseData) {
                //    AcademicCoursesModel acModel = new AcademicCoursesModel();
                //    foreach (var c in classes)
                //    {
                //        ClassesModel clasModel = new ClassesModel();
                //        acModel.listClasses.Add(clasModel);
                //        foreach (var s in subjects)
                //        {
                //            SubjectsNewModel subModel = new SubjectsNewModel();

                //            var temp = tutorSubList.Where(z => z.fkSubjectId == s.fkClassId).FirstOrDefault();
                //            if (temp != null)
                //            {

                //                clasModel.listSubejcts.Add(subModel);
                //                objResponse.listAcademicCourse.Add(acModel);
                //            }







                //        }
                //    }

                //}

                objResponse.listAcademicCourse = (from c in courseData.Where(a => a.isExtraCurricular == false)
                                                  select new AcademicCoursesModel
                                                  {
                                                      courseId = Convert.ToString(c.pkId),
                                                      courseName = c.courseName,
                                                      isApproved = tutorSubList.Any(x => x.fkSubjectId == c.pkId && x.subjectType == 1).ToString(),
                                                      listClasses = (from cl in classes.Where(a => a.fkCourseId == c.pkId)
                                                                     select new ClassesModel
                                                                     {
                                                                         classId = Convert.ToString(cl.pkId),
                                                                         className = cl.className,
                                                                         isApproved = tutorSubList.Any(x => x.fkSubjectId == cl.pkId && x.subjectType == 2).ToString(),
                                                                         listSubejcts = (from sub in subjects.Where(a => a.fkClassId == cl.pkId)
                                                                                         select new SubjectsNewModel
                                                                                         {
                                                                                             subjectId = Convert.ToString(sub.pkId),
                                                                                             isApproved = tutorSubList.Any(x => x.fkSubjectId == sub.pkId && x.subjectType == 3).ToString(),
                                                                                             subjectName = sub.subjectName,
                                                                                         }).ToList()
                                                                     }).ToList()


                                                  }).ToList();
                objResponse.listCurricularCourses = (from c in courseData.Where(a => a.isExtraCurricular == true)
                                                     select new CurricularCoursesModel
                                                     {
                                                         courseId = Convert.ToString(c.pkId),
                                                         courseName = c.courseName,
                                                         isApproved = tutorSubList.Any(x => x.fkSubjectId == c.pkId && x.subjectType == 1).ToString(),
                                                         listClasses = (from cl in classes.Where(a => a.fkCourseId == c.pkId)
                                                                        select new ClassesModel
                                                                        {
                                                                            classId = Convert.ToString(cl.pkId),
                                                                            className = cl.className,
                                                                            isApproved = tutorSubList.Any(x => x.fkSubjectId == cl.pkId && x.subjectType == 2).ToString(),
                                                                            listSubejcts = (from sub in subjects.Where(a => a.fkClassId == cl.pkId)
                                                                                            select new SubjectsNewModel
                                                                                            {
                                                                                                subjectId = Convert.ToString(sub.pkId),
                                                                                                isApproved = tutorSubList.Any(x => x.fkSubjectId == sub.pkId && x.subjectType == 3).ToString(),
                                                                                                subjectName = sub.subjectName,
                                                                                            }).ToList()
                                                                        }).ToList()


                                                     }).ToList();






                //objResponse.tutorList = tutorSubList;
                return objResponse;
            }

            //  throw new NotImplementedException();
        }

        internal bool SetTutorSubjects(TutorSubjectModel objReq)
        {

            using (var db = new WizzDataContext())
            {


                List<tblTutorSubject> objTutorSubList = new List<tblTutorSubject>();
                Int64 userId = Convert.ToInt64(objReq.userId);
                objTutorSubList = db.tblTutorSubjects.Where(x => x.fkTutorId == userId).ToList();
                if (objTutorSubList.Count > 0)
                {
                    db.tblTutorSubjects.DeleteAllOnSubmit(objTutorSubList);
                    db.SubmitChanges();
                    objTutorSubList = new List<tblTutorSubject>();
                }

                if (objReq.subjectIdList.Count > 0)
                    foreach (var n in objReq.subjectIdList)
                    {
                        tblTutorSubject objTutorSubject = new tblTutorSubject();
                        objTutorSubject.isApproved =Convert.ToBoolean(n.isApproved);

                        objTutorSubject.fkSubjectId = Convert.ToInt32(n.id);
                        objTutorSubject.fkTutorId = userId;
                        objTutorSubject.subjectType = 3;
                        objTutorSubList.Add(objTutorSubject);
                    }
                if (objReq.classIdList.Count > 0)
                    foreach (var n in objReq.classIdList)
                    {
                        tblTutorSubject objTutorSubject = new tblTutorSubject();
                        objTutorSubject.isApproved = Convert.ToBoolean(n.isApproved);
                        objTutorSubject.fkSubjectId = Convert.ToInt32(n.id);
                        objTutorSubject.fkTutorId = userId;
                        objTutorSubject.subjectType = 2;
                        objTutorSubList.Add(objTutorSubject);
                    }
                if (objReq.courseIdList.Count > 0)
                {

                    foreach (var n in objReq.courseIdList)
                    {
                        tblTutorSubject objTutorSubject = new tblTutorSubject();
                        objTutorSubject.isApproved = Convert.ToBoolean(n.isApproved);
                        objTutorSubject.fkSubjectId = Convert.ToInt32(n.id);
                        objTutorSubject.fkTutorId = userId;
                        objTutorSubject.subjectType = 1;
                        objTutorSubList.Add(objTutorSubject);
                    }
                }
                db.tblTutorSubjects.InsertAllOnSubmit(objTutorSubList);
                db.SubmitChanges();
            }
            return true;
        }
    }
}