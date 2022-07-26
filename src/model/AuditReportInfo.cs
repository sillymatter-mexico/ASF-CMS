using System;
using System.Collections.Generic;
using System.Web;

namespace asf.cms.model
{
    public class AuditReportInfo
    {
        public virtual int ReportId
        {
            set;
            get;
        }
        public virtual int PublicationId
        {
            set;
            get;
        }
        public virtual string Permalink
        {
            set;
            get;
        }
        public virtual int Year
        {
            set;
            get;
        }
        public virtual string Title
        {
            set;
            get;
        }
        public virtual int Published
        {
            set;
            get;
        }
        public virtual DateTime PublishDate
        {
            set;
            get;
        }
        public virtual Int64 LoadedAudits
        {
            set;
            get;
        }
        public virtual bool IsPublished
        {
            get { return (this.Published == 1); }
        }

    }
}
