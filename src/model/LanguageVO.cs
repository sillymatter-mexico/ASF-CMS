using System;
using System.Collections.Generic;
using System.Web;

namespace asf.cms.model
{
    public class LanguageVO
    {
        public virtual int Id
        {
            set;
            get;
        }
        public virtual String Iso639_1
        {
            set;
            get;
        }
        public virtual String Iso639_3
        {
            set;
            get;
        }
     
    }
}
