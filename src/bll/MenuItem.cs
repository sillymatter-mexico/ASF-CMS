using System;
using System.Collections.Generic;
using System.Web;
using asf.cms.model;
using asf.cms.dal;

namespace asf.cms.bll
{
    public class MenuItem
    {
        public SectionHasMenuVO Item;
        public Section Section;

        public MenuItem()
        {
            Item = new SectionHasMenuVO();
            Item.SectionHasMenuId = new SectionHasMenuIdVO();
        }
        public MenuItem(SectionHasMenuVO item)
        {
            Item = item;
        }
        public MenuItem(SectionHasMenuVO item, Section section)
        {
            Item = item;
            Section = section;
        }
    }
}
