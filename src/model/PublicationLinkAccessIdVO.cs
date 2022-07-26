using System;
using System.Collections.Generic;
using System.Web;

namespace asf.cms.model
{
    [Serializable]
    public class PublicationLinkAccessIdVO

    {
        public virtual DateTime AccessDate { get; set; }

        public virtual string AccessUrl { get; set; }

        public override bool Equals(object obj)
        {
            PublicationLinkAccessIdVO other = obj as PublicationLinkAccessIdVO;
            if (other is null)
                return false;

            return AccessDate.Year == other.AccessDate.Year &&
                   AccessDate.Month == other.AccessDate.Month &&
                   AccessDate.Day == other.AccessDate.Day &&
                   AccessUrl == other.AccessUrl;
        }

        public override int GetHashCode()
        {
            return AccessDate.Year.GetHashCode() ^
                   AccessDate.Month.GetHashCode() ^
                   AccessDate.Day.GetHashCode() ^
                   AccessUrl.GetHashCode();
        }
    }
}