using System;
using System.Collections.Generic;
using System.Web;

namespace asf.cms.model
{
    public class SearchResultVO
    {
        public virtual int Id
        {
            get;
            set;
        }
        public virtual string Content
        {
            get;
            set;
        }
        public virtual string Title
        {
            get;
            set;
        }
        public virtual string Link
        {
            get;
            set;
        }
        public virtual string Class
        {
            get;
            set;
        }
        public virtual string Subclass
        {
            get;
            set;
        }
        public virtual double Score
        {
            get;
            set;
        }
    }
}
