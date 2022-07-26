using System;
using System.Collections.Generic;
using System.Web;

namespace asf.cms.model
{
    public class FileVO
    {
        public virtual int Id
        {
            set;
            get;
        }
        public virtual int PublicationId
        {
            set;
            get;
        }
        public virtual String Mime
        {
            set;
            get;
        }
        public virtual String Path
        {
            set;
            get;
        }
        public virtual String Name
        {
            set;
            get;
        }
        public virtual String Title
        {
            set;
            get;
        }
        public virtual int IsMain
        {
            set;
            get;
        } 
    }
}
