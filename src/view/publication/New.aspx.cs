using System;
using asf.cms.controller;
using asf.cms.model;

namespace asf.cms.view.publication
{
    public partial class New : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["permalink"] = ((PublicationVO)Context.Items["publication"]).Permalink;
        }
    }
}
