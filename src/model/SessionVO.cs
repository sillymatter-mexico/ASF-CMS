using System;
using System.Collections.Generic;
using System.Web;

namespace asf.cms.model
{
    public class SessionVO
    {
        public virtual String Id
        {
            set;
            get;
        }
      
        public virtual DateTime Created
        {
            set;
            get;
        }
        public virtual DateTime Expires
        {
            set;
            get;
        }
        public virtual DateTime LockDate
        {
            set;
            get;
        }
        public virtual int LockId
        {
            set;
            get;
        }
        public virtual int Timeout
        {
            set;
            get;
        }
        public virtual bool Locked
        {
            set;
            get;
        }
        public virtual string SessionItems
        {
            set;
            get;
        }
        public virtual int Flags
        {
            set;
            get;
        }

    }
}
