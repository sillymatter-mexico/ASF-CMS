using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.widgets.ui;
using asf.cms.widgets;
using asf.cms.bll;

namespace asf.cms.view.widgets.ui
{
    public class CounterWidget : BaseWidget
    {
        public override string FormattedContent { get; set; }
        public int CurrentPublicationId { get; set; }

        protected override void configure(List<WidgetConfigurationParamVO> configs)
        {
            //Read the file template
            string templateContent = WidgetTemplates.CounterWidgetTemplate;
            int visitas = 0;

            //Fill it accordingly, with the provided parameters
            if (configs.Count == 0)
            {
                //The same as "this"
                visitas = Publication.GetById(CurrentPublicationId).Visitas;
            }
            else
            {

                foreach (WidgetConfigurationParamVO wcp in configs)
                {
                    string cname = wcp.ConfigurationParamName;

                    if (cname == "this")
                    {
                        visitas = Publication.GetById(CurrentPublicationId).Visitas;
                    }
                    else if (cname == "global")
                    {
                        visitas = Publication.GetById(GlobalPublicationId).Visitas;
                    }
                    else if (cname == "publication_id")
                    {
                        int specificPublicationId;
                        int.TryParse(wcp.ConfigurationParamValue, out specificPublicationId);
                        visitas = Publication.GetById(specificPublicationId).Visitas;
                    }
                }
            }

            FormattedContent = templateContent.Replace("@ActualCount", visitas.ToString());

        }

        public int GlobalPublicationId { get; set; }
    }
}