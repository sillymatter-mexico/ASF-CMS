using System;
using System.Collections.Generic;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using asf.cms.model;
using asf.cms.helper;
using asf.cms.bll;

namespace asf.cms.dal
{
    public class SectionDAL:DAL<SectionVO>
    {
        public IList<SectionVO> GetAll()
        {
            return this.list("select s from SectionVO as s where s.Active=true and s.Type=null order by s.Position");
        }
        public IList<SectionVO> GetByUser(string username)
        {
            string query = "select s.* from section s, group_has_section gs, group_has_user gu " +
            "where gu.user_username='"+username+"' " +
            "and gs.group_id=gu.group_id " +
            "and s.active=1 and s.type is null " +
            "and s.id=gs.section_id order by s.position;";
            return this.list(query,typeof(SectionVO));
        }
        public IList<SectionVO> GetRootSections()
        {
            return this.list("select s from SectionVO as s where s.ParentSectionId is null and s.Active=true and s.Type=null order by s.Position");
        }

        public IList<SectionVO> GetRootSectionsVisible()
        {
            return this.list("select s from SectionVO as s where s.ParentSectionId is null and s.Active=true and s.Type=null and s.SitemapExclude=0 order by s.Position");
        }

        public IList<SectionVO> GetByParentSectionId(int parentId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("parentId", parentId);
            return this.list("select s from SectionVO as s where s.ParentSectionId=:parentId and s.Active=true and s.SitemapExclude=0 order by s.IsMain desc, s.Position asc", param);
        }

        internal IList<SectionVO> GetSpecials()
        {
            return this.list("select s from SectionVO as s where s.Active=true and s.Type!=null");
        }

        public SectionVO GetMain()
        {
            return this.getObject("select s from SectionVO as s where s.IsMain=true and s.Active=true and s.Type=null");
        }
        public SectionVO GetByPermalink(string permalink)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("permalink", permalink);
            return this.getObject("select s from SectionVO as s where s.Permalink=:permalink and s.Active=true", param);
        }

        public IList<SectionVO> GetByMenu(string menuKey)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("menuKey", menuKey);
            string query= "select s.* from section as s, section_has_menu sm where sm.menu_key= :menuKey and s.id=sm.section_id and s.active=1 and s.type is null order by sm.position";
            return this.list(query, param,typeof(SectionVO));
        }
        public IList<SectionVO> GetByGroup(int groupId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("groupId", groupId);
            string query = "select s.* from section as s, group_has_section gh where gh.group_id=:groupId and s.id=gh.section_id and s.active=1 and s.type is null order by s.position";
            return this.list(query, param, typeof(SectionVO));
        }
        public int SetUniqueMainSection(int sectionId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("sectionId", sectionId);
            string query = "update section set is_main=0 where id!=:sectionId";
            return this.Update(query, param);
        }
        public int UpdateViews(int sectionId, int views)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("sectionId", sectionId);
            param.Add("views", views);
            string query = "update section set visitas=:views where id=:sectionId";
            return this.Update(query, param);
        }
        public int Inactive(int sectionId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("sectionId", sectionId);
            string query="update section set active=0 where id=:sectionId";
            return this.Update(query,param);
         }
        public int InactiveChilds(string parentSections)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("active", 0);
            string query="update section set active=:active where parent_section_id in("+parentSections+")";
            return this.Update(query,param);
        }
        public string GetJoinedInactive()
        {
            string query = "select group_concat(cast(id as char(5))) as result from section where active=0";
            return this.getString(query);
        }
        #region Special Sections
        public IList<SectionVO> GetByType(string type)
        {
            string query = "select s.* from section as s where type=:type and active=1 order by position";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("type", type);
            return this.list(query, param, typeof(SectionVO));
        }
        #endregion
    }
}
