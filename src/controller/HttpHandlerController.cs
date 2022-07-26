using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.view.widgets.ui;
using asf.cms.bll;
using asf.cms.model;
using System.Text.RegularExpressions;
using System.Text;

namespace asf.cms.controller
{
    public class HttpHandlerController : Controller
    {
        private Regex MainContentExpression = new Regex("\\#\\$[\\w\\W]*?\\$\\#");
        private string UploadsURL = "http://{0}:{1}/{2}/uploads{3}";

        public HttpHandlerController(HttpContext context)
            : base(context)
        {
        }
        public void VideoRSSHandler()
        {
            string publicationId = Request["publicationId"];
            string host = Request.Url.Host;
            int port = Request.Url.Port;
            string appPath = Request.ApplicationPath;
            string rsslink = "HttpHandler/VideoRSSHandler?pubId=" + publicationId;
            string rssdesc = "Videos of the publication " + publicationId;
            Publication pb = new Publication();
            List<FileVO> videoFilesToDisplay = pb.GetVideosByPublicationId(publicationId);
            string rssResponse = buildRSSResponse(videoFilesToDisplay, rsslink, rssdesc, host, port, appPath);
            Response.ContentType = "text/xml";
            ShowPage("util.aspx");
            Response.Output.Write(rssResponse);
        }

        public void AnnouncementRSSHandler()
        {
            string host = Request.Url.Host;
            string appPath = Request.ApplicationPath;
            int port = Request.Url.Port;
            string rsslink = "HttpHandler/AnnouncementRSSHandler";
            string rssdesc = "Announcemnt videos";
            Publication pb = new Publication();
            List<FileVO> videoFilesToDisplay = pb.GetAllVideos();
            string rssResponse = buildRSSResponse(videoFilesToDisplay, rsslink, rssdesc, host, port, appPath);
            Response.ContentType = "text/xml";
            ShowPage("util.aspx");
            Response.Output.Write(rssResponse);
        }

        private string buildRSSResponse(List<FileVO> filesToDisplay, string rssLink, string rssdesc, string host, int port, string applicationPath)
        { 
            string templateContent = WidgetTemplates.VideoRSSTemplate;

            templateContent = templateContent.Replace("@RSSTitle", "Publication videos");
            templateContent = templateContent.Replace("@RSSLink", rssLink);
            templateContent = templateContent.Replace("@RSSDescription", rssdesc);
            templateContent = templateContent.Replace("@RSSAtomLink", rssLink);
            templateContent = templateContent.Replace("@RSSLanguage", "es");
            templateContent = templateContent.Replace("@RSSLastBuildDate", DateTime.Now.ToShortTimeString());
            templateContent = templateContent.Replace("@RSSPubDate", DateTime.Now.ToShortTimeString());

            Match mainMatch = MainContentExpression.Match(templateContent);
            if (mainMatch == null || mainMatch.Groups.Count == 0)
                throw new ApplicationException("The template content::" + templateContent + " is malformed");

            string videoRow = string.Empty;
            StringBuilder rssSB = new StringBuilder();

            foreach (System.Text.RegularExpressions.Group g in mainMatch.Groups)
            {
                if (g.Success && g.Value.Contains("#$"))
                {
                    videoRow = g.Value;
                    foreach (FileVO fileVO in filesToDisplay)
                    {
                        string thisVideoRow = videoRow;
                        thisVideoRow = thisVideoRow.Replace("#$", string.Empty);
                        thisVideoRow = thisVideoRow.Replace("$#", string.Empty);
                        thisVideoRow = thisVideoRow.Replace("@RSSItemTitle", fileVO.Title);
                        thisVideoRow = thisVideoRow.Replace("@RSSItemLink", string.Format(UploadsURL, host, port, applicationPath, fileVO.Path));
                        thisVideoRow = thisVideoRow.Replace("@RSSItemDescription", fileVO.Name);
                        thisVideoRow = thisVideoRow.Replace("@RSSItemPubDate", DateTime.Now.ToShortTimeString());
                        thisVideoRow = thisVideoRow.Replace("@RSSItemMediaContent", string.Format(UploadsURL, host, port, applicationPath, fileVO.Path));
                        thisVideoRow = thisVideoRow.Replace("@RSSItemMimeType", fileVO.Mime);
                        thisVideoRow = thisVideoRow.Replace("@RSSItemThumbnail", string.Format(UploadsURL, host, port, applicationPath, fileVO.Path.Replace(fileVO.Name, "thumbTest.jpg")));

                        rssSB.Append(thisVideoRow);
                    }
                    break;
                }
            }

            //Finally, transform the whole content
            templateContent = templateContent.Replace(videoRow, rssSB.ToString());

            return templateContent;
        
        }

    }
}