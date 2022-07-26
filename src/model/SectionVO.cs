using System;
using System.Collections.Generic;
using System.Web;


namespace asf.cms.model
{
    public class SectionVO
    {
        public virtual int Id
        {
            set;
            get;
        }
        public virtual int? ParentSectionId
        {
            set;
            get;
        }
     
        public virtual DateTime Created
        {
            set;
            get;
        }
        public virtual DateTime Updated
        {
            set;
            get;
        }

        public virtual bool IsMain
        {
            set;
            get;
        }
        
        public virtual string Permalink
        {
            set;
            get;
        }
        public virtual string RedirectTo
        {
            set;
            get;
        }
        public virtual string RedirectOptions
        {
            set;
            get;
        }
        public virtual int Visitas
        {
            set;
            get;
        }
        public virtual bool NewsInclude
        {
            set;
            get;
        }
        public virtual int Position
        {
            set;
            get;
        }
        public virtual bool Active
        {
            set;
            get;
        }
        public virtual bool SitemapExclude
        {
            set;
            get;
        }
        public virtual string Type
        {
            set;
            get;
        }
        public virtual string CssClass
        {
            set;
            get;
        }
    }
}
