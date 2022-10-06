using System;
using System.Collections.Generic;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using asf.cms.model;
using asf.cms.helper;
using NHibernate.Transform;
using asf.cms.exception;

namespace asf.cms.dal
{
    public class MenuDAL:DAL<MenuVO>
    {
        public IList<MenuVO> GetMenus()
        {
            IList<MenuVO> Lista = new List<MenuVO>();

            string query = "SELECT menu_key as MenuKey, css_class as CSSClass FROM menu";

            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                try
                {
                    ISQLQuery iquery = session.CreateSQLQuery(query);
                    iquery.SetResultTransformer(Transformers.AliasToBean(typeof(MenuVO)));
                    Lista = iquery.List<MenuVO>();
                }
                catch (Exception ex)
                {
                    throw new ProcessException("Error en el proceso.");
                }
                finally
                {
                    session.Close();
                }
            }

            return Lista;
        }
        public IList<SearchResultVO> SearchInSite(String q)
        {
            IList<SearchResultVO> Lista = new List<SearchResultVO>();

/*            String query = "select s.id as Id, sl.content as Content, sl.content as Title," +
               " case when s.redirect_to is not null and s.redirect_to!=''" +
               " then  s.redirect_to else s.permalink end Link,'Section' as Class," +
               " case when s.redirect_to is not null and s.redirect_to!=''" +
               " then 'link' else 'permalink' end Subclass, match(sl.content) against('" + q + "') and s.id=sl.section_id as Score" +
               " from section s, section_label sl" +
               " where match(sl.content) against('" + q + "') and s.id=sl.section_id" +
               " and s.active=1  union" +
               " select p.id as Id, concat(p.content) as Content, p.title, p.permalink as Link, 'Publication' as Class," +
               " 'permalink' as Subclass, match(p.title,p.content) against('" + q + "') as score" +
               " from publication p" +
               " where match(p.title,p.content) against('" + q + "') and p.active=1 and p.status=1 union" +
               " select f.id, f.name, f.title,f.path, 'File','path',match(f.title,f.name) against('" + q + "')" +
               "  from file f, publication p" +
               " where match(f.title,f.name) against('" + q + "') and p.id=f.publication_id" +
               " and p.active=1 and p.status=1 and mime not like '%video%'" +
               " order by score desc;";*/

            string slcontent = q.Replace("-*$*-", "sl.content");
            string ptitle = q.Replace("-*$*-", "p.title");
            string pcontent = q.Replace("-*$*-", "p.content");
            string ftitle = q.Replace("-*$*-", "f.title");
            string fname = q.Replace("-*$*-", "f.name");

            String query = "select s.id as Id, sl.content as Content, sl.content as Title," +
           " case when s.redirect_to is not null and s.redirect_to!=''" +
           " then  s.redirect_to else s.permalink end Link,'Section' as Class," +
           " case when s.redirect_to is not null and s.redirect_to!=''" +
           " then 'link' else 'permalink' end Subclass, 2 as Score" +
           " from section s, section_label sl" +
           " where (1=0 "+slcontent+") and s.id=sl.section_id" +
           " and s.active=1  union" +
           " select p.id as Id, concat(p.content) as Content, p.title, p.permalink as Link, 'Publication' as Class," +
           " 'permalink' as Subclass, 1 as score" +
           " from publication p" +
           " where (1=0 "+ptitle+" "+pcontent+")  and p.active=1 and p.status=1 union" +
           " select f.id, f.name, ifnull(f.title,f.name),f.path, 'File','path',0 " +
           "  from file f, publication p" +
           " where (1=0 " + ftitle + " " + fname + ") and p.id=f.publication_id" +
           " and p.active=1 and p.status=1 and mime not like '%video%'" +
           " order by score desc;";

            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                try
                {
                    ISQLQuery iquery = session.CreateSQLQuery(query);
                    iquery.SetResultTransformer(Transformers.AliasToBean(typeof(SearchResultVO)));
                    Lista = iquery.List<SearchResultVO>();
                }
                catch (Exception ex)
                {
                    throw new ProcessException("Error en el proceso.");
                }
                finally
                {
                    session.Close();
                }
            }

            return Lista;
        }
        public IList<SearchResultVO> GetSiteContent()
        {
            IList<SearchResultVO> Lista = new List<SearchResultVO>();

            String query = "select s.id as Id, sl.content as Content," +
                " case when s.redirect_to is not null and s.redirect_to!=''" +
                " then  s.redirect_to else s.permalink end Link,'Section' as Class," +
                " case when s.redirect_to is not null and s.redirect_to!=''" +
                " then 'link' else 'permalink' end Subclass" +
                " from section s, section_label sl" +
                " where s.id=sl.section_id" +
                " and s.active=1 union" +
                " select p.id as Id, p.title as Content,p.permalink as Link, 'Publication' as Class, 'permalink' as Subclass" +
                " from publication p" +
                " where p.active=1 and p.status=1 union" +
                /*" select p.id, p.content,p.permalink, 'Publication', 'permalink'" +
                " from publication p" +
                " where p.active=1 and p.status=1 union" +*/
                " select f.id, f.title,f.path, 'File','path' from file f, publication p" +
                " where p.id=f.publication_id and mime not like '%video%'" +
                " and p.active=1 and p.status=1 union" +
                " select f.id, f.name,f.path, 'File','path' from file f, publication p" +
                " where p.id=f.publication_id" +
                " and p.active=1 and p.status=1 and mime not like '%video%';";
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                try
                {
                    ISQLQuery iquery = session.CreateSQLQuery(query);
                    iquery.SetResultTransformer(Transformers.AliasToBean(typeof(SearchResultVO)));
                    Lista = iquery.List<SearchResultVO>();
                }
                catch (Exception ex)
                {
                    throw new ProcessException("Error en el proceso.");
                }
                finally
                {
                    session.Close();
                }
            }

            return Lista;
        }
    }
}
