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
    public class RecuperacionWidget:BaseWidget
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
                else if (cname == "reference_id")
                {
                    int reference_id;
                    if (int.TryParse(configParam.ConfigurationParamValue, out reference_id))
                        this.executeShow(reference_id);
                    else
                        FormattedContent = "";
                }
            }
        }

        public override string FormattedContent
        { get; set; }

        private void executeList()
        {
            System.IO.StringWriter sw = new System.IO.StringWriter();
            Recuperacion rbll = new Recuperacion();
            List<RecuperacionVO> recuperaciones = rbll.GetList();
            HttpContext.Current.Items.Add("Recuperaciones", recuperaciones);
            HttpContext.Current.Server.Execute("/" + System.Configuration.ConfigurationSettings.AppSettings["RootDir"] + "/view/" + "/recuperacion/FriendlyList.aspx", sw);
            this.FormattedContent = sw.ToString();
            HttpContext.Current.Items.Remove("Recuperaciones");
        }
        private void executeShow(int referenceId)
        {
            System.IO.StringWriter sw = new System.IO.StringWriter();
            int id = referenceId;
            RecuperacionDAL rdal = new RecuperacionDAL();
            RecuperacionVO r = rdal.GetById(id);
            HttpContext.Current.Items.Add("Recuperacion", r);
            HttpContext.Current.Items.Add("src", "directWidget");
            HttpContext.Current.Server.Execute("/" + System.Configuration.ConfigurationSettings.AppSettings["RootDir"] + "/view/" + "/recuperacion/FriendlyShow.aspx", sw);
            this.FormattedContent = sw.ToString();
            HttpContext.Current.Items.Remove("Recuperacion");
            HttpContext.Current.Items.Remove("src");
        }
        private void executeEmpty()
        {
            Recuperacion rbll = new Recuperacion();
            List<RecuperacionVO> recuperaciones = rbll.GetList();
            if (recuperaciones.Count == 1)
                this.executeShow(recuperaciones[0].Id);
            else
                this.executeList();
 
        }

    }
}
