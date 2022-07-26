using System;
using System.Collections.Generic;
using System.Web;

namespace asf.cms.model
{
    public class NewsVO
    {

        public virtual int Id { get; set; }

        public virtual string Title { get; set; }

        public virtual string Permalink { get; set; }

        public virtual DateTime Updated { get; set; }

        public virtual bool NewsPin { get; set; }

        public virtual int NewsTTL { get; set; }

        public virtual string Content { get; set; }

        public virtual bool IncludeInSection { get { return includeInSection == 1; } set { includeInSection = (value ? 1 : 0); } }
        public virtual int includeInSection { get; set; }
//        public virtual bool IncludeInSection { get; set; }

        public virtual bool IncludeInPublication { get { return includeInPublication == 1; } set { includeInPublication =  (value ? 1 : 0); } }
        public virtual int includeInPublication { get; set; }
  //      public virtual bool IncludeInPublication { get; set; }

        public virtual bool IncludeIn { get; set; }

        public virtual bool IsMain { get; set; }

        public virtual int Level { get; set; }

        public virtual bool IsSection { get; set; }

        public virtual int LanguageId { get; set; }

        public virtual string SectionTitle { get; set; }
    }
}