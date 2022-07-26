using System;
using System.Collections.Generic;
using System.Web;

namespace asf.cms.model
{
    public class UserVO
    {

        public virtual String Username
        {
            set;
            get;
        }
        public virtual String Password
        {
            set;
            get;
        }
        public virtual String Type
        {
            set;
            get;
        }
        public virtual Boolean Active
        {
            set;
            get;
        }
    }
}
