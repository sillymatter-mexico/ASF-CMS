using System;
using System.Collections.Generic;
using System.Web;

namespace asf.cms.model
{
    [Serializable]  
    public class SectionLabelIdVO
    {
        public virtual int SectionId { get; set; }
        public virtual int LanguageId { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var t = obj as SectionLabelIdVO;
            if (t == null)
                return false;
            if (SectionId == t.SectionId && LanguageId == t.LanguageId)
                return true;
            return false;
        }
        public override int GetHashCode()
        {
            return (SectionId + "|" + LanguageId).GetHashCode();
        }
    }
}
