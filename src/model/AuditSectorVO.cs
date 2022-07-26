using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace asf.cms.model
{
    public class AuditSectorVO
    {
        public virtual int Id
        {
            get;
            set;
        }
        public virtual string Name
        {
            get;
            set;
        }
        public virtual string AsfKey
        {
            get;
            set;
        }
    }
}
