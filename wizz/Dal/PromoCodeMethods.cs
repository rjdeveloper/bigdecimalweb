using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using wizz.Models;

namespace wizz.Dal
{
    public class PromoCodeMethods
    {
        internal int ApplyPromoCode(PromoCodeReq objReq)
        {
            try
            {
                using (var db = new WizzDataContext())
                {
                    tblPromoCode promoCodeData = new tblPromoCode();
                    long fkUserId = Convert.ToInt64(objReq.userId);
                    promoCodeData = db.tblPromoCodes.Where(p => p.promoCode == objReq.promocode && p.isDelete == false).FirstOrDefault();
                    if (promoCodeData == null)
                        return 0;
                    else if (DateTime.UtcNow.IsBetween(promoCodeData.validFrom.Value, promoCodeData.validTo.Value) || promoCodeData.isActive == false)
                        return 3;
                    else if (db.tblPromoHistories.Any(d => d.fkUserId == fkUserId && d.fkPromoCodeId == promoCodeData.pkPromoCodeId))
                        return 2;
                    else if (db.tblPromoHistories.Count(x => x.fkPromoCodeId == promoCodeData.pkPromoCodeId) > promoCodeData.usageCount)
                        return 3;
                    else
                    {
                        tblPromoHistory objHistory = new tblPromoHistory();
                        objHistory.fkPromoCodeId = promoCodeData.pkPromoCodeId;
                        objHistory.createdDate = DateTime.UtcNow;
                        objHistory.fkUserId = Convert.ToInt64(objReq.userId);
                        db.tblPromoHistories.InsertOnSubmit(objHistory);
                        //  tblUser userDetails = new tblUser();
                        var userDetails = db.tblUsers.Where(u => u.pkUserId == fkUserId).FirstOrDefault();
                        userDetails.credits += Convert.ToInt32(promoCodeData.discountPercentage);
                        db.SubmitChanges();
                        return 1;
                    }
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
    public static class CompareClass
    {
        public static bool IsBetween(this DateTime val, DateTime low, DateTime high)
        {
            return val > low && val < high;
        }
    }
}