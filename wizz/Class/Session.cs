using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using wizz.Models;
using System.Data.Entity;
namespace wizz.Class
{
    /// <summary>
    /// Tasks related to session management
    /// </summary>
    public class Session
    {
        internal string AcceptTutor(CreateSesion objRequest)
        {
            string response = "";


            //using (var db = new WizzDataContext())
            //{

            //    // response = sp call

            //}
            return response;
        }

        #region oldCode
        //internal List<RespNewRequest> GetSessions(Entity objRequest)
        //{
        //    List<RespNewRequest> response = new List<RespNewRequest>();
        //    using (var db = new WizzDataContext())
        //    {
        //        response = (from i in db.usp_GetNewRequest(0, Convert.ToInt64(objRequest.userId), true)
        //                    select new RespNewRequest
        //                    {
        //                        accompanyNum = Convert.ToString(i.accompanyNum),
        //                        assignment = i.assignment,
        //                        calculatedAmount = Convert.ToString(i.calculatedAmount),
        //                        className = Convert.ToString(i.className),
        //                        forMinutes = Convert.ToString(i.forMinutes),
        //                        latitude = Convert.ToString(i.latitude),
        //                        longitude = Convert.ToString(i.longitude),
        //                        profilepic = string.IsNullOrWhiteSpace(i.tutorPic) ? "" : i.tutorPic.IndexOf("http") >= 0 ? i.tutorPic : Constants.imagepath + i.tutorPic,

        //                        requestId = Convert.ToString(i.requestId),
        //                        uniqueRequestId = Convert.ToString(i.uniqueRequestId),
        //                        name = i.tutorName,
        //                        id = Convert.ToString(i.fkTutorId),
        //                        // tutorId = Convert.ToString(i.fkTutorId),
        //                        // tutorName = i.tutorName,
        //                        isNow = i.isNow,
        //                        location = i.location,
        //                        listSlot = (from l in db.tblRequestStudents
        //                                    where l.uniqueRequestId == i.uniqueRequestId
        //                                    select new TimeSlot
        //                                    {
        //                                        dayNo = l.day == null ? "0" : l.day.ToString(),
        //                                        slotId = l.pkRequirementId.ToString(),
        //                                        fromTime = l.fromTime,
        //                                        toTime = l.toTime,
        //                                        isBooked = l.isBooked == true ? "True" : "False",
        //                                    }).ToList(),// im i in objDBContext.usp_GetItemListForApp(0, 0, Convert.ToInt64(objReq.merchantId), 0, 0, 0, "", 0)
        //                        listTutorSlot = (from l in db.tblSlotManagements
        //                                         where l.uniqueRequestId == i.uniqueRequestId
        //                                         select new TimeSlot
        //                                         {
        //                                             dayNo = l.day == null ? "0" : l.day.ToString(),
        //                                             slotId = l.pkId.ToString(),
        //                                             fromTime = l.fromTime,
        //                                             toTime = l.toTime,
        //                                         }).ToList()
        //                    }).ToList();


        //    }


        //    return response;
        //}
        //internal RespNewRequest GetSessionById(Entity objRequest)
        //{
        //    RespNewRequest response = new RespNewRequest();
        //    using (var db = new WizzDataContext())
        //    {
        //        response = (from i in db.usp_GetSessionInfoById(objRequest.uniqueRequestId,Convert.ToInt64(objRequest.userId))
        //                    select new RespNewRequest
        //                       {
        //                           className = Convert.ToString(i.className),
        //                           profilepic = string.IsNullOrWhiteSpace(i.profilePic) ? "" : i.profilePic.IndexOf("http") >= 0 ? i.profilePic : Constants.imagepath + i.profilePic,
        //                           name = i.userName,
        //                           isNow=i.isNow.ToString(),
        //                           calculatedAmount=Convert.ToString(i.calculatedAmount),
        //                           sessionStartTime=i.sessionStartTime,
        //                           sessionEndTime=i.sesssionEndTime,
        //                         requestId=objRequest.uniqueRequestId,
        //                           id = Convert.ToString(i.pkUserId),
        //                           listSlot = (from l in db.tblRequestStudents
        //                                       where l.uniqueRequestId == objRequest.uniqueRequestId
        //                                       select new TimeSlot
        //                                       {
        //                                           dayNo = l.day == null ? "0" : l.day.ToString(),
        //                                           slotId = l.pkRequirementId.ToString(),
        //                                           fromTime = l.fromTime,
        //                                           toTime = l.toTime,
        //                                           isBooked = l.isBooked == true ? "True" : "False",
        //                                       }).ToList(),// im i in objDBContext.usp_GetItemListForApp(0, 0, Convert.ToInt64(objReq.merchantId), 0, 0, 0, "", 0)
        //                           listTutorSlot = (from l in db.tblSlotManagements
        //                                            where l.uniqueRequestId == objRequest.uniqueRequestId
        //                                            select new TimeSlot
        //                                            {
        //                                                dayNo = l.day == null ? "0" : l.day.ToString(),
        //                                                slotId = l.pkId.ToString(),
        //                                                fromTime = l.fromTime,
        //                                                toTime = l.toTime,
        //                                            }).ToList()
        //                       }).FirstOrDefault();


        //    }


        //    return response;
        //}
        //internal HistoryModel GetSessionHistory(Entity objRequest)
        //{
        ////     public string paidAmount { get; set; }
        ////public string sessionDate { get; set; }
        ////public string tutorId { get; set; }
        ////public string tutorName { get; set; }
        ////public string tutorImage { get; set; }
        ////public string subject { get; set; }

        //    HistoryModel response = new HistoryModel();



        //    //using (var db = new WizzDataContext())
        //    //{
        //    for (int i = 1; i < 5; i++)
        //    {

        //        HistoryModel sm = new HistoryModel();
        //        sm.availableAmount = "50" + i * 24;
        //        sm.learnHours = "4" + i * i * 4;
        //        sm.listTutor = new List<HistoryTutor>();
        //        HistoryTutor ts = new HistoryTutor();
        //        ts.paidAmount = "" + i * 100;
        //        ts.tutorId = "7" + i;
        //        ts.sessionDate = DateTime.UtcNow.AddHours(i).ToString("dd MMM yyyy");
        //        switch (i)
        //        {
        //            case 1:

        //                ts.tutorName = "Bruce waine";
        //                ts.subject = "Chemistry";
        //                ts.tutorImage = "http://farm9.static.flickr.com/8386/8490887372_fb1622edac_m.jpg";
        //                break;
        //            case 2:
        //                ts.tutorName = "Berry allen";
        //                ts.subject = "Physics";
        //                ts.tutorImage = "http://farm4.static.flickr.com/3308/5759383374_1f948b988b_m.jpg";
        //                break;
        //            case 3:
        //                ts.tutorName = "Oliver Queen";
        //                ts.subject = "Physical";
        //                ts.tutorImage = "http://farm8.static.flickr.com/7258/13547010673_e62471bfc0_m.jpg";
        //                break;
        //            case 4:
        //                ts.tutorName = "Clark";
        //                ts.tutorImage = "http://farm9.static.flickr.com/8386/8490887372_fb1622edac_m.jpg";
        //                ts.subject = "Astrology";
        //                break;                     
        //        }

        //        response.listTutor.Add(ts);


        //    }



        //    // response = sp call

        //    // }
        //    return response;

        //    //using (var db = new WizzDataContext())
        //    //{

        //    //    // response = sp call

        //    //}
        //    //return response;
        //}

        //internal List<RespNewRequest> GetSessionsForTutor(Entity objRequest)
        //{
        //    List<RespNewRequest> response = new List<RespNewRequest>();
        //    using (var db = new WizzDataContext())
        //    {
        //        response = (from i in db.usp_GetNewRequest(Convert.ToInt64(objRequest.userId), 0, true)
        //                    select new RespNewRequest
        //                    {
        //                        accompanyNum = Convert.ToString(i.accompanyNum),
        //                        assignment = i.assignment,
        //                        calculatedAmount = Convert.ToString(i.calculatedAmount),
        //                        className = Convert.ToString(i.className),
        //                        forMinutes = Convert.ToString(i.forMinutes),
        //                        latitude = Convert.ToString(i.latitude),
        //                        longitude = Convert.ToString(i.longitude),
        //                        profilepic = string.IsNullOrWhiteSpace(i.studentPic) ? "" : i.studentPic.IndexOf("http") >= 0 ? i.studentPic : Constants.imagepath + i.studentPic,

        //                        requestId = Convert.ToString(i.requestId),
        //                        uniqueRequestId = Convert.ToString(i.uniqueRequestId),
        //                        name = i.studenName,
        //                        id = Convert.ToString(i.fkStudentId),
        //                        // tutorId = Convert.ToString(i.fkTutorId),
        //                        // tutorName = i.tutorName,
        //                        isNow = i.isNow,
        //                        location = i.location,
        //                        listSlot = (from l in db.tblRequestStudents
        //                                    where l.uniqueRequestId == i.uniqueRequestId
        //                                    select new TimeSlot
        //                                    {
        //                                        dayNo = l.day == null ? "0" : l.day.ToString(),
        //                                        slotId = l.pkRequirementId.ToString(),
        //                                        fromTime = l.fromTime,
        //                                        toTime = l.toTime,
        //                                        isBooked = l.isBooked == true ? "True" : "False",
        //                                    }).ToList(), // im i in objDBContext.usp_GetItemListForApp(0, 0, Convert.ToInt64(objReq.merchantId), 0, 0, 0, "", 0)
        //                        listTutorSlot = (from l in db.tblSlotManagements
        //                                         where l.uniqueRequestId == i.uniqueRequestId
        //                                         select new TimeSlot
        //                                         {
        //                                             dayNo = l.day == null ? "0" : l.day.ToString(),
        //                                             slotId = l.pkId.ToString(),
        //                                             fromTime = l.fromTime,
        //                                             toTime = l.toTime,
        //                                         }).ToList()

        //                    }).ToList();


        //    }

        //    //if (response==null || response.Count==0)
        //    //{
        //    //for (int i = 1; i < 5; i++)
        //    //{

        //    //    RespNewRequest sm = new RespNewRequest();
        //    //    switch (i)
        //    //    {
        //    //        case 1:
        //    //            sm.assignment = "Trigo,Geometery,Calculus";
        //    //            sm.className = "Math";
        //    //            sm.forMinutes = "125";
        //    //            sm.name = "McMohan";
        //    //            sm.profilepic = "https://staging10.techaheadcorp.com/wizz/WebImages/95e6458ba8824e02885a26fcfc2d8e50.jpg";
        //    //            sm.id = "10" + i;

        //    //            break;
        //    //        case 2:
        //    //            sm.assignment = "Thermodynamics,Gravity,Law of Motion";
        //    //            sm.className = "12";
        //    //            sm.forMinutes = "" + i * 2;
        //    //            sm.name = "Shrikant";
        //    //            sm.profilepic = "https://staging10.techaheadcorp.com/wizz/WebImages/15a29c136a054446a2cd79884c878f16.jpg";
        //    //            sm.id = "10" + i;
        //    //            break;
        //    //        case 3:
        //    //            sm.assignment = "Compiler design,Automata,Turing machine";
        //    //            sm.className = "9";
        //    //            sm.forMinutes = "" + i * 2;
        //    //            sm.name = "Yash";
        //    //            sm.profilepic = "https://staging10.techaheadcorp.com/wizz/WebImages/1ba0ba5992ca43f086837f30e7720972.jpg";
        //    //            sm.id = "10" + i;
        //    //            break;
        //    //        case 4:
        //    //            sm.assignment = "basics of c,Printers,History of computers";
        //    //            sm.className = "16";
        //    //            sm.forMinutes = "" + i * 2;
        //    //            sm.name = "Shweta";
        //    //            sm.profilepic = "https://staging10.techaheadcorp.com/wizz/WebImages/1c84e4b5e8df49ea9940794310f23c45.jpg";
        //    //            sm.id = "10" + i;
        //    //            break;

        //    //    }
        //    //    sm.accompanyNum = (i * i + 2).ToString();
        //    //    sm.listSlot = new List<TimeSlot>();
        //    //    TimeSlot ts = new TimeSlot();
        //    //    ts.dayNo = "" + i;
        //    //    ts.fromTime = DateTime.UtcNow.AddHours(i).ToString("dd-MMM-yyyy");
        //    //    ts.toTime = DateTime.UtcNow.AddHours(i + 2).ToString("dd-MMM-yyyy");
        //    //    ts.slotId = "" + i * 32;
        //    //    response.Add(sm);
        //    //   // .Add(sm);
        //    //}



        //    // response = sp call

        //    //  }
        //    return response;
        //}
        //internal TutorHistoryModel GetTutorSessionHistory(Entity objRequest)
        //{
        //    //     public string paidAmount { get; set; }
        //    //public string sessionDate { get; set; }
        //    //public string tutorId { get; set; }
        //    //public string tutorName { get; set; }
        //    //public string tutorImage { get; set; }
        //    //public string subject { get; set; }

        //    TutorHistoryModel response = new TutorHistoryModel();

        //    response.availableAmount = "500"  ;
        //    response.learnHours = "4" ;

        //    //using (var db = new WizzDataContext())
        //    //{
        //    for (int i = 1; i < 5; i++)
        //    {

        //        HistoryStudent ts = new HistoryStudent();
        //        ts.paidAmount = "" + i * 100;
        //        ts.studentId = "7" + i;
        //        ts.sessionDate = DateTime.UtcNow.AddHours(i).ToString("dd MMM yyyy");
        //        switch (i)
        //        {
        //            case 1:

        //                ts.studentName = "Bruce waine";
        //                ts.subject = "Chemistry";
        //                ts.studentImage = "staging10.techaheadcorp.com/wizz/WebImages/0c02ecd2bcb44faca7f80bb6cf52cb18.jpg";
        //                break;
        //            case 2:
        //                ts.studentName = "Berry allen";
        //                ts.subject = "Physics";
        //                ts.studentImage = "staging10.techaheadcorp.com/wizz/WebImages/15a29c136a054446a2cd79884c878f16.jpg";
        //                break;
        //            case 3:
        //                ts.studentName = "Oliver Queen";
        //                ts.subject = "Physical";
        //                ts.studentImage = "staging10.techaheadcorp.com/wizz/WebImages/1c84e4b5e8df49ea9940794310f23c45.jpg";
        //                break;
        //            case 4:
        //                ts.studentName = "Clark";
        //                ts.studentImage = "staging10.techaheadcorp.com/wizz/WebImages/33fc9351b4e64434b64938d03e8935ec.jpg";
        //                ts.subject = "Astrology";
        //                break;
        //        }


        //        response.listStudent.Add(ts);

        //    }
        //    // response = sp call

        //    // }
        //    return response;

        //    //using (var db = new WizzDataContext())
        //    //{

        //    //    // response = sp call

        //    //}
        //    //return response;
        //}
        //internal bool BookTimeSlot(TimeSlotEntity objReq)
        //{
        //    using (var db = new WizzDataContext())
        //    {

        //        var reqData = db.tblRequestStudents.Where(x => x.uniqueRequestId == objReq.uniqueRequestId).FirstOrDefault();
        //        //  var sessionData=db.tblRequests.Where(x=>x.uniqueRequestId==objReq.uniqueRequestId).FirstOrDefault();
        //        if (reqData == null)
        //            return false;
        //        reqData.isBooked = true;


        //        tblSlotManagement slotData;
        //        List<tblSlotManagement> slotList = new List<tblSlotManagement>();
        //        slotList = db.tblSlotManagements.Where(x => x.uniqueRequestId == objReq.uniqueRequestId).ToList();
        //        if (slotList.Count > 0)
        //        {
        //            db.tblSlotManagements.DeleteAllOnSubmit(slotList);
        //            db.SubmitChanges();

        //        }
        //        slotList = new List<tblSlotManagement>();
        //        foreach (var n in objReq.listSlot)
        //        {
        //            slotData = new tblSlotManagement();
        //            slotData.day = Convert.ToInt16(n.dayNo);
        //            slotData.fkStudentId = Convert.ToInt64(objReq.studentId);
        //            slotData.fkTutorId = Convert.ToInt64(objReq.tutorId);
        //            slotData.fromTime = n.fromTime;
        //            slotData.toTime = n.toTime;
        //            slotData.updatedDate = DateTime.UtcNow;
        //            slotData.createdDate = DateTime.UtcNow;
        //            slotData.uniqueRequestId = objReq.uniqueRequestId;
        //            slotList.Add(slotData);
        //        }

        //        reqData.updatedDate = DateTime.UtcNow;
        //        db.tblSlotManagements.InsertAllOnSubmit(slotList);
        //        db.SubmitChanges();
        //    }
        //    return true;

        //}

        //internal List<RespNewRequest> GetHistoryForUser(Entity objReq)
        //{
        //    List<RespNewRequest> objResp = new List<RespNewRequest>();
        //    long studentId = 0;
        //    long tutorId = 0;
        //    if (objReq.isTutor == "True")
        //    {
        //        tutorId = Convert.ToInt64(objReq.userId);
        //    }
        //    else
        //    {
        //        studentId = Convert.ToInt64(objReq.userId);
        //    }
        //    using (var db = new WizzDataContext())
        //    {
        //        objResp = (from i in db.usp_GetUserHistory(tutorId, studentId)
        //                   select new RespNewRequest
        //                   {
        //                       accompanyNum = Convert.ToString(i.accompanyNum),
        //                       assignment = i.assignment,
        //                       calculatedAmount = Convert.ToString(objReq.isTutor == "True"?i.tutorEarnedAmt:i.calculatedAmount),
        //                       className = Convert.ToString(i.className),
        //                       forMinutes = Convert.ToString(i.forMinutes),
        //                       latitude = Convert.ToString(i.latitude),
        //                       longitude = Convert.ToString(i.longitude),
        //                       profilepic = studentId == 0 ? (string.IsNullOrWhiteSpace(i.studentPic) ? "" : i.studentPic.IndexOf("http") >= 0 ? i.studentPic : Constants.imagepath + i.studentPic) : (string.IsNullOrWhiteSpace(i.tutorPic) ? "" : i.tutorPic.IndexOf("http") >= 0 ? i.tutorPic : Constants.imagepath + i.tutorPic),
        //                       sessionDate = i.sessionenddate,
        //                       requestId = Convert.ToString(i.requestId),
        //                       uniqueRequestId = Convert.ToString(i.uniqueRequestId),
        //                       name = studentId == 0 ? i.studenName : i.tutorName,
        //                       id = studentId == 0 ? Convert.ToString(i.fkStudentId) : Convert.ToString(i.fkTutorId),
        //                       //   tutorId = Convert.ToString(i.fkTutorId),
        //                       // tutorName = i.tutorName,

        //                       location = i.location,
        //                       listSlot = (from l in db.tblRequestStudents
        //                                   where l.uniqueRequestId == i.uniqueRequestId
        //                                   select new TimeSlot
        //                                   {
        //                                       dayNo = l.day == null ? "0" : l.day.ToString(),
        //                                       slotId = l.pkRequirementId.ToString(),
        //                                       fromTime = l.fromTime,
        //                                       toTime = l.toTime
        //                                   }).ToList() // im i in objDBContext.usp_GetItemListForApp(0, 0, Convert.ToInt64(objReq.merchantId), 0, 0, 0, "", 0)

        //                   }).ToList();
        //        return objResp;
        //    }

        //}
        #endregion
        #region V2Code
        internal bool StartSession(CreateSesion objReq)
        {
            using (var db = new WizzDataContext())
            {

                var sessionData = db.tblSessions.Where(x => x.pkSessionId == Convert.ToInt64(objReq.sessionId)).FirstOrDefault();
                if (sessionData == null)
                    return false;
                sessionData.sessionStartTime = objReq.startTime;
                sessionData.sessionDate = DateTime.UtcNow.Date.ToString("dd mm yyyy");
                sessionData.isStarted = true;
                sessionData.isComplete = false;
                sessionData.isCancelled = false;
                sessionData.isDelete = false;
                sessionData.updatedDate = DateTime.UtcNow;
                //sending push to the student
                SendSessionStartPushes(db, Convert.ToInt64(sessionData.studentId), sessionData.uniqueRequestId, sessionData.pkSessionId);

                db.SubmitChanges();
            }
            return true;
        }
        int globalMinutes = 0;
        decimal totalSessionAmt = 0;
        internal string CalculateSessionCost(string fromTime, string toTime, int friendsCount, WizzDataContext db, Int64 tutorId)
        {

            double fromTimes = Convert.ToDouble(fromTime);
            double toTimes = Convert.ToDouble(toTime);
            //double currentTimeStamp = DateTime.UtcNow.Ticks;
            double currentTimeStamp = (double)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            var diffInSeconds = (toTimes - fromTimes);

            double minutes = diffInSeconds / 60;
            int totalMins = Convert.ToInt32(minutes);
            if (totalMins < 30)
                totalMins = 30;
            
            globalMinutes = totalMins;
            var tutorData = db.tblTutorProfiles.FirstOrDefault(x => x.fkUserId == tutorId);
            Decimal totalCost = 0;
            int totalFees = Convert.ToInt32(tutorData.feesPerHour) / 60;
            totalCost = (totalMins * totalFees);
            if (friendsCount == 0)
            {


            }
            else
            {
                totalSessionAmt = totalCost;
                totalCost = totalCost / friendsCount;

            }
          //  totalCost += totalCost + 25 / 100;
            return totalCost.ToString();
        }
        internal SessionEndResponseModel EndSession(CreateSesion objReq)
        {
            SessionEndResponseModel resp = new SessionEndResponseModel();
            using (var db = new WizzDataContext())
            {

                var sessionData = db.tblSessions.Where(x => x.pkSessionId == Convert.ToInt64(objReq.sessionId)).FirstOrDefault();
                if (sessionData == null)
                    return resp;
                sessionData.sessionEndtime = objReq.endTime;

                sessionData.isComplete = true;
                sessionData.sessionDate = DateTime.UtcNow.ToString();
                sessionData.updatedDate = DateTime.UtcNow;
                int friendCount = db.tblInviteFriends.Where(x => x.uniqueRequestId == objReq.uniqueRequestId && x.isDelete == false).ToList().Count;

                if (friendCount == 0)
                { }
                else
                    friendCount = friendCount - 1;
                objReq.earnedAmt = CalculateSessionCost(sessionData.sessionStartTime, sessionData.sessionEndtime, friendCount, db, Convert.ToInt32(sessionData.tutorId));
                sessionData.totalMinutes = globalMinutes;
                resp.scheduleCounts = Convert.ToString(db.tblSessions.Where(x => x.uniqueRequestId == objReq.uniqueRequestId).ToList().Count);
                if (friendCount == 0)
                    resp.sessionCost = objReq.earnedAmt;
                else
                    resp.sessionCost = Convert.ToString(Convert.ToDecimal(objReq.earnedAmt) * friendCount);

                sessionData.sessionAmount =totalSessionAmt==0?resp.sessionCost:totalSessionAmt.ToString();
                resp.uniqueRequestId = sessionData.uniqueRequestId;
                resp.sessionId = sessionData.pkSessionId.ToString();
                var studentData = db.tblStudentRequests.Where(x => x.uniqueStudentRequestId == objReq.uniqueRequestId).FirstOrDefault();
                resp.latitude = Convert.ToString(studentData.lat);
                resp.longitude = Convert.ToString(studentData.longi);
                resp.location = Convert.ToString(studentData.location);
                db.SubmitChanges();
                SendSessionEndPushes(objReq.earnedAmt, objReq.uniqueRequestId, db, Convert.ToInt64(sessionData.studentId), sessionData.pkSessionId, Convert.ToInt64(sessionData.tutorId));
            }
            return resp;
        }
        private void SendSessionStartPushes(WizzDataContext db, long studentId, string uniqueRequestId, long sessionId)
        {
            var sessionStudents = db.tblInviteFriends.Where(x => x.uniqueRequestId == uniqueRequestId).ToList();
            if (sessionStudents.Count == 0)
            {
                var studentData = db.tblUsers.Where(x => x.pkUserId == studentId).FirstOrDefault();//missed is tutor intentionally
                if (!string.IsNullOrEmpty(studentData.deviceToken))
                {
                    PushData push = new PushData()
                    {
                        registration_ids = studentData.deviceToken,
                        uniqueRequestid = uniqueRequestId.ToString(),
                        sessionId = sessionId.ToString(),
                        message = PushMessages.sessionStart,
                        type = Convert.ToInt16(PushType.sessionStart).ToString(),
                        badge = Convert.ToString(studentData.badge),
                    };

                    if (studentData.deviceType == Convert.ToInt16(DeviceType.ios))//ios
                    {
                        SendPush objIOS = new SendPush();
                        objIOS.ConnectToAPNS(push);
                    }
                    else if (studentData.deviceType == Convert.ToInt16(DeviceType.android))//android
                    {
                        Android objAndroid = new Android();
                        objAndroid.send(push);
                    }
                }
            }
            else
            {
                #region multiple pushes
                foreach (var n in sessionStudents)
                {
                    if (n.fkFriendId == 0)
                        continue;
                    var studentData = db.tblUsers.Where(x => x.pkUserId == n.fkFriendId).FirstOrDefault();//missed is tutor intentionally
                    if (!string.IsNullOrEmpty(studentData.deviceToken))
                    {
                        PushData push = new PushData()
                        {
                            registration_ids = studentData.deviceToken,
                            uniqueRequestid = uniqueRequestId.ToString(),
                            sessionId = sessionId.ToString(),
                            message = PushMessages.sessionStart,
                            type = Convert.ToInt16(PushType.sessionStart).ToString(),
                            badge = Convert.ToString(studentData.badge),
                        };

                        if (studentData.deviceType == Convert.ToInt16(DeviceType.ios))//ios
                        {
                            SendPush objIOS = new SendPush();
                            objIOS.ConnectToAPNS(push);
                        }
                        else if (studentData.deviceType == Convert.ToInt16(DeviceType.android))//android
                        {
                            Android objAndroid = new Android();
                            objAndroid.send(push);
                        }
                    }



                }
                #endregion

            }

        }

        private void SendSessionEndPushes(string sessionAmt, string uniqueRequestid, WizzDataContext db, long studentId, long sessionId, long tutorId)
        {


            var sessionStudents = db.tblInviteFriends.Where(x => x.uniqueRequestId == uniqueRequestid).ToList();
            if (sessionStudents != null)
            {
                if (sessionStudents.Count == 0)
                {
                    var studentData = db.tblUsers.Where(x => x.pkUserId == studentId).FirstOrDefault();//missed is tutor intentionally
                    if (!string.IsNullOrEmpty(studentData.deviceToken))
                    {

                        PushData push = new PushData()
                        {
                            registration_ids = studentData.deviceToken,
                            uniqueRequestid = uniqueRequestid.ToString(),
                            sessionId = sessionId.ToString(),
                            tutorId = tutorId.ToString(),
                            amount = Convert.ToString(sessionAmt),
                            //   userId = Convert.ToString(sessionData.fkTutorId),
                            message = PushMessages.sessionEnd,
                            type = Convert.ToInt16(PushType.sessionEnd).ToString(),
                            badge = Convert.ToString(studentData.badge)
                        };
                        // push.data = CommonMethod.ObjectToJson(objPush);
                        if (studentData.deviceType == Convert.ToInt16(DeviceType.ios))//ios
                        {
                            SendPush objIOS = new SendPush();
                            objIOS.ConnectToAPNS(push);
                        }
                        else if (studentData.deviceType == Convert.ToInt16(DeviceType.android))//android
                        {
                            Android objAndroid = new Android();
                            objAndroid.send(push);
                        }

                    }
                }
                else
                {
                    #region multiple pushes
                    foreach (var n in sessionStudents)
                    {
                        if (n.fkFriendId == 0)
                            continue;

                        var studentData = db.tblUsers.Where(x => x.pkUserId == n.fkFriendId).FirstOrDefault();//missed is tutor intentionally
                        if (!string.IsNullOrEmpty(studentData.deviceToken))
                        {

                            PushData push = new PushData()
                            {
                                registration_ids = studentData.deviceToken,
                                uniqueRequestid = uniqueRequestid.ToString(),
                                amount = Convert.ToString(sessionAmt),
                                //   userId = Convert.ToString(sessionData.fkTutorId),
                                message = PushMessages.sessionEnd,
                                type = Convert.ToInt16(PushType.sessionEnd).ToString(),
                                badge = Convert.ToString(studentData.badge),
                                sessionId = sessionId.ToString()
                            };
                            // push.data = CommonMethod.ObjectToJson(objPush);
                            if (studentData.deviceType == Convert.ToInt16(DeviceType.ios))//ios
                            {
                                SendPush objIOS = new SendPush();
                                objIOS.ConnectToAPNS(push);
                            }
                            else if (studentData.deviceType == Convert.ToInt16(DeviceType.android))//android
                            {
                                Android objAndroid = new Android();
                                objAndroid.send(push);
                            }

                        }
                    }
                    #endregion
                }

            }




        }
        internal bool CancelSession(CreateSesion objReq)
        {
            using (var db = new WizzDataContext())
            {
                tblRequest sessionData;
                sessionData = db.tblRequests.Where(x => x.uniqueRequestId == objReq.sessionId).FirstOrDefault();
                if (sessionData == null)
                    return false;
                sessionData.isCancelled = true;
                sessionData.updatedDate = DateTime.UtcNow;
                db.SubmitChanges();
            }
            return true;
        }



        internal List<SessionInfo> GetSessionInfo(SessionEntity objReq)
        {
            List<SessionInfo> listResp = new List<SessionInfo>();
            SessionInfo objResp = new SessionInfo();
            using (var db = new WizzDataContext())
            {
                var sessionData = db.tblSessions.Where(x => x.pkSessionId == Convert.ToInt64(objReq.sessionId) && x.isDelete == false).FirstOrDefault();
                objResp.sessionStartTime = sessionData.sessionStartTime;
                objResp.sessionEndTime = sessionData.sessionEndtime;
                objResp.sessionTime = "";
                var data = db.tblUsers.Where(x => x.pkUserId == sessionData.tutorId).FirstOrDefault();
                objResp.tutorName = data.userName;
                objResp.profilePic = data.profilePic;
                listResp.Add(objResp);
                return listResp;
            }

        }

        internal bool SessionReviewByTutor(SessionReviewModel objReq)
        {

            using (var db = new WizzDataContext())
            {
                var sessionData = db.tblSessions.Where(x => x.pkSessionId == Convert.ToInt64(objReq.sessionId)).FirstOrDefault();

                if (Convert.ToBoolean(objReq.isreschedule))//if reschedule then homework and session notes  is neccessary and than reschedule request
                {
                    sessionData.homeWork = objReq.homeWork;
                    sessionData.sessionNotes = objReq.sessionNotes;
                    sessionData.isComplete = true;
                    sessionData.sessionAmount = objReq.sessionCost;
                    tblSession sessionObj = new tblSession();
                    sessionObj.createdDate = DateTime.UtcNow;
                    sessionObj.fromTime = timeConversionMethod(objReq.fromTime);
                    sessionObj.toTime = timeConversionMethod(objReq.toTime);
                    sessionObj.uniqueRequestId = objReq.uniqueRequestId;
                    sessionObj.dayType = Convert.ToInt16(objReq.dayType);
                    sessionObj.isDelete = false;
                    sessionObj.homeWork = objReq.homeWork;
                    sessionObj.sessionNotes = objReq.sessionNotes;
                    sessionObj.updatedDate = DateTime.UtcNow;
                    sessionObj.studentId = Convert.ToInt64(objReq.studentId);
                    sessionObj.tutorId = Convert.ToInt64(objReq.userId);
                    //var studentData = db.tblStudentRequests.Where(x => x.uniqueStudentRequestId == objReq.uniqueRequestId).FirstOrDefault();
                    //tutorRating.avgRatingTutor = ((tutorRating.avgRatingTutor) + (rating.punctual + rating.knowledgable + rating.helpful) / 3) / 2;
                    //studentData.lat = Convert.ToDouble(studentData);
                    //studentData.longi = Convert.ToDouble(objReq.longitude);
                    //studentData.location = Convert.ToString(objReq.location);

                    db.tblSessions.InsertOnSubmit(sessionObj);

                }
                else
                {
                    sessionData.homeWork = objReq.homeWork;
                    sessionData.sessionNotes = objReq.sessionNotes;
                    sessionData.isComplete = true;
                    sessionData.sessionAmount = objReq.sessionCost;

                }

                var objUsers = db.tblInviteFriends.Where(x => x.uniqueRequestId == sessionData.uniqueRequestId).ToList();
                if (objUsers.Count > 0)
                {
                    foreach (var n in objUsers)
                    {
                        if (n.fkFriendId == Convert.ToInt32(objReq.userId) || n.isDelete == true)
                        {

                            continue;
                        }
                        var objUser = db.tblUsers.Where(x => x.pkUserId == Convert.ToInt64(n.fkFriendId)).FirstOrDefault();
                        objUser.avgRatingStudent = (Convert.ToDecimal(objUser.avgRatingStudent) + Convert.ToDecimal(objReq.rating)) / 2;
                        db.SubmitChanges();
                    }

                }
                else
                {

                    var objUser = db.tblUsers.Where(x => x.pkUserId == Convert.ToInt64(sessionData.studentId)).FirstOrDefault();
                    objUser.avgRatingStudent = (Convert.ToDecimal(objUser.avgRatingStudent) + Convert.ToDecimal(objReq.rating)) / 2;
                    //db.SubmitChanges();
                }
                db.SubmitChanges();

            }
            //throw new NotImplementedException();
            return true;
        }
        internal string timeConversionMethod(string time)
        {


            int originalTime = Convert.ToInt32(time);


            if (originalTime % 2 == 0)
                originalTime = ((originalTime / 2) * 100);
            else
                originalTime = ((originalTime / 2) * 100) + 50;


            return originalTime.ToString();
        }
        internal string ReverseTimeConversionMethod(string time)
        {
            return Convert.ToString(Convert.ToInt32(time) * 2 / 100);
        }
        #endregion

        internal bool SessionReviewByStudent(StudentReviewModel objReq)
        {
            using (var db = new WizzDataContext())
            {
                tblTutorRating rating = new tblTutorRating();
                var sessionData = db.tblSessions.FirstOrDefault(x => x.pkSessionId == Convert.ToInt64(objReq.sessionId));
                rating = db.tblTutorRatings.FirstOrDefault(x => x.sessionId == objReq.sessionId && x.fkStudentId == Convert.ToInt32(objReq.userId));
                if (rating == null)
                {

                    rating = new tblTutorRating();
                    rating.createdDate = DateTime.UtcNow;
                    rating.sessionId = objReq.sessionId;
                    rating.userName = objReq.userName;
                    rating.fkStudentId = Convert.ToInt32(objReq.userId);
                    rating.fkTutorId = sessionData.tutorId;
                    rating.updatedDate = DateTime.UtcNow;
                    rating.punctual = 0;
                    rating.knowledgable = 0;
                    rating.helpful = 0;
                    rating.review = objReq.reviewText;
                    db.tblTutorRatings.InsertOnSubmit(rating);
                }
                else
                {

                    rating.userName = objReq.userName;
                    rating.updatedDate = DateTime.UtcNow;
                    rating.review = objReq.reviewText;
                }
                db.SubmitChanges();
                return true;


            }
        }


        internal bool SessionRatingByStudent(StudentRatingwModel objReq)
        {
            using (var db = new WizzDataContext())
            {
                tblTutorRating rating = new tblTutorRating();
                var sessionData = db.tblSessions.FirstOrDefault(x => x.pkSessionId == Convert.ToInt64(objReq.sessionId));
                rating = db.tblTutorRatings.FirstOrDefault(x => x.sessionId == objReq.sessionId && x.fkStudentId == Convert.ToInt32(objReq.userId));
                var tutorRating = db.tblUsers.Where(x => x.pkUserId == Convert.ToInt64(sessionData.tutorId)).FirstOrDefault();
                if (rating == null)
                {

                    rating = new tblTutorRating();
                    rating.createdDate = DateTime.UtcNow;
                    rating.sessionId = objReq.sessionId;
                    rating.userName = objReq.userName;
                    rating.fkStudentId = Convert.ToInt32(objReq.userId);
                    rating.fkTutorId = sessionData.tutorId;
                    rating.updatedDate = DateTime.UtcNow;
                    rating.punctual = Convert.ToInt16(objReq.punctual);
                    rating.knowledgable = Convert.ToInt16(objReq.knowledge);
                    rating.helpful = Convert.ToInt16(objReq.helpFul);
                    rating.review = "";
                    tutorRating.avgRatingTutor = ((tutorRating.avgRatingTutor) + (rating.punctual + rating.knowledgable + rating.helpful) / 3) / 2;
                    db.tblTutorRatings.InsertOnSubmit(rating);
                }
                else
                {

                    rating.userName = objReq.userName;
                    rating.updatedDate = DateTime.UtcNow;
                    rating.punctual = Convert.ToInt16(objReq.punctual);
                    rating.knowledgable = Convert.ToInt16(objReq.knowledge);
                    rating.helpful = Convert.ToInt16(objReq.helpFul);
                    tutorRating.avgRatingTutor = ((tutorRating.avgRatingTutor) + (rating.punctual + rating.knowledgable + rating.helpful) / 3) / 2;
                    
                }
                db.SubmitChanges();
                return true;


            }
        }

        internal List<HistoryDetailsModel> GetHistoryDetailsForStudent(SessionHistoryRequestModel objReq)
        {
            using (var db = new WizzDataContext())
            {

                List<HistoryDetailsModel> historyList = new List<HistoryDetailsModel>();
                int userId = Convert.ToInt32(objReq.userId);
                var sessionList = db.usp_GetHistoryDetailsForStudent(objReq.uniqueRequestId).ToList();
                int i = 1;
                foreach (var c in sessionList)
                {
                    HistoryDetailsModel model = new HistoryDetailsModel();
                    model.sno = Convert.ToString(i++);
                    model.sessionHours = c.forMinutes.ToString();
                    model.sessionCost = c.sessionCost.ToString();
                    model.homework = c.homeWork.ToString();
                    model.latitude = Convert.ToString(c.lat);
                    model.longitude = Convert.ToString(c.longi);
                    model.sessionDate = c.updatedDate.Value.ToString("dd mm yyyy");
                    model.sessionNotes = c.sessionNotes.ToString();
                    model.homework = c.homeWork.ToString();
                    model.paymentType = c.paymentType.ToString();
                    //model.u = c.uniqueRequestId;
                    //model.profilepic = c.profilePic.ToString();
                    //model.tutorRating = Convert.ToString(c.tutorRating);
                    //model.totalHours = c.TotalMinutes.ToString();
                    historyList.Add(model);
                }

                return historyList;
            }
        }

        internal List<FilteredHistoryResponsetoStudent> GetHistoryForStudent(Entity objReq)
        {
            using (var db = new WizzDataContext())
            {

                List<FilteredHistoryResponsetoStudent> studentList = new List<FilteredHistoryResponsetoStudent>();
                List<FilteredHistoryResponsetoStudent> responseList = new List<FilteredHistoryResponsetoStudent>();
                int userId = Convert.ToInt32(objReq.userId);
                var  sessionList = db.usp_GetHistoryForStudent(userId).ToList();

                List<usp_GetHistoryForStudentResult> ListSession = new List<usp_GetHistoryForStudentResult>();
                //   var subList = sessionList.Select(s => s.subjectId).Distinct().ToList();
                if (sessionList == null || sessionList.Count == 0)
                {

                    return studentList;

                }
                foreach (var n in sessionList)
                {

                    var data = sessionList.Where(x => x.uniqueRequestId == n.uniqueRequestId).ToList();
                    if (data != null && data.Count > 1)
                    {
                        if (!ListSession.Any(x => x.uniqueRequestId == n.uniqueRequestId)) {

                            var count = sessionList.Where(x => x.uniqueRequestId == n.uniqueRequestId).Sum(x => x.TotalMinutes);

                            var avgRating = sessionList.Where(x => x.uniqueRequestId == n.uniqueRequestId).Average(x => x.tutorRating);

                            data[0].TotalMinutes = count;
                            data[0].tutorRating = avgRating;


                            ListSession.Add(data[0]);
                        }
                      //  var tutorRating = sessionList.Where(x => x.uniqueRequestId == n.uniqueRequestId);
                        //int totalMins = 0;
                        //decimal rat = 0;
                        //foreach (var val in count) {
                        //    totalMins += Convert.ToInt32(val.TotalMinutes);
                        //    rat += Convert.ToDecimal(val.tutorRating);
                        //}

                       
                        //sessionList.RemoveAll(x => x.uniqueRequestId == n.uniqueRequestId);
                    }
                    else
                    {
                        if (!ListSession.Any(x=>x.uniqueRequestId==n.uniqueRequestId))
                        ListSession.Add(n);   
                    }


                }
                //List<String> uniqueList = new List<string>();
                //foreach (var n in sessionList)
                //{
                    
                //    uniqueList.Add(n.uniqueRequestId);


                //}

                //int i = 0;
                foreach (var c in ListSession)
                {
                    var list = ListSession.Where(s => s.subjectId == c.subjectId).ToList();
                    if (!studentList.Any(x => x.subjectId == c.subjectId.ToString()))
                    {
                        FilteredHistoryResponsetoStudent FilteredObj = new FilteredHistoryResponsetoStudent();
                        FilteredObj.subjectId = c.subjectId.ToString();
                        FilteredObj.subjectType = c.subjectType.ToString();
                        FilteredObj.subjectName = c.subjectName.ToString();
                        List<HistoryResponsetoStudent> model = new List<HistoryResponsetoStudent>();
                        foreach (var sess in ListSession)
                        {

                            //var data = studentList.DetailsList.Where(x => x.uniqueRequestId == sess.uniqueRequestId).FirstOrDefault();
                            //if (data == null)
                            //{
                            model = (from r in ListSession.Where(x => x.subjectId == c.subjectId)
                                     select new HistoryResponsetoStudent
                                     {
                                         // studentList.Where(x=>x.)
                                         tutorId = r.tutorId.ToString(),
                                         tutorName = r.tutorName,
                                         sessionId = r.sessionId.ToString(),
                                         tutorRating = Convert.ToString(r.tutorRating),
                                         totalHours = r.TotalMinutes.ToString(),
                                         profilepic = r.profilePic.ToString(),
                                         searchCode = r.searchCode.ToString(),
                                         uniqueRequestId = r.uniqueRequestId.ToString(),
                                     }).ToList();

                            //   }
                            //else {

                            //    data.totalHours = Convert.ToString(Convert.ToInt32(data.totalHours) + sess.TotalMinutes);
                            //    data.tutorRating = Convert.ToString((Convert.ToInt32(data.tutorRating) + sess.tutorRating)/2);
                            //}

                        }


                        FilteredObj.DetailsList = model;
                        studentList.Add(FilteredObj);
                    }
                }

                //foreach (var n in studentList)
                //{
                //    foreach (var p in n.DetailsList) {
                //        var data = n.DetailsList.Where(x => x.uniqueRequestId == p.uniqueRequestId).FirstOrDefault();
                //    if(data==null){
                //   responseList.Add(n) ;
                    
                //    }else{
                //        data.totalHours = Convert.ToString(Convert.ToInt32(data.totalHours) +Convert.ToInt32(p.totalHours));
                //       // data.tutorRating = Convert.ToString((Convert.ToInt32(data.tutorRating) + Convert.ToInt32(p.tutorRating)) / 2);
                //        n.DetailsList.Add(data);
                //        responseList.Add(n);
                //    }
                    
                   
                    
                    
                //    }
                   

                //}

                return studentList;
            }
        }

        internal List<FilteredHistoryResponsetoTutor> GetHistoryForTutor(Entity objReq)
        {
            using (var db = new WizzDataContext())
            {

                List<FilteredHistoryResponsetoTutor> studentList = new List<FilteredHistoryResponsetoTutor>();
                int userId = Convert.ToInt32(objReq.userId);
                var sessionList = db.usp_GetHistoryForTutor(userId).ToList();
                List<usp_GetHistoryForTutorResult> ListSession = new List<usp_GetHistoryForTutorResult>();
                if (sessionList == null || sessionList.Count == 0)
                {

                    return studentList;

                }
                foreach (var n in sessionList)
                {

                    var data = sessionList.Where(x => x.uniqueRequestId == n.uniqueRequestId).ToList();
                    if (data != null && data.Count > 1)
                    {
                        if (!ListSession.Any(x => x.uniqueRequestId == n.uniqueRequestId))
                        {

                            var count = sessionList.Where(x => x.uniqueRequestId == n.uniqueRequestId).Sum(x => x.TotalMinutes);

                            var avgRating = sessionList.Where(x => x.uniqueRequestId == n.uniqueRequestId).Average(x => x.tutorRating);

                            data[0].TotalMinutes = count;
                            data[0].tutorRating = avgRating;


                            ListSession.Add(data[0]);
                        }
                        //var countS = sessionList.Where(x => x.uniqueRequestId == n.uniqueRequestId).ToList();
                        ////var tutorRating = sessionList.Where(x => x.uniqueRequestId == n.uniqueRequestId).Sum(x => x.tutorRating);
                        //int totalMins = 0;
                        //decimal rat = 0;

                        //foreach (var val in countS)
                        //{
                        //    totalMins += Convert.ToInt32(val.TotalMinutes);
                        //    rat += Convert.ToDecimal(val.tutorRating);
                        //}

                        //data[0].TotalMinutes = totalMins;
                        //data[0].tutorRating = rat / 3;
                     

                        //if (!ListSession.Any(x => x.uniqueRequestId == n.uniqueRequestId))
                        //    ListSession.Add(data[0]);
                        //sessionList.RemoveAll(x => x.uniqueRequestId == n.uniqueRequestId);
                    }
                    else
                    {
                        if (!ListSession.Any(x => x.uniqueRequestId == n.uniqueRequestId))
                            ListSession.Add(n);
                    }


                }

                foreach (var c in ListSession)
                {
                    var list = ListSession.Where(s => s.subjectId == c.subjectId).ToList();
                    if (!studentList.Any(x => x.subjectId == c.subjectId.ToString()))
                    {
                        FilteredHistoryResponsetoTutor FilteredObj = new FilteredHistoryResponsetoTutor();
                        FilteredObj.subjectId = c.subjectId.ToString();
                        FilteredObj.subjectType = c.subjectType.ToString();
                        FilteredObj.subjectName = c.subjectName.ToString();
                        List<HistoryResponsetoTutor> model = new List<HistoryResponsetoTutor>();
                        model = (from r in ListSession.Where(x => x.subjectId == c.subjectId)
                                 select new HistoryResponsetoTutor
                                 {
                                     studentId = r.tutorId.ToString(),
                                     studentName = r.tutorName,
                                     sessionId = r.sessionId.ToString(),
                                     tutorRating = Convert.ToString(r.tutorRating),
                                     totalHours = r.TotalMinutes.ToString(),
                                     profilepic = r.profilePic.ToString(),
                                     uniqueRequestId = r.uniqueRequestId.ToString(),
                                 }).ToList();
                        FilteredObj.DetailsList = model;
                        studentList.Add(FilteredObj);


                    }


                    //return studentList;
                }
                return studentList;
            }


        }

        internal List<HistoryDetailsModel> GetHistoryDetailsForTutor(SessionHistoryRequestModel objReq)
        {
            using (var db = new WizzDataContext())
            {

                List<HistoryDetailsModel> historyList = new List<HistoryDetailsModel>();
                int userId = Convert.ToInt32(objReq.userId);
                var sessionList = db.usp_GetHistoryDetailsForTutor(objReq.uniqueRequestId).ToList();
                int i = 1;
                foreach (var c in sessionList)
                {
                    HistoryDetailsModel model = new HistoryDetailsModel();
                    model.sno = Convert.ToString(i++);
                    model.sessionHours = c.forMinutes.ToString();
                    model.sessionCost = c.sessionCost.ToString();
                    model.homework = c.homeWork.ToString();
                    model.latitude = Convert.ToString(c.lat);
                    model.longitude = Convert.ToString(c.longi);
                    model.sessionDate = c.updatedDate.Value.ToString("dd mm yyyy");
                    model.sessionNotes = c.sessionNotes.ToString();
                    model.homework = c.homeWork.ToString();
                    model.paymentType = c.paymentType.ToString();
                    //model.u = c.uniqueRequestId;
                    //model.profilepic = c.profilePic.ToString();
                    //model.tutorRating = Convert.ToString(c.tutorRating);
                    //model.totalHours = c.TotalMinutes.ToString();
                    historyList.Add(model);
                }

                return historyList;
            }
        }
    }
}