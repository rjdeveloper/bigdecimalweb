using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using wizz.Areas.Admin.Models;
using wizz.Models;
using wizz.Class;
using Newtonsoft.Json;
using System.Web.Security;



namespace wizz.Areas.Admin.AdminDal
{
    public class AdminMethods
    {
        #region Payal
        internal string adminLogin(AdminModel objReq)
        {
            string i = "0";
            try
            {
                using (var db = new WizzDataContext())
                {
                    tblUser admin = new tblUser();
                    if (objReq != null)
                    {
                        admin = db.tblUsers.Where(e => e.userEmail == objReq.email && e.registerType == 1).FirstOrDefault();
                        if (admin != null)
                        {
                            if (objReq.password.ToLower() == admin.password)
                            {

                                i = "loggedIn";
                                CurrentUser user = new CurrentUser()
                                {
                                    PkUserId = Convert.ToString(admin.pkUserId),
                                    Email = admin.userEmail,
                                    isActive = Convert.ToString(admin.isActive),
                                    usertype = Convert.ToString(admin.registerType),
                                };
                                string json = JsonConvert.SerializeObject(user);
                                FormsAuthentication.SetAuthCookie(json, true);

                            }
                            else
                            {
                                i = "Invalid password !";
                            }
                        }
                        else
                        {
                            i = "The Email Id is not registered with us !";
                        }
                    }
                    return i;
                }
            }
            catch (Exception ex)
            {
                i = ex.ToString();
                return i;
            }
        }
        internal int ChangePassword(AdminChangePassword objReq)
        {
            using (var db = new WizzDataContext())
            {

                tblUser tUser = new tblUser();
                CurrentUser cu = new CurrentUser();
                tUser = db.tblUsers.Where(x => x.pkUserId == Convert.ToInt32(cu.PkUserId)).FirstOrDefault();
                if (tUser.password != objReq.oldPassword)
                    return 0;
                else
                {
                    tUser.password = objReq.newPassword.Trim();
                    db.SubmitChanges();
                    return 1;
                }
            }

        }

        //internal SettingModel GetSettingsForAdmin()
        //{
        //    //System.Data.Entity.Core.Objects.ObjectParameter UpdatedLastSyncTime = new System.Data.Entity.Core.Objects.ObjectParameter("UpdatedLastSyncTime", typeof(string));

        //    SettingModel response = new SettingModel();

        //    using (var db = new WizzDataContext())
        //    {

        //        //get class list
        //        response.listClass = (from c in db.tblClasses.Where(a => a.isActive == true && a.isDelete != true)
        //                              select new SubjectsModel
        //                              {
        //                                  subjectId = Convert.ToString(c.pkClassId),
        //                                  subjectName = c.className
        //                              }).ToList();
        //        response.listColleges = (from c in db.tblColleges.Where(a => a.isDelete != true)
        //                                 select new Colleges
        //                                 {
        //                                     collegeId = Convert.ToString(c.pkId),
        //                                     collegeName = c.collegeName
        //                                 }).ToList();

        //        response.listMajor = (from c in db.tblCourses.Where(a => a.isDelete != true)
        //                              select new Majors
        //                              {
        //                                  majorId = Convert.ToString(c.pkCourseId),
        //                                  majorName = c.courseName,
        //                                  fkClassId = Convert.ToString(c.fkClassId)
        //                              }).ToList();

        //    }
        //    return response;
        //}
        //internal List<SettingsModel> GetAdminSettings(SettingsModel objReq)
        //{
        //    List<SettingsModel> settings = new List<SettingsModel>();
        //    try
        //    {
        //        using (var db = new WizzDataContext())
        //        {
        //            settings = db.tblSettings.Select(s => new SettingsModel()
        //            {
        //                perHourFees = s.perHourFees,
        //                commission = s.commission,
        //                perStudentCharge = s.perStudentCharge
        //            }).ToList();
        //        }
        //        return settings;
        //    }
        //    catch (Exception ex) { return settings; }
        //}

        //internal int SaveAdminSettings(SettingsModel objReq)
        //{
        //    try
        //    {
        //        using (var db = new WizzDataContext())
        //        {
        //            tblSetting settings = new tblSetting();
        //            settings = db.tblSettings.Where(e => e.pkSettingId != null).FirstOrDefault();
        //            if (settings == null)
        //            {
        //                settings = new tblSetting();
        //                settings.perHourFees = objReq.perHourFees;
        //                settings.commission = objReq.commission;
        //                settings.perStudentCharge = objReq.perStudentCharge;
        //                settings.timeStamp = DateTime.UtcNow.Ticks;
        //                db.tblSettings.InsertOnSubmit(settings);
        //                db.SubmitChanges();
        //                return 1;                              // Save
        //            }
        //            else
        //            {
        //                settings.perHourFees = objReq.perHourFees;
        //                settings.commission = objReq.commission;
        //                settings.perStudentCharge = objReq.perStudentCharge;
        //                settings.timeStamp = DateTime.UtcNow.Ticks;
        //                db.SubmitChanges();
        //                return 2;                              // Update
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return -1;
        //    }
        //}

        //internal int SaveClass(ClassModel objReq)
        //{
        //    try
        //    {
        //        using (var db = new WizzDataContext())
        //        {
        //            tblClass objClass = new tblClass();
        //            objClass = db.tblClasses.Where(e => e.pkClassId == objReq.pkClassId && e.isDelete == false).FirstOrDefault();
        //            if (objClass == null)
        //            {
        //                objClass = new tblClass();
        //                objClass.className = objReq.className;
        //                objClass.isActive = true;
        //                objClass.isDelete = false;
        //                objClass.fkCollegeId = objReq.fkCollegeId;
        //                objClass.createdDate = System.DateTime.UtcNow;
        //                objClass.updatedDate = System.DateTime.UtcNow;
        //                objClass.timeStamp = DateTime.UtcNow.Ticks;
        //                db.tblClasses.InsertOnSubmit(objClass);
        //                db.SubmitChanges();
        //                return 1;                           // Save
        //            }
        //            else if (objReq.pkClassId > 0 && objClass != null)
        //            {

        //                if (objClass.className.ToLower().Trim() == objReq.className.ToLower().Trim() && objClass.fkCollegeId == objReq.fkCollegeId)    //Already exists
        //                    return 3;
        //                objClass.className = objReq.className;
        //                objClass.fkCollegeId = objReq.fkCollegeId;
        //                objClass.updatedDate = System.DateTime.UtcNow;
        //                objClass.timeStamp = DateTime.UtcNow.Ticks;
        //                db.SubmitChanges();
        //                return 2;                          // Update
        //            }

        //            else
        //            {
        //                return 0;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //}

        //internal int SaveCollege(CollegeModel objReq)
        //{
        //    try
        //    {
        //        using (var db = new WizzDataContext())
        //        {
        //            tblCollege collegeData = new tblCollege();
        //            collegeData = db.tblColleges.Where(e => e.pkId == objReq.pkCollegeId && e.isDelete == false).FirstOrDefault();


        //            if (collegeData == null)
        //            {
        //                if (db.tblClasses.Where(e => e.className == objReq.collegeName && e.isDelete == false).Any())
        //                    return 3;                         //Already exists
        //                collegeData = new tblCollege();
        //                collegeData.collegeName = objReq.collegeName;

        //                collegeData.isDelete = false;
        //                collegeData.createdDate = System.DateTime.UtcNow;
        //                collegeData.updatedDate = System.DateTime.UtcNow;
        //                db.tblColleges.InsertOnSubmit(collegeData);
        //                db.SubmitChanges();
        //                return 1;                           // Save
        //            }
        //            else if (objReq.pkCollegeId > 0 && collegeData != null)
        //            {
        //                collegeData.collegeName = objReq.collegeName;
        //                collegeData.updatedDate = System.DateTime.UtcNow;
        //                db.SubmitChanges();
        //                return 2;                          // Update
        //            }

        //            else
        //            {
        //                return 0;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //}

        //internal List<ClassModel> GetClassesData()
        //{
        //    List<ClassModel> classData = new List<ClassModel>();
        //    try
        //    {
        //        using (var db = new WizzDataContext())
        //        {
        //            classData = db.tblClasses.ToList().Where(e => e.isDelete == false).Select(e => new ClassModel
        //            {
        //                pkClassId = e.pkClassId,
        //                className = e.className,
        //                isActive = Convert.ToBoolean(e.isActive),
        //                fkCollegeId = Convert.ToInt32(e.fkCollegeId),
        //                isDelete = Convert.ToBoolean(e.isDelete),
        //                createdDate = Convert.ToString(e.createdDate.Value.ToShortDateString()),
        //                updatedDate = Convert.ToString(e.updatedDate.Value.ToShortDateString())
        //            }).ToList();
        //        }
        //        return classData;
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Message.ToString();
        //        return classData;
        //    }
        //}

        //internal List<CollegeModel> GetCollegesData()
        //{
        //    List<CollegeModel> collegeData = new List<CollegeModel>();
        //    try
        //    {
        //        using (var db = new WizzDataContext())
        //        {
        //            collegeData = db.tblColleges.ToList().Where(e => e.isDelete == false).Select(e => new CollegeModel
        //            {
        //                pkCollegeId = e.pkId,
        //                collegeName = e.collegeName,
        //                isDelete = Convert.ToBoolean(e.isDelete),

        //                createdDate = Convert.ToString(e.createdDate.Value.ToShortDateString()),
        //                updatedDate = Convert.ToString(e.updatedDate.Value.ToShortDateString())
        //            }).ToList();
        //        }
        //        return collegeData;
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Message.ToString();
        //        return collegeData;
        //    }
        //}

        //internal bool ClassActions(ActionModel objReq)
        //{
        //    using (var db = new WizzDataContext())
        //    {

        //        tblClass classData = new tblClass();
        //        classData = db.tblClasses.Where(x => x.pkClassId == objReq.pkId && x.isDelete == false).FirstOrDefault();
        //        if (classData == null)
        //            return false;
        //        else
        //        {
        //            if (objReq.type == 2)
        //                classData.isDelete = true;
        //            else
        //                classData.isActive = !classData.isActive;
        //            db.SubmitChanges();
        //            return true;
        //        }
        //    }
        //}

        //internal bool CollegeActions(ActionModel objReq)
        //{
        //    using (var db = new WizzDataContext())
        //    {

        //        tblCollege collegeData = new tblCollege();
        //        collegeData = db.tblColleges.Where(x => x.pkId == objReq.pkId && x.isDelete == false).FirstOrDefault();
        //        if (collegeData == null)
        //            return false;
        //        else
        //        {
        //            collegeData.isDelete = true;
        //            db.SubmitChanges();
        //            return true;
        //        }
        //    }
        //}

        //#endregion

        //#region Rishabh
        //internal bool SavePromoCode(PromoModel objReq)
        //{
        //    using (var db = new WizzDataContext())
        //    {
        //        tblPromoCode promoData = new tblPromoCode();
        //        promoData = db.tblPromoCodes.Where(p => p.pkPromoCodeId == objReq.pkPromoId && p.isActive == true && p.isDelete == false).FirstOrDefault();
        //        if (promoData == null)
        //        {
        //            if (db.tblPromoCodes.Any(p => p.promoCode == objReq.promoName && p.isDelete == false))
        //                return false;
        //            promoData = new tblPromoCode();
        //            promoData.promoCode = objReq.promoName;
        //            promoData.isActive = true;
        //            promoData.isDelete = false;
        //            promoData.validFrom = Convert.ToDateTime(objReq.validFrom);
        //            promoData.validTo = Convert.ToDateTime(objReq.validTo);
        //            promoData.updatedDate = DateTime.UtcNow;
        //            promoData.discountPercentage = objReq.discount;
        //            promoData.createdDate = DateTime.UtcNow;
        //            promoData.description = objReq.description;
        //            promoData.usageCount = objReq.counts;
        //            db.tblPromoCodes.InsertOnSubmit(promoData);
        //        }
        //        else if (objReq.pkPromoId > 0 && promoData != null)
        //        {

        //            promoData.promoCode = objReq.promoName.Trim();
        //            promoData.isActive = true;
        //            promoData.isDelete = false;
        //            promoData.validFrom = Convert.ToDateTime(objReq.validFrom);
        //            promoData.validTo = Convert.ToDateTime(objReq.validTo);
        //            promoData.updatedDate = DateTime.UtcNow;
        //            promoData.discountPercentage = objReq.discount;
        //            promoData.usageCount = objReq.counts;
        //            promoData.createdDate = DateTime.UtcNow;
        //            promoData.description = objReq.description;
        //        }
        //        else
        //        {

        //            return false;
        //        }
        //        db.SubmitChanges();
        //        return true;
        //    }

        //}
        //internal List<PromoModel> GetPromoCodes()
        //{
        //    using (var db = new WizzDataContext())
        //    {


        //        List<PromoModel> objList = new List<PromoModel>();
        //        return objList = db.GetPromoCodes().ToList().Select((m, i) => new PromoModel()
        //        {
        //            counts = Convert.ToInt32(m.counts),
        //            usageTimes = Convert.ToInt32(m.usageTimes),
        //            promoName = m.promoName,
        //            validFrom = m.validFrom,
        //            validTo = m.validTo,
        //            pkPromoId = m.pkPromoId,
        //            discount = Convert.ToInt32(m.discount),
        //            description = m.description,
        //            sno = Convert.ToString(i + 1),
        //            isActive = Convert.ToBoolean(m.isActive)
        //        }).ToList();
        //    }

        //}

        //internal bool PromoActions(ActionModel objReq)
        //{

        //    using (var db = new WizzDataContext())
        //    {

        //        tblPromoCode promoData = new tblPromoCode();
        //        promoData = db.tblPromoCodes.Where(x => x.pkPromoCodeId == objReq.pkId && x.isDelete == false).FirstOrDefault();
        //        if (promoData == null)
        //            return false;
        //        else
        //        {
        //            if (objReq.type == 2)
        //                promoData.isDelete = true;
        //            else
        //                promoData.isActive = !promoData.isActive;
        //            db.SubmitChanges();
        //            return true;
        //        }
        //    }

        //}
        //#region courseMethods
        //internal bool SaveCourse(CourseModel objReq)
        //{


        //    using (var db = new WizzDataContext())
        //    {
        //        tblCourse courseData = new tblCourse();
        //        courseData = db.tblCourses.Where(p => p.courseName == objReq.courseName.Trim() && p.isActive == true && p.isDelete == false).FirstOrDefault();
        //        if (courseData == null)
        //        {
        //            courseData = new tblCourse();
        //            courseData.courseName = objReq.courseName;
        //            courseData.isActive = true;
        //            courseData.isDelete = false;
        //            courseData.fkClassId = objReq.fkClassId;
        //            courseData.updatedDate = DateTime.UtcNow;
        //            courseData.createdDate = DateTime.UtcNow;
        //            db.tblCourses.InsertOnSubmit(courseData);
        //            db.SubmitChanges();
        //        }
        //        else if (objReq.pkCourseId > 0 && courseData != null)
        //        {


        //            courseData.courseName = objReq.courseName;
        //            courseData.isActive = true;
        //            courseData.isDelete = false;
        //            courseData.fkClassId = objReq.fkClassId;
        //            courseData.updatedDate = DateTime.UtcNow;
        //            courseData.createdDate = DateTime.UtcNow;
        //            db.SubmitChanges();
        //        }
        //        else
        //        {

        //            return false;
        //        }

        //        return true;
        //    }

        //}

        //internal List<CourseModel> GetCourses()
        //{

        //    using (var db = new WizzDataContext())
        //    {


        //        List<CourseModel> objList = new List<CourseModel>();
        //        return objList = db.tblCourses.Where(c => c.isDelete == false).ToList().Select((m, i) => new CourseModel()
        //        {
        //            pkCourseId = m.pkCourseId,
        //            fkClassId = Convert.ToInt32(m.fkClassId),
        //            courseName = m.courseName,
        //            isActive = Convert.ToBoolean(m.isActive),
        //            isDelete = Convert.ToBoolean(m.isDelete),
        //            sno = Convert.ToString(i + 1)
        //        }).ToList();
        //    }

        //}
        //internal List<ClassModel> GetClasses()
        //{
        //    using (var db = new WizzDataContext())
        //    {
        //        List<ClassModel> objList = new List<ClassModel>();
        //        return objList = db.tblClasses.Where(c => c.isDelete == false && c.isActive == true).ToList().Select((m, i) => new ClassModel()
        //        {
        //            pkClassId = m.pkClassId,
        //            className = m.className
        //        }).ToList();
        //    }
        //}
        //internal bool CourseActions(ActionModel objReq)
        //{
        //    using (var db = new WizzDataContext())
        //    {

        //        tblCourse courseData = new tblCourse();
        //        courseData = db.tblCourses.Where(x => x.pkCourseId == objReq.pkId && x.isDelete == false).FirstOrDefault();
        //        if (courseData == null)
        //            return false;
        //        else if (db.tblTutorSubjects.Any(x => x.fkSubjectId == objReq.pkId) && objReq.type == 2)
        //            return false;
        //        else
        //        {
        //            if (objReq.type == 2)
        //                courseData.isDelete = true;
        //            else
        //                courseData.isActive = !courseData.isActive;
        //            db.SubmitChanges();
        //            return true;
        //        }
        //    }
        //}
        #endregion
        //#region User methods
        //internal List<UserAdminModel> GetStudents()
        //{
        //    using (var db = new WizzDataContext())
        //    {
        //        List<UserAdminModel> objList = new List<UserAdminModel>();
        //        return objList = db.tblUsers.Where(c => c.isDelete == false).ToList().Select((m, i) => new UserAdminModel()
        //         {
        //             userEmail = m.userEmail,
        //             pkUserId = Convert.ToInt32(m.pkUserId),
        //             profilePic = m.profilePic,
        //             userName = m.userName,
        //             credits = m.credits,
        //             isActive = Convert.ToBoolean(m.isActive),
        //             isDelete = Convert.ToBoolean(m.isDelete),
        //             sno = Convert.ToString(i + 1),
        //             isVarified = Convert.ToBoolean(m.isVarified),
        //         }).ToList();

        //    }


        //}

        //internal List<TutorDescriptionModel> GetTutors()
        //{
        //    using (var db = new WizzDataContext())
        //    {
        //        List<TutorDescriptionModel> objList = new List<TutorDescriptionModel>();
        //        return objList = db.usp_GetTutorsForAdmin().ToList().Select((m, i) => new TutorDescriptionModel()
        //        {
        //            userEmail = m.userEmail,
        //            pkUserId = Convert.ToInt32(m.pkUserId),
        //            profilePic = m.profilePic,
        //            userName = m.userName,
        //            sno = Convert.ToString(i + 1),
        //            isApproved = Convert.ToBoolean(m.isApproved),
        //            docUrl = m.docName == null ? "" : m.docName,
        //            passingYear = m.passingYear,
        //            fkClassId = Convert.ToInt32(m.fkclassId),
        //            fkCollegeId = Convert.ToInt32(m.fkcollegeId),
        //            subjects = m.subjects,

        //        }).ToList();

        //    }


        //}


        //internal bool UserActions(ActionModel objReq)
        //{
        //    using (var db = new WizzDataContext())
        //    {

        //        tblUser userData;
        //        userData = db.tblUsers.Where(x => x.pkUserId == objReq.pkId).FirstOrDefault();
        //        if (userData == null)
        //            return false;
        //        else
        //        {
        //            if (objReq.type == 2)
        //            {
        //                userData.isDelete = true;
        //                var profileData = db.tblTutorProfiles.Where(x => x.fkUserId == objReq.pkId).FirstOrDefault();
        //                db.tblTutorProfiles.DeleteOnSubmit(profileData);


        //            }

        //            else
        //                userData.isActive = !userData.isActive;
        //            db.SubmitChanges();
        //            return true;
        //        }
        //    }
        //}
        //#endregion


        //#region tutorMethods

        //internal List<TutorSubjectModel> GetTutorSubjects(string userId)
        //{

        //    using (var db = new WizzDataContext())
        //    {
        //        List<TutorSubjectModel> objList = new List<TutorSubjectModel>();
        //        return objList = db.tblTutorSubjects.Where(x => x.pkTutorSubjectId != null && x.fkTutorId == Convert.ToInt64(userId)).ToList().Select((m, i) => new TutorSubjectModel()
        //        {
        //            pkTutorSubjectId = Convert.ToInt32(m.pkTutorSubjectId),
        //            subjectId = Convert.ToInt32(m.fkSubjectId),
        //            isApproved = Convert.ToBoolean(m.isApproved)
        //        }).ToList();

        //    }

        //}


        ////internal bool ApproveTutorSubjects(TutorSubjectsModel objReq)
        ////{
        ////    using (var db = new WizzDataContext())
        ////    {
        ////        //List<TutorSubjectModel> objList = new List<TutorSubjectModel>();
        ////        var objList = db.tblTutorSubjects.Where(x => x.fkTutorId == objReq.userId).ToList();
        ////        db.tblTutorSubjects.DeleteAllOnSubmit(objList);
        ////        //  db.SubmitChanges();
        ////        tblTutorSubject tutSubObj;
        ////        List<tblTutorSubject> dbSubList = new List<tblTutorSubject>();
        ////        foreach (var n in objReq.tutorSubjectList)
        ////        {
        ////            tutSubObj = new tblTutorSubject();
        ////            tutSubObj.fkTutorId = objReq.userId;
        ////            tutSubObj.fkSubjectId = n.subjectId;
        ////            tutSubObj.isApproved = n.isApproved;
        ////            dbSubList.Add(tutSubObj);
        ////        }
        ////        db.tblTutorSubjects.InsertAllOnSubmit(dbSubList);
        ////        db.SubmitChanges();

        ////        var tutorProfile = db.tblTutorProfiles.Where(t => t.fkUserId == objReq.userId).FirstOrDefault();
        ////        if (objReq.tutorSubjectList.Where(s => s.isApproved == true).Count() > 0)
        ////        {
        ////            if (tutorProfile.isApproved != true)
        ////            {
        ////                tutorProfile.isApproved = true;
        ////            }
        ////            var tutor = db.tblUsers.Where(t => t.pkUserId == objReq.userId).FirstOrDefault();
        ////            //send push


        ////            PushData push = new PushData();
        ////            push.registration_ids = tutor.deviceToken;
        ////            push.message = PushMessages.approveSubject;
        ////            push.type = Convert.ToInt16(PushType.approveTutor).ToString();
        ////            push.badge = Convert.ToString(tutor.badge);
        ////            if (tutor.deviceType == Convert.ToInt16(DeviceType.ios))//ios
        ////            {
        ////                SendPush objIOS = new SendPush();
        ////                objIOS.ConnectToAPNS(push);
        ////            }
        ////            else if (tutor.deviceType == Convert.ToInt16(DeviceType.android))//android
        ////            {
        ////                Android objAndroid = new Android();
        ////                objAndroid.send(push);
        ////            }

        ////        }
        ////        else
        ////        {
        ////            tutorProfile.isApproved = false;
        ////        }
        ////        db.SubmitChanges();

        ////    }
        ////    return true;
        ////}
        //#endregion
   











        internal List<MajorModel> GetMajors()
        {
            throw new NotImplementedException();
        }
    }
}