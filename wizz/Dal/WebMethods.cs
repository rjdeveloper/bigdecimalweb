using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using wizz.Class;
using wizz.Models;

namespace wizz.Dal
{
    public class WebMethods
    {
        #region Rishabh

        internal int ChangePassword(ChangePasswordModel objReq)
        {
            using (var db = new WizzDataContext())
            {

                tblUser tUser = new tblUser();
                int fkuserid = Convert.ToInt32(objReq.userId);
                tUser = db.tblUsers.Where(x => x.pkUserId == fkuserid).FirstOrDefault();
                if (tUser == null)
                    return 0;
                else if (tUser.isActive == false || tUser.isDelete == true)
                    return 2;
                else if (tUser.password != objReq.oldPassword)
                    return 3;
                else
                {
                    tUser.password = objReq.Password.Trim();
                    db.SubmitChanges();
                    return 1;
                }
            }

        }

        internal RespIsTeacherRequest isTeacher(IsTeacherRequest objReq)
        {
            var isTutor = Convert.ToBoolean(objReq.isTutor);
            RespIsTeacherRequest response = new RespIsTeacherRequest();
            response.isAlreadySignup = "False"; 
            using (var db = new WizzDataContext())
            {
               
                tblUser tUser = new tblUser();
                int fkuserid = Convert.ToInt32(objReq.userId);
                tUser = db.tblUsers.Where(x => x.pkUserId == fkuserid).FirstOrDefault();
                if (tUser == null)
                    response.isTutor="False";
                   // return false;
                else
                {
                    var tutor = db.tblTutorProfiles.Where(t => t.fkUserId == fkuserid).FirstOrDefault();
                    //check already signup or not
                   
                    if (tutor==null)
                    {
                        response.isAlreadySignup = "False"; 
                    }
                    else
                    {
                        response.isAlreadySignup = "True";
                    }
                    


                    if (tutor == null && objReq.isTutor=="True")
                    {
                        response.isAlreadySignup = "False";
                    }
                    else
                    {
                        response.isTutor = isTutor.ToString();
                        tUser.isTeacher = isTutor;
                        db.SubmitChanges();
                    }

                  //  return true;
                }
            }
          return  response;
        }

        internal bool UserLogout(ReqLogOut objReq)
        {
            using (var db = new WizzDataContext())
            {

                tblUser tUser = new tblUser();
                int fkuserid = Convert.ToInt32(objReq.userId);
                tUser = db.tblUsers.Where(x => x.pkUserId == fkuserid).FirstOrDefault();
                if (tUser == null)
                    return false;
                else
                {
                    tUser.deviceToken = null;
                    db.SubmitChanges();
                    return true;
                }
            }
        }

        //internal bool AcceptStudent(StudentTeacherMap objReq)
        //{
        //   // var isTutor = Convert.ToBoolean(objReq.userId);
        //    using (var db = new WizzDataContext())
        //    {
        //        if (objReq.isAccept.ToLower() == "true")
        //        {
        //            tblRequest reqStudent = new tblRequest();
        //            var studentList = db.tblRequestStudents.Where(x => x.isAccepted != true && x.isDelete == false && x.uniqueRequestId == objReq.uniqueRequestId && x.fkUserId == Convert.ToInt32(objReq.tutorId)).ToList();

        //            studentList.ForEach(a => { a.isAccepted = true; a.updatedDate = DateTime.UtcNow; });
        //            db.SubmitChanges();//update student request
                
        //            var totalList = db.tblRequests.Where(x => x.isAccepted != true &&  x.uniqueRequestId == objReq.uniqueRequestId && x.fkStudentId==Convert.ToInt32(objReq.studentId)).ToList();
        //            var deleteList = totalList.Where(x => x.fkTutorId != Convert.ToInt32(objReq.tutorId)).ToList();
                   

        //            //delete all tutors who recived the notiifcation except the tutor who is accepeting the notification
        //            reqStudent = totalList.Where(x => x.fkTutorId == Convert.ToInt32(objReq.tutorId)).FirstOrDefault();
        //            db.tblRequests.DeleteAllOnSubmit(deleteList);
        //            db.SubmitChanges();

        //            //accept the request
        //            if (reqStudent == null)//if request is not exist
        //                return false;
        //            reqStudent.isAccepted = true;
        //            reqStudent.updatedDate = DateTime.UtcNow;
        //            var userData = db.tblUsers.Where(x => x.pkUserId == Convert.ToInt32(objReq.studentId)&& x.isTeacher==false).FirstOrDefault();
        //            var teacherData = db.tblUsers.Where(x => x.pkUserId == Convert.ToInt32(objReq.tutorId)).FirstOrDefault();
        //            if (userData!=null &&!string.IsNullOrEmpty(userData.deviceToken))
        //            {
                        
        //             //   objPush.type = Convert.ToInt16(PushType.acceptRequest).ToString();

        //                PushData push = new PushData()
        //                {
        //                    registration_ids = userData.deviceToken,
        //                    //push.data = CommonMethod.ObjectToJson(objPush);
        //                    message = PushMessages.FormatMessage(PushMessages.acceptRequest, teacherData.userName),
        //                    type = Convert.ToInt16(PushType.acceptRequest).ToString(),
        //                    badge = Convert.ToString(userData.badge)
        //                };
        //                if (userData.deviceType == Convert.ToInt16(DeviceType.ios))//ios
        //                {
        //                    SendPush objIOS = new SendPush();
        //                    objIOS.ConnectToAPNS(push);
        //                }
        //                else if (userData.deviceType == Convert.ToInt16(DeviceType.android))//android
        //                {
        //                    Android objAndroid = new Android();
        //                    objAndroid.send(push);

        //                }
        //            }

        //            // update tblrequest by isAccept=1 and delete rest of request

        //        }
        //        else
        //        {

        //            var objRequestData = db.tblRequests.Where(x => x.isAccepted == false && x.fkTutorId ==Convert.ToInt32(objReq.tutorId) && x.uniqueRequestId==objReq.uniqueRequestId).FirstOrDefault();
        //            if (objRequestData != null)
        //            {
        //                db.tblRequests.DeleteOnSubmit(objRequestData);
        //                db.SubmitChanges();
        //            }
        //            else {

        //                return false;
        //            }
                
        //        }
              
        //        db.SubmitChanges();
        //        return true;
                
        //    }
        //}

        #endregion
        internal bool SendPushForIos(IosPushModel push)
        {
            
            tblUser receiverData = new tblUser();
            using (var db = new WizzDataContext())
            {
                receiverData = db.tblUsers.Where(c => c.pkUserId == Convert.ToInt32(push.receiverId)).FirstOrDefault();
                var sendor = db.tblUsers.Where(c => c.pkUserId == Convert.ToInt32(push.senderId)).FirstOrDefault();
                // objPush.deviceToken = receiverData.deviceToken;

                PushData objPush = new PushData()
                {
                    message = sendor.userName + ": " + push.Message,
                    type = Convert.ToInt16(PushType.chatPush).ToString(),
                    messageType=push.Type,
                    userId=push.senderId,
                    subject=push.subjectName,
                    registration_ids = receiverData.deviceToken,
                    badge = Convert.ToString(receiverData.badge),
                    uniqueRequestid=push.requestId
                };



                // push.type = PushType.newRequest.ToString();

                // push.data = CommonMethod.ObjectToJson(objPush);
                //  push.Type =push.Type;
                //     push.messageId = push.messageId;
                //   push.Message = push.Message;
                //    push.senderName=push

                if (receiverData.deviceType == 1)//ios
                {
                    SendPush objIOS = new SendPush();
                    objIOS.ConnectToAPNS(objPush);
                }
                else if (receiverData.deviceType == 2)//android
                {
                    PushForIos objAndroid = new PushForIos();
                    objAndroid.SendPushForIos(push);

                }
            }
            return true;
            ///throw new NotImplementedException();
        }
    }
}