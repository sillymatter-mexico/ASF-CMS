using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using asf.cms.bll;

namespace asf.cms.util
{
    public class Encoder
    {
        public static String ToMD5(string str)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] data=encoding.GetBytes(str);
            
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            data = x.ComputeHash(data);
            string ret = "";
            for (int i = 0; i < data.Length; i++)
                ret += data[i].ToString("x2").ToLower();
            return ret;
        }
        public static string RemoveSigns(string str)
        {
            str = str.Trim();
            string consignos = "áàäéèëíìïóòöúùuñÁÀÄÉÈËÍÌÏÓÒÖÚÙÜÑçÇ";
            string sinsignos = "aaaeeeiiiooouuunAAAEEEIIIOOOUUUNcC";
            
            for (int v = 0; v < sinsignos.Length; v++)
            {
                string i = consignos.Substring(v, 1);
                string j = sinsignos.Substring(v, 1);
                str = str.Replace(i, j);
            }
            Regex regex = new Regex("[^A-Za-z0-9_]", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
            str = regex.Replace(str, ""); 
            return str; 
        }
        public static string RemoveHTML(string text)
        { 
            string s= Regex.Replace(text, @"<(.|\n)*?>", string.Empty);
            s = Regex.Replace(s, @"(\s|\n|\t)+", " "); 
            s = Regex.Replace(s, "(\")+", "'"); 
            return s;
        }
        public static string getMime(string extension)
        {
            Dictionary<string, string> mimes = new Dictionary<string, string>();
            mimes.Add(".bmp", "image/bmp");
            mimes.Add(".doc", "application/msword");
            mimes.Add(".flv", "video/x-flv");
            mimes.Add(".gif", "image/gif");
            mimes.Add(".jpeg", "image/jpeg");
            mimes.Add(".jpg", "image/jpeg");
            mimes.Add(".pdf", "application/pdf");
            mimes.Add(".png", "image/png");
            mimes.Add(".pps", "application/mspowerpoint");
            mimes.Add(".ppt", "application/mspowerpoint");
            mimes.Add(".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow");
            mimes.Add(".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
            mimes.Add(".tiff", "image/tiff");
            mimes.Add(".txt", "text/plain");
            mimes.Add(".xls", "application/excel");
            mimes.Add(".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            mimes.Add(".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            mimes.Add(".f4v", "video/x-flv");
            mimes.Add(".zip", "application/x-zip-compressed");
            mimes.Add(".swf", "application/x-shockwave-flash");

            if (mimes.ContainsKey(extension.ToLower()))
                return mimes[extension];
            return "text/plain";
        }
        public static string DictionaryToJson(Dictionary<string, string> dictionary)
        {
            string json = "[";
            int i = 0;
            foreach (KeyValuePair<string, string> kvp in dictionary)
            {
                if (i > 0)
                    json += ",";
                // json += "{\"key\":\"" + kvp.Key + "\",\"value\":\"" + kvp.Value + "\"}";
                json += "{" + getJSONNameValue("key", kvp.Key) + "," + getJSONNameValue("value", kvp.Value) + "}";
                i++;
            }
            return json + "]";
        }

        public static string SortDictionaryToJson(Dictionary<string, string> dictionary)
        {
            List<SortableKeyValue> lista= SortableKeyValue.GetListFromDictionary(dictionary);
            lista.Sort();
            string json = "[";
            int i = 0;
            foreach (SortableKeyValue kvp in lista)
            {
                if (i > 0)
                    json += ",";
                // json += "{\"key\":\"" + kvp.Key + "\",\"value\":\"" + kvp.Value + "\"}";
                json += "{" + getJSONNameValue("key", kvp.Key) + "," + getJSONNameValue("value", kvp.Value) + "}";
                i++;
            }
            return json + "]";
        }
        public static string getJSONNameValue(String name, object value)
        {
            if (value == null)
                return "\"" + name + "\":\"\"";
            return "\"" + name + "\":\"" + value.ToString().Replace("\"","\\\"") + "\"";
        }
        public static string ListToJson(List<IJsonable> list)
        {
            StringBuilder sb = new StringBuilder("[");
            int i = 0;
            foreach (IJsonable obj in list)
            {
                if (i > 0)
                    sb.Append(",");
                // json += "{\"key\":\"" + kvp.Key + "\",\"value\":\"" + kvp.Value + "\"}";
                sb.Append(obj.ToJson());
                i++;
            }
            sb.Append("]");
            return sb.ToString();
        }
        public static int romanToInt(String roman)
        {
            switch (roman.ToUpper())
            {
                case "I":
                    return 1;
                case "II":
                    return 2;
                case "III":
                    return 3;
                case "IV":
                    return 4;
                case "V":
                    return 5;
                case "VI":
                    return 6;
                case "VII":
                    return 7;
                case "VIII":
                    return 8;
                case "IX":
                    return 9;
                case "X":
                    return 10;
                case "XI":
                    return 11;
                case "XII":
                    return 12;
                case "XIII":
                    return 13;
                case "XIV":
                    return 14;
                case "XV":
                    return 15;
                case "XVI":
                    return 16;
                case "XVII":
                    return 17;
                case "XVIII":
                    return 18;
                case "XIX":
                    return 19;
                case "XX":
                    return 20;
                case "XXI":
                    return 21;
                case "XXII":
                    return 22;
                case "XXIII":
                    return 23;
                case "XXIV":
                    return 24;
                case "XXV":
                    return 25;
                case "XXVI":
                    return 26;
                case "XXVII":
                    return 27;
                case "XXVIII":
                    return 28;
                case "XXIX":
                    return 29;
                case "XXX":
                    return 30;
                case "L":
                    return 50;
                default:
                    int salida;
                    if (int.TryParse(roman, out salida))
                        return salida;
                    else
                        return 100;
            }
        }

        public static Encoding GetFileEncoding(String path)
        {
            if (!File.Exists(path))
                return null;
            Encoding encoding;
            using (var reader = new StreamReader(path)) 
            {
                encoding = reader.CurrentEncoding;
            }
            return encoding;
        }

        public static string ForceEncodingLatin1(string value)
        {
            Encoding iso = Encoding.GetEncoding("iso-8859-1");
            Encoding utf8 = Encoding.UTF8;
            byte[] utfBytes = utf8.GetBytes(value);
            byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
            return iso.GetString(isoBytes);
        }

        public static bool IsEncodingLatin1(string value)
        {
            Regex uni = new Regex(@"[^\u0000-\u00FF]+", RegexOptions.Multiline);
            MatchCollection matches = uni.Matches(value);

            return matches.Count == 0;
        }
    }
}

