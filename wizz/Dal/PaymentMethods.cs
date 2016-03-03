using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using wizz.Models;

namespace wizz.Dal
{
    /// <summary>
    /// Method tos ave payment info
    /// </summary>
    public class PaymentMethods
    {
        internal bool SaveCardInfo(PaymentInfo objReq)
        {

            using (var db = new WizzDataContext())
            {
                bool update = true;
                tblCardDetail objCard = new tblCardDetail();
                long userpkId=Convert.ToInt64(objReq.userId);
                objCard = db.tblCardDetails.Where(c => c.fkUserId == userpkId).FirstOrDefault();
                if (objCard == null) {
                    update = false;
                    objCard = new tblCardDetail();
                }
                //objCard.paymentType = Convert.ToInt16(objReq.paymentType);
                objCard.cardType = objReq.cardType;
                objCard.createdDate = DateTime.UtcNow;
                objCard.digitCvv = objReq.digitCvv.Trim();
                objCard.cardHolderName = objReq.cardHolderName == null ? "" : objReq.cardHolderName;
                objCard.cardNumber = objReq.cardNumber;
                objCard.bankName = objReq.bankName == null ? "" : objReq.bankName;
                objCard.fkUserId = userpkId;
                objCard.lastFour = Convert.ToInt32(objReq.lastFour);
                objCard.isActive = true;
                objCard.isDelete = false;
                objCard.validMonth = objReq.validMonth.Trim();
                objCard.validYear = objReq.validYear.Trim();
                try
                {
                    if (!update)
                        db.tblCardDetails.InsertOnSubmit(objCard);
                    db.SubmitChanges();
                    return true;
                }
                catch(Exception ex)
                {
                    return false;

                }
            }

           
        }

       
    }
}