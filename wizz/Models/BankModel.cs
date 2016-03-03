using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wizz.Models
{
    public class BankModel
    {
    }
    public class ReqTutorAccountDetails
    {
        public string userId { get; set; }
        public string accountId { get; set; }
    }
    public class RespTutorAccountDetails : ReqTutorAccountDetails
    {
        public RespTutorAccountDetails() { }
        public RespTutorAccountDetails(string _userId, string _accountId, string _bankName, string _accountHolderName,
            string _accountNumber, string _ifscCode, string _micrCode, string _branchAddress)
        {
            userId = _userId;
            accountId = _accountId;
            bankName = _bankName;
            accountHolderName = _accountHolderName;
            accountNumber = _accountNumber;
            ifscCode = _ifscCode;
            micrCode = _micrCode;
            branchAddress = _branchAddress;
        }// Declaring Parameterized constructor with Parameters


        public string bankName { get; set; }
        public string accountHolderName { get; set; }
        public string accountNumber { get; set; }
        public string ifscCode { get; set; }
        public string micrCode { get; set; }
        public string branchAddress { get; set; }
    }
}