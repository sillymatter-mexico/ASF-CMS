using System;
using System.Collections.Generic;
using System.Web;

namespace asf.cms.model
{
    public class SectionLabelVO
    {
        public virtual SectionLabelIdVO SectionLabelId
        {
            get;
            set;
        }
        /*public virtual int SectionId
        {
            set;
            get;
        }
        public virtual int LanguageId
        {
            set;
            get;
        }*/
        public virtual String Content
        {
            set;
            get;
        }
    }
}
