using System;
using System.Collections.Generic;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using asf.cms.model;
using asf.cms.helper;

namespace asf.cms.dal
{
    public class SectionLabelDAL : DAL<SectionLabelVO>
    {
        public SectionLabelVO GetById(int sectionId, int languageId)
        {
            SectionLabelIdVO slid=new SectionLabelIdVO();
            slid.LanguageId=languageId;
            slid.SectionId=sectionId;
            return this.GetById(slid);
        }
        /*Gets the object in the language, if the language is not found returns the title in other
         * language*/
         
        public SectionLabelVO GetByFitLanguage(int sectionId, int languageId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("sId", sectionId);
            param.Add("lId", languageId);
            string query = "select * from section_label where section_id=:sId order by language_id=:lId desc limit 1;";
            return this.getObject(query,param,typeof(SectionLabelVO));
        }
        /*public IList<SectionLabelVO> GetMainByLanguage(int languageId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add(":langId", languageId);
            return this.list("select s from SectionLabelVO as sl, Section as s " +
            "where s.IsMain=true and sl.SectionId=s.Id and sl.LanguageId=:langId and s.Active=true", param);
           // return this.listSQL("select sl.* from section_label sl, section s "+
            //"where s.is_main=true and sl.section_id<4 and sl.section_id=s.id and sl.language_id="+languageId,param);
        }
        public IList<SectionLabelVO> GetByMenuKey(String menukey,int languageId)
        {

            string query = "select sm.section_id as section_id,2 as language_id, ifnull(sl.content,sl2.content) as content " +
                " from section_has_menu sm " +
                " left join section_label sl on sl.section_id=sm.section_id and sl.language_id=2 " +
                " left join section_label sl2 on sl2.section_id=sm.section_id and sl2.language_id!=2 " +
                " where  menu_key='PRINCIPAL'";
            return this.list(query, typeof(SectionLabelVO));
 
        }
        */
    }
}
