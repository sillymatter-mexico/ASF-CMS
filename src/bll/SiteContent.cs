using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Net.Mail;
using asf.cms.model;
using asf.cms.dal;
using asf.cms.util;
using System.Text.RegularExpressions;
using log4net;

namespace asf.cms.bll
{
    public class SiteContent
    {
        private List<SearchResultVO> SearchResults;
        private static StringBuilder jsonSearchableContent = null;
        public static StringBuilder GetJsonSearchableContent()
        {
            if (jsonSearchableContent == null)
            {
                SiteContent sc = new SiteContent();
                sc.SearchResults = sc.GetSearchResults();
                jsonSearchableContent = sc.SearchResultsToJson();
            }
            return jsonSearchableContent;
        }
        public List<SearchResultVO> GetSearchResults(String q)
        {
            string[] words = q.Split(' ');
            string or = "";
            foreach (string w in words)
                or += " or -*$*- like '%" + w.Trim() + "%'";

            MenuDAL mdal = new MenuDAL();
            List<SearchResultVO> searchResults = new List<SearchResultVO>(mdal.SearchInSite(or));
            RemoveWidgets(searchResults);
            foreach (SearchResultVO sr in searchResults)
            {
                sr.Link = buildLink(sr);
                if (sr.Class == "Publication")
                    sr.Content = GetResume(sr.Content, words);

                foreach (string w in words)
                {
                    sr.Title = markWord(sr.Title, w);
                    sr.Content = markWord(sr.Content, w);
                }

            }
            return searchResults;
        }
        public string GetResume(string str, string[] words)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            str = asf.cms.util.Encoder.RemoveHTML(str);
            string plain = asf.cms.util.Encoder.RemoveSigns(str).ToLower();
            //          int inicial1 = 0;
            int inicial2 = 0;
            //            string resumen="";
            foreach (string w in words)
            {

                inicial2 = plain.IndexOf(asf.cms.util.Encoder.RemoveSigns(w).ToLower());
                if (inicial2 == -1)
                    continue;
                if (inicial2 > 40)
                    inicial2 -= 40;
                int count = str.Length - inicial2;
                if (count > 350)
                    count = 350;

                if (inicial2 < 40)
                    return str.Substring(0, count);
                return "... " + str.Substring(inicial2, count) + "...";

            }
            if (inicial2 == -1 && str.Length > 350)
                return str.Substring(0, 350);
            return str;
        }

        public string GetResumeWithFormat(string str)
        {
            string mode = ConfigurationManager.AppSettings.Get("ShortContentMode").ToUpper();
            int limit = 200;
            int.TryParse(ConfigurationManager.AppSettings.Get("ShortContentLimit"), out limit);

            switch (mode)
            {
                case "MIXED":
                    return GetResumeWithFormatFirstPoint(str, limit);
                case "LIMIT":
                    return GetResumeWithFormatLimit(str, limit);
                case "POINT":
                default:
                    return GetResumeWithFormatFirstPoint(str);

            }
        }
        public string GetResumeWithFormatFirstPoint(string str, int limit = int.MaxValue)
        {
            Regex regex = new Regex("<(.|\n)*?>", RegexOptions.Multiline);
            string clean = regex.Replace(str, String.Empty);

            int firstPointIdx = clean.IndexOf('.') + 1;
            firstPointIdx = firstPointIdx == 0 ? limit : firstPointIdx;

            return GetResumeWithFormatLimit(str, firstPointIdx > limit ? limit : firstPointIdx);
        }
        public string GetResumeWithFormatLimit(string str, int limit)
        {
            if (str == null)
                return "";
            int charCounter = 0;
            int charPointer = 0;
            List<string> openTags = new List<string>();
            Regex tagsRegex = new Regex("<(.|\n)*?>", RegexOptions.Multiline);
            Match currentMatch = tagsRegex.Match(str);
            string resumedContent = "";

            while (charPointer < str.Length && charCounter < limit && currentMatch.Success)
            {
                if (currentMatch.Index > charPointer)
                {
                    int length = currentMatch.Index - charPointer;
                    if (charCounter + length > limit)
                    {
                        string[] pieces = str.Substring(charPointer, length).Split(' ');
                        string tmpString = "";
                        int max = limit - charCounter;
                        bool cont = true;
                        for (int idx=0; idx < pieces.Length && tmpString.Length < max && cont; idx++)
                        {
                            bool isLast = pieces.Length - 1 == idx;

                            if (tmpString.Length + pieces[idx].Length < max)
                            {
                                tmpString += isLast ? pieces[idx] : pieces[idx] + " ";
                            }
                            else
                            {
                                cont = false;
                                if (tmpString.Length < 1)
                                    continue;
                                char last = tmpString[tmpString.Length - 1];
                                if (last == ',' || last == ';' || last == ':')
                                    tmpString = tmpString.Substring(0, tmpString.Length - 1) + "...";
                                else if(last != '.')
                                    tmpString += "...";
                            }
                        }
                        resumedContent += tmpString;
                        charCounter += tmpString.Length; //max;
                        charPointer += length;
                    }
                    else
                    {
                        charCounter += length;
                        resumedContent += str.Substring(charPointer, length);
                        charPointer += length;
                    }
                }
                if (charCounter < limit)
                {
                    int closeIndex = currentMatch.Value.LastIndexOf("/");
                    if (closeIndex == 1) // Closing tag
                        openTags.RemoveAt(openTags.Count - 1);
                    else if (closeIndex < 0 || closeIndex < currentMatch.Value.Length - 2) //Not a closing tag
                        openTags.Add(currentMatch.Value);
                    resumedContent += currentMatch.Value;
                    charPointer += currentMatch.Length;

                    currentMatch = currentMatch.NextMatch();
                }
            }

            for (int i = openTags.Count - 1; i >= 0; i--)
            {
                string tag = openTags[i];
                if(tag.IndexOf(' ') > 0)
                    tag = tag.Split(' ')[0] + ">";
                resumedContent += tag.Insert(1, "/");
            }

            return resumedContent;
        }

        private string markWord(string str, string word)
        {
            if (string.IsNullOrEmpty(str))
                return "";

            int inicial = str.ToLower().IndexOf(word.ToLower());
            int final = inicial + word.Length;
            if (inicial != -1)
            {
                str=str.Insert(final, "</b>");
                str=str.Insert(inicial, "<b>");
            }
            return str;
        }
        private List<SearchResultVO> GetSearchResults()
        {
            if (SearchResults == null)
            {
                MenuDAL mdal = new MenuDAL();
                SearchResults = new List<SearchResultVO>(mdal.GetSiteContent());
                RemoveWidgets(SearchResults);
            }
            return SearchResults;
        }
        private void RemoveWidgets(List<SearchResultVO> results)
        {
            Regex regex = new Regex(@"#\$\{[^\}]+\}"); string content = "";
            foreach (SearchResultVO svo in results)
            {
                if (!String.IsNullOrEmpty(svo.Content) && svo.Content.Contains("{"))
                {
                    content = regex.Replace(svo.Content, "");
                    svo.Content = content;
                }
                if (!String.IsNullOrEmpty(svo.Title))
                    svo.Title = regex.Replace(svo.Title, "");
            }
 
        }
        private StringBuilder SearchResultsToJson()
        {
            int i = 0;
            StringBuilder jsonResult = new StringBuilder("[");
            foreach (SearchResultVO sr in SearchResults)
            {
                if (i > 0)
                    jsonResult.Append(",");
                try
                {
                    String link = buildLink(sr);
                    sr.Content = String.IsNullOrEmpty(sr.Content) ? "" : sr.Content;
                    jsonResult.Append("{" + getJSONNameValue("id", sr.Id) + ",");
                    jsonResult.Append(getJSONNameValue("link", link) + ",");
                    jsonResult.Append(getJSONNameValue("content", asf.cms.util.Encoder.RemoveHTML(sr.Content)) + ",");
                    if (sr.Class == "Publication") sr.Class = "Publicación";
                    if (sr.Class == "File") sr.Class = "Archivo";
                    if (sr.Class == "Section") sr.Class = "Sección";

                    jsonResult.Append(getJSONNameValue("clase", sr.Class) + "}");
                    i++;
                }
                catch (Exception ex) {
                
                }
            }
            
            jsonResult.Append("]");
            return jsonResult;
        }
         public string getJSONNameValue(string name, object value)
         {
             return "\"" + name + "\":\"" + value + "\"";
         }
         private string buildLink(SearchResultVO sr)
         {
             string link="";
             if (sr.Subclass == "permalink")
                 link = "../" + sr.Class + "/" + sr.Link;
             else if (sr.Subclass == "link")
                 link = sr.Link;
             else if (sr.Subclass == "path")
                 link = "../uploads" + sr.Link;
             return link;

         }

        public bool SendContactFormEmail(string content, string toPosfix = "")
        {
            try
            {
                if (toPosfix == null)
                    toPosfix = "";
                string server = ConfigurationManager.AppSettings.Get("SMTPServer");
                int port = int.Parse(ConfigurationManager.AppSettings.Get("SMTPPort"));
                bool sslEnable = bool.Parse(ConfigurationManager.AppSettings.Get("SMTPSSL"));
                string fromAddr = ConfigurationManager.AppSettings.Get("SMTPMailFrom");
                string toAddr = ConfigurationManager.AppSettings.Get("SMTPMailTo" + toPosfix);
                if(toAddr == null)
                    toAddr = ConfigurationManager.AppSettings.Get("SMTPMailTo");

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(server);

                mail.From = new MailAddress(fromAddr);
                mail.To.Add(toAddr);
                mail.Subject = "Correo de Formulario Sitio Web";
                mail.Body = content;

                SmtpServer.Port = port;
                SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings.Get("SMTPUsername"), ConfigurationManager.AppSettings.Get("SMTPPassword"));
                SmtpServer.EnableSsl = sslEnable;

                SmtpServer.Send(mail);
                return true;
            }
            catch(Exception e)
            {
                ILog log = LogManager.GetLogger("SiteContent");
                log.Error("Error al enviar correo automatizado.", e);
                return false;
            }
        }

    }
}
