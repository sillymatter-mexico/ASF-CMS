using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.widgets.ui;
using asf.cms.widgets;
using System.Text.RegularExpressions;
using System.Web.Hosting;

namespace asf.cms.view.widgets.ui
{
    public class VideoWidget : BaseWidget
    {
        private const string rssURIHandler = "../HttpHandler/VideoRSSHandler?publicationId={0}";
        private Regex MainContentExpression = new Regex("\\#\\$[\\w\\W]*?\\$\\#");

        public override string FormattedContent { get; set; }

        public VideoWidget()
        { }

        protected override void configure(List<WidgetConfigurationParamVO> configs)
        {
            string videoWidgetTemplate = WidgetTemplates.VideoWidgetTemplate;
            videoWidgetTemplate = videoWidgetTemplate.Replace("@VideoPlayerSWF", "../view/flash/ASFFullPlayer.swf");

            //Flash vars are as follows: type=@type&playlist=@playlist&rssUrl=@rssUrl&rssURI=@rssURIHandler
            videoWidgetTemplate = videoWidgetTemplate.Replace("@type", "fullPlayer");
            videoWidgetTemplate = videoWidgetTemplate.Replace("@playlist", "true");
            videoWidgetTemplate = videoWidgetTemplate.Replace("@rssURIHandler", string.Format(rssURIHandler, this.CurrentPublicationId));

            FormattedContent = videoWidgetTemplate;
        }


        public int CurrentPublicationId { get; set; }
    }
}