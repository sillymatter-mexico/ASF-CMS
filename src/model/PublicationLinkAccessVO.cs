using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;

namespace asf.cms.model
{
    public class PublicationLinkAccessVO
    {
        public virtual PublicationLinkAccessIdVO PublicationLinkAccessId { get; set; }
        
        public virtual uint HitCount { get; set; }
    }
}