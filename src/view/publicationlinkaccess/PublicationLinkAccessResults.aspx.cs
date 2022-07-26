using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace asf.cms.view.publicationlinkaccess
{
    public partial class PublicationLinkAccessResults : asf.cms.controller.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] == null)
                throw new UnauthorizedAccessException("Usuario no autenticado");
        }
    }
}