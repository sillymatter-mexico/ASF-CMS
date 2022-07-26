using System;
using System.Collections.Generic;
using System.Web;

namespace asf.cms.model
{
    public class AuditEntityVO
    {
        public virtual int Id
        {
            get;
            set;
        }
        public virtual string EntityKey
        {
            get;
            set;
        }
        public virtual int SectorId
        {
            get;
            set;
        }
        public virtual string ShortName
        {
            get;
            set;
        }
        public virtual string Name
        {
            get;
            set;
        }
        public virtual string DepKey
        {
            get;
            set;
        }
        public virtual string TypeKey
        {
            get;
            set;
        }
    }
}
