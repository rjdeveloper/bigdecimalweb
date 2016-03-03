using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wizz.Areas.Admin.Models
{
    public class SettingsModel
    {
        public int perHourFees { get; set; }
        public int commission { get; set; }
        public int perStudentCharge { get; set; }
    }
}