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
    public class GrupoFuncionalVO
    {
        public virtual int Id
        {
            set;
            get;
        }
        public virtual String Code
        {
            set;
            get;
        }
        public virtual String Name
        {
            set;
            get;
        }
    }
}
