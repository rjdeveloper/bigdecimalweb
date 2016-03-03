using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wizz.Models
{
    /// <summary>
    /// All enums used in the code is defined here
    /// </summary>
    public class Enums
    {
         enum CardType {
             MasterCard=1,
             Visa=2,
             IndiaMaestro=3,
             CitiBankMaestro=4,
             ICICIPIn=5
         }
         enum PaymentType
         {
            
             CreditCard=1,
             DebitCard=2,
             NetBanking = 3,
             ThirdPaty=4,
             
         }
    }
  
}