﻿using System;
using System.Collections.Generic;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using asf.cms.model;
using asf.cms.helper;
using NHibernate.Transform;
using System.Text;
using asf.cms.exception;
using System.Data.SqlClient;

namespace asf.cms.dal
{
    public class PublicationDAL:DAL<PublicationVO>
    {
        protected static string IS_MAIN_MYSQL_32 = "_isMain";
        protected static string IS_MAIN_MYSQL_64 = "IsMain";

        protected static string SITEMAP_EXCLUDE_MYSQL_32 = "_sitemapExclude";
        protected static string SITEMAP_EXCLUDE_MYSQL_64 = "SitemapExclude";

#if MYSQL32
        protected static string IS_MAIN = IS_MAIN_MYSQL_32;
        protected static string SITEMAP_EXCLUDE = SITEMAP_EXCLUDE_MYSQL_32;
#else
        protected static string IS_MAIN = IS_MAIN_MYSQL_64;
        protected static string SITEMAP_EXCLUDE = SITEMAP_EXCLUDE_MYSQL_64;
#endif

        public IList<PublicationAdminVO> ListPublicationAdmin(string orden)
        {
            IList<PublicationAdminVO> Lista = new List<PublicationAdminVO>();

            if(orden=="")
                orden= "order by Section, position, Status, Title;";

            string query = "select publication.id as Id, title as Title, publication.language_id as LanguageId, " +
                "publication.autogenerated_type as AutogeneratedType, section_label.content as Section, status as Status, " +
                "publication.sitemap_exclude as " + SITEMAP_EXCLUDE + ", is_main as " + IS_MAIN + " " +
                "from publication left join section_label on section_label.language_id=publication.language_id and section_label.section_id=publication.section_id " +
                "where publication.active=1 and publication.autogenerated_type is null and Status != 4 " + ExpresionStringHelper.replaceEscapeCharacter(orden);
                
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                try
                {
                    ISQLQuery iquery = session.CreateSQLQuery(query);
                    iquery.SetResultTransformer(Transformers.AliasToBean(typeof(PublicationAdminVO)));
                    Lista = iquery.List<PublicationAdminVO>();
                }
                catch (Exception ex)
                {
                    throw new ProcessException("Error en el proceso.");
                }
                finally
                {
                    session.Close();
                }
                
                return Lista;
            }

            /*string query = "select publication.id as Id,title as Title, publication.language_id as LanguageId,publication.autogenerated_type as AutogeneratedType, section_label.content as Section, status as Status " +
               ", case when publication.sitemap_exclude=0 then false else true end as _sitemapExclude," +
               "case when is_main=0 then false else true end as isMain " +
               "from publication, section_label " +
               "where section_label.language_id=publication.language_id " +
               "and section_label.section_id=publication.section_id " +
               "and publication.active=1 " + orden;*/
        }
        public IList<PublicationAdminVO> ListPublicationAdmin(String username,string orden)
        {
            IList<PublicationAdminVO> Lista = new List<PublicationAdminVO>();

            string query = "select publication.id  as Id, title as Title, publication.language_id as LanguageId, section_label.content as Section, " +
                "status as Status, publication.sitemap_exclude as " + SITEMAP_EXCLUDE + ", is_main as " + IS_MAIN + " " +
                "from publication, section_label , group_has_section sg, group_has_user gu "+
                "where gu.user_username=:username "+
                "and sg.group_id=gu.group_id "+
                "and publication.section_id=sg.section_id "+
                "publication.active=1 "+
                "and section_label.language_id=publication.language_id "+
                "and section_label.section_id=publication.section_id order by Section,position, Status, Title;";

            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                try
                {
                    ISQLQuery iquery = session.CreateSQLQuery(query);
                    iquery.SetParameter("username", username);
                    iquery.SetResultTransformer(Transformers.AliasToBean(typeof(PublicationAdminVO)));
                    Lista = iquery.List<PublicationAdminVO>();                    
                }
                catch (Exception ex)
                {
                    throw new ProcessException("Error en el proceso.");
                }
                finally
                {
                    session.Close();
                }

                return Lista;
            }

            /*string query = "select publication.id  as Id,title as Title, publication.language_id as LanguageId,section_label.content as Section, status as Status  " +
                ", case when publication.sitemap_exclude=0 then false else true end as _sitemapExclude," +
                "case when is_main=0 then false else true end as isMain " +
                "from publication, section_label , group_has_section sg, group_has_user gu " +
                "where gu.user_username='" + username + "' " +
                "and sg.group_id=gu.group_id " +
                "and publication.section_id=sg.section_id " +
                "publication.active=1 " +
                "and section_label.language_id=publication.language_id " +
                "and section_label.section_id=publication.section_id order by Section,position, Status, Title;";*/
        }
        public IList<PublicationAdminVO> ListPublicationAdmin(int sectionId)
        {
            IList<PublicationAdminVO> Lista = new List<PublicationAdminVO>();

            string query = "select publication.id as Id,title as Title, publication.language_id as LanguageId, " +
                "publication.sitemap_exclude as " + SITEMAP_EXCLUDE + ", publication.position as Position, " +
                "is_main as " + IS_MAIN + ", status as Status, section_label.content as Section, publication.autogenerated_type as AutogeneratedType " +
                "from publication, section_label " +
                "where publication.section_id=:sectionId " +
                "and section_label.language_id=publication.language_id " +
                "and publication.active=1 " +
                "and section_label.section_id=publication.section_id order by Section,position, Status, Title;";

            

            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                try
                {
                    ISQLQuery iquery = session.CreateSQLQuery(query);
                    iquery.SetParameter("sectionId", sectionId);
                    iquery.SetResultTransformer(Transformers.AliasToBean(typeof(PublicationAdminVO)));
                    Lista = iquery.List<PublicationAdminVO>();
                }
                catch (Exception ex)
                {
                    throw new ProcessException("Error en el proceso.");
                }
                finally
                {
                    session.Close();
                }
                
                return Lista;
            }

            /*string query = "select publication.id as Id,title as Title, publication.language_id as LanguageId,section_label.content as Section, publication.position as Position, " +
                "case when publication.sitemap_exclude=0 then false else true end as _sitemapExclude," +
                "case when is_main=0 then false else true end as isMain,status as Status " +
                "from publication, section_label " +
                "where publication.section_id='" + sectionId + "' " +
                "and section_label.language_id=publication.language_id " +
                "and publication.active=1 " +
                "and section_label.section_id=publication.section_id order by Section,position, Status, Title;";*/
        }

        public void Metodo(string filtro)
        {
            string consulta = "select + from table where " + filtro + "";

            //ejecutar consulta
        }

        public IList<PublicationVO> ListPublicationsByNewsInclude(bool news_include)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("news_include", news_include);
                string query = "select p from PublicationVO as p where p.NewsInclude=:news_include and p.Active=true order by position";
                return this.list(query, param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public PublicationVO GetPrincipalBySection(int sectionId, int languageId)
        {
            try
            {
                

                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("sId", sectionId);
                param.Add("lId", languageId);
                string query = "select * from publication where section_id=:sId and is_main=1 and status=1 and active=1 and language_id=:lId limit 1;";
                return this.getObject(query, param, typeof(PublicationVO));
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public IList<PublicationVO> GetListBySection(int sectionId, int languageId, bool mapVisible = true)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("sectionId", sectionId);
                param.Add("languageId", languageId);

                string query = "select p from PublicationVO as p where p.SectionId=:sectionId and p.LanguageId=:languageId and p.Active=true and p.Status=1 " + (mapVisible ? "and p.SitemapExclude=0" : "") + "order by p.IsMain desc, p.Position asc, p.NewsPin desc, p.Updated desc";

                return this.list(query, param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public IList<PublicationVO> GetListBySection(int sectionId, int languageId, string fieldsToOrderBy)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("sectionId", sectionId);
                param.Add("languageId", languageId);

                StringBuilder sb = new StringBuilder();
                string[] fieldsArr = fieldsToOrderBy.Split(',');
                int cont = 0;
                foreach (string field in fieldsArr)
                {
                    sb.Append("p.");
                    sb.Append(field);
                    if (cont != fieldsArr.Length - 1)
                    {
                        sb.Append(",");
                    }
                    cont++;
                }

                string query = "select p from PublicationVO as p where p.SectionId=:sectionId and p.LanguageId=:languageId  and p.Active=true and p.Status=1 order by " + ExpresionStringHelper.replaceEscapeCharacter(sb.ToString()) + " ASC";
                return this.list(query, param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public IList<PublicationVO> GetOrderedListBySection(int sectionId, int languageId)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("sectionId", sectionId);
                param.Add("languageId", languageId);

                string query = "select p from PublicationVO as p where p.SectionId=:sectionId and p.LanguageId=:languageId and p.Active=true and p.Status=1 order by p.Position, p.Updated desc";
                return this.list(query, param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }
            
        }
        public IList<PublicationVO> GetWithoutContentBySectionId(int sectionId)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("sId", sectionId);
                string query = "select p.Id,p.LanguageId,p.SectionId, p.Permalink,p.Title,p.Created,p.Updated,p.Published,p.IsMain, "
                + "p.IsAutogenerated,p.unpublished,p.Status,p.Visitas,p.HasWidgets, p.NewsInclude,p.NewsPin,p.NewsTtl,p.NewsContent,p.Position,p.Active "
                + "from PublicationVO as p where p.SectionId=:sId  and p.Status=1 and p.Active=true order by p.Position;";
                return this.list(query, param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public PublicationVO GetByPermalink(String permalink)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("permalink", ExpresionStringHelper.replaceEscapeCharacter(permalink));
                string query = "select p from PublicationVO as p where p.Permalink=:permalink and p.Active=true";
                return this.getObject(query, param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public int SetUniqueMainBySection(int publicationId,int sectionId, int languageId)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("sectionId", sectionId);
                param.Add("publicationId", publicationId);
                param.Add("languageId", languageId);
                string query = "update publication set is_main=0 where section_id=:sectionId and id!=:publicationId and language_id=:languageId";
                return this.Update(query, param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public int SetHasWidgetsBySection(int publicationId, int sectionId, bool has_widgets)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("sectionId", sectionId);
                param.Add("publicationId", publicationId);
                param.Add("has_widgets", has_widgets);

                string query = "update publication set has_widgets=:has_widgets where section_id=:sectionId and id=:publicationId";
                return this.Update(query, param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public IList<PublicationVO> GetAll()
        {
            try
            {
                return this.list("select p from PublicationVO as p where p.Active=true order by p.NewsPin desc, p.Updated desc");
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public IList<PublicationVO> GetAll(int languageId)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("language_id", languageId);
                return this.list("select p from PublicationVO as p where p.Active=true and p.LanguageId=:language_id order by p.NewsPin, p.Updated desc", param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public int UpdateViews(int publicationId, int views)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("publicationId", publicationId);
                param.Add("views", views);
                string query = "update publication set visitas=:views where id=:publicationId";
                return this.Update(query, param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public int UpdateMeta(int publicationId, string meta)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("publicationId", publicationId);
                param.Add("meta", ExpresionStringHelper.replaceEscapeCharacter(meta));
                string query = "update publication set meta=:meta where id=:publicationId";
                return this.Update(query, param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }
      
        public int UpdatePublicationNews(int publicationId,string newsContent,int newsTTL, bool newsInclude,bool newsPin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("publicationId", publicationId);
                param.Add("newsContent", ExpresionStringHelper.replaceEscapeCharacter(newsContent));
                param.Add("newsTTL", newsTTL);
                param.Add("newsInclude", newsInclude);
                param.Add("newsPin", newsPin);

                string query = "update publication set news_content=:newsContent,news_ttl=:newsTTL,news_include=:newsInclude,news_pin=:newsPin where id=:publicationId";
                return this.Update(query, param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public IList<PublicationVO> GetHistoric(int publicationId, int limit)
        {
            try
            {
                string query = "SELECT p FROM PublicationVO as p WHERE p.Permalink LIKE :permalink AND p.Status=4 ORDER BY p.Unpublished DESC";
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("permalink", publicationId + "_%");

                return list(query, param, limit, 0);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        } 

        public int PruneHistoric(int publicationId, int offset)
        {
            try
            {
                string query = "SELECT p FROM PublicationVO as p WHERE p.Permalink LIKE :permalink AND p.Status=4 ORDER BY p.Unpublished DESC";
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("permalink", publicationId + "_%");

                IList<PublicationVO> r = list(query, param, 1000, offset);

                foreach (PublicationVO p in r)
                {
                    Delete(p);
                }

                return r.Count;
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public int Inactive(string sectionJoined)
        {
            try
            {
                string query = "update publication set active=0 where section_id in (" + ExpresionStringHelper.replaceEscapeCharacter(sectionJoined) + ")";
                return this.Update(query, new Dictionary<string, object>());
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public int Inactive(int publicationId)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("publicationId", publicationId);
                string query = "update publication set active=0 where id=:publicationId";
                return this.Update(query, param);
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public DateTime getFechaUltimaModificacion()
        {
            try
            {
                string query = "select p from PublicationVO as p where p.Active=1 order by p.Updated desc Limit 0,1";
                IList<PublicationVO> p = this.list(query);
                if (p.Count > 0)
                    return p[0].Updated;
                return DateTime.Today;
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }

        public DateTime getFechaUltimaModificacionPorSeccion(int sectionId)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("sectionId", sectionId);

                string query = "select p from PublicationVO as p where p.Active=1 and p.SectionId=:sectionId order by p.Updated desc Limit 0,1";
                IList<PublicationVO> p = this.list(query);
                if (p.Count > 0)
                    return p[0].Updated;
                return DateTime.Today;
            }
            catch (Exception ex)
            {
                throw new ProcessException("Error en el proceso.");
            }
            finally
            {
                
            }            
        }
    }
}
