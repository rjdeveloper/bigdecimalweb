using wizz.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wizz.Class
{
    public class CurrentUser
    {
        public string PkUserId { get; set; }       
        public string Email { get; set; }
        public string isActive { get; set; }       
        public string usertype { get; set; }
        public string mobileNo { get; set; }
        public string zipCode { get; set; }       
        public CurrentUser()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string json = HttpContext.Current.User.Identity.Name;
                var user = JsonConvert.DeserializeObject<CurrentUserModel>(json);
                if (user != null)
                {
                    this.PkUserId = user.PkUserId;                  
                    this.Email = user.email;
                  
                    this.usertype = user.usertype;
                  
                }
                else
                {
                    this.ReSet();
                }
            }
            else
            {
                this.ReSet();
            }

        }


        public class EntityUserId
        {
            public EntityUserId()
            {
                this.PkUserId = "";
            }
            public string PkUserId { get; set; }
        }

        public class CurrentUserModel : EntityUserId
        {
            public string email { get; set; }
            public string usertype { get; set; }
            
        }


        private void ReSet()
        {           
            this.Email = String.Empty;
            this.isActive = String.Empty;           
            this.usertype = String.Empty;           
            this.mobileNo = String.Empty;
            this.zipCode = String.Empty;
          
        }
    }
}