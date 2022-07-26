using System;
using System.Collections.Generic;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using asf.cms.model;
using asf.cms.helper;

namespace asf.cms.dal
{
    public class SectionHasMenuDAL:DAL<SectionHasMenuVO>
    {
        public SectionHasMenuVO GetById(String menuKey, int sectionId)
        {
            SectionHasMenuIdVO shmidvo = new SectionHasMenuIdVO();
            shmidvo.MenuKey = menuKey;
            shmidvo.SectionId = sectionId;
            return this.GetById(shmidvo);
        }
      public IList<SectionHasMenuVO> GetByMenu(String menuKey)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("menukey", menuKey);
            return this.list("select shm from SectionHasMenuVO as shm where shm.id.MenuKey=:menukey order by shm.Position",param);
        }
        public List<SectionHasMenuVO> UpdateMenu(string menuKey, List<SectionHasMenuVO> items)
        {
            return this.UpdateList(items, "delete SectionHasMenuVO shm where shm.id.MenuKey='" + menuKey + "'");
        }
    }
}
