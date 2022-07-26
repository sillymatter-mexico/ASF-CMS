using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.widgets.ui;
using asf.cms.bll;
using asf.cms.widgets;

namespace asf.cms.view.widgets.ui
{
    public class MapWidget : BaseWidget
    {
        protected override void configure(List<cms.widgets.WidgetConfigurationParamVO> configs)
        {
            string templateContent = WidgetTemplates.MapWidgetTemplate;

            SectionTree st = new SectionTree();

            string mapHTML = string.Empty;
            if (configs.Count == 0)
            {
                //Only the sections
                mapHTML = st.getTree().ToHtmlFullyExpanded(0, true);
            }
            else 
            {
                foreach (WidgetConfigurationParamVO cparam in configs)
                {
                    if (cparam.ConfigurationParamName == "all")
                    {
                        //The publications as leafs too
                        SectionTreeNode root = st.getTree();
                        mapHTML = st.getTree().ToHtmlFullyExpandedWithPosts(0, true);
                        break;
                    }
                }
            }
            

            FormattedContent = templateContent.Replace("@MapStructure", mapHTML);


        }

        public override string FormattedContent { get; set; }
    }
}