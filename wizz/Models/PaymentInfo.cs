using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace wizz.Models
{
    /// <summary>
    /// Model for Card details
    /// <usage>Saving user Card info</usage>
    /// </summary>
    public class PaymentInfo
    {
        public PaymentInfo() {

            this.cardHolderName = "";
            this.cardType = "1";
            this.paymentType = "Visa";
            

        }
        ///// <summary>
        ///// Optional field added for future scope
        ///// </summary>
        //public string pkCardId { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        public string userId { get; set; }

        //[Required(ErrorMessage = "PaymentType is required")]
     //  [StringLength(1, ErrorMessage = "paymentType should not be more than 1 character")]
        public string paymentType { get; set; }

        //[Required(ErrorMessage = "Card Name is required")]
        /// <summary>
        /// Card type should be mastercard or visa or accordingly in string format
        /// </summary>
        public string cardType { get; set; }

        [Required(ErrorMessage = "Card Number is required")]

        public string cardNumber { get; set; }

        [Required(ErrorMessage = "Valid Month is required")]
        public string validMonth { get; set; }

        [Required(ErrorMessage = "Valid Year is required")]
        public string validYear { get; set; }
        /// <summary>
        /// Optional field added for future scope
        /// </summary>
        public string accountNo { get; set; }
        
        //[Required(ErrorMessage = "Card Holder Name is required")]
        /// <summary>
        /// Card holder name is not required as of now
        /// </summary>
        public string cardHolderName { get; set; }
        /// <summary>
        /// Cvv is required
        /// </summary>
        [Required(ErrorMessage = "Cvv is required")]
        public string digitCvv { get; set; }
        /// <summary>
        /// Last four digits of the card
        /// </summary>
         [Required(ErrorMessage = "last Four digits of card are required")]
        public string lastFour { get; set; }
         /// <summary>
         /// Optional field added for future scope
         /// </summary>
        public string bankName { get; set; }

    }
}