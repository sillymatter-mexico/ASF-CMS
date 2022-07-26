using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.view.widgets.ui;

namespace asf.cms.widgets.ui
{
    public abstract class BaseWidget
    {

        protected abstract void configure(List<WidgetConfigurationParamVO> configs);

        public abstract string FormattedContent { get; set; }

        /// <summary>
        /// Receives the raw content and returns it with its embedded widgets
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetWidgetFormattedContent(string content, int globalPublicationId, int currentPublication, HttpRequest request)
        {
            List<WidgetConfigurationVO> commands = WidgetParser.GetConfigurationFromContent(content);

            foreach (WidgetConfigurationVO command in commands)
            {
                WidgetNames cmdName = command.CommandStrongName;

                switch (cmdName)
                { 
                    case WidgetNames.news:
                        NewsWidget nw = new NewsWidget();
                        nw.Host = request.Url.Host;
                        nw.AppPath = request.ApplicationPath;
                        nw.Port = request.Url.Port;
                        nw.configure(command.ConfigurationParams);
                        content = content.Replace(command.FullCommand, nw.FormattedContent);
                        break;
                    case WidgetNames.counter:
                        CounterWidget cw = new CounterWidget();
                        cw.GlobalPublicationId = globalPublicationId;
                        cw.CurrentPublicationId = currentPublication;
                        cw.configure(command.ConfigurationParams);
                        content = content.Replace(command.FullCommand, cw.FormattedContent);
                        break;
                    case WidgetNames.map:
                        MapWidget mw = new MapWidget();
                        mw.configure(command.ConfigurationParams);
                        content = content.Replace(command.FullCommand, mw.FormattedContent);
                        break;
                    case WidgetNames.video_player:
                        VideoWidget vid_w = new VideoWidget();
                        vid_w.CurrentPublicationId = currentPublication;
                        vid_w.configure(command.ConfigurationParams);
                        content = content.Replace(command.FullCommand, vid_w.FormattedContent);
                        break;
                    case WidgetNames.announce:
                        AnnounceWidget ann_w = new AnnounceWidget();
                        ann_w.CurrentPublicationId = currentPublication;
                        ann_w.configure(command.ConfigurationParams);
                        content = content.Replace(command.FullCommand, ann_w.FormattedContent);
                        break;
                    case WidgetNames.modification:
                        ModificationWidget mod_w = new ModificationWidget();
                        mod_w.CurrentPublicationId = currentPublication;
                        mod_w.configure(command.ConfigurationParams);
                        content = content.Replace(command.FullCommand, mod_w.FormattedContent);
                        break;
                    case WidgetNames.recuperaciones:
                        RecuperacionWidget rw = new RecuperacionWidget();
                        rw.configure(command.ConfigurationParams);
                        content = content.Replace(command.FullCommand, rw.FormattedContent);
                        break;
                    case WidgetNames.audit:
                        AuditWidget aw = new AuditWidget();
                        aw.configure(command.ConfigurationParams);
                        content = content.Replace(command.FullCommand, aw.FormattedContent);
                        break;
                }

            }

            return content;

        }

    }
}