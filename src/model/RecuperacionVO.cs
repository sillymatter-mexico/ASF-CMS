using System;
using System.Data;
using System.Configuration;

namespace asf.cms.model
{
    public class RecuperacionVO
    {
        public RecuperacionVO()
        {
            this.Files = "{}";
        }
        public virtual int Id
        {
            set;
            get;
        }
        public virtual String DirectoryPath
        {
            set;
            get;
        }
        public virtual String Title
        {
            set;
            get;
        }
        public virtual DateTime CreationDate
        {
            set;
            get;
        }
        public virtual bool Active
        {
            set;
            get;
        }
        public virtual String Files
        {
            set;
            get;
        }

    }
}
