using System;
using asf.cms.controller;

namespace asf.cms.view.news
{
    public partial class New : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["permalink"] = ((String)Context.Items["permalink"]);
        }
    }
}