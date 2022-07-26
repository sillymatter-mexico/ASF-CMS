using System;

namespace asf.cms.view
{
    public partial class Login : asf.cms.controller.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
/*            if (GetRequestVar("User") != null)
            {
                if (Session["User"] == null)
                    Session.Add("User", GetRequestVar("User"));
                Session["User"] = GetRequestVar("User");
                Response.Redirect("~/SectionAdm/List");
            }
            if (GetRequestVar("isLogout") != null)
            {
                if (Session["User"] != null)
                Session["User"] = null;
            }
                */
        }
    }
}
