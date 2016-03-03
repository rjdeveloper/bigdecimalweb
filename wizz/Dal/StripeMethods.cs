using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using wizz.Class;
using wizz.Models;

namespace wizz.Dal
{
    /// <summary>
    /// Stripe methods
    /// <developer>Rishabh </developer>
    /// </summary>
    public class StripeMethods
    {
        internal bool SaveStripeDetails(StripeModel objReq)
        {
            using (var db = new WizzDataContext())
            {


                tblUser userData = new tblUser();
                userData = db.tblUsers.Where(x => x.pkUserId == Convert.ToInt64(objReq.userId)).FirstOrDefault();
                userData.stripeId = objReq.stripeAcKey;
                db.SubmitChanges();
                return true;
            }


        }

        internal StripeModel GetStripeInfo(Entity objReq)
        {

            using (var db = new WizzDataContext())
            {

                var userData = db.tblUsers.Where(x => x.pkUserId == Convert.ToInt64(objReq.userId)).FirstOrDefault();
                StripeModel objStripe = new StripeModel();
                objStripe.stripeAcKey = userData.stripeId;
                objStripe.stripeLiveKey = Constants.stripeLiveKey;
                objStripe.stripeSecretKey = Constants.stripeSecretKey;
                return objStripe;
            }

        }

        internal int SavePaymentDetails(StripePaymentModel objReq)
        {

            using (var db = new WizzDataContext())
            {

                try
                {
                  //  return 1;
                    if (db.tblPaymentDetails.Any(x => x.sessionId == Convert.ToInt64(objReq.sessionId) && x.isSuccess == true && x.studentId == Convert.ToInt64(objReq.userId)))
                    {

                        return 2;
                    }
                    else
                    {
                        var sessionData = db.tblSessions.Where(x => x.pkSessionId == Convert.ToInt64(objReq.sessionId)).FirstOrDefault();
                  tblPaymentDetail paymentData = new tblPaymentDetail();
                        paymentData.studentId = Convert.ToInt64(objReq.userId);
                        paymentData.sessionId = Convert.ToInt64(objReq.sessionId);
                        paymentData.tutorId = Convert.ToInt64(sessionData.tutorId);
                        paymentData.transactionId = objReq.transactionId;
                        paymentData.isSuccess = true;
                        paymentData.createdDate = DateTime.UtcNow;
                        paymentData.paymentType = Convert.ToInt16(objReq.paymentType);
                        db.tblPaymentDetails.InsertOnSubmit(paymentData);
                     //   var requestData = db.tblSessions.Where(x => x.hasPaid == false && x.uniqueRequestId == objReq.uniqueRequestId).FirstOrDefault();
                        //if (requestData == null)
                        //    return 2;
                        //requestData.hasPaid = true;

                        db.SubmitChanges();
                        return 1;
                    }
                }
                catch (TimeoutException e)
                {
                    return 3;

                }
                catch (Exception e)
                {
                    return 0;

                }

            }

        }
    }
}