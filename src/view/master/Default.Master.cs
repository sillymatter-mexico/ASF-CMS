using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using asf.cms.model;
using asf.cms.bll;

namespace asf.cms.view.master
{
    public partial class Default : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        { 

        }
        protected Object GetRequestVar(String var)
        {
            if (HttpContext.Current.Items[var] == null)
                return null;
            return HttpContext.Current.Items[var];
        }
    }
}
