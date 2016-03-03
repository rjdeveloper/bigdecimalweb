using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace wizz.Models
{
    public class StripeModel
    {
        [Required]
        public string userId { get; set; }
        [Required] 
        public string stripeAcKey { get; set; }

        public string stripeLiveKey { get; set; }
        public string stripeSecretKey { get; set; }
    }
    public class StripePaymentModel {
        public StripePaymentModel() {
            this.transactionId = "";
        }
        [Required]
        public string userId { get; set; }
        [Required]
        public string sessionId { get; set; }
       
        public string transactionId { get; set; }
        [Required]
        public string paymentType { get; set; }
    
    
    }
}