using System;
using System.Collections.Generic;
using System.Web;

namespace asf.cms.model
{
    [Serializable]
    public class SectionHasMenuIdVO
    {
        public virtual string MenuKey { get; set; }
        public virtual int SectionId { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var t = obj as SectionHasMenuIdVO;
            if (t == null)
                return false;
            if (SectionId == t.SectionId && MenuKey== t.MenuKey)
                return true;
            return false;
        }
        public override int GetHashCode()
        {
            return (SectionId + "|" + MenuKey).GetHashCode();
        }
    }
}
