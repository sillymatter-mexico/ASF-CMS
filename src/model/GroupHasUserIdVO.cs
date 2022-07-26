using System;
using System.Collections.Generic;
using System.Web;

namespace asf.cms.model
{
    public class GroupHasUserIdVO
    {
        public virtual int GroupId
        {
            get;
            set;
        }
        public virtual string Username
        {
            get;
            set;
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var t = obj as GroupHasUserIdVO;
            if (t == null)
                return false;
            if (GroupId == t.GroupId && Username == t.Username)
                return true;
            return false;
        }
        public override int GetHashCode()
        {
            return (GroupId + "|" + Username).GetHashCode();
        }
    }
}
