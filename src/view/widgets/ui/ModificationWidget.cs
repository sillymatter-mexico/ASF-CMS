using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.widgets.ui;
using asf.cms.widgets;
using asf.cms.bll;

namespace asf.cms.view.widgets.ui
{
    public class ModificationWidget : BaseWidget
    {

        public override string FormattedContent { get; set; }
        public int CurrentPublicationId { get; set; }

        protected override void configure(List<WidgetConfigurationParamVO> configs)
        {
            //Read the file template
            string templateContent = WidgetTemplates.ModificationWidgetTemplate;
            DateTime modificationDate = DateTime.Now;

            //Fill it accordingly, with the provided parameters
            if (configs.Count == 0)
            {
                //The same as "this"
                modificationDate = Publication.GetById(CurrentPublicationId).Updated;
            }
            else
            {

                foreach (WidgetConfigurationParamVO wcp in configs)
                {
                    string cname = wcp.ConfigurationParamName;

                    if (cname == "this")
                    {
                        modificationDate = Publication.GetById(CurrentPublicationId).Updated;
                    }
                    else if (cname == "global")
                    {
                        modificationDate = Publication.GetLastUpdateDate();
                    }
                    else if (cname == "section_id")
                    {
                        int sectionId;
                        int.TryParse(wcp.ConfigurationParamValue, out sectionId);
                        modificationDate = Publication.GetLastUpdateDateBySectionId(sectionId);
                    }
                    else if (cname == "publication_id")
                    {
                        int specificPublicationId;
                        int.TryParse(wcp.ConfigurationParamValue, out specificPublicationId);
                        modificationDate = Publication.GetById(specificPublicationId).Updated;
                    }
                }
            }

            FormattedContent = templateContent.Replace("@ModificationDate", modificationDate.ToString());

        }
    }
}