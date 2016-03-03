using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace wizz.Class
{
    public static class StringExtension
    {
        public static string RemoveSpecialCharacters(this string value)
        {
            return Regex.Replace(value, "[^0-9a-zA-Z]+", "");
        }
        public static Int64 SetValue(this string value)
        {            
            if(!string.IsNullOrEmpty(value))
            {
                return Convert.ToInt64(value);
            }
            
            return 0;

        }
    }
}