using System;
using System.Collections.Generic;
using System.Web;

namespace asf.cms.model
{
    public class GroupHasSectionIdVO
    {
        public virtual int GroupId
        {
            get;
            set;
        }
        public virtual int SectionId
        {
            get;
            set;
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var t = obj as GroupHasSectionIdVO;
            if (t == null)
                return false;
            if (GroupId == t.GroupId && SectionId == t.SectionId)
                return true;
            return false;
        }
        public override int GetHashCode()
        {
            return (GroupId + "|" + SectionId).GetHashCode();
        }
    }
}
