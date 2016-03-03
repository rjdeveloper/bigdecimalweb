using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wizz.Models
{
    public class BankDetail
    {
       public string  userId {get;set;}
       public string bankName { get; set; }
        public string  accountHolderName {get;set;}
        public string  accountNumber {get;set;}
        public string  ifscCode {get;set;}
        public string  micrCode {get;set;}
        public string  branchAddress {get;set;}
     
    }
}