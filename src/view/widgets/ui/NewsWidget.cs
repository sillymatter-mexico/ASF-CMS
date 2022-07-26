using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.widgets.ui;
using asf.cms.widgets;
using System.IO;
using System.Text.RegularExpressions;
using asf.cms.model;
using asf.cms.bll;
using System.Text;

namespace asf.cms.view.widgets.ui
{
    public class NewsWidget : BaseWidget
    {
        private Regex MainContentExpression = new Regex("\\#\\$[\\w\\W]*?\\$\\#");

        private string PermalinkURL = "http://{0}:{1}{2}/{3}/{4}";

        public string Host { get; set; }

        public string AppPath { get; set; }

        public int Port { get; set; }

        public NewsWidget()
        { }

        public override string FormattedContent { get; set; }

        protected override void configure(List<WidgetConfigurationParamVO> configs)
        {
            //Read the file template
            string templateContent = WidgetTemplates.NewsWidgetTemplate;

            News newsBLL = new News();
            List<NewsVO> newsToDisplay = new List<NewsVO>();
            //Fill it accordingly, with the provided parameters
            if (configs.Count == 0)
            {
                //Default news, use all available
                //Go to the backend for the content
                newsToDisplay = newsBLL.GetAllNewsFromView();
            }
            else
            {
                foreach (WidgetConfigurationParamVO configParam in configs)
                {
                    string cname = configParam.ConfigurationParamName;
                    if (cname == "all")
                    {
                        newsToDisplay.AddRange(newsBLL.GetAllNews());
                    }
                    else if (cname == "section_id") 
                    {
                        int section_id;
                        int.TryParse(configParam.ConfigurationParamValue, out section_id);
                        newsToDisplay.AddRange(newsBLL.GetNewsBySectionId(section_id));
                    }
                    else if (cname == "publication_id")
                    {
                        int pubId;
                        int.TryParse(configParam.ConfigurationParamValue, out pubId);
                        newsToDisplay.AddRange(newsBLL.GetNewsByPublicationId(pubId));
                    }
                }
            }

            Match mainMatch = MainContentExpression.Match(templateContent);
            if(mainMatch == null || mainMatch.Groups.Count==0)
                throw new ApplicationException("The template content::" + templateContent + " is malformed");

            string newsRow = string.Empty;
            StringBuilder newsSB = new StringBuilder();

            foreach (System.Text.RegularExpressions.Group g in mainMatch.Groups)
            {
                if (g.Success && g.Value.Contains("#$"))
                {
                    newsRow = g.Value;
                    foreach (NewsVO newVO in newsToDisplay)
                    {
                        string thisNewsRow = newsRow;
                        thisNewsRow = thisNewsRow.Replace("#$", string.Empty);
                        thisNewsRow = thisNewsRow.Replace("$#", string.Empty);
                        //thisNewsRow = thisNewsRow.Replace("@NewsPermalink", string.Format(PermalinkURL, Host, Port, AppPath, "Publication", newVO.Permalink));
                        thisNewsRow = thisNewsRow.Replace("@NewsPermalink", "../Publication/"+newVO.Permalink);
                        thisNewsRow = thisNewsRow.Replace("@NewsDate", newVO.Updated.ToString("dd/MM/yyyy"));
                        thisNewsRow = thisNewsRow.Replace("@NewsTitle", newVO.Title);

                        if (newVO.Content != null && !string.IsNullOrEmpty(newVO.Content))
                        {
                            thisNewsRow = thisNewsRow.Replace("@NewsContentRow", "<tr><td colspan='2' align='center'>" + newVO.Content + "</td></tr>");
                        }
                        else
                        {
                            thisNewsRow = thisNewsRow.Replace("@NewsContentRow", string.Empty);
                        }

                        newsSB.Append(thisNewsRow);
                    }
                    break;
                }
            }

            //Finally, transform the whole content
            templateContent = templateContent.Replace(newsRow, newsSB.ToString());

            //Set the content
            FormattedContent = templateContent;

        }

    }
}