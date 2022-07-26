using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using asf.cms.view.widgets.ui;

namespace asf.cms.view.master
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] == null)
                //Response.Redirect("~/Login/Default",true); //Ciclo infinito;
                throw new  UnauthorizedAccessException("Usuario no autenticado");
        }
        protected String GetMessage(String var)
        {
            if (HttpContext.Current.Items[var] == null)
                return "";
            return HttpContext.Current.Items[var].ToString();
        }
        protected Object GetRequestVar(String var)
        {
            if (HttpContext.Current.Items[var] == null)
                return null;
            return HttpContext.Current.Items[var];
        }
    }
}
