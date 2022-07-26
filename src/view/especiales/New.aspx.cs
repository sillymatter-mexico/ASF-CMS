using System;
using asf.cms.controller;
using asf.cms.model;

namespace asf.cms.view.especiales
{
    public partial class New : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["permalink"] = ((EspecialesVO)Context.Items["publication"]).Permalink;
        }
    }
}
