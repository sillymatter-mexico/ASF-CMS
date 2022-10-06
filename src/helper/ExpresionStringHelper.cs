using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.MobileControls.Adapters;

namespace asf.cms.helper
{
    public sealed class ExpresionStringHelper
    {
        public static string replaceEscapeCharacter(string str)
        {
            // Create  a string array and add the special characters you want to remove
            string[] chars = new string[] { "\\", "\\Z", ";", ":", "[", "]", "\0", "\b",
               "\t", "\n", "\r", "\\_", "\\0", "\"", "\'", "\\%" };
            //Iterate the number of times based on the String array length.
            for (int i = 0; i < chars.Length; i++)
            {
                if (str.Contains(chars[i]))
                {
                    str = str.Replace(chars[i], "");
                }
            }
            return str;
        }
    }
}