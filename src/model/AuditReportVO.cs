using System;
using System.Collections.Generic;
using System.Web;

namespace asf.cms.model
{
    public class AuditReportVO
    {
        public virtual int Id
        {
            set;
            get;
        }
        public virtual int Year
        {
            set;
            get;
        }
        public virtual int Type
        {
            set;
            get;
        }
        public virtual string DirectoryPath
        {
            set;
            get;
        }
        public virtual bool Published
        {
            set;
            get;
        }
        public virtual string PublicationPermalink
        {
            set;
            get;
        }
        public virtual string MainFilePath
        {
            set;
            get;
        }
        public virtual string PresentationPath
        {
            set;
            get;
        }
        public virtual string ExecutiveReportPath
        {
            set;
            get;
        }
        public virtual string IndexPermalink
        {
            set;
            get;
        }
    }
}
