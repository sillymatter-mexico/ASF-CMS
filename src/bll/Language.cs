using System;
using System.Collections.Generic;
using System.Web;
using System.Threading;
using System.Globalization;

namespace asf.cms.bll
{
    public class Language
    {
        public static int GetCurrentLanguageId()
        {
//            int currlang = (Thread.CurrentThread.CurrentCulture.ToString() == "en-US") ? 2 : 1;
            int currlang = 1;

            if (HttpContext.Current.Session["currentLanguage"] == null)
                HttpContext.Current.Session["currentLanguage"] = currlang;
//            if (Thread.CurrentThread.CurrentCulture.ToString() == "en-US")
            currlang = (int)HttpContext.Current.Session["currentLanguage"];

            return currlang;
        }
        public static string GetLanguageName(int languageId)
        {
            // if (Thread.CurrentThread.CurrentCulture.ToString() == "en-US")
            int currentlang = (int)HttpContext.Current.Session["currentLanguage"];
            if (currentlang == 2)
                return languageId == 1 ? "Spanish" : "English";
            return languageId == 1 ? "Español" : "Ingles";
        }
        public static string GetLanguageCode(int lang)
        {
            if (lang == 2)
                return "en-US";
            return "es-MX";
        }
        public static CultureInfo  GetOpositeCulture()
        {
            if(GetCurrentLanguageId()==1)
              return  CultureInfo.CreateSpecificCulture("en-US");
            return CultureInfo.CreateSpecificCulture("es-MX");
        }
        public static void ChangeLanguage()
        {
            int currentlang = (int)HttpContext.Current.Session["currentLanguage"];
            if (currentlang == 1)
                HttpContext.Current.Session["currentLanguage"] = 2;
            else
                HttpContext.Current.Session["currentLanguage"] = 1;
        }
    }
}
