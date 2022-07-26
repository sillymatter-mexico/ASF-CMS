using asf.cms.bll;
using asf.cms.model;
using System;
using System.Collections.Generic;
using System.Web;

namespace asf.cms.controller
{
    public class PublicationLinkAccessAdmController : Controller
    {
        public PublicationLinkAccessAdmController(HttpContext Context) : base(Context)
        {
            isAdmin = 1;
        }

        public void List()
        {
            IList<PublicationLinkAccessResultVO> linkAccessResults = PublicationLinkAccess.GetPublicationLinkAccessResults();

            Items.Add("userType", GetCurrentUserType());
            Items.Add("selectedTab", "PublicationLinkAccessResults");
            Items.Add("publicationLinkAccessResults", linkAccessResults);
            ShowPage("publicationlinkaccess/PublicationLinkAccessResults.aspx");
        }
    }
}