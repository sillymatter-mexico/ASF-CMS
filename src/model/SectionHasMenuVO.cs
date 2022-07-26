using System;
using System.Collections.Generic;
using System.Web;

namespace asf.cms.model
{
    public class SectionHasMenuVO
    {
        public virtual SectionHasMenuIdVO SectionHasMenuId
        {
            get;
            set;
        }
        public virtual int Position
        {
            get;
            set;
        }
        public virtual String CSSClass
        {
            get;
            set;
        }
    }
}
