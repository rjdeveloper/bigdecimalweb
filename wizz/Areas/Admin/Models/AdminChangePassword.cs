﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wizz.Areas.Admin.Models
{
    public class AdminChangePassword
    {
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }
}