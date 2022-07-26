using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.widgets.ui;
using asf.cms.widgets;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using System.Text;

namespace asf.cms.view.widgets.ui
{
    public class AnnounceWidget : BaseWidget
    {
        private const string rssURIHandler = "../HttpHandler/AnnouncementRSSHandler";

        public override string FormattedContent { get; set; }

        public AnnounceWidget()
        { }

        protected override void configure(List<WidgetConfigurationParamVO> configs)
        {
            string announceWidgetTemplate = WidgetTemplates.AnnounceWidgetTemplate;
            announceWidgetTemplate = announceWidgetTemplate.Replace("@AnnouncePlayerSWF", "../view/flash/ASFMiniPlayer.swf");

            //MandatoryFlashVars

            StringBuilder announceFlashVarsSB = new StringBuilder();

            announceFlashVarsSB.Append(string.Format("type={0}", "miniPlayer"));
            announceFlashVarsSB.Append("&");
            announceFlashVarsSB.Append(string.Format("playlist={0}", "true"));
            announceFlashVarsSB.Append("&");
            announceFlashVarsSB.Append(string.Format("rssUrl={0}", rssURIHandler));

            announceWidgetTemplate = announceWidgetTemplate.Replace("@AnnounceFlashVars", announceFlashVarsSB.ToString());

            FormattedContent = announceWidgetTemplate;
        }


        public int CurrentPublicationId { get; set; }
    }
}