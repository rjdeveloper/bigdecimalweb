using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace wizz.Models
{
    public class PromoCodeReq
    {
        /// <summary>
        /// Promo code should be string and of minimum length 4 and of max length 12
        /// </summary>
        [Required(ErrorMessage = "Promo code is required")]
        [StringLength(12, ErrorMessage = "Invalid Promo code", MinimumLength =4)]
        public string promocode { get; set; }
        /// <summary>
        /// Userid is required
        /// </summary>
        [Required(ErrorMessage = "UserId is required")]
        [StringLength(12, ErrorMessage = "Invalid userId", MinimumLength = 1)]
        public string userId { get; set; }

    }
}