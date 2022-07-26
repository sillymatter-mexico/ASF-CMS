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
using asf.cms.dal;


namespace asf.cms.view.widgets.ui
{
    public class AuditWidget:BaseWidget
    {
        protected override void configure(List<WidgetConfigurationParamVO> configs)
        {
            if (configs.Count == 0)
            {    
                this.executeEmpty();
                return;
            }
            foreach (WidgetConfigurationParamVO configParam in configs)
            {
                string cname = configParam.ConfigurationParamName;
                if (cname == "list")
                    this.executeList();
                else if (cname == "search")
                {
                    this.executeSearch();
                }
                else if (cname == "index_permalink")
                {
                    string permalink=configParam.ConfigurationParamValue;
                        this.executeIndex(permalink);
                }
            }
        }

        public override string FormattedContent
        { get; set; }

        private void executeList()
        {
            System.IO.StringWriter sw = new System.IO.StringWriter();
            AuditReport ar = new AuditReport();
            List<AuditReportItem> list= ar.GetDisplayableList();
            HttpContext.Current.Items.Add("list", list);
            HttpContext.Current.Server.Execute("/" + System.Configuration.ConfigurationSettings.AppSettings["RootDir"] + "/view/audit/ListReports.aspx", sw);
            this.FormattedContent = sw.ToString();
            HttpContext.Current.Items.Remove("list");
        }
        private void executeSearch()
        {
            System.IO.StringWriter sw = new System.IO.StringWriter();
            HttpContext.Current.Server.Execute("/" + System.Configuration.ConfigurationSettings.AppSettings["RootDir"] + "/view/audit/Search.aspx", sw);
            this.FormattedContent = sw.ToString();
        }
        private void executeIndex(string referenceId)
        {
            this.executeList();

        }
        private void executeEmpty()
        {
                this.executeList();
 
        }

    }
}
